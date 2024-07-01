@echo off
pscp -r ./FDR/bin/Release/net8.0/* pi@192.168.1.3:/home/pi/FDR/
pscp -r ./FDR.Web/bin/Release/net8.0/* pi@192.168.1.3:/home/pi/FDR.Web/
pause
