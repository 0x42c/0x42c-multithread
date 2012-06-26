#!/bin/sh
mono build0x42c.exe --little-endian
mono debug/Lettuce.exe ../bin/dump/0x42c.bin ../bin/kernel/kernel.lst --connect LEM1802,GenericClock,GenericKeyboard
