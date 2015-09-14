using System;
using System.Net;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using LibMonitor;

namespace LibHTTPServer
{
    public class HTTPServer : IDisposable
    {
        private string LogFile { get; set; }
        private HttpListener listener;

        public HTTPServer(string logFile)
        {
            if (!HttpListener.IsSupported)
            {
                throw new Exception("Windows XP SP2 or Server 2003 and any Windows OS above these is required to use the HttpListener class.");
            }

            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8000/");
            listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;

            Console.WriteLine("Visit http://localhost:8000/ to see latest web page logs");

            LogFile = logFile;
        }

        public void Dispose()
        {
            if (listener.IsListening)
                listener.Stop();
            listener.Close();
        }

        public void Start()
        {
            listener.Start();

            // Listen forever in a different thread
            Task.Run(() =>
            {
                while (true)
                {
                    // Check if the listener is listening
                    if (!listener.IsListening)
                        break;

                    // Listen for requests. This will block
                    HttpListenerContext context = listener.GetContext();

                    HttpListenerRequest request = context.Request;
                    // Obtain a response object.
                    HttpListenerResponse response = context.Response;
                    // Construct a response. 
                    string responseString = HtmlResponse();
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                    // Get a response stream and write the response to it.
                    response.ContentLength64 = buffer.Length;
                    response.OutputStream.Write(buffer, 0, buffer.Length);
                }
            });
        }

        private string HtmlResponse()
        {
            if (!File.Exists(LogFile))
                return "<!DOCTYPE html><html><body><h2>There are no results to show as of now.</h2></body></html>";

            string html = @"<!DOCTYPE html>
                            <html>
                                <head>
                                    <title>Web Page Monitor</title>
                                    <style>
                                        table, th, td {{
                                            border: 1px solid black;
                                            border-collapse: collapse;
                                        }}
                                        th, td {{
                                            padding: 15px;
                                        }}
                                    </style>
                                </head>
                                <body>
                                     <h2>Latest results of web page monitor stats</h2>
                                     <table style='width: 100 % '>
                                        <tr style='font-weight:bold'>
                                            <td> Url </td>
                                            <td> Page is alive </td>
                                            <td> Content valid </td>
                                            <td> Response time </td>
                                            <td> Last checked </td>
                                        </tr>
                                        {0}
                                    </table>
                                </body>
                            </html>";

            return String.Format(html, ParsePageResults());
        }

        private string ParsePageResults()
        {
            var dataStore = Logger.ReadLogs(LogFile);

            string placeHolder = @"<tr>
                        <td>{0}</td>
                        <td>{1}</td>
                        <td>{2}</td>
                        <td>{3}</td>
                        <td>{4}</td>
                    </tr>";

            List<string> rows = new List<string>();
            foreach (var url in dataStore.Keys)
            {
                var p = dataStore[url].OrderByDescending(x => x.Item4).First();

                string time = new DateTime(0, DateTimeKind.Utc).AddSeconds(p.Item4).ToString("MM/dd/yy H:mm:ss");
                rows.Add(String.Format(placeHolder, url, p.Item1, p.Item2, p.Item3, time));
            }
            
            return String.Join("\n", rows.ToArray());
        }

        public void Stop()
        {
            if (listener.IsListening)
                listener.Stop();
        }
    }
}
