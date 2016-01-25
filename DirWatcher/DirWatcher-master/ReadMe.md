## Dirwatcher

Windows service used to send email warnings when files are not picked by integration systems.

## About application

Application is designed to be run as a simple clock job on the server. A config file must be present that defines the folder path, the timeout and a small comment (CVS file).

Example of a line in the config file:

`C:\test\;3600;A simple example;` 
