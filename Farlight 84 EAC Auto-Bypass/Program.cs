using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Farlight_84_EAC_Auto_Bypass
{
    internal class Program
    {
        static string SettingsPath = Environment.CurrentDirectory + "\\Settings.json";
        static string FolderPath = "";

        static void Main(string[] args)
        {
            InitConsole();
        }

        static void InitConsole()
        {
            Console.Title = "[STEAM] Farlight 84 EAC Auto-Bypass - written by ArilisDev";

            string jsonContent = File.ReadAllText(SettingsPath);

            // Parse the JSON content
            JObject settingsObject = JObject.Parse(jsonContent);
            string farlightFolderPath = (string)settingsObject["farlightfolder"];
            if (farlightFolderPath.Length > 5)
            {
                FolderPath = farlightFolderPath;
                Console.WriteLine("Launching Farlight . . .");
                FolderPath += "\\EasyAntiCheat";
                ExploitEAC();
            } else
            {
                Console.WriteLine("There was no set file path inside of Settings.json, please put your farlight folder path as the value.\n");
                Thread.Sleep(5000);
            }

            
        }

        static string IndentJson(string json, int spaces)
        {
            string[] lines = json.Split('\n');

            // Exclude the last line (closing brace) from indentation
            for (int i = 1; i < lines.Length - 1; i++)
            {
                lines[i] = new string(' ', spaces) + lines[i];
            }

            return string.Join("\n", lines);
        }

        static void ExploitEAC()
        {
            Console.WriteLine("Performing Settings.Json Exploit . . .");
            string SettingsPath = Path.Combine(FolderPath, "Settings.json");
            if (Directory.Exists(FolderPath))
            {
                if (File.Exists(SettingsPath))
                {
                    string jsonContents = File.ReadAllText(SettingsPath);
                    JObject settingsObject = JObject.Parse(jsonContents);

                    JToken productIdToken = settingsObject.SelectToken("productid");

                    if (productIdToken != null)
                    {
                        string productId = productIdToken.ToString();
                        if (!string.IsNullOrEmpty(productId) && productId.Length > 0)
                        {
                            productId = "1" + productId.Substring(0);

                            settingsObject["productid"] = productId;

                            string modifiedJson = settingsObject.ToString(Newtonsoft.Json.Formatting.Indented);
                            modifiedJson = IndentJson(modifiedJson, 2);

                            File.WriteAllText(SettingsPath, modifiedJson);
                            LaunchGame();
                            Console.Write("Exploit ran, running game . . .");
                            Thread.Sleep(2000);
                            Console.Write("Shutting Down . . .\n");
                            Thread.Sleep(1500);
                            productId = productId.Substring(1);

                            settingsObject["productid"] = productId;

                            string modifiedJson2 = settingsObject.ToString(Newtonsoft.Json.Formatting.Indented);
                            modifiedJson2 = IndentJson(modifiedJson2, 2);

                            File.WriteAllText(SettingsPath, modifiedJson2);
                            Process.GetCurrentProcess().Kill();
                        }
                    }
                }
            }
        }

        static void LaunchGame()
        {
            Process.Start("steam://rungameid/1928420");
        }
    }
}
