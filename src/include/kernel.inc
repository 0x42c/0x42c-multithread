; 0x42c kernel.inc
; April 24th, 2012
; Licensed with the MIT license.
; Internal kernel include file for 0x42c

; Memory Layout
.equ kernel_code            0x0000
.equ kernel_memory          0x1000
.equ userspace_memory       0x2000
.equ userspace_memory_end   0xFFFF

.equ temp_kern_stack        0xFFFF