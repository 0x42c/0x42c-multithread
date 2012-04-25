; 0x42c kernel.inc
; April 24th, 2012
; Licensed with the MIT license.
; Internal kernel include file for 0x42c

; Memory Layout
.equ kernel_code            0x0000
.equ kernel_memory          0x1000

    .equ proc_table             0x1000
    .equ proc_entry_length      2
    .equ max_proccess           8
    .equ proc_table_length      proc_entry_length * max_processes
    
        .equ proc_entry_stack       0
        .equ proc_entry_flags       1
        
            .equ proc_flags_suspended       0b0001
            .equ proc_flags_cansuspend      0b0010
            .equ proc_flags_highpriority    0b0100
            .equ proc_flags_asroot          0b1000
            
.equ os_bootloader          0x1500
.equ userspace_memory       0x2000
.equ userspace_memory_end   0xFFFF