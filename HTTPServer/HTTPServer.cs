using System;
using System.Net;
using System.Threading.Tasks;

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

            Console.WriteLine("Server started on http://localhost:8000/");

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
            return "<HTML><BODY> Hello world!</BODY></HTML>";
        }

        public void Stop()
        {
            if (listener.IsListening)
                listener.Stop();
        }
    }
}
