echo off

set msg=%1
IF [%1] == [] GOTO promptInput
GOTO afterPrompt

:promptInput
set /p msg="Enter commit message: "

:afterPrompt
git add *
git commit -m "%msg%"
git push

pause