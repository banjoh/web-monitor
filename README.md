# Web Monitor
A web monitor service that checks connectivity, authenticity and response times of a list of web pages provided in a config json file. The config file is provided as a command line parameter i.e
```
WebMonitor.exe config.json
```

##### Config file
The config JSON file contains an interval (in seconds) as the first object to periodically monitor the list of web pages. The second object is a list of web pages which has a url to download content from and a string pattern to check from the downloaded content. Below is an example config.json file.
```
{
  "interval": 5,
  "pages": [
    {
      "url": "https://msdn.microsoft.com/en-us",
      "pattern": "Microsoft"
    },
    {
      "url": "http://www.engadget.com/",
      "pattern": "Engadget"
    }
  ]
}
```

##### Log file

#### Dependencies
* .NET 4.5.2
* Newtonsoft.Json 7.0.1
