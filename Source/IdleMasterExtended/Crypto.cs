using System;
using System.Runtime.InteropServices;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace IdleMasterExtended
{
    public static class Crypto
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct DATA_BLOB
        {
            public int cbData;
            public IntPtr pbData;
        }

        [DllImport("crypt32.dll", SetLastError = true)]
        private static extern bool CryptUnprotectData(
            ref DATA_BLOB pDataIn,
            IntPtr szDataDescr,
            IntPtr pOptionalEntropy,
            IntPtr pvReserved,
            IntPtr pPromptStruct,
            int dwFlags,
            ref DATA_BLOB pDataOut);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr LocalFree(IntPtr hMem);

        public static byte[] DecryptWithDPAPI(byte[] ciphertext)
        {
            var inputBlob = new DATA_BLOB
            {
                cbData = ciphertext.Length,
                pbData = Marshal.AllocHGlobal(ciphertext.Length)
            };

            Marshal.Copy(ciphertext, 0, inputBlob.pbData, ciphertext.Length);

            var outputBlob = new DATA_BLOB();

            try
            {
                if (!CryptUnprotectData(ref inputBlob, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 0, ref outputBlob))
                {
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
                }

                byte[] decryptedData = new byte[outputBlob.cbData];
                Marshal.Copy(outputBlob.pbData, decryptedData, 0, outputBlob.cbData);
                return decryptedData;
            }
            finally
            {
                Marshal.FreeHGlobal(inputBlob.pbData);
                if (outputBlob.pbData != IntPtr.Zero)
                {
                    LocalFree(outputBlob.pbData);
                }
            }
        }

        private const int NonceSize = 12;
        private const int MinEncryptedDataSize = NonceSize + 1;

        public static byte[] DecryptWithChromium(byte[] key, byte[] ciphertext)
        {
            if (ciphertext.Length < MinEncryptedDataSize)
            {
                throw new ArgumentException("LENGTH -> ?", nameof(ciphertext));
            }

            byte[] nonce = new byte[NonceSize];
            Array.Copy(ciphertext, 3, nonce, 0, NonceSize);
            byte[] encryptedPassword = new byte[ciphertext.Length - (3 + NonceSize)];
            Array.Copy(ciphertext, 3 + NonceSize, encryptedPassword, 0, encryptedPassword.Length);

            return AESGCMDecrypt(key, nonce, encryptedPassword);
        }

        private static byte[] AESGCMDecrypt(byte[] key, byte[] nonce, byte[] encryptedData)
        {
            if (key.Length != 16 && key.Length != 24 && key.Length != 32)
                throw new ArgumentException("LENGTH -> ?", nameof(key));

            var cipher = CipherUtilities.GetCipher("AES/GCM/NoPadding");
            var parameters = new AeadParameters(new KeyParameter(key), 128, nonce, null);

            cipher.Init(false, parameters);
            byte[] plaintext = new byte[cipher.GetOutputSize(encryptedData.Length)];
            int len = cipher.ProcessBytes(encryptedData, 0, encryptedData.Length, plaintext, 0);
            cipher.DoFinal(plaintext, len);

            return plaintext;
        }
    }
}