using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Organic;
using System.IO;

namespace build0x42c
{
    class Program
    {
        static bool generateReports = false;
        static string startupDirectory;
        static bool bigEndian = true;
        static ushort kernelSize, bootloaderSize;

        static List<ListEntry> Kernel;
        static List<ListEntry> Bootloader;

        static void Main(string[] args)
        {
            DateTime startupTime = DateTime.Now;

            // Parse arguments
            foreach (string arg in args)
            {
                if (arg.StartsWith("-"))
                {
                    switch (arg)
                    {
                        case "--debug-mode":
                            Console.ReadKey(true);
                            break;
                        case "--generate-reports":
                        case "-r":
                            generateReports = true;
                            break;
                        case "--little-endian":
                        case "-l":
                            bigEndian = false;
                            break;
                        default:
                            Console.WriteLine("Invalid arguments.");
                            return;
                    }
                }
            }

            Console.WriteLine("0x42c Build Tool     Copyright 0x42c Team 2012");
            startupDirectory = Directory.GetCurrentDirectory();

            // Clean up old build
            if (Directory.Exists("../bin"))
                Directory.Delete("../bin", true);
            Directory.CreateDirectory("../bin");
            Directory.CreateDirectory("../bin/kernel");
            Directory.CreateDirectory("../bin/bootloader");
            Directory.CreateDirectory("../bin/userspace");
            Directory.CreateDirectory("../bin/dump");
            if (generateReports)
            {
                if (Directory.Exists("../reports"))
                    Directory.Delete("../reports", true);
                Directory.CreateDirectory("../reports");
            }

            // Build kernel
            Console.WriteLine("Building 0x42c-kernel...");
            Directory.SetCurrentDirectory("../src/kernel");
            Assembler kernelAssembler = new Assembler();
            kernelAssembler.ForceLongLiterals = true;
            kernelAssembler.IncludePath = "../include";
            using (StreamReader sr = new StreamReader("base.dasm"))
            {
                string code = sr.ReadToEnd();
                Kernel = kernelAssembler.Assemble(code, "base.dasm");
            }
            Directory.SetCurrentDirectory(startupDirectory);
            CreateOutput(Kernel, "../bin/kernel", "kernel");

            // Build bootloader
            Console.WriteLine("Building 0x42c-bootloader...");
            Directory.SetCurrentDirectory("../src/bootloader");
            Assembler bootloaderAssembler = new Assembler();
            kernelAssembler.ForceLongLiterals = true;
            bootloaderAssembler.IncludePath = "../include";
            using (StreamReader sr = new StreamReader("base.dasm"))
            {
                string code = sr.ReadToEnd();
                Bootloader = bootloaderAssembler.Assemble(code, "base.dasm");
            }
            Directory.SetCurrentDirectory(startupDirectory);
            CreateOutput(Bootloader, "../bin/bootloader", "bootloader");

            // Patch all files together
            Console.WriteLine("Creating memory dump...");
            using (Stream dump = File.Open("../bin/dump/0x42c.bin", FileMode.Create))
            {
                dump.Write(new byte[0xFFFF * 2], 0, 0xFFFF * 2);
                dump.Flush();

                // Write kernel to dump
                dump.Seek(0, SeekOrigin.Begin);
                kernelSize = WriteToStream(Kernel, dump);

                // Write bootloader to dump
                dump.Seek(kernelAssembler.Values["os_bootloader"], SeekOrigin.Begin);
                bootloaderSize = WriteToStream(Bootloader, dump);
            }

            Console.WriteLine("Build complete " + (DateTime.Now - startupTime).TotalMilliseconds + "ms");
        }

        private static ushort WriteToStream(List<ListEntry> output, Stream stream)
        {
            ushort size = 0;
            foreach (var entry in Kernel)
            {
                if (entry.Output != null)
                {
                    foreach (ushort value in entry.Output)
                    {
                        byte[] buffer = BitConverter.GetBytes(value);
                        if (bigEndian)
                            Array.Reverse(buffer);
                        stream.Write(buffer, 0, buffer.Length);
                        size++;
                    }
                }
            }
            stream.Flush();
            return size;
        }

        private static void CreateOutput(List<ListEntry> output, string location, string filename)
        {
            // create output files

            // Create listing file
            string listing = Assembler.CreateListing(output);
            using (StreamWriter sw = new StreamWriter(Path.Combine(location, filename + ".lst")))
                sw.Write(listing);
            // Create binary file
            using (Stream s = File.Open(Path.Combine(location, filename + ".bin"), FileMode.Create))
            {
                foreach (var entry in output)
                {
                    if (entry.Output != null)
                    {
                        foreach (ushort value in entry.Output)
                        {
                            byte[] buffer = BitConverter.GetBytes(value);
                            if (bigEndian)
                                Array.Reverse(buffer);
                            s.Write(buffer, 0, buffer.Length);
                        }
                    }
                }
            }
            // Output errors/warnings to console
            foreach (var entry in output)
            {
                if (entry.ErrorCode != ErrorCode.Success)
                {
                    Console.WriteLine("Error " + entry.FileName + " (line " +
                        entry.LineNumber + "): " + ListEntry.GetFriendlyErrorMessage(entry.ErrorCode));
                }
                if (entry.WarningCode != WarningCode.None)
                {
                    Console.WriteLine("Warning " + entry.WarningCode + " (line " + 
                        entry.LineNumber + "): " + ListEntry.GetFriendlyWarningMessage(entry.WarningCode));
                }
            }
        }
    }
}
