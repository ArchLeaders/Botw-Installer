@ECHO OFF

:: Start BotW
START "BOTW" "$cemu\Cemu.exe" --game "$base\code\U-King.rpx" -f

:: Start DS4Windows if it exists
IF EXIST "$ds4\DS4Windows.exe" (
    START "DS4" "$ds4\DS4Windows.exe"
)
