using custom_message_based_implementation.consts;
using custom_message_based_implementation.encoding;
using message_based_communication.model;
using System;
using System.Threading;
using NLog;

namespace server_module
{
    class Program
    {
        private static bool IsLocalhost = true;
        private const string SELF_IP = "sip";
        private const string SELF_COMM_PORT = "scp";
        private const string SELF_REG_PORT = "srp";
        private const string IS_LOCALHOST = "isLocal";


        static void Main(string[] args)
        {
            Console.WriteLine("Server Module is starting...");

            checked
            {
                try
                {
                    SetupNLog();
                    //ServerModule must have itself as the router
                    var portToListenForRegistration = new Port() { ThePort = 5523 };


                    var self_conn_info = new ConnectionInformation()
                    {
                        IP = new IP() { TheIP = null }, // is set after the reading of the system args because isLocalhost might change
                        Port = new Port() { ThePort = 5522 } // todo port stuff
                    };


                    //setting network infromation with sys args
                    foreach (var arg in args)
                    {
                        var split = arg.Split(":");
                        if (2 != split.Length)
                        {
                            throw new ArgumentException("Got badly formatted system arguments");
                        }

                        if (split[0].Equals(SELF_IP)) // set self ip
                        {
                            self_conn_info.IP.TheIP = split[1];
                            Console.WriteLine("Overriding self ip with: " + split[1]);
                        }
                        else if (split[0].Equals(SELF_COMM_PORT)) // set self communication port
                        {
                            self_conn_info.Port.ThePort = Convert.ToInt32(split[1]);
                            Console.WriteLine("Overriding self communication port with: " + split[1]);
                        }
                        else if (split[0].Equals(SELF_REG_PORT)) // set self registration port
                        {
                            portToListenForRegistration.ThePort = Convert.ToInt32(split[1]);
                            Console.WriteLine("Overriding register to self port with: " + split[1]);
                        }
                        else if (split[0].Equals(IS_LOCALHOST))
                        {
                            if (split[1].Equals("true", StringComparison.InvariantCultureIgnoreCase))
                            {
                                IsLocalhost = true;
                                Console.WriteLine("Overriding is localhost with:" + IsLocalhost);
                            }
                            else if (split[1].Equals("false", StringComparison.InvariantCultureIgnoreCase))
                            {
                                IsLocalhost = false;
                                Console.WriteLine("Overriding is localhost with:" + IsLocalhost);
                            }
                            else
                            {
                                Console.WriteLine("Exception in overriding localhost");
                                throw new Exception("ERROR IN SYSTEM ARGUMENTS");
                            }
                        }
                    }

                    if (null == self_conn_info.IP.TheIP)
                    {
                        self_conn_info.IP.TheIP = IsLocalhost ? "127.0.0.1" : ServerModule.GetIP();
                    }

                    var serverModule = new ServerModule(portToListenForRegistration, new ModuleType() { TypeID = ModuleTypeConst.MODULE_TYPE_SERVER_MODULE }, new CustomEncoder());

                    serverModule.Setup(self_conn_info, portToListenForRegistration, self_conn_info, new CustomEncoder());

                    Console.WriteLine("Server Module has started successfully with IP: " + self_conn_info.IP.TheIP +
                                      ", Communication port: " + self_conn_info.Port.ThePort
                                      +", Registration port: " + portToListenForRegistration.ThePort
                                      );

                    Console.WriteLine("Putting main thread to sleep in a loop"); // used because Console.REadKey is not supported when running docker
                    while (true)
                    {
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Encountered exception while starting ServerModule: " + ex.Message);
                }
            }
        }

        private static void SetupNLog()
        {
            var config = new NLog.Config.LoggingConfiguration();
            var logFile = "server-module-log.txt";

            /*
            var rootFolder = System.AppDomain.CurrentDomain.BaseDirectory;
            if (File.Exists(Path.Combine(rootFolder, logFile)))
            {  
                File.Delete(Path.Combine(rootFolder, logFile));
            }
            */

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = logFile };

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            // Apply config           
            LogManager.Configuration = config;
        }
    }
}
