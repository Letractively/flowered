@ECHO OFF
@IF %ERRORLEVEL% NEQ 0 PAUSE & EXIT
ECHO %1

REM Call main build script
%windir%\Microsoft.NET\Framework\v3.5\msbuild.exe .\simpleclient.proj /t:Clean

@IF %ERRORLEVEL% NEQ 0 PAUSE