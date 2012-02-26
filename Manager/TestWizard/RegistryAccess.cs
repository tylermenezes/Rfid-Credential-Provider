using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace TestWizard
{
    class RegistryAccess
    {
        private static bool isBoxed = (IntPtr.Size == 4 && !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432")));
        public static readonly RegistryKey Primary = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, (isBoxed) ? RegistryView.Registry64 : RegistryView.Default).OpenSubKey("SOFTWARE\\Tyler Menezes\\Rfid Login", true);
        private static readonly RegistryKey Keys = Primary.OpenSubKey("Keys", true);

        private string RfidToken;

        public RegistryAccess(string token)
        {
            RfidToken = token;
        }


        public string Username
        {
            get
            {
                return GetDecryptedValue(System.Text.Encoding.ASCII.GetString((byte[])TokenKey.GetValue("Username")));
            }
            set
            {
                TokenKey.SetValue("Username", System.Text.Encoding.ASCII.GetBytes(GetEncryptedValue(value)), RegistryValueKind.Binary);
            }
        }

        public string Password
        {
            get
            {
                return GetDecryptedValue(System.Text.Encoding.ASCII.GetString((byte[])TokenKey.GetValue("Password")));
            }
            set
            {
                TokenKey.SetValue("Password", System.Text.Encoding.ASCII.GetBytes(GetEncryptedValue(value)), RegistryValueKind.Binary);
            }
        }

        public string Domain
        {
            get
            {
                return GetDecryptedValue(System.Text.Encoding.ASCII.GetString((byte[])TokenKey.GetValue("Domain")));
            }
            set
            {
                TokenKey.SetValue("Domain", System.Text.Encoding.ASCII.GetBytes(GetEncryptedValue(value)), RegistryValueKind.Binary);
            }
        }

        private string GetEncryptedValue(string toEncrypt)
        {
            char[] e = toEncrypt.ToCharArray();
            string key = Hash(RfidToken + TokenSalt);
            for (int i = 0; i < toEncrypt.Length; i++)
            {
                e[i] = (char)(e[i] ^ key[i % key.Length]);
                e[i] = (char)(((int)e[i]) + 1);
            }

            return new string(e);
        }

        private string GetDecryptedValue(string toDecrypt)
        {
            char[] e = toDecrypt.ToCharArray();
            string key = Hash(RfidToken + TokenSalt);
            for (int i = 0; i < toDecrypt.Length; i++)
            {
                e[i] = (char)(((int)e[i]) - 1);
                e[i] = (char)(e[i] ^ key[i % key.Length]);
            }

            return new string(e);
        }

        private string TokenSalt
        {
            get
            {
                return (string)TokenKey.GetValue("Salt");
            }
        }

        private RegistryKey TokenKey
        {
            get
            {
                if (Keys.OpenSubKey(Hash(RfidToken + KeySalt)) == null)
                {
                    Keys.CreateSubKey(Hash(RfidToken + KeySalt));
                    Keys.OpenSubKey(Hash(RfidToken + KeySalt), true).SetValue("Salt", Hash(((new Random()).Next(Int32.MaxValue) + DateTime.Now.Ticks).ToString()));
                }
                return Keys.OpenSubKey(Hash(RfidToken + KeySalt), true);
            }
        }

        private static string KeySalt
        {
            get
            {
                return (string)Keys.GetValue("Salt");
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