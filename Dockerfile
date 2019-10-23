# used for creating the docker image that will have the server module

# this is a base ubuntu image with .Net Core 2.2 installed
FROM gaticks/bachelor-project:BaseUbuntuNetCore2.2 as base 

# copy application 
COPY /src/server-module/server-module/bin/Debug/netcoreapp2.2/publish/ /data/server-module/

# used for incomming communication
EXPOSE 5522/TCP
# used for incomming registration requests
EXPOSE 5523/TCP 

ENTRYPOINT ["dotnet", "/data/server-module/server-module.dll"]

#CMD ["/bin/bash"]

CMD ["scp:5522", "srp:5523", "isLocal:false"]




########FROM SO-MODULE#####3
# first three arguments are for self setup, last two are for infromation on the router the module connectes to
#currently ip to connect to host is 172.17.0.7
#currently ip to connect to container is 172.29.64.1
