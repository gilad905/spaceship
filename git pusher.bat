@echo off

set msg=%1
IF [%1] == [] GOTO promptInput
GOTO afterPrompt

:promptInput
set /p msg="Enter commit message (without double quotes): "

:afterPrompt
cd "C:\Users\gilad\Documents\Programming\Gaming\Spaceship"
git add *
git commit -m "%msg%"
git push

pause