#!/bin/sh
mkdir ../bin
mono orgASM.exe ../src/kernel/base.dcpu --output-file ../bin/kernel.bin --listing ../bin/kernel.lst --include ../src/include --working-directory ../src/kernel