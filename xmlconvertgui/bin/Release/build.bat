@echo off 

 Echo .git>exclude.txt
 Echo %1\media>>exclude.txt
 Echo %1\themes>>exclude.txt

 if exist %3 rmdir %3 /S /Q
 md %3\media\

 ECHO ----------------------------------------
 Echo Building main skin XBT
 ECHO ----------------------------------------
 START /B /WAIT %2 -dupecheck -input %1\media -output %3\media\Textures.xbt

 ECHO ----------------------------------------
 Echo Finished building main skin XBT
 if exist %1\themes (
     Echo Building theme skin XBT Files
     ECHO ----------------------------------------
     for /f "tokens=*" %%f in ('dir /b/ad %1\themes') do START /B /WAIT %2 -dupecheck -input %1\themes\%%f -output %3\media\%%f.xbt
     Echo Finished Building theme skin XBT Files
 )
 ECHO ----------------------------------------
 Echo Copying other files
 ECHO ----------------------------------------

 for /f "tokens=*" %%c in ('dir /b/ad %1') do xcopy %1\%%c "%3\%%c" /Q /S /I /Y /EXCLUDE:exclude.txt
 for /f "tokens=*" %%c in ('dir /b/a-d %1') do copy %1\%%c "%3\%%c"

 del exclude.txt
 pause