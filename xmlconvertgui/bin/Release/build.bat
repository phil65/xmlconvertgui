@echo off 

 Echo .git>exclude.txt
 Echo %1\media>>exclude.txt
 Echo %1\themes>>exclude.txt

 ECHO ----------------------------------------
 Echo Building main skin XBT
 ECHO ----------------------------------------
 START /B /WAIT TexturePacker/TexturePacker.exe -dupecheck -input %1\media -output %1\media\Textures.xbt

 ECHO ----------------------------------------
 Echo Finished building main skin XBT
 if exist %1\themes (
     Echo Building theme skin XBT Files
     ECHO ----------------------------------------
     for /f "tokens=*" %%f in ('dir /b/ad %1\themes') do START /B /WAIT TexturePacker/TexturePacker.exe -dupecheck -input %1\themes\%%f -output %1\media\%%f.xbt
     Echo Finished Building theme skin XBT Files
 )
 del exclude.txt
 pause