#r "mscorlib"
#r "System"
#r "System.Linq"

using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Win32;

public static void RegisterDropbox()
{
    Console.WriteLine("This file modifies the registry in Windows to add Dropbox as a service within Office 2013/2016");
    Console.WriteLine("This script is provided as is and no warranty or support of any form is offered");
    Console.WriteLine();
    Console.WriteLine(@"Please enter the complete path your dropbox folder in the following format (e.g. C:\Users\Username\Location):");
    string dropboxFolderPath = Console.ReadLine();

    RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Office\Common\Cloud Storage\2c0ed794-6d21-4c07-9fdb-f076662715ad", true);
    if (registryKey == null)
    {
        // Create the registry key.
        registryKey = Registry.LocalMachine.CreateSubKey(@"Software\Microsoft\Office\Common\Cloud Storage\2c0ed794-6d21-4c07-9fdb-f076662715ad");
    }

    // Set registry key values.
    registryKey.SetValue("DisplayName", "Dropbox", RegistryValueKind.String);
    registryKey.SetValue("Description", "Dropbox is a free service that lets you bring all your photos, docs, and videos anywhere.", RegistryValueKind.String);
    registryKey.SetValue("Url48x48", "http://dl.dropbox.com/u/46565/metro/Dropbox_48x48.png", RegistryValueKind.String);
    registryKey.SetValue("LearnMoreURL", "https://www.dropbox.com/", RegistryValueKind.String);
    registryKey.SetValue("ManageURL", "https://www.dropbox.com/account", RegistryValueKind.String);
    registryKey.SetValue("LocalFolderRoot", dropboxFolderPath, RegistryValueKind.String);

    RegistryKey thumbnailsRegistryKey = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Office\Common\Cloud Storage\2c0ed794-6d21-4c07-9fdb-f076662715ad\Thumbnails", true);
    if (thumbnailsRegistryKey == null)
    {
        thumbnailsRegistryKey = Registry.LocalMachine.CreateSubKey(@"Software\Microsoft\Office\Common\Cloud Storage\2c0ed794-6d21-4c07-9fdb-f076662715ad\Thumbnails");
    }

    thumbnailsRegistryKey.SetValue("Url48x48", "http://dl.dropbox.com/u/46565/metro/Dropbox_48x48.png");
    thumbnailsRegistryKey.SetValue("Url40x40", "http://dl.dropbox.com/u/46565/metro/Dropbox_40x40.png");
    thumbnailsRegistryKey.SetValue("Url32x32", "http://dl.dropbox.com/u/46565/metro/Dropbox_32x32.png");
    thumbnailsRegistryKey.SetValue("Url24x24", "http://dl.dropbox.com/u/46565/metro/Dropbox_24x24.png");
    thumbnailsRegistryKey.SetValue("Url20x20", "http://dl.dropbox.com/u/46565/metro/Dropbox_20x20.png");
    thumbnailsRegistryKey.SetValue("Url16x16", "http://dl.dropbox.com/u/46565/metro/Dropbox_16x16.png");

    registryKey.Close();
    thumbnailsRegistryKey.Close();

    Console.WriteLine("All done!");
}

string[] supportedWindowsVersions = { "6.2", "6.3", "10.0" };

Process windowsVersionProcess = new Process
{
    StartInfo = new ProcessStartInfo
    {
        FileName = "systeminfo",
        UseShellExecute = false,
        RedirectStandardOutput = true,
        CreateNoWindow = true
    }
};

windowsVersionProcess.Start();

string windowsVersionProcessOutput = string.Empty;

windowsVersionProcess.WaitForExit();

windowsVersionProcessOutput = windowsVersionProcess.StandardOutput.ReadToEnd();
var match = Regex.Match(windowsVersionProcessOutput, "\r\nOS Version:.*\r\n");
var osVersion = match.Value;

string detectedVersion = supportedWindowsVersions.SingleOrDefault(wv => osVersion.Contains(wv));

if (detectedVersion != null)
{
    switch (detectedVersion)
    {
        case "6.2":
            Console.WriteLine("Detected Windows 8. Proceeding with Dropbox cloud service provider add.");
            RegisterDropbox();
            break;

        case "6.3":
            Console.WriteLine("Detected Windows 8.1. Proceeding with Dropbox cloud service provider add.");
            RegisterDropbox();
            break;

        case "10.0":
            Console.WriteLine("Detected Windows 10. Proceeding with Dropbox cloud service provider add.");
            RegisterDropbox();
            break;

        default:
            Console.WriteLine("Detected a unsupported version of Windows. Script will not proceed.");
            break;
    }
}
else
{
    Console.WriteLine("Script was unable to determine your version of Windows.");
}