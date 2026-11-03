@echo off
rem Double-click to run the PoiNeo console version
chcp 65001 > nul
cd /d "%~dp0"
dotnet run --project src\PoiNeo.Cli
pause
