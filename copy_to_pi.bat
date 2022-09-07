@echo off
pscp -r ./FDR/bin/Release/net6.0/* pi@192.168.1.3:/home/pi/FDR/
pscp -r ./FDR.UI/bin/Release/net6.0/* pi@192.168.1.3:/home/pi/FDR.UI/
pause
