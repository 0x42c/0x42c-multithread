@echo off
IF EXIST ..\bin GOTO SKIPBINDIR
mkdir ..\bin
:SKIPBINDIR
orgASM.exe ../src/kernel/base.dasm --output-file ../bin/kernel.bin --listing ../bin/kernel.lst 
--include ../include --working-directory ../src/kernel
