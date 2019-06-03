docker run --rm -it --cpuset-cpus 0-1  -v /Users/iacoware/projects/doubleloop/async-lab:/app/ -w /app/ mcr.microsoft.com/dotnet/core/sdk:2.2 dotnet run 1
