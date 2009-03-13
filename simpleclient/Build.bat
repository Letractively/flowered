@ECHO OFF
@IF %ERRORLEVEL% NEQ 0 PAUSE & EXIT
ECHO %1

REM Call main build script
%windir%\Microsoft.NET\Framework\v3.5\msbuild.exe /logger:FileLogger,Microsoft.Build.Engine;verbosity=normal;logfile=.\SimpleClient.log /property:MSBuildCommunityTasksPath=.\ .\SimpleClient.proj

@IF %ERRORLEVEL% NEQ 0 PAUSE