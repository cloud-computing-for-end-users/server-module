using message_based_communication.model;
using message_based_communication.module;
using custom_message_based_implementation.interfaces;
using System;
using custom_message_based_implementation.proxy;
using System.Collections.Generic;
using custom_message_based_implementation.model;
using message_based_communication.encoding;

namespace server_module
{
    public class ServerModule : BaseRouterModule, IServerModule
    {
        public ServerModule(Port portForRegistrationToRouter, ModuleType moduleType, Encoding customEncoding) : base(portForRegistrationToRouter, moduleType, customEncoding)
        {
        }

        public override string CALL_ID_PREFIX => "SERVER_MODULE_CALLING_";

        protected override string MODULE_ID_PREFIXES => "SERVER_MODULE_GIVEN_ID_";

        public override void HandleRequest(BaseRequest message)
        {
            throw new NotImplementedException("Does not handle any requests yet");
        }


        public void ForFunTest()
        {
            Console.WriteLine("Trying to get list of running applications from SO");
            new SlaveOwnerServermoduleProxy(proxyHelper,this).GetListOfRunningApplications(HandleForFunResponse);
        }

        private void HandleForFunResponse(List<ApplicationInfo> list)
        {
            Console.WriteLine("Applications:");

            foreach (var item in list)
            {
                Console.WriteLine("Application name: " + item.ApplicationName);
            }
        }

    }
}
