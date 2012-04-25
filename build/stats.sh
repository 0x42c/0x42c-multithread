#!/bin/sh
if [ -d "../bin" ]; then
    mkdir ../bin
fi
mono orgASM.exe ../src/kernel/base.dcpu --output-file ../bin/kernel.bin --listing ../bin/kernel.lst --include ../include --working-directory ../src/kernel --equate ENABLE_STATS 1