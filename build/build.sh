#!/bin/sh
if [ -d "../bin" ]; then
    mkdir ../bin
fi
mono orgASM.exe ../src/kernel/base.dasm --output-file ../bin/kernel.bin --listing ../bin/kernel.lst --include ../include --working-directory ../src/kernel
