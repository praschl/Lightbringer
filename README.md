# Lightbringer
The idea is to create a UI that lets you manage the (Win32)-Services of many Servers in one place.
And since you will probably not be interested in all services, you can select which one you want to see.

# A warning before hand
This is still work in progress and absolutely NO SECURITY features have been implemented yet.
Anyone with access to the website or with http access to the hosted REST apis of the servers can at least start and stop them.

# Wording
*TODO*

# Installation
The application consists of two parts: A web application and a service that hosts a REST api. 

The web application can be installed in IIS in either a new website or a virtual application.
The service should be installed on every machine on which you want to manage the services.

## Firewalls
Both applications take and actively send http requests to each other, so the firewall should let this communication pass.

## Sorry
There is currently no automatic installers.
And also no release packages.

You will need to build and install yourself for now :-/

# Usage
## Basics
Make sure you open the UI with the servername in the URL, do not use "localhost". 
This is required because the url will be passed to the services as a root url for notifications, and if you use "localhost",
the services will try to communicate notifications to localhost - which is themselves, and the web ui will never get a notification.

### localhost exception
You can use localhost in one situation though. When the web application and service runs on the same machine (for experimenting or testing)
localhost is fine.

# Configuration
Once you open the UI, go to "Select Daemons" and for each server where you installed the service, 
add its name (you dont have to pass the full url, if you go with the defaults).

# Problems
## The UI isn't updated when a service starts or stops.
Make sure you opened the UI with the machine name in the url, not with "localhost".
