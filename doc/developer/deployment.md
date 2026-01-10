# Deployment Guide

This guide covers deploying ACAL in production environments using various methods.

## Table of Contents
- [Docker Deployment](#docker-deployment)
- [Traditional Deployment](#traditional-deployment)
- [Reverse Proxy Setup](#reverse-proxy-setup)
- [Security Considerations](#security-considerations)
- [Monitoring and Maintenance](#monitoring-and-maintenance)
- [Troubleshooting Deployment](#troubleshooting-deployment)

## Docker Deployment

Docker is the recommended deployment method for ACAL, providing consistency and ease of updates.

### Prerequisites

- Docker Engine 20.10+ or Docker Desktop
- 100MB+ available disk space
- Network access for pulling images

### Quick Deploy

```bash
# Create configuration directory
mkdir -p ~/acal-config

# Create images directory (optional)
mkdir -p ~/acal-images

# Copy your configuration file
cp appsettings.json ~/acal-config/

# Pull and run the container
docker run -d \
  --name acal \
  --restart unless-stopped \
  -p 5000:8080 \
  -v ~/acal-config:/app/config \
  -v ~/acal-images:/app/images \
  -e TZ=America/New_York \
  arizonagreentea0905/acal:latest
```

### Configuration Options

#### Volume Mounts

```bash
-v /host/path/config:/app/config    # Required: Configuration files
-v /host/path/images:/app/images    # Optional: Image directory
-v /host/path/logs:/app/logs        # Optional: Log persistence
```

#### Environment Variables

```bash
-e TZ=America/New_York              # Timezone
-e ASPNETCORE_ENVIRONMENT=Production # Environment
```

#### Port Mapping

```bash
-p 5000:8080                        # Map host:5000 to container:8080
-p 80:8080                          # Map host:80 to container:8080
```

### Docker Compose

Create `docker-compose.yml`:

```yaml
version: '3.8'

services:
  acal:
    image: arizonagreentea0905/acal:latest
    container_name: acal
    restart: unless-stopped
    ports:
      - "5000:8080"
    volumes:
      - ./config:/app/config
      - ./images:/app/images
      - ./logs:/app/logs
    environment:
      - TZ=America/New_York
      - ASPNETCORE_ENVIRONMENT=Production
```

Deploy with:

```bash
docker-compose up -d
```

### Update Strategy

#### Pull and Restart

```bash
# Pull latest image
docker pull arizonagreentea0905/acal:latest

# Stop and remove old container
docker stop acal
docker rm acal

# Start new container with same configuration
docker run -d \
  --name acal \
  --restart unless-stopped \
  -p 5000:8080 \
  -v ~/acal-config:/app/config \
  -v ~/acal-images:/app/images \
  arizonagreentea0905/acal:latest
```

#### Docker Compose Update

```bash
# Pull latest image
docker-compose pull

# Restart with new image
docker-compose up -d
```

### Health Checks

Add health check to Docker Compose:

```yaml
services:
  acal:
    # ... other config ...
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost:8080"]
      interval: 30s
      timeout: 10s
      retries: 3
      start_period: 40s
```

## Traditional Deployment

Deploy ACAL without Docker to various hosting environments.

### Prerequisites

- .NET 10 Runtime
- Web server (IIS, Nginx, Apache)
- 100MB+ available disk space

### Build for Production

```bash
# Clone repository
git clone https://github.com/ArizonaGreenTea05/ACAL.git
cd ACAL

# Restore dependencies
dotnet restore

# Build and publish
dotnet publish CalendarView/CalendarView.Web \
  -c Release \
  -o ./publish \
  --self-contained false
```

### Windows IIS Deployment

#### 1. Install Prerequisites

- .NET 10 Hosting Bundle
- IIS with ASP.NET Core Module

#### 2. Create IIS Site

1. Open IIS Manager
2. Right-click "Sites" → "Add Website"
3. Configure:
   - Site name: ACAL
   - Physical path: `C:\inetpub\acal`
   - Port: 80 or 443 (with SSL)

#### 3. Deploy Files

Copy published files to `C:\inetpub\acal`

#### 4. Configure Application Pool

1. Select ACAL Application Pool
2. Set:
   - .NET CLR Version: No Managed Code
   - Managed Pipeline Mode: Integrated

#### 5. Set Permissions

Grant `IIS_IUSRS` read/execute permissions on the deployment folder.

#### 6. Configure web.config

Ensure `web.config` exists with:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <location path="." inheritInChildApplications="false">
    <system.webServer>
      <handlers>
        <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
      </handlers>
      <aspNetCore processPath="dotnet"
                  arguments=".\CalendarView.Web.dll"
                  stdoutLogEnabled="false"
                  stdoutLogFile=".\logs\stdout"
                  hostingModel="inprocess" />
    </system.webServer>
  </location>
</configuration>
```

### Linux (Systemd) Deployment

#### 1. Prepare Application

```bash
# Copy published files
sudo mkdir -p /var/www/acal
sudo cp -r ./publish/* /var/www/acal/

# Set permissions
sudo chown -R www-data:www-data /var/www/acal
```

#### 2. Create Systemd Service

Create `/etc/systemd/system/acal.service`:

```ini
[Unit]
Description=ACAL Calendar Application
After=network.target

[Service]
Type=notify
User=www-data
Group=www-data
WorkingDirectory=/var/www/acal
ExecStart=/usr/bin/dotnet /var/www/acal/CalendarView.Web.dll
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=acal
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
```

#### 3. Start Service

```bash
# Reload systemd
sudo systemctl daemon-reload

# Enable service
sudo systemctl enable acal

# Start service
sudo systemctl start acal

# Check status
sudo systemctl status acal
```

#### 4. View Logs

```bash
sudo journalctl -u acal -f
```

## Reverse Proxy Setup

Configure a reverse proxy for production deployment.

### Nginx

#### Configuration

Create `/etc/nginx/sites-available/acal`:

```nginx
server {
    listen 80;
    server_name your-domain.com;

    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        
        # WebSocket support
        proxy_set_header Connection "upgrade";
        
        # Timeouts
        proxy_connect_timeout 7d;
        proxy_send_timeout 7d;
        proxy_read_timeout 7d;
    }
}
```

#### Enable Site

```bash
# Link configuration
sudo ln -s /etc/nginx/sites-available/acal /etc/nginx/sites-enabled/

# Test configuration
sudo nginx -t

# Reload Nginx
sudo systemctl reload nginx
```

#### SSL with Let's Encrypt

```bash
# Install certbot
sudo apt install certbot python3-certbot-nginx

# Obtain certificate
sudo certbot --nginx -d your-domain.com

# Auto-renewal is configured automatically
```

### Apache

#### Configuration

Create `/etc/apache2/sites-available/acal.conf`:

```apache
<VirtualHost *:80>
    ServerName your-domain.com
    
    ProxyPreserveHost On
    ProxyPass / http://localhost:5000/
    ProxyPassReverse / http://localhost:5000/
    
    # WebSocket support
    RewriteEngine on
    RewriteCond %{HTTP:Upgrade} websocket [NC]
    RewriteCond %{HTTP:Connection} upgrade [NC]
    RewriteRule ^/?(.*) "ws://localhost:5000/$1" [P,L]
    
    ErrorLog ${APACHE_LOG_DIR}/acal-error.log
    CustomLog ${APACHE_LOG_DIR}/acal-access.log combined
</VirtualHost>
```

#### Enable Site

```bash
# Enable required modules
sudo a2enmod proxy
sudo a2enmod proxy_http
sudo a2enmod proxy_wstunnel
sudo a2enmod rewrite

# Enable site
sudo a2ensite acal

# Reload Apache
sudo systemctl reload apache2
```

### Traefik (Docker)

Add to `docker-compose.yml`:

```yaml
version: '3.8'

services:
  acal:
    image: arizonagreentea0905/acal:latest
    container_name: acal
    restart: unless-stopped
    volumes:
      - ./config:/app/config
      - ./images:/app/images
    labels:
      - "traefik.enable=true"
      - "traefik.http.routers.acal.rule=Host(`acal.your-domain.com`)"
      - "traefik.http.routers.acal.entrypoints=websecure"
      - "traefik.http.routers.acal.tls.certresolver=letsencrypt"
      - "traefik.http.services.acal.loadbalancer.server.port=8080"
    networks:
      - traefik

networks:
  traefik:
    external: true
```

## Security Considerations

### HTTPS/SSL

**Always use HTTPS in production:**

1. **Let's Encrypt** (Free, automated)
   - Use Certbot for Nginx/Apache
   - Auto-renewal configured

2. **Custom Certificate**
   - Purchase from CA
   - Configure in reverse proxy

### Authentication

Enable authentication in production:

```json
{
  "AuthenticationConfig": {
    "Enabled": true,
    "Username": "admin",
    "Password": "secure-random-password-here"
  }
}
```

**Best Practices:**
- Use strong passwords (16+ characters)
- Consider external authentication (OAuth, LDAP)
- Rotate credentials regularly

### File Permissions

**Linux:**
```bash
# Configuration files
chmod 600 /path/to/appsettings.json

# Application directory
chmod 755 /var/www/acal
chown www-data:www-data /var/www/acal

# Log directory
chmod 755 /var/www/acal/logs
```

**Docker:**
```bash
# Secure configuration directory
chmod 700 ~/acal-config
chmod 600 ~/acal-config/appsettings.json
```

### Firewall Configuration

```bash
# Allow HTTP/HTTPS only
sudo ufw allow 80/tcp
sudo ufw allow 443/tcp

# Block direct access to app port
sudo ufw deny 5000/tcp
```

### Regular Updates

- Monitor for security updates
- Update Docker images regularly
- Keep .NET runtime updated
- Update reverse proxy software

## Monitoring and Maintenance

### Log Management

#### Docker Logs

```bash
# View logs
docker logs acal

# Follow logs
docker logs -f acal

# Last 100 lines
docker logs --tail 100 acal
```

#### Application Logs

Configured in `appsettings.json`:

```json
{
  "LoggingConfig": {
    "LoggingPath": "logs/log.debug",
    "FilteredLoggingPath": "logs/log.information"
  }
}
```

**Log Rotation:**

Linux (logrotate):

```bash
# /etc/logrotate.d/acal
/var/www/acal/logs/*.log {
    daily
    rotate 14
    compress
    delaycompress
    missingok
    notifempty
}
```

### Performance Monitoring

#### Resource Usage

```bash
# Docker stats
docker stats acal

# System monitoring
htop
```

#### Application Health

Monitor:
- Response time
- Calendar refresh success rate
- Error log frequency
- Memory usage trends

### Backup Strategy

**Configuration Backup:**

```bash
# Backup configuration
tar -czf acal-config-$(date +%Y%m%d).tar.gz ~/acal-config/

# Backup images
tar -czf acal-images-$(date +%Y%m%d).tar.gz ~/acal-images/
```

**Automated Backup Script:**

```bash
#!/bin/bash
# /usr/local/bin/backup-acal.sh

BACKUP_DIR="/backups/acal"
DATE=$(date +%Y%m%d)

# Create backup
tar -czf "$BACKUP_DIR/acal-$DATE.tar.gz" \
  ~/acal-config \
  ~/acal-images

# Keep last 7 days
find "$BACKUP_DIR" -name "acal-*.tar.gz" -mtime +7 -delete
```

Add to crontab:
```bash
0 2 * * * /usr/local/bin/backup-acal.sh
```

### Update Procedures

#### Docker Update

```bash
# 1. Backup configuration
tar -czf acal-backup.tar.gz ~/acal-config

# 2. Pull new image
docker pull arizonagreentea0905/acal:latest

# 3. Stop and remove old container
docker stop acal && docker rm acal

# 4. Start new container
docker run -d --name acal --restart unless-stopped \
  -p 5000:8080 \
  -v ~/acal-config:/app/config \
  -v ~/acal-images:/app/images \
  arizonagreentea0905/acal:latest

# 5. Verify
docker logs -f acal
```

#### Traditional Update

```bash
# 1. Backup current deployment
tar -czf acal-backup.tar.gz /var/www/acal

# 2. Build new version
cd ACAL
git pull
dotnet publish -c Release -o ./publish

# 3. Stop service
sudo systemctl stop acal

# 4. Replace files
sudo cp -r ./publish/* /var/www/acal/

# 5. Start service
sudo systemctl start acal

# 6. Verify
sudo systemctl status acal
```

## Troubleshooting Deployment

### Container Won't Start

```bash
# Check logs
docker logs acal

# Verify volume mounts
docker inspect acal | grep -A 10 Mounts

# Test configuration
docker run --rm -v ~/acal-config:/app/config \
  arizonagreentea0905/acal:latest \
  dotnet CalendarView.Web.dll --help
```

### High Memory Usage

- Reduce image directory size
- Increase calendar refresh interval
- Limit concurrent connections
- Check for memory leaks in logs

### Slow Performance

- Enable caching in reverse proxy
- Optimize image sizes
- Reduce calendar count
- Check network latency to calendar sources

### Connection Issues

```bash
# Test connectivity
curl http://localhost:5000

# Check port binding
netstat -tlnp | grep 5000

# Test reverse proxy
curl -I http://your-domain.com
```

## Production Checklist

Before going live:

- [ ] HTTPS configured and tested
- [ ] Authentication enabled with strong password
- [ ] Firewall configured
- [ ] File permissions secured
- [ ] Backups configured
- [ ] Monitoring in place
- [ ] Log rotation configured
- [ ] Update procedure documented
- [ ] Error pages customized
- [ ] Performance tested
- [ ] Recovery procedure tested

## Support

For deployment issues:
- Check [Troubleshooting Guide](../user/troubleshooting.md)
- Review [GitHub Issues](https://github.com/ArizonaGreenTea05/ACAL/issues)
- Consult application logs
- Test in development environment first
