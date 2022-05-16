using System;
using System.IO;
using System.Net;
using System.Text;

namespace SusLangBuildEngine
{
    public static class BuildEngine
    {
        
        public static void BuildSusLangScriptFromFile(string path, string destinationPath) =>
            BuildSusLangScript(File.ReadAllText(path), destinationPath);
        public static void BuildSusLangScript(string script, string destinationPath)
        {
            //Create the .exe Loader file
            using (WebClient wc = new WebClient())
                wc.DownloadFile("http://api.zenonet.de/SusLang/SusLangBuildLoader.exe", destinationPath);
            
            //Open a fileStream to this file
            FileStream stream = File.Open(destinationPath, FileMode.Append);
            
            //Sets the position to a new byte right behind the file
            stream.Position = stream.Length;
            
            //Create a long to store the script index
            long scriptIndex = stream.Length;
            
            //Get the bytes of the script using the ASCII-Encoding
            byte[] buffer = Encoding.ASCII.GetBytes(script);
            
            //Write the script to the Stream
            stream.Write(buffer);
            
            //Get the 8 bytes of the scriptIndex long
            buffer = BitConverter.GetBytes(scriptIndex);
            
            //Write the scriptIndex long to the stream
            stream.Write(buffer);
            
            //Write all changes to the hard drive
            stream.Flush();
            
            //Close the stream
            stream.Close();
        }

    }
}