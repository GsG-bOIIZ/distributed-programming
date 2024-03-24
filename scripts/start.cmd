@echo off

cd "C:\nats-server"
start "" "nats-server.exe"

cd "C:\Users\danil\source\repos\RP\distributed-programming\RankCalculator"
start "" dotnet run

cd "C:\Users\danil\source\repos\RP\distributed-programming\Valuator"

start dotnet run --urls "http://0.0.0.0:5001"
start dotnet run --urls "http://0.0.0.0:5002"

cd "C:\nginx-1.25.4"
start "" "nginx.exe"


