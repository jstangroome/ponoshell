@echo off
setlocal
powershell.exe -version 2 -command ^& '%~dp0gather.ps1'
exit /b %errorlevel%