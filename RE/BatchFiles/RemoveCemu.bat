@ECHO off

:: Delete Cemu Directory
ECHO Removing Cemu . . .
FOR %%F IN ("$cemu") DO (
	DEL "%%F" /Q /F
)
FOR /D %%D IN ("$cemu") DO (
	:: Skip mlc01 folder
	IF NOT "%%D"=="$cemu\mlc01" (
		RMDIR "%%D" /Q /S
	)
)

:: Remove Shortcuts
ECHO Removing shortcuts . . .
DEL "$desktop\Cemu.lnk" /Q /F
DEL "$start\Cemu.lnk" /Q /F

:: Remove Registry Key
ECHO Removing registry keys . . .
REG delete "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Cemu" /F

:: Delete Self
PAUSE
DEL "$root\cemu.bat" /Q /F
