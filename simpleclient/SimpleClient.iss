[CustomMessages]
FloweredAppName=Flowered
FloweredAppVersion=1.0
FloweredAppExe=Flowered.UI.SimpleClient.exe

[Setup]
AppName={cm:FloweredAppName}
AppVerName={cm:FloweredAppName} {cm:FloweredAppVersion}
VersionInfoDescription=Flowered Standalone Application
VersionInfoProductName={cm:FloweredAppName}
DefaultDirName={pf}\{cm:FloweredAppName}
DefaultGroupName={cm:FloweredAppName}
UninstallDisplayIcon={app}\{cm:FloweredAppExe}
OutputDir=.\Target\Installer
OutputBaseFilename=Flowered_Setup
AppID={cm:FloweredAppName}
Uninstallable=true
MinVersion=5.01,5.01.2600sp2
Compression=lzma/max
SolidCompression=true

[Files]
Source: ".\Target\Bin\*"; Excludes: "*.xml,*.pdb,*.log"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\{cm:FloweredAppName}"; Filename: "{app}\{cm:FloweredAppExe}"; IconIndex: 0
Name: "{group}\{cm:UninstallProgram,{cm:FloweredAppName}}"; Filename: {uninstallexe}
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

