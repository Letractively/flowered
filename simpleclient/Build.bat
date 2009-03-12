@ECHO OFF
@IF %ERRORLEVEL% NEQ 0 PAUSE & EXIT
ECHO %1

REM Call main build script
%windir%\Microsoft.NET\Framework\v3.5\msbuild.exe /l:FileLogger,Microsoft.Build.Engine;verbosity=normal;logfile=.\simpleclient.log .\simpleclient.proj

@IF %ERRORLEVEL% NEQ 0 PAUSE