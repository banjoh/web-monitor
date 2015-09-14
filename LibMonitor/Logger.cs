using System;
using System.IO;

namespace LibMonitor
{
    public class Logger: IDisposable
    {
        public Logger(string logFile)
        {
            // Create file if it does not exist
            // Open the file and close it in the destructor/Close operation
        }

        public void LogResult(PageResult result)
        {
            // Write log to file
            Console.WriteLine(result);
        }

        public void Dispose()
        {
            throw new NotImplementedException("Implement me!!");
        }
    }
}
