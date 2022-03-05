@ECHO OFF

:: Remove DS4Windows
RMDIR "$ds4" /S /Q

:: Remove Shortcuts
ECHO Removing shortcuts . . .
DEL "$desktop\DS4Windows.lnk" /Q /F
DEL "$start\DS4Windows.lnk" /Q /F

:: Remove Registry Key
ECHO Removing registry keys . . .
REG delete "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\DS4Windows" /F

:: Remove Self
PAUSE
DEL "$root\ds4windows.bat" /Q /F
