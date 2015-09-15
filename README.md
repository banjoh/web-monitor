# Web Monitor
### Console application
A web monitor service that checks connectivity, authenticity and response times of a list of web pages provided in a config json file. The config file is provided as a command line parameter i.e
```
WebMonitor.exe config.json
```

##### Config file
The config JSON file contains an **interval** (in seconds) as the first object to periodically monitor the list of **pages**. The second object is a list of web pages which has a url to download content from and a string pattern to check from the downloaded content. Below is an example config.json file.
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
Web monitor logs are stored in a log.json file. The log contains a dictionary of JSON objects. Each object contains a **url** as the key an array as the value. The array contains a **search pattern**, **page alive**, **pattern matched**, **response time** and a **timestamp**. An example of the JSON file is below.
```
{
	"https://msdn.microsoft.com/en-us": [{
		"Item1": "Microsoft",
		"Item2": true,
		"Item3": true,
		"Item4": 10512,
		"Item5": 63577910265
	},
	{
		"Item1": "Microsoft",
		"Item2": true,
		"Item3": true,
		"Item4": 569,
		"Item5": 63577910270
	}],
	"http://stackoverflow.com/": [{
		"Item1": "Stack Overflow",
		"Item2": true,
		"Item3": true,
		"Item4": 9610,
		"Item5": 63577910264
	},
	{
		"Item1": "Stack Overflow",
		"Item2": true,
		"Item3": true,
		"Item4": 701,
		"Item5": 63577910271
	}]
}'
```

##### Dependencies
* .NET 4.5.2
* Newtonsoft.Json 7.0.1

### Distributed application approach
