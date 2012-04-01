@echo off 

 Echo .git>exclude.txt
 Echo %1\media>>exclude.txt
 Echo %1\themes>>exclude.txt

 if exist %3 rmdir %3 /S /Q
 md %3\media\

 REM ECHO ----------------------------------------
 REM Echo Building main skin XBT
 REM ECHO ----------------------------------------
 REM START /B /WAIT %2 -dupecheck -input %1\media -output %3\media\Textures.xbt

 REM ECHO ----------------------------------------
 REM Echo Finished building main skin XBT
 REM if exist %1\themes (
     REM Echo Building theme skin XBT Files
     REM ECHO ----------------------------------------
     REM for /f "tokens=*" %%f in ('dir /b/ad %1\themes') do START /B /WAIT %2 -dupecheck -input %1\themes\%%f -output %3\media\%%f.xbt
     REM Echo Finished Building theme skin XBT Files
 REM )
 ECHO ----------------------------------------
 Echo Copying other files
 ECHO ----------------------------------------

 for /f "tokens=*" %%c in ('dir /b/ad %1') do xcopy %1\%%c "%3\%%c" /Q /S /I /Y /EXCLUDE:exclude.txt
 for /f "tokens=*" %%c in ('dir /b/a-d %1') do copy %1\%%c "%3\%%c"

 del exclude.txt
 pause