@echo off
cd debug
build0x42c.exe --little-endian
dtemu.exe ../../bin/dump/0x42c.bin
cd ..