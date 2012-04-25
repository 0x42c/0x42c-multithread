; 0x42c kernel.inc
; April 24th, 2012
; Licensed with the MIT license.
; Internal kernel include file for 0x42c

; Memory Layout
.equ kernel_code            0x0000
.equ kernel_memory          0x1000
.equ userspace_memory       0x2000
.equ hardware_memory        0x8000
.equ userspace_memory2      0xA000

; Useful values
.equ hw_videoRAM            0x8000
    .equ hw_video_width     12
    .equ hw_video_height    32
.equ hw_video_border_color  0x8280
.equ hw_keybuffer           0x9000
.equ hw_key_lastpressed     0x9010
