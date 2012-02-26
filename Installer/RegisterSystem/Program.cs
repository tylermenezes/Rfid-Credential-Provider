using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.Runtime.InteropServices;

namespace RegisterSystem
{
    class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);

         [DllImport("kernel32.dll", SetLastError=true)]
        public static extern bool Wow64EnableWow64FsRedirection(ref IntPtr ptr);

        private static bool isBoxed = (!Environment.Is64BitProcess && Environment.Is64BitOperatingSystem);
        public static readonly RegistryKey Software = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, (isBoxed) ? RegistryView.Registry64 : RegistryView.Default).OpenSubKey("SOFTWARE", true);
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Generating Registry Structure...");
                RegistryKey Base = Software.CreateSubKey("Tyler Menezes");
                Base = Base.CreateSubKey("Rfid Login");

                Console.WriteLine("Adding default configuration...");
                Base.SetValue("Port", "COM3");
                Base.SetValue("Message Start", 0x02);
                Base.SetValue("Message End", 0x0d);

                Console.WriteLine("Reversing Entropy...");
                string randomSalt = Hash(((new Random()).Next(int.MaxValue) + DateTime.Now.Ticks).ToString());

                Console.WriteLine("Enumerating the null set...");
                Base = Base.CreateSubKey("Keys");
                Base.SetValue("Salt", randomSalt);

                Console.WriteLine("Doing magical Windows-y things...");
                string provider = (Environment.Is64BitOperatingSystem)? "64" : "32";
                provider = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\CredentialProviders\\" + provider + "\\RfidCredentialProvider.dll";
                string to = Environment.GetEnvironmentVariable("SystemRoot") + "\\System32\\RfidCredentialProvider.dll";

                IntPtr p = new IntPtr();

                if (isBoxed)
                {
                    Wow64DisableWow64FsRedirection(ref p);
                }

                if (File.Exists(to))
                {
                    File.Delete(to);
                }
                File.Copy(provider, to);

                if (isBoxed)
                {
                    Wow64EnableWow64FsRedirection(ref p);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                throw ex;
            }
        }

        private static string Hash(string toHash)
        {
            System.Security.Cryptography.SHA1CryptoServiceProvider x = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(toHash);
            data = x.ComputeHash(data);
            string o = BitConverter.ToString(data).Replace("-", "").ToUpper();
            return o;
        }
    }
}
