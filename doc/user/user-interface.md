# User Interface Guide

This guide explains ACAL's user interface, available layouts, and interactive features.

## Table of Contents
- [Overview](#overview)
- [Available Layouts](#available-layouts)
- [Navigation](#navigation)
- [Main Components](#main-components)
- [Display Modes](#display-modes)
- [Customization](#customization)

## Overview

ACAL provides a clean, modern interface designed to display your calendar events, photos, and music controls in an elegant, customizable layout. The interface automatically adapts to different screen sizes and orientations.

## Available Layouts

ACAL offers four main layout options, each optimized for different use cases:

### 1. Agenda with Image and Background

**Layout:** `AgendaWithImageAndBackground`

This layout displays an agenda view of your upcoming events alongside a rotating photo display, with a blurred background image.

**Best for:**
- Digital photo frames
- Kiosk displays
- Family information centers

**Features:**
- List view of upcoming events sorted chronologically
- Side panel with rotating photos
- Background photo with customizable blur
- Color-coded event cards by calendar
- Date and time display

**Configuration:**
```json
{
  "Design": {
    "PageLayout": "AgendaWithImageAndBackground",
    "PictureDirectory": "../images",
    "ChangePictureAfterMinutes": 5
  }
}
```

### 2. Agenda with Background

**Layout:** `AgendaWithBackground`

Similar to the previous layout but without the side photo panel, providing more space for event information.

**Best for:**
- Wall-mounted displays focused on schedule
- Smaller screens
- Minimalist setups

**Features:**
- Full-width agenda view
- Background photo with blur effect
- More space for event details
- Clear event categorization

**Configuration:**
```json
{
  "Design": {
    "PageLayout": "AgendaWithBackground"
  }
}
```

### 3. Calendar with Image and Background

**Layout:** `CalendarWithImageAndBackground`

Traditional month calendar view with photo display.

**Best for:**
- Long-term planning view
- Office displays
- Shared family calendars

**Features:**
- Full month calendar grid
- Event indicators on calendar dates
- Side panel with rotating photos
- Quick overview of event distribution

**Configuration:**
```json
{
  "Design": {
    "PageLayout": "CalendarWithImageAndBackground"
  }
}
```

### 4. Calendar with Background

**Layout:** `CalendarWithBackground`

Month calendar view without the side photo panel.

**Best for:**
- Focused calendar viewing
- Smaller displays
- Month-at-a-glance needs

**Features:**
- Full-width calendar grid
- Clear date visibility
- Event indicators
- More screen space for calendar details

**Configuration:**
```json
{
  "Design": {
    "PageLayout": "CalendarWithBackground"
  }
}
```

## Navigation

### Home Page

The home page automatically displays your configured default layout. You can access it at:
```
http://your-server:5000/
```

### Direct Layout Access

Access specific layouts directly via URL:
- Agenda view: `http://your-server:5000/agenda`
- Calendar view: `http://your-server:5000/calendar`

### Changing Layouts

To switch between layouts, update the `PageLayout` setting in your `appsettings.json` file and restart the application.

## Main Components

### Date and Time Display

**Location:** Top of the interface

Shows current date and time in your configured format. Can be toggled on/off via configuration.

**Configuration:**
```json
{
  "Design": {
    "ShowDate": true,
    "ShowTime": true,
    "LongDateFormat": "dddd, dd. MMMM yyyy",
    "ShortTimeFormat": "HH:mm"
  }
}
```

### Event Cards

**Location:** Main content area (in agenda views)

Event cards display individual calendar events with the following information:
- Event title
- Start and end times
- Event description (if available)
- Calendar color indicator
- All-day event badge (if applicable)

**Visual Indicators:**
- Color bar on the left matches the calendar color
- Transparency can be adjusted via `EventCardDimmingRatio`
- Current/ongoing events may have special highlighting

### Calendar Grid

**Location:** Main content area (in calendar views)

The calendar grid shows:
- Current month and year
- All dates in a traditional grid format
- Event indicators on dates with events
- Today's date highlighting
- Navigation controls for month switching

### Color Legend

**Location:** Typically bottom or side of interface

Shows all configured calendars with their assigned colors and names.

**Configuration:**
```json
{
  "Design": {
    "ShowColorLegend": true
  }
}
```

### Photo Display

**Location:** Side panel (in layouts with image)

Automatically cycles through images from your configured directory.

**Features:**
- Automatic rotation based on configured interval
- Supports JPEG, PNG, and GIF formats
- Scales to fit available space
- Can be positioned left or right (responsive to screen orientation)

### Music Player (Spotify Integration)

**Location:** Bottom of interface (when Spotify is configured)

Controls for Spotify playback:
- Currently playing track information
- Play/pause controls
- Skip track controls
- Volume control
- Album artwork

**Note:** Requires Spotify Premium and proper configuration.

## Display Modes

### Portrait Mode

Optimized for vertical displays (e.g., tablet in portrait orientation).

**Behaviors:**
- Photo panel appears above or below content (configurable)
- Narrower event cards for better readability
- Adjusted spacing for vertical layout

**Configuration:**
```json
{
  "Design": {
    "SwapPictureAndContentInPortrait": false
  }
}
```

### Landscape Mode

Optimized for horizontal displays (e.g., desktop monitors, TV screens).

**Behaviors:**
- Photo panel appears on left or right side
- Wider event cards with more information
- Better use of horizontal space

**Configuration:**
```json
{
  "Design": {
    "SwapPictureAndContentInLandscape": false
  }
}
```

### Responsive Design

The interface automatically adapts to:
- Screen size changes
- Window resizing
- Device orientation changes
- Different resolutions

## Customization

### Color Schemes

Customize the application's color scheme:

```json
{
  "Design": {
    "BackColorName": "#1c1c1c",
    "ForeColorName": "LightGray"
  }
}
```

**Popular Schemes:**

**Dark Mode:**
```json
"BackColorName": "#1c1c1c",
"ForeColorName": "LightGray"
```

**Light Mode:**
```json
"BackColorName": "#ffffff",
"ForeColorName": "#333333"
```

**High Contrast:**
```json
"BackColorName": "#000000",
"ForeColorName": "#ffffff"
```

### Event Card Styling

Adjust event card transparency for different visual effects:

```json
{
  "Design": {
    "EventCardDimmingRatio": 0.3
  }
}
```

**Values:**
- `0.0` - Completely transparent (events blend with background)
- `0.5` - Semi-transparent (balanced visibility)
- `1.0` - Fully opaque (maximum contrast)

### Background Blur

Control background image blur for better text readability:

```json
{
  "Design": {
    "Designs": {
      "AgendaWithBackground": {
        "CustomBackgroundImageBlur": "5px"
      }
    }
  }
}
```

**Recommended Values:**
- `0px` - No blur (sharp background)
- `2px` - Subtle blur (slight effect)
- `5px` - Medium blur (balanced)
- `10px` - Heavy blur (very soft background)

### Scroll Behavior

Enable scrolling for long event lists:

```json
{
  "Design": {
    "ShowScrollBar": true
  }
}
```

**When to Enable:**
- Many daily events
- Small screen sizes
- Detailed event information

**When to Disable:**
- Kiosk displays (auto-fitting is preferred)
- Large screens with few events
- Clean, minimal aesthetic

## Tips for Best Experience

### For Kiosk/Display Mode
- ✅ Use landscape orientation
- ✅ Disable scrollbar for cleaner look
- ✅ Enable authentication to prevent unwanted interaction
- ✅ Use layouts with background for visual appeal
- ✅ Set appropriate photo rotation interval (3-10 minutes)

### For Personal Desktop Use
- ✅ Use agenda view for detailed event information
- ✅ Enable scrollbar for easy navigation
- ✅ Configure calendar refresh for frequently changing schedules
- ✅ Use calendar view for month-at-a-glance planning

### For Wall-Mounted Displays
- ✅ Choose high-contrast color schemes
- ✅ Use larger text/date formats
- ✅ Test visibility from typical viewing distance
- ✅ Consider room lighting when choosing colors
- ✅ Use agenda view with background for best visibility

### For Touch Screens
- ✅ Enable scrollbar for touch scrolling
- ✅ Use layouts without side panels for more touch area
- ✅ Consider portrait orientation for tablet displays
- ✅ Test touch responsiveness with your specific device

## Accessibility Features

While ACAL is primarily designed as a display application, it includes:

- High contrast mode support (via color configuration)
- Readable font sizes
- Clear visual hierarchies
- Color-blind friendly calendar color options
- Keyboard navigation support

For specific accessibility needs, adjust the color scheme and text formatting settings to meet your requirements.

## Browser Compatibility

ACAL works best with modern browsers:

- ✅ Google Chrome/Chromium (recommended)
- ✅ Microsoft Edge
- ✅ Mozilla Firefox
- ✅ Safari
- ⚠️ Internet Explorer (not supported)

For kiosk mode, we recommend using Chrome in kiosk mode:
```bash
chrome --kiosk http://localhost:5000
```
