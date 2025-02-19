using System;
using System.IO;

namespace PythonCheckerApp
{
    public class PythonChecker
    {
        public bool IsPythonFromMicrosoftStore()
        {
            string userProfile = Environment.GetEnvironmentVariable("USERPROFILE");
            

            // will check the directory "C:\Users\<username>\AppData\Local\Microsoft\WindowsApps"
            string directoryPath = Path.Combine(userProfile, @"AppData\Local\Microsoft\WindowsApps");

            if (Directory.Exists(directoryPath))
            {
                // obtain sub-folders
                string[] subDirectories = Directory.GetDirectories(directoryPath);

                // verify if a folder start with PythonSoftwareFoundation
                foreach (var subDir in subDirectories)
                {
                    if (Path.GetFileName(subDir).StartsWith("PythonSoftwareFoundation", StringComparison.OrdinalIgnoreCase))
                    {
                        return true; // folder found
                    }
                }
            }

            return false; // folder not found
        }
    }
}