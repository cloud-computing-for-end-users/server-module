using custom_message_based_implementation.consts;
using custom_message_based_implementation.encoding;
using message_based_communication.model;
using System;
using System.Threading;

namespace server_module
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Booting the server module...");

            checked
            {
                try
                {
                    var serverModuleRegistrationPort = new Port() { ThePort = 5523 };

                    var serverModule = new ServerModule(serverModuleRegistrationPort, new ModuleType() { TypeID = ModuleTypeConst.MODULE_TYPE_SERVER_MODULE }, new CustomEncoder());

                    var server_module_connection_informtion = new ConnectionInformation()
                    {
                        IP = new IP() { TheIP = "127.0.0.1" },
                        Port = new Port() { ThePort = 5522 }
                    };

                    serverModule.Setup(server_module_connection_informtion, serverModuleRegistrationPort, server_module_connection_informtion, new CustomEncoder());

                    Console.WriteLine("The servermodule have started sucessfully");
                    ////test
                    //Thread.Sleep(10000);
                    //serverModule.ForFunTest();
                    //Thread.Sleep(5000);
                    //serverModule.ForFunTest();


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
