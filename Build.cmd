@echo off
powershell -ExecutionPolicy ByPass -NoProfile -command "& """%~dp0eng\common\Build.ps1""" -restore -build -pack -warnAsError 0 %*"
exit /b %ErrorLevel%