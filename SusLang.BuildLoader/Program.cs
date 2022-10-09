using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace SusLang.BuildLoader
{
    internal static class Program
    {
        static void Main()
        {
            //Prepare SusLang interpreter
            Compiler.Logging.Stream = Console.Out;
            
            //Get the current assembly
            Assembly asm = Assembly.GetAssembly(typeof(Program));
            
            if (asm is null)
            {
                Console.WriteLine("Unable to start BuildLoader: Invalid Assembly");
                Environment.Exit(1);
            }

            //Get the path to the current assembly and get a fileStream to it 
            string asmPath = Process.GetCurrentProcess().MainModule!.FileName;
            FileStream selfReference = File.OpenRead(asmPath!);
            
            //Prepare the FileStream to read the last 8 bytes (a long representing the start of the SusLang-Script)
            selfReference.Position = selfReference.Length - 8;
            byte[] buffer = new byte[8];

            //Read the script index
            selfReference.Read(buffer);
            long scriptIndex = BitConverter.ToInt64(buffer);

            //Get the length of the script
            long length = selfReference.Length - scriptIndex - 8; // -8 because we don't want to read the index long
            buffer = new byte[length];
            
            //Read the script itself
            selfReference.Position = scriptIndex;
            selfReference.Read(buffer);

            //Convert the bytes to a string with ASCII-encoding
            string script = Encoding.ASCII.GetString(buffer);
            
            //Execute the script
            Compiler.Execute(script);
            
            Console.WriteLine("\n\nProgram finished. Press any key to exit");
            Console.Read();
        }
    }
}