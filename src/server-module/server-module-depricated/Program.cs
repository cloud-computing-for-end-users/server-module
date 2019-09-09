using System;
using System.Collections.Generic;
using System.Text;

namespace server_module
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server module is starting...");

            ServerModule serverModule = new ServerModule();
            ServerModuleMessageHandler responseHandler = new ServerModuleMessageHandler(serverModule);

            responseHandler.Start();
        }
    }
}
