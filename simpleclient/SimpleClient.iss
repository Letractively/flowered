; Copyright (c) 2009 Daniel Schubert
;
; Licensed under the Apache License, Version 2.0 (the "License");
; you may not use this file except in compliance with the License.
; You may obtain a copy of the License at
;
;    http://www.apache.org/licenses/LICENSE-2.0
;
; Unless required by applicable law or agreed to in writing, software
; distributed under the License is distributed on an "AS IS" BASIS,
; WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
; See the License for the specific language governing permissions and
; limitations under the License.

[CustomMessages]
FloweredAppName=Flowered
FloweredAppVersion=1.0
FloweredAppExe=Flowered.UI.SimpleClient.exe
FloweredAppLogPath=Logs
FloweredAppSnapshootPath=Snapshoots

[Setup]
AppID={cm:FloweredAppName}
AppName={cm:FloweredAppName}
AppVerName={cm:FloweredAppName} {cm:FloweredAppVersion}
VersionInfoDescription=Flowered Standalone Application
VersionInfoProductName={cm:FloweredAppName}
DefaultDirName={pf}\{cm:FloweredAppName}
DefaultGroupName={cm:FloweredAppName}
UninstallDisplayIcon={app}\{cm:FloweredAppExe}
OutputDir=.\Target\Installer
OutputBaseFilename=Flowered_Setup
Uninstallable=true
MinVersion=5.01,5.01.2600sp2
Compression=lzma/max
SolidCompression=true
PrivilegesRequired=admin
LicenseFile=LICENSE.txt

[Dirs]
Name: "{app}\{cm:FloweredAppLogPath}"
Name: "{app}\{cm:FloweredAppSnapshootPath}"

[Files]
Source: ".\Target\Bin\*"; Excludes: "*.xml,*.pdb,*.log"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\{cm:FloweredAppName}"; Filename: "{app}\{cm:FloweredAppExe}"; IconIndex: 0
Name: "{group}\{cm:UninstallProgram,{cm:FloweredAppName}}"; Filename: {uninstallexe}
Name: "{group}\Log Files"; Filename: "{app}\{cm:FloweredAppLogPath}"; Flags: foldershortcut
Name: "{group}\Snapshoots"; Filename: "{app}\{cm:FloweredAppSnapshootPath}"; Flags: foldershortcut
Name: "{userdesktop}\{cm:FloweredAppName}"; Filename: "{app}\{cm:FloweredAppExe}"; Tasks: desktopicon
Name: "{userstartup}\{cm:FloweredAppName}"; Filename: "{app}\{cm:FloweredAppExe}"; IconIndex: 0

[Run]
Filename: "{app}\{cm:FloweredAppExe}"; Description: "{cm:LaunchProgram,{cm:FloweredAppName}}"; Flags: postinstall nowait skipifsilent

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"

[_ISTool]
EnableISX=false

[InstallDelete]
Name: {app}; Type: filesandordirs;

[UninstallDelete]
Name: {app}; Type: filesandordirs;
