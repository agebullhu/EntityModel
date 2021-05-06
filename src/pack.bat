@echo off

set type="Release"


set /p debug="(Debug)Y/N >"


if "%debug%" == "y" set type="Debug"
if "%debug%" == "Y" set type="Debug"


echo pack to %type%


cd C:\Projects\ZeroTeam\EntityModel\src

dotnet pack Agebull.EntityModel.Core.sln -c:%type% -o:"C:\Projects\nuget"  -p:IncludeSymbols=true -p:SymbolPackageFormat=snupkg --include-source --force 