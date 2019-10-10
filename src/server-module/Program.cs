using custom_message_based_implementation.consts;
using custom_message_based_implementation.encoding;
using message_based_communication.model;
using System;
using System.Threading;

namespace server_module
{
    class Program
    {
        private const bool IsLocalhost = true;

        static void Main(string[] args)
        {
            Console.WriteLine("Server Module is starting...");

            checked
            {
                try
                {
                    var serverModuleRegistrationPort = new Port() { ThePort = 5523 };

                    var serverModule = new ServerModule(serverModuleRegistrationPort, new ModuleType() { TypeID = ModuleTypeConst.MODULE_TYPE_SERVER_MODULE }, new CustomEncoder());

                    var server_module_connection_information = new ConnectionInformation()
                    {
                        IP = new IP() { TheIP = IsLocalhost ? "127.0.0.1" : "10.152.212.21" },
                        Port = new Port() { ThePort = 5522 } // todo port stuff
                    };

                    serverModule.Setup(server_module_connection_information, serverModuleRegistrationPort, server_module_connection_information, new CustomEncoder());

                    Console.WriteLine("Server Module has started successfully with IP: " + server_module_connection_information.IP.TheIP);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Encountered exception while starting ServerModule: " + ex.Message);
                }
            }
            Console.ReadKey();
        }
    }
}
