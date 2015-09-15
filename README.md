# Web Monitor
### Console application
A web monitor service that checks connectivity, authenticity and response times of a list of web pages provided in a config JSON file. The config file is provided as a command line parameter i.e
```
WebMonitor.exe config.json
```

##### Config file
The config JSON file contains an **interval** (in seconds) object to periodically monitor the list of **pages**. The second object, **pages**, is a list of web pages which has a **url** to download content from and a string **pattern** to check from the downloaded content. Below is an example config.json file.
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
Web monitor logs are periodically stored in a log.json file. The log contains a dictionary of JSON objects. Each object contains a **url** as the key and an array as the value. The array contains a **search pattern**, **page alive**, **pattern matched**, **response time** and a **timestamp**. An example of the JSON file is below.
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
}
```

##### Dependencies
* .NET 4.5.2
* Newtonsoft.Json 7.0.1

### Distributed application approach
To monitor the same list of web pages in different locations globally, we would have to use a distributed environment for this purpose. A good candidate would be Hadoop framework

##### Topology and implementation
Each geographical location would host a node (more than 1 maybe for fault tolerance). These nodes would contain an implementation using slightly modified libraries of the web monitor libraries used in the console application. In the implementation, the key-value pairs used by the mappers and reducers would be **<(url, location), [stats]>**. This would mean that the log file stored in the distributed filesystem (HDFS) would contian an extra value, location. The mappers would produce a location of where the node is and the stats collected from the node's location i.e. connect to the web pages and collect stats. The reducers would then aggregate the data from the mappers into a distributed log file.

##### Security concerns
Through HDFS access control for users and "bots" accessing the files, Kerberos for user authentication and SSL for encryption of the network used between the globally distributed nodes, security concerns such as man-in-the-middle attacks and authenticity etc can be mitigated.
