# Full installer for The Legend of Zelda: Breath of the Wild.
\**Does not use, download, or include any of the ToZ: BotW game files owned by Nintendo®\**

### ***Note: Because this is only tested by myself, bugs and errors will most likely occur on various other machines until I clean up all of them.***
### ***So if you can, please report any errors as either a GitHub issue on this repo or a post in my [Discord Channel](https://discord.gg/cbA3AWwfJj)!***
(Ping Marcus _`ArchLeaders#0903`_)

---

## Setup

Download the latest release and save it anywhere on your PC. (MS Edge thinks it has a virus because it's a `.exe` file. Don't worry, it's safe.)

Windows may also think it's a dangerous file due to it being unsigned.

![img1](https://user-images.githubusercontent.com/80713508/141951310-c3408d23-dd57-4e39-b3e8-0bf2a9720061.png)

To allow the installer to run, click `More Info` then `Run Anyway`

## Getting started

If you just want to play Botw (maybe with mods too), [this guide](guide) will get you there.

If you want to go further and make a custom install to your machine, short explanations of each button are written under **Usage**.

---

## Basic Tab Usage

![basic_tab_img](https://user-images.githubusercontent.com/80713508/141963270-85cecf5a-9226-46f3-af50-3e30cb6c3b93.png)

### Use Mods:
> Installs python (3.7 by default), Edge WebView 2 Runtime, and BCML. This must be checked (green) if you want to play Botw with mods.

### Create Shortcuts
> Fairly self-explanatory, this creates Desktop, Start, and Program shortcuts (uninstallers) for Cemu, BCML, and Botw. The shortcuts created are defined in the advanced tab.

![img2](https://user-images.githubusercontent.com/80713508/141955758-1541ccb4-7582-42e6-a84e-22b368274d5f.png)
![e_img3](https://user-images.githubusercontent.com/80713508/141954939-285a6534-8bcb-41d0-84a4-3a8bff6c58a5.png)

### Run After Install
> Runs Botw after installation.

### None; DS4Windows; BetterJoy
> These applications are required to connect unsupported controllers (DualShock4, Switch joycons, etc) to Windows to then use in Cemu.
To find out if you need one, see [this list](list).

### Cemu Path
> This is the path to your Cemu folder. (Folder containing Cemu.exe.) This defaults to `C:\Users\%username%\Games\Cemu`.
It can be changed of course, but it's recommended you don't point it to a folder inside Documents.

### Dump Path
> This is the path to your game files. (Files dumped from your WiiU.) If you are unsure where these are, you can try the search feature, it will look for your game files, but it takes a while and can be fooled. It's recommended to set your Dump Path manually.

---

## Advanced Tab Usage

![adv_tab_img](https://user-images.githubusercontent.com/80713508/141963223-89851ed4-cdb7-4944-b5af-a6d1ddea59c8.png)

### Install Python
> Installs Python. Can be unchecked if you already have one of the valid versions installed. (3.7 to 3.8)

### 3.8 (x64)
> Installs python 3.8.10 64 Bit if Install Python is checked.

### 3.7 _x64)
> Installs python 3.7.8 64 Bit if Install Python is checked.

### Visual C++
> Installs Visual C++ Redistributable 2015 - 2019 `14.29.30135.0`. Can be unchecked if it's already installed.

### Py Docs
> Installs the python documentation.

### Install BCML
> Installs BCML (`pip install bcml`, writes settings.json)

### Base Game Path
> Autofilled from the basic tab. Can be changed if your game files are not in the standard layout.

### Update Path
> Autofilled from the basic tab. Can be changed if your game files are not in the standard layout.

### DLC Path
> Autofilled from the basic tab. Can be changed if your game files are not in the standard layout.

### Python Path
> Installation path for Python. Only applicable if Install Python is checked.

### mlc01 Path
> Path to mlc01 for Cemu. Leave empty to have it in the Cemu root directory.

### BCML Data Path
> Path to BCML data. (Storage for BCML mods, cache, etc.)

## What's installed/downloaded
```
Cemu
DS4Windows
BetterJoy
Visual C++ 2015 - 2019
.NET 5.0 Runtime
VGEB Driver
BCML
Python 3.7.8
Python 3.8.10
Edge WebView 2 Runtime
And some others probably...
```
