using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;



namespace IdleMasterExtended
{
    public class CookieSqlite
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
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset ExpireDate { get; set; }
    }

    public class ChromiumCookie
    {
        public List<CookieSqlite> Cookies { get; set; } = new List<CookieSqlite>();

        private const string QueryChromiumCookie = "SELECT name, encrypted_value, host_key, path, creation_utc, expires_utc, is_secure, is_httponly, has_expires, is_persistent FROM cookies";

        public void Extract(byte[] masterKey)
        {
            string tempFilename = "";
            using (var db = new SQLiteConnection($"Data Source={tempFilename};Version=3;"))
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

                        var cookie = new CookieSqlite
                        {
                            KeyName = key,
                            Host = host,
                            Path = path,
                            EncryptValue = encryptValue,
                            IsSecure = isSecure,
                            IsHTTPOnly = isHTTPOnly,
                            HasExpire = hasExpire,
                            IsPersistent = isPersistent,
                            CreateDate = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero).AddMilliseconds(createDate),
                            ExpireDate = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero).AddMilliseconds(expireDate)
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
            File.Delete(tempFilename);
        }

        public int Len()
        {
            return Cookies.Count;
        }
    }
}