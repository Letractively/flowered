@ECHO OFF
@IF %ERRORLEVEL% NEQ 0 PAUSE & EXIT
ECHO %1

.\Tools\NArrange\bin\narrange-console /t .\Source\Flowered.UI.SimpleClient.sln

@IF %ERRORLEVEL% NEQ 0 PAUSE