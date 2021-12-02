# Install Breath of the Wild with Botw Installer.

### [Controls Layout](https://github.com/ArchLeaders/Botw-Installer/new/master#controls-layout---basic-tab)  |  [Dumping Botw](https://github.com/ArchLeaders/Botw-Installer/new/master#dumping-botw)  |  [Quick Install](https://github.com/ArchLeaders/Botw-Installer/new/master#quick-install)
#

> \*Disclaimer
>
> This tool does not include, use, or download any part of **The Legend of Zelda: Breath of the Wild**.
>
> To play **The Legend of Zelda: Breath of the Wild** legally you must obtain the game from a legitimate
> retailer and dump the game from your WiiU console.
>
> It is illegal to obtain the game files from anywhere else, even if you own the game.

## Notice

***Because this is only tested by myself, bugs and errors will most likely occur on various other machines until I clean up all of them.***
***So if you can, please report any errors as either a GitHub issue on this repo or a post in my [Discord Channel](https://discord.gg/cbA3AWwfJj)!***
> Ping Marcus *`ArchLeaders#0903`*

# Controls Layout - Basic Tab

![mk_basic](https://user-images.githubusercontent.com/80713508/144360611-79b5287b-ef6c-4d47-907d-ca1779fe0446.png)

### 1. Navigation Tabs. *(Click on or near the text to change tabs.)*
> Basic: Holds the basic install options. *E.g. Path to Cemu installation.*
> 
> Shortcuts: Select which shortcuts to install. *E.g. Desktop for BCML*
> 
> Advanced: Holds the advanced install options. *E.g. Path to Python installation.*

### 2. Browse for Cemu Installation Folder.
> Opens a windows browse dialog for you to navigate to a suitable folder.
 
ℹ | It's recommended that you do not make your Cemu folder in Documents or a subfolder in Documents.

### 3. Change App Theme
> Toggles between light and dark theme.

### 4. Path to Cemu Installation
> This is the folder in which Cemu will be installed.

### 5. Path to Botw Files
> This is where your game files are located. *(read-only)*

### 6. Controller Profiles
> Check any you wish to be installed. If none are checked when installing, the first one (Nintendo Standard Layout) will be used.

### 7. Help
> When clicked you will be re-directed to this page.

### 8. Install Homebrew with Dumpling
> Downloads the latest Homebrew and Dumpling and extracts them to an empty drive (SD card). If no empty drive (SD Card) is found you will be asked to save a `wiiu` folder.
> Note that this will extract files next to the `wiiu` folder as well as inside it.

### 9. Use Mods
> Enables various [advanced]() options so that mods can be installed and used.

<details>
<summary>Advanced Options Enabled:</summary>
  <br>
  &emsp;- Python version 3.8.10 in <code>C:\Python</code> by default.
  <br>
  &emsp;- BCML | Storage directory set to <code>%localappdata%\bcml</code> by default.
</details>

### 10. Basic Shortcuts
> Installs the default set of shortcuts.

<details>
<summary>Defaults:</summary>
  <br>&emsp;Botw | Desktop
  <br>&emsp;Cemu | Desktop
  <br>&emsp;BetterJOy | Start Menu
  <br>&emsp;BCML | Start Menu
  <br>&emsp;BotW | Start Menu
  <br>&emsp;Cemu | Start Menu
  <br>&emsp;DS4Windows | Start Menu
</details>

### 11. DS4Windows
> Installs DS4Windows in `%localappdata%\BotwData\DS4Windows`, the path is hardcoded.

### 12. BetterJoy
> Installs BetterJoy in `%localappdata%\BotwData\BetterJoy`, the path is hardcoded.

### 13. Run After Install
> Launches Botw and if applicable DS4Windows or BetterJoy. *(If both are installed only DS4Windows will run.)*

### 14. Install
> Starts installing Botw. This should take roughly 5 - 10 minutes depending on your PC's CPU speed.

# Controls Layout - Shortcuts Tab

![mk_lnk](https://user-images.githubusercontent.com/80713508/144368069-f5b3d32d-a7d7-4440-91a3-e4f6bbc87fd8.png)

### 1. Desktop
> Installs a shortcut on the desktop for each selected application. *E.g. If BCML is checked, a shortcut for BCML will be created on the desktop.*

### 2. Start Menu
> Adds a shortcut in the start menu for each selected application. *E.g. If BCML is checked, a shortcut for BCML will be added to the start menu.*

# Controls Layout - Advanced

![mk_adv](https://user-images.githubusercontent.com/80713508/144369830-f75771ac-be61-49d4-93f2-2820ff369ecb.png)

### 1. Python Version
> Defines which python version to install.

### 2. Install Python
> Defines whether or not Python will be installed.
>
> ℹ | Even if you are not installing Python, make sure the path to python leads to your existing installation.

### 3. Py Docs
> Defines whether or not the Python Documentation will be installed with python. *Not applicable if `Install Python` is not checked.*

### 4. Path To Python Installation (new/existing)
> This will be the path where the python installation is or will be.

### 5. Install BCML *`B`otw `C`ross-Platform `M`od `L`oader*
> Defines whether or not BCML will be installed. This is required to use mods in Botw.

### 6. Path To BCML Data
> This is where BCML stores the currently installed mods for Botw. *Not applicable if `Install BCML` is not checked*

### 7. Install Cemu
> Defines whether or not Cemu will be installed. Only uncheck if it's already installed and `Path To Cemu` points to it.

### 8. Path to mlc01 Path
> This is where Cemu stores games, updates, DLC and save. It's recommended to set this to a folder on your alternative drive if you have one.

### 9. Copy Base Game
> Copies the base game into Cemu's mlc01 folder. Recommended if your game files are currently on an SD card.

### 10. Install Graphic Packs (GFX)
> Defines whether or not to install the latest community graphic packs.
> Installing this is highly recommended, without it, your Botw performance will be greatly decreased even on powerful systems.

### 11. Path to Base Game `content` Files.
> This is the path to your Base Game content files. If your game files are on your pc and fully intact, this will be automatically set.

### 12. Path to Update `content` Files.
> This is the path to your Update content files. If your game files are on your pc and fully intact, this will be automatically set.

### 13. Path to DLC `content` Files.
> This is the path to your DLC content files. If you have the DLC dumped but this is not set you will have to manually browse for your DLC's `content` folder.
> It will be the `content` folder containing the folders `0010`, `0011`, and `0012`.

### 14. Search and Verify Tool
> When clicked this searches your PC for Botw game files and verifies them. Only required if there are not there and the initial search has been completed.

# Dumping Botw

### Requirments

- SD Card
- External drive to dump the game files to. (This can be your SD card if it's big enough.)
- A WiiU with the Disc or Digital copy of Breath of the Wild.
- [Botw Installer]()
- A bit of patience.

---

### SD Card Setup

1. Put your SD card and any drives you plan to use into your PC. You will need to format them as FAT32, if your SD card or drive is larger than 32GB you will need [this](http://ridgecrop.co.uk/guiformat.exe) tool.

Over 32GB | Open `guiformat.exe` and select the drive/sd card letter from the dropdown menu, then click start. ***Warning! Formating a drive will remove all of it's files forever, so make sure you have everything you wanted off!***

![gui-format](https://user-images.githubusercontent.com/80713508/144481592-31514561-c136-4afc-8684-5898fcbddf81.png)

Under/Equal 32GB | Right-click the drive(s)/sd card you want to use in File Explorer and click the `Format...` option. Set the `File system` to `FAT32` and click start. ***Warning! Formating a drive will remove all of it's files forever, so make sure you have everything you wanted off!***

![format](https://user-images.githubusercontent.com/80713508/144484707-a2e6ee1f-bc3e-4658-917a-a4b206d07b37.png)

2. Remove any external drives from your pc (leave the SD card in).

3. Download and run [Botw Installer]() bypassing the SmartScreen warning and allowing admin permissions.
   Click the Homebrew Install button (SD card icon, [button 8](https://github.com/ArchLeaders/Botw-Installer/new/master#controls-layout---basic-tab)).
   Wait for the prompt saying it's complete, then verify there's a `wiiu` folder in your SD card.
   
![homebrew](https://user-images.githubusercontent.com/80713508/144382832-a7f647eb-09de-4f0d-844a-8b871c20d1bf.png)

### Homebrew / Dumping

1. Eject your SD Card and plug it into your WiiU. Before continuing, make sure that `Auto Power Off` is turned off in your WiiU's settings.
2. From the WiiU Menu launch the `Internet Browser` and go to `wiiuexploit.xyz`. Click on `Run Homebrew Launcher`. *(If it freezes force restart your WiiU and try again.)*
3. In the `Homebrew Launcher` locate `Mocha CFW` and load it. *(If you are sent to the WiiU menu, repeat step four.)*
4. Back in the `Homebrew Launcher load `Dumpling` and click one of the first two options based on your game version. *(Digital/Disk)*
5. If you selected `Digital` locate Botw and press `A` to select it then `+` to start dumping.
6. Follow the on-screen instructions.
7. Wait for the dump to complete. This will take a while.
8. Once complete remove the device you dumped the game onto (thumb drive/SD card) and insert it into your pc.
9. You should now have a copy of Botw on your PC. Congrats! **Don't move the files of your sd card/drive just yet.*
 
    You can now continue to the [install step](https://github.com/ArchLeaders/Botw-Installer/new/master#quick-install---after-dump).
    
# Quick Install - After Dump

### Requirments

- Windows x64 PC
- *Botw installer executable and game files, but presumably you have followed the Dumping Guide and already and have these.)

1. Assuming Botw installer is open, navigate to the `Advanced` tab and click the `Search and Verify` button (Magnifying glass, [button 14](https://github.com/ArchLeaders/Botw-Installer/new/master#controls-layout---advanced-tab))
2. While it searches check the `Copy Base Game` option ([button 9](https://github.com/ArchLeaders/Botw-Installer/new/master#controls-layout---advanced-tab)) and change any other options you would like.
3. Once the search has been complete and you are satisfied with your settings, click `Install`.
4. Wait for it to complete. And that's it. Have fun playing Botw!

***If you encountered any errors or had any issues, please report them! More info [here](https://github.com/ArchLeaders/Botw-Installer/new/master#note)***

# Quick Install

### Requirments

- Windows x64 PC
- Botw Game Files (See [Dumping Guide](https://github.com/ArchLeaders/Botw-Installer/new/master#dumping-botw))
- Botw Installer

1. Run BotwInstaller bypassing the SmartScreen and allowing admin permissions.
2. Change any settings you would like and click install.
3. Wait for it to complete. And that's it. Have fun playing Botw!

***If you encountered any errors or had any issues, please report them! More info [here](https://github.com/ArchLeaders/Botw-Installer/new/master#note)***

## Credits

Author: Marcus S.

## Special Thanks

Torphedo: UI Design and Debugging

Gabendo: UI Feedback
