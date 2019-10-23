using System;
using System.Collections.Generic;
using System.Text;
using NetMQ;
using NetMQ.Sockets;
using protocol.Exceptions;
using protocol.server_methods;
using static net_mq_util.NetMqUtil;
using protocol.methods.server_methods;
using net_mq_util;
using protocol.model;

namespace server_module
{
    public class ServerModuleMessageHandler
    {
        public const int SERVER_MODULE_PORT = net_mq_util.NetMqUtil.SERVER_MODULE_PORT;

        private ServerModule serverModule;
        public ServerModuleMessageHandler(ServerModule serverModule)
        {
            this.serverModule = serverModule;
        }




        public void Start()
        {

            //var routerSocket = new RouterSocket();

            //routerSocket.Bind("tcp://0.0.0.0:" + SERVER_MODULE_PORT);
            var responseSocket = new ResponseSocket();
            responseSocket.Bind("tcp://0.0.0.0:" + SERVER_MODULE_PORT);

            while (true)
            {
                //read and handle messages from the queue

                //must not be redirected as all frames will be "pop'ed" off
                PrintWhenDebugging("Server module is ready to start recving messages");

                var message = responseSocket.ReceiveMultipartMessage();


                var clone = NetMqUtil.CloneShallowNetMqMessage(message);
                var targetType = net_mq_decoder.NetMqDecoder.GetTargetType(clone);



                if (TargetType.ServerModule.Equals(targetType)) //handle message for self
                {
                    var result = HandleRegisterServermoduleMethod(message);
                    responseSocket.SendMultipartMessage(result);
                }
                else // route the message
                {
                    RouteMessage(message, targetType);
                }



            }
        }

        private Boolean 

        protected NetMQMessage HandleRegisterServermoduleMethod(NetMQMessage message)
        {
            object result = null;

            //var route1 = message.Pop();
            //var route2 = message.Pop();
            //var customRoute3 = message.Pop();

            var decodedMethod = net_mq_decoder.NetMqDecoder.DecodeServerModuleMethod(message);
            var serverMethod = decodedMethod.Item1;

            if(serverMethod is HelloWorldMethod)
            {
                var method = (HelloWorldMethod)serverMethod;
                this.serverModule.HelloWorld();
                result = ProtocolConstants.RESPONDE_NULL; // will be encoded as the return of a void method call

            }
            else if(serverMethod is RegisterServermoduleMethod)
            {
                var method = (RegisterServermoduleMethod)serverMethod;

                result = this.serverModule.RegisterDatabaseServermodule(method.ConnectionInfo);

            }
            else if(serverMethod is RegisterFileServermoduleMethod)
            {
                var method = (RegisterFileServermoduleMethod)serverMethod;

                result = this.serverModule.RegisterFileServermodule(method.ConnectionInfo);

            }
            else if(serverMethod is RegisterSlaveOwnerServermoduleMethod)
            {
                var method = (RegisterSlaveOwnerServermoduleMethod)serverMethod;

                var servermoduleID = this.serverModule.RegisterSlaveOwnerServermodule(method.ConnectionInfo);
                result = new ServermoduleID() { ID = servermoduleID };
            }

            return net_mq_encoder.NetMqEncoder.GenerateResponse(decodedMethod.Item2.Item1, decodedMethod.Item2.Item2, result);
        }








        protected void RouteMessage(NetMQMessage message, TargetType targetType)
        {
            //var onlyOutputSocket = this.serverModule.GetConnectedSocket(targetType);
            //onlyOutputSocket.SendMultipartMessage(message);
            throw new NotImplementedException();
        }






    }
}
