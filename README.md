#GPX Merge
This is a small utility that I created to solve a problem that I was having with my GPS devices.

### The Problem
I'm a mountain biker and I use a Garmin Edge 500 with a heart rate montior.  The problem is that the Garmin often has very inaccurate GPS data, so I've start also recording my rides using the Strava Android application.  I needed a way to bring the heart rate data from the Garmin into the more accurate Strava recorded file.

### The Solution
I record the ride on both the Garmin Edge 500 and the Strava Android app.  I upload the raw Garmin file to Garmin Connect and export it in GPX format.  I also export the Strava file in GPX format and run this tool which outputs a new GPX file that I manually upload to Strava.

### The Code
The code is right now in a state where I have proven that the this will work conceptually.  But the code sucks.  It's terrible.  I'm not in the least bit proud of it.  Please don't judge me by this code. :-)