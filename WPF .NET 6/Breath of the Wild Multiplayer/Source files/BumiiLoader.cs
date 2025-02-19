using Breath_of_the_Wild_Multiplayer.MVVM.Model.DTO;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;

namespace Breath_of_the_Wild_Multiplayer.Source_files
{
    public static class BumiiLoader
    {
        public static Tuple<string, BumiiDTO?> readBumii()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                InitialDirectory = Directory.GetCurrentDirectory(),
                Filter = "Bumii files (*.bumii, *.sbactorpack, *.yml)|*.bumii;*.sbactorpack;*.yml",
            };

            BumiiDTO? bumii = null;
            string path = "";

            if(openFileDialog.ShowDialog() == true)
            {
                string cmdOutput = runCMD(openFileDialog.FileName);

                try
                {
                    bumii = JsonConvert.DeserializeObject<BumiiDTO>(cmdOutput);
                    path = openFileDialog.FileName;
                } 
                catch
                {
                    throw new Exception("Error reading bumii file. Make sure the file is properly formatted.");
                }
            }

            return new Tuple<string, BumiiDTO?>(path, bumii);
        }

        private static string runCMD(string bumiiPath)
        {
            ProcessStartInfo start = new ProcessStartInfo()
            {
                FileName = Directory.GetCurrentDirectory() + "\\Resources\\bumii_IO.exe",
                Arguments = bumiiPath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            var cmd = Process.Start(start);
            string output = cmd.StandardOutput.ReadToEnd();

            cmd.WaitForExit();

            return output;
        }
    }
}
