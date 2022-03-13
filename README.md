# Microservices Architecture and Implementation on .NET 6

## Prepare environment

* Install dotnet core version in file `global.json` - `SourceCode\aspnetrun-microservices\global.json`
* Visual Studio 2022
* Docker Desktop
* Ubuntu on WSL
* Tye [link](https://github.com/dotnet/tye)

## How to run the project

Run command for build project 
Go to folder contain file `docker-compose` or `tye.yaml`

*Run command create network only first time*
```
docker network create -d bridge my-network
```
1. Using docker-compose
```Powershell
docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml up -d
```
2. Using tye
```Powershell
tye run --port 9900
```

## Access To Application

### Using Docker Compose Build

* aspnetrunbasics: http://localhost:8006

* aspnetrunbasicblazor: http://localhost:8007

* webstatus: http://localhost:8008/healthchecks-ui

### Using Tye Build
* aspnetrunbasics: http://localhost:9006

* aspnetrunbasicblazor: http://localhost:9007

* webstatus: http://localhost:9008/healthchecks-ui

## Describe the project

The project implement base on course in [Microservices Architecture and Implementation on .NET 5]([https://link](https://www.udemy.com/course/microservices-architecture-and-implementation-on-dotnet/)) and [Microservices Architecture on .NET with applying CQRS, Clean Architecture and Event-Driven Communication](https://medium.com/aspnetrun/microservices-architecture-on-net-3b4865eea03f) 

> In this article we will show how to build microservices on .NET environments with using ASP.NET Core Web API applications, Docker for containerize and orchestrator, Microservices communications with gRPC and RabbitMQ and using API Gateways with Ocelot API Gateway, and using different databases platforms NoSQL(MongoDB, Redis) and Relational databases(PostgreSQL, SqlServer) and using Dapper, Entity Framework Core for ORM Tools, and using best practices CQRS with Clean Architecture implementation.
