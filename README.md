# Battle Teams mono-internal cheat

### [YouTube video](https://youtu.be/_Lk0pOXE-js)
### [Join the community](https://t.me/precache)

## Screenshot
<img src="https://i.ibb.co/hmK1wFw/Screenshot.png" alt="Screenshot" style="width: 50%">

### Setup:
1. Download the [DLL](https://github.com/ho9/bt-internal/releases/tag/x)
2. Download any mono-injector, for example [SharpMonoInjector](https://github.com/warbler/SharpMonoInjector/releases)
3. Join the game
4. Run cmd with administrator privileges
5. Execute this command `"PATHTOYOUR\smi.exe" inject -p "SSJJ_BattleClient_Unity" -a "PATHTOYOUR\chuj.xyz.dll" -n chuj.xyz -c Loader -m Init`

### How to work with the source code:
1. Install .NET 3.5
2. Open the project file in Visual Studio
3. Right-click on your project in Visual Studio's Solution Explorer
4. Select "Add" > "Reference..."
5. Navigate to `\Blood Strike\battle\1\SSJJ_BattleClient_Unity_Data\Managed` and add all necessary DLL's

<hr>

### Changelog:
- 21.07.2023 - release
- 27.07.2023 - fixed after the new patch and added viewmodel offset
