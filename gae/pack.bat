REM Copyright ©2009 Daniel Schubert
REM 
REM Licensed under the Apache License, Version 2.0 (the "License");
REM you may not use this file except in compliance with the License.
REM You may obtain a copy of the License at
REM 
REM    http://www.apache.org/licenses/LICENSE-2.0
REM 
REM Unless required by applicable law or agreed to in writing, software
REM distributed under the License is distributed on an "AS IS" BASIS,
REM WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
REM See the License for the specific language governing permissions and
REM limitations under the License.

@ECHO OFF
@IF %ERRORLEVEL% NEQ 0 PAUSE & EXIT
ECHO %1

java -jar .\tools\yuicompressor-2.4.2.jar --type css -o .\src\static\css\flowered.min.css .\src\static\css\flowered.css
java -jar .\tools\yuicompressor-2.4.2.jar --type css -o .\src\static\css\jquery-ui.min.css .\src\static\css\jquery-ui.css
java -jar .\tools\yuicompressor-2.4.2.jar --type js -o .\src\static\js\flowered.min.js .\src\static\js\flowered.js
java -jar .\tools\yuicompressor-2.4.2.jar --type js -o .\src\static\js\marker.min.js .\src\static\js\marker.js
java -jar .\tools\yuicompressor-2.4.2.jar --type js -o .\src\static\js\toolbar.min.js .\src\static\js\toolbar.js

@IF %ERRORLEVEL% NEQ 0 PAUSE
