using System;
using System.Collections.Generic;
using NetMQ.Sockets;
using protocol.Exceptions;
using protocol.model;
using static net_mq_util.NetMqUtil;
using protocol.server_module_interfaces;

namespace server_module
{
    public class ServerModule : IServerModule
    {

        private static Random RANDOM = new Random();


        //private Dictionary<TargetType, RequestSocket> servermodulesConnectionInfo;
        private Dictionary<int, Tuple<RequestSocket, TargetType>> servermoduleManagementDictionary;
        private HashSet<int> inUseServermoduleID;


        public ServerModule()
        {
            //this.servermodulesConnectionInfo = new Dictionary<TargetType, RequestSocket>();
            this.servermoduleManagementDictionary = new Dictionary<int, Tuple<RequestSocket, TargetType>>();
            inUseServermoduleID = new HashSet<int>();
            //start the server module message handler


        }

        public RequestSocket GetConnectedSocket(int servermoduleID)
        {
            throw new NotImplementedException();
            //if (servermoduleManagementDictionary.ContainsKey(servermoduleID))
            //{
            //    return servermoduleManagementDictionary[servermoduleID];
            //}
            //else
            //{
            //    throw new MethodFailedException();
            //}
        }

        public void HelloWorld()
        {
            Console.WriteLine("Hello world method was called on the serverModule");
        }



        //TODO this must be changed if the system should be able to scale out
        #region register servermodules
        public int RegisterDatabaseServermodule(ConnectionInfo connInfo)
        {
            return RegisterServermodule(TargetType.DatabaseServermodule, connInfo);
        }

        public int RegisterFileServermodule(ConnectionInfo connInfo)
        {
            return RegisterServermodule(TargetType.FileServermodule, connInfo);
        }

        public int RegisterSlaveOwnerServermodule(ConnectionInfo connInfo)
        {
            return RegisterServermodule(TargetType.SlaveOwnerServermodule, connInfo);
        }

        private int RegisterServermodule(TargetType targetType, ConnectionInfo connInfo)
        {
            var moduleId = GenerateUniqueServermoduleID();
            var reqSocket = new RequestSocket("tcp://" + connInfo.Ip.Ip + ":" + connInfo.Port.ThePort);
            this.servermoduleManagementDictionary.Add(moduleId, new Tuple<RequestSocket, TargetType>(reqSocket, TargetType.DatabaseServermodule));
            return moduleId;
        }

        private int GenerateUniqueServermoduleID()
        {
            lock (inUseServermoduleID) //TODO this should be moved to its own process so that there can be several server modules 
            {
                int moduleID = -1;
                do
                {
                    moduleID = RANDOM.Next();
                } while (this.inUseServermoduleID.Contains(moduleID));
                this.inUseServermoduleID.Add(moduleID);
                return moduleID ;
            }
        }

        #endregion
    }
}
