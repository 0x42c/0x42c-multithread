; 0x42c kernel.inc
; April 24th, 2012
; Licensed with the MIT license.
; Internal kernel include file for 0x42c

; Constant values
.equ min_proc_stack         8
.equ interrupts_frequency   1 ; 60 / 1 = 60Hz

; Memory Layout
.equ kernel_code            0x0000
.equ kernel_memory          0x1000
    ; kernel_memory breakdown
    .equ proc_table             kernel_memory
        .equ proc_entry_length      2
        .equ max_proccesses         8
        .equ proc_table_length      proc_entry_length * max_proccesses
            ; proccess entry breakdown
            .equ proc_entry_stack       0
            .equ proc_entry_flags       1
                ; process flags breakdown
                .equ proc_flags_suspended       0b0001
                .equ proc_flags_cansuspend      0b0010
                .equ proc_flags_highpriority    0b0100
                .equ proc_flags_asroot          0b1000
                
    .equ lib_table              proc_table + proc_table_length
        .equ lib_entry_length       2
        .equ max_libraries          16
        .equ lib_table_length       lib_entry_length * max_libraries
            ; library table breakdown
            .equ lib_entry_id           0
            .equ lib_entry_address      1
            
    .equ driver_table           lib_table + lib_table_length
        .equ driver_entry_length    4
        .equ max_drivers            32
        .equ driver_table_length    driver_entry_length * max_drivers
            ; driver table breakdown
            .equ driver_entry_int       0 ; Interrupt handler
            .equ driver_entry_message   1
            .equ driver_entry_id        2
            .equ driver_entry_address   3
    
.equ kernel_temp_mem        0x1100
    .equ system_clock           kernel_temp_mem + 1
    .equ current_thread_id      system_clock + 1
    .equ total_loaded_threads   current_thread_id + 1
.equ kernel_temp_mem_end    0x1400
.equ os_bootloader          0x1800
.equ os_interrupt_hook      0x1D00
.equ userspace_memory       0x2000
.equ userspace_memory_end   0xFFFF