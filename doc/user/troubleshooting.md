# Troubleshooting and FAQ

This guide helps you resolve common issues and answers frequently asked questions about ACAL.

## Table of Contents
- [Common Issues](#common-issues)
- [Calendar Issues](#calendar-issues)
- [Display Issues](#display-issues)
- [Authentication Issues](#authentication-issues)
- [Spotify Issues](#spotify-issues)
- [Docker Issues](#docker-issues)
- [Frequently Asked Questions](#frequently-asked-questions)

## Common Issues

### Application Won't Start

**Symptoms:** Application crashes on startup or displays error messages.

**Solutions:**

1. **Check Configuration File:**
   - Verify `appsettings.json` is valid JSON
   - Use a JSON validator (e.g., jsonlint.com)
   - Check for missing commas or brackets

2. **Verify Dependencies:**
   - For manual installation: Ensure .NET SDK 10.0+ is installed
   - For Docker: Ensure Docker is running

3. **Check Logs:**
   - Look in the `logs/` directory for error messages
   - Check console output for startup errors

4. **Verify File Permissions:**
   - Ensure the application has read access to config files
   - Ensure write access to logs directory

### Application is Slow or Unresponsive

**Symptoms:** UI is sluggish, events take long to load.

**Solutions:**

1. **Check Calendar Refresh Rate:**
   ```json
   {
     "Calendars": {
       "RefreshAfterMinutes": 60  // Increase if too frequent
     }
   }
   ```

2. **Reduce Number of Calendars:**
   - Each calendar source adds loading time
   - Consider consolidating calendars

3. **Optimize Photo Directory:**
   - Reduce image file sizes
   - Limit number of images in directory
   - Use compressed formats (JPEG instead of PNG)

4. **Adjust Photo Change Interval:**
   ```json
   {
     "Design": {
       "ChangePictureAfterMinutes": 5  // Increase for better performance
     }
   }
   ```

### Blank Screen

**Symptoms:** Application loads but displays nothing.

**Solutions:**

1. **Check Browser Console:**
   - Open browser developer tools (F12)
   - Look for JavaScript errors
   - Check network tab for failed requests

2. **Verify Configuration:**
   - Ensure `PageLayout` is set to a valid value
   - Check that at least one calendar is configured

3. **Clear Browser Cache:**
   - Clear browser cache and cookies
   - Perform a hard refresh (Ctrl+F5)

4. **Check Content:**
   - Verify calendar URLs are accessible
   - Ensure image directory exists (if configured)

## Calendar Issues

### Calendars Not Loading

**Symptoms:** No events appear, calendar section is empty.

**Solutions:**

1. **Verify Calendar URL Format:**
   - URLs must use pipe `|` instead of colon `:`
   - Correct: `"https|//calendar.example.com/calendar.ics"`
   - Incorrect: `"https://calendar.example.com/calendar.ics"`

2. **Test Calendar URL:**
   - Copy the URL (replace `|` with `:`)
   - Open in browser to verify it downloads an ICS file
   - Check if the URL requires authentication

3. **Check Network Access:**
   - Ensure the server can reach calendar URLs
   - Check firewall settings
   - For Docker, ensure container has internet access

4. **Review Logs:**
   - Look for "Failed to load calendar" messages
   - Check for HTTP error codes (401, 403, 404, etc.)

5. **Verify ICS Format:**
   - Ensure the calendar source provides valid ICS format
   - Some calendar services may have specific export requirements

### Events Not Updating

**Symptoms:** Old events still showing, new events not appearing.

**Solutions:**

1. **Check Refresh Interval:**
   ```json
   {
     "Calendars": {
       "RefreshAfterMinutes": 1  // Reduce for testing
     }
   }
   ```

2. **Restart Application:**
   - Calendar data is cached
   - Restart to force immediate refresh

3. **Verify Calendar Source:**
   - Check if calendar was updated at the source
   - Some services have propagation delays

### Duplicate Events

**Symptoms:** Same event appears multiple times.

**Solutions:**

1. **Check for Duplicate Calendar Sources:**
   - Verify you haven't added the same calendar URL twice
   - Check if multiple calendars share events

2. **Review Calendar Definitions:**
   ```json
   {
     "Calendars": {
       "Definitions": {
         // Remove duplicate entries
       }
     }
   }
   ```

### Wrong Time Zone

**Symptoms:** Events display at incorrect times.

**Solutions:**

1. **Check System Time Zone:**
   - For Docker: Container uses UTC by default
   - Set timezone environment variable:
   ```bash
   docker run -e TZ=America/New_York ...
   ```

2. **Verify Calendar Time Zone:**
   - Check if calendar source specifies correct timezone
   - Some ICS files may not include timezone data

## Display Issues

### Images Not Showing

**Symptoms:** Blank space where photos should appear.

**Solutions:**

1. **Verify Directory Path:**
   - For Docker: Ensure volume is mounted correctly
   ```bash
   docker run -v ~/images:/app/images ...
   ```
   - For manual: Check path in configuration
   ```json
   {
     "Design": {
       "PictureDirectory": "../images"
     }
   }
   ```

2. **Check Image Files:**
   - Supported formats: JPEG, PNG, GIF
   - Verify files exist in the directory
   - Check file permissions

3. **Review Logs:**
   - Look for image loading errors
   - Check file access warnings

### Layout Issues

**Symptoms:** Elements overlap, layout appears broken.

**Solutions:**

1. **Clear Browser Cache:**
   - Clear cache and perform hard refresh
   - Try different browser

2. **Check Screen Size:**
   - Some layouts work better on certain screen sizes
   - Try different layout option

3. **Verify CSS Loading:**
   - Check browser console for 404 errors
   - Ensure static files are being served

### Colors Not Applying

**Symptoms:** Custom colors not showing, default colors appear.

**Solutions:**

1. **Verify Color Format:**
   - Use hex format: `"#FF0000"`
   - Or named colors: `"red"`
   - Invalid format will be ignored

2. **Check Configuration Syntax:**
   ```json
   {
     "Design": {
       "BackColorName": "#1c1c1c",  // Note: Hex format
       "ForeColorName": "LightGray"  // Note: Named color
     }
   }
   ```

3. **Clear Browser Cache:**
   - Styles may be cached
   - Hard refresh to reload

## Authentication Issues

### Can't Log In

**Symptoms:** Authentication prompt keeps appearing, credentials rejected.

**Solutions:**

1. **Verify Credentials:**
   - Check username and password in configuration
   - Passwords are case-sensitive
   - No extra spaces in configuration file

2. **Check Configuration:**
   ```json
   {
     "AuthenticationConfig": {
       "Enabled": true,
       "Username": "admin",  // Check spelling
       "Password": "yourpassword"  // Check case
     }
   }
   ```

3. **Clear Browser Credentials:**
   - Browser may have cached wrong credentials
   - Clear saved passwords for the site
   - Try incognito/private browsing mode

### Authentication Not Working

**Symptoms:** No login prompt appears, or authentication is bypassed.

**Solutions:**

1. **Verify Authentication is Enabled:**
   ```json
   {
     "AuthenticationConfig": {
       "Enabled": true  // Must be true
     }
   }
   ```

2. **Restart Application:**
   - Configuration changes require restart
   - For Docker: `docker restart acal`

3. **Check Middleware:**
   - Ensure BasicAuthenticationMiddleware is enabled
   - Review startup logs

## Spotify Issues

### Spotify Not Connecting

**Symptoms:** Music player doesn't appear or shows errors.

**Solutions:**

1. **Verify Spotify Configuration:**
   ```json
   {
     "SpotifyServiceLoginData": {
       "ClientId": "your-client-id",
       "ClientSecret": "your-client-secret"
     }
   }
   ```

2. **Check Spotify Developer Dashboard:**
   - Verify application is active
   - Check redirect URIs are configured
   - Ensure credentials match

3. **Regenerate Tokens:**
   - Use SpotifyTokenHelper to generate fresh tokens
   - Token may have expired

4. **Verify Premium Account:**
   - Spotify integration requires Premium subscription
   - Free accounts will not work

### Spotify Playback Issues

**Symptoms:** Can't control playback, buttons don't work.

**Solutions:**

1. **Check Active Device:**
   - Spotify must have an active playback device
   - Start playback on any device first

2. **Verify Token:**
   - Access token may have expired
   - Refresh token should auto-renew
   - If issues persist, regenerate tokens

3. **Check Network:**
   - Ensure server can reach Spotify API
   - Check firewall settings

## Docker Issues

### Container Won't Start

**Symptoms:** Docker container exits immediately or fails to start.

**Solutions:**

1. **Check Volume Mounts:**
   ```bash
   # Verify directories exist
   ls ~/acal-config
   ls ~/acal-images
   ```

2. **Check Configuration File:**
   ```bash
   # Verify file exists and is readable
   cat ~/acal-config/appsettings.json
   ```

3. **Review Docker Logs:**
   ```bash
   docker logs acal
   ```

4. **Verify Port Availability:**
   ```bash
   # Check if port 5000 is already in use
   netstat -an | grep 5000
   ```

### Can't Access Container

**Symptoms:** Can't reach application at localhost:5000.

**Solutions:**

1. **Verify Container is Running:**
   ```bash
   docker ps | grep acal
   ```

2. **Check Port Mapping:**
   ```bash
   docker port acal
   # Should show: 8080/tcp -> 0.0.0.0:5000
   ```

3. **Try Different Port:**
   ```bash
   docker run -p 8080:8080 ...
   # Then access at localhost:8080
   ```

4. **Check Firewall:**
   - Ensure firewall allows localhost connections
   - Try accessing from same machine first

### Configuration Changes Not Applied

**Symptoms:** Changed configuration but no effect.

**Solutions:**

1. **Restart Container:**
   ```bash
   docker restart acal
   ```

2. **Verify Volume Mount:**
   ```bash
   docker inspect acal | grep -A 10 Mounts
   ```

3. **Check File Location:**
   - Configuration must be in mounted directory
   - File must be named `appsettings.json`

## Frequently Asked Questions

### General Questions

**Q: Do I need Spotify Premium?**
A: Yes, Spotify integration requires a Premium account. The Spotify API does not support playback control for free accounts.

**Q: Can I use multiple displays?**
A: Yes, run multiple instances of ACAL (on different ports or machines) with different configurations.

**Q: Is internet connection required?**
A: Yes, for loading calendar data and Spotify integration. However, ACAL will cache calendar data and continue displaying previously loaded events if the connection is lost.

**Q: What file formats are supported for photos?**
A: JPEG (.jpg, .jpeg), PNG (.png), and GIF (.gif) formats are supported.

**Q: Can I add multiple photo directories?**
A: Currently, only one directory is supported. However, you can organize photos in a single directory with subdirectories (all images will be found recursively).

### Calendar Questions

**Q: How many calendars can I add?**
A: There's no hard limit, but performance may degrade with many calendars. We recommend 5-10 calendars maximum for optimal performance.

**Q: Can I use Google Calendar?**
A: Yes! Export your Google Calendar as an ICS link (found in calendar settings) and add it to the configuration. Remember to replace `://` with `|//`.

**Q: Can I filter which events are shown?**
A: Currently, all events from configured calendars are shown. You can control this at the calendar source level by creating filtered calendar views.

**Q: How far in advance does ACAL show events?**
A: In Agenda view, ACAL typically shows upcoming events for the next few weeks. In Calendar view, it shows the current month.

### Configuration Questions

**Q: Can I change settings without restarting?**
A: No, configuration changes require an application restart to take effect.

**Q: How do I secure my configuration file?**
A: For Docker, use appropriate file permissions (chmod 600) and consider using Docker secrets. For manual deployments, restrict file access to the application user only.

**Q: Can I use environment variables for sensitive data?**
A: This is not currently built-in, but you can modify the application code to support environment variables for sensitive settings.

### Performance Questions

**Q: Why is calendar loading slow?**
A: Calendar loading speed depends on the response time of your calendar sources. Consider using calendar sources with good performance and increasing the refresh interval.

**Q: How much memory does ACAL use?**
A: Typical memory usage is 100-200MB, depending on the number of calendars and images. Docker containers may use additional overhead.

**Q: Can I run ACAL on a Raspberry Pi?**
A: Yes! ACAL can run on Raspberry Pi 3 or newer. Use the ARM Docker images or build from source for ARM architecture.

## Getting Additional Help

If you've tried the solutions above and still have issues:

1. **Check GitHub Issues:**
   - [ACAL GitHub Issues](https://github.com/ArizonaGreenTea05/ACAL/issues)
   - Search for similar issues
   - Review closed issues for solutions

2. **Review Logs:**
   - Check `logs/log.debug` for detailed information
   - Include relevant log excerpts when reporting issues

3. **Create a GitHub Issue:**
   - Provide clear description of the problem
   - Include configuration (remove sensitive data)
   - Share relevant log entries
   - Describe steps to reproduce

4. **Community Support:**
   - Check YouTrack for known issues: [YouTrack Board](https://sugoi.youtrack.cloud/projects/ACAL/agiles/195-1/current)

## Diagnostic Checklist

When troubleshooting, work through this checklist:

- [ ] Configuration file is valid JSON
- [ ] All required fields are present in configuration
- [ ] Calendar URLs use pipe `|` instead of colon `:`
- [ ] Application has internet access
- [ ] Required ports are available
- [ ] File permissions are correct
- [ ] .NET SDK / Docker is up to date
- [ ] Browser is supported and up to date
- [ ] Logs have been reviewed for errors
- [ ] Application has been restarted after configuration changes
