@echo off
build0x42c.exe
cd debug
lettuce.exe ../../bin/dump/0x42c.bin ../../bin/kernel/kernel.lst --connect LEM1802,GenericClock,GenericKeyboard
cd ..