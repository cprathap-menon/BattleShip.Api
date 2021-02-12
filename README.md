BattleShip State tracker API as the name suggests works as a state tracker for BattleShip game.

This API mainly supports 3 endpoints,

1. Creation of a game board 10x10. 
2. Endpoint to place battleships on the board.
3. Endpoint to hit an a x/y coordinate on the board.
4. Endpoint to clear the existing cached board.

Once the board is created board is persisted in-memory cache and it has an expiry of 60 mins. 

Any subsequent actions on the board is managed by keeping the state in cache and the expiry is pushed to another hour from the point of update.

Testing

API can be fully tested via swagger or postman, currently not secured and allows all anonymous requests.


This application is written in .NET Core 3.1 using Visual Studio (IDE)


##  Requirements

[Install](https://dotnet.microsoft.com/download#/current) the latest .NET Core 3.x SDK

[Install](https://visualstudio.microsoft.com/vs/community/) Visual Studio 2019 Community Edition (Free)

IIS Express


## Packages used

This project uses the following third party nuget packages,

1. Autofac.Extensions.DependencyInjection
2. NUnit
4. NewtonSoft.Json
5. Swashbuckle

## Running in VisualStudio

* Set Startup project: BattleShip.Api

* At solution level (BattleShip.Api in this case) right click and Rebuild Solution, this will restore and build all the dependencies.

## API Documentation

* https://localhost:44300/api/docs/index.html


## Assumptions and trade-offs

* No logging / out of scope
* Security Layer and data layer for this project has not been implemented as security and persistence were out of scope.
* Once a board is created it stays in the memory cache for a set 60 mins.
* You need to create a board before adding a ship to board.
