using System;
using System.IO;
using System.Linq;
using System.Data.SQLite;
using System.Collections.Generic;

namespace IdleMasterExtended
{
    public enum BrowserType
    {
        Chrome,
        MSEdge,
    }

    public static class BrowserTypes
    {
        public static string AsString(this BrowserType browserType)
        {
            switch (browserType)
            {
                case BrowserType.Chrome:
                    return "Chrome";
                case BrowserType.MSEdge:
                    return "MSEdge";
                default:
                    throw new ArgumentOutOfRangeException(nameof(browserType), browserType, null);
            }
        }
    }

    public class StoredCookie
    {
        public string Host { get; set; }
        public string Path { get; set; }
        public string KeyName { get; set; }
        public byte[] EncryptValue { get; set; }
        public string Value { get; set; }
        public bool IsSecure { get; set; }
        public bool IsHTTPOnly { get; set; }
        public bool HasExpire { get; set; }
        public bool IsPersistent { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpireDate { get; set; }
    }

    public class ChromiumCookie
    {
        public static string CookiePath { get; set; }
        public List<StoredCookie> Cookies { get; set; } = new List<StoredCookie>();

        private const string QueryChromiumCookie = "SELECT name, encrypted_value, host_key, path, creation_utc, expires_utc, is_secure, is_httponly, has_expires, is_persistent FROM cookies";

        public void Extract(byte[] masterKey)
        {
            using (var db = new SQLiteConnection($"Data Source={CookiePath};Version=3;"))
            {
                db.Open();
                using (var command = new SQLiteCommand(QueryChromiumCookie, db))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string key = reader.GetString(0);
                        byte[] encryptValue = (byte[])reader[1];
                        string host = reader.GetString(2);
                        string path = reader.GetString(3);
                        long createDate = reader.GetInt64(4);
                        long expireDate = reader.GetInt64(5);
                        bool isSecure = reader.GetInt32(6) != 0;
                        bool isHTTPOnly = reader.GetInt32(7) != 0;
                        bool hasExpire = reader.GetInt32(8) != 0;
                        bool isPersistent = reader.GetInt32(9) != 0;

                        var cookie = new StoredCookie
                        {
                            KeyName = key,
                            Host = host,
                            Path = path,
                            EncryptValue = encryptValue,
                            IsSecure = isSecure,
                            IsHTTPOnly = isHTTPOnly,
                            HasExpire = hasExpire,
                            IsPersistent = isPersistent,
                            CreateDate = new DateTime(createDate),
                            ExpireDate = new DateTime(expireDate),
                        };


                        if (encryptValue.Length > 0)
                        {
                            byte[] value = null;
                            if (masterKey.Length == 0)
                            {
                                value = Crypto.DecryptWithDPAPI(encryptValue);
                            }
                            else
                            {
                                value = Crypto.DecryptWithChromium(masterKey, encryptValue);
                            }
                            cookie.Value = System.Text.Encoding.UTF8.GetString(value);
                        }

                        Cookies.Add(cookie);
                    }
                }
            }

            Cookies = Cookies.OrderByDescending(c => c.CreateDate).ToList();
        }

        public string GetUserDataPath(BrowserType browserType)
        {
            string userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string driveLetter = Path.GetPathRoot(userProfilePath).ToUpper();

            switch (browserType)
            {
                case BrowserType.Chrome:
                    return Path.Combine(driveLetter, "Users", Path.GetFileName(userProfilePath), "AppData", "Local", "Google", "Chrome", "User Data");
                case BrowserType.MSEdge:
                    return Path.Combine(driveLetter, "Users", Path.GetFileName(userProfilePath), "AppData", "Local", "Microsoft", "Edge", "User Data");
                default:
                    throw new ArgumentOutOfRangeException(nameof(browserType), browserType, null);
            }
        }


        public List<string> GetProfilesOf(string userDataDir)
        {
            List<string> profilePaths = new List<string>();

            if (Directory.Exists(userDataDir))
            {
                var profileDirectories = Directory.GetDirectories(userDataDir);

                foreach (var profileDir in profileDirectories)
                {
                    string preferencesFilePath = Path.Combine(profileDir, "Preferences");
                    if (File.Exists(preferencesFilePath))
                    {
                        profilePaths.Add(profileDir);
                    }
                }
            }

            return profilePaths;
        }

        public bool ResolveCookiePath(string profileDir)
        {
            var cookiePath = Path.Combine(profileDir, "Network", "Cookies");
            if (File.Exists(cookiePath))
            {
                CookiePath = cookiePath;
                return true;
            }

            return false;
        }

        public int Len()
        {
            return Cookies.Count;
        }
    }
}