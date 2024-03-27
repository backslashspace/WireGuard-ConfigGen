using System;
using System.IO;
using System.Security.Cryptography;

namespace ConfigGen
{
    internal static class xHash
    {
        internal readonly struct KnownHashes
        {
            internal const String Curve_DLL = "dbe7a98f5f5c5cad194aee5233cb5bec5b0f44a32494241ac2d196b83f071d9f";
            internal const String Enc_DLL = "7a359eb885051da2a478cbdc952416272c8899082b6fad47eee7c4ec27ca9c46";
        }

        internal static Boolean CompareHash(String filePath, String hash)
        {
            String fileHash = ComputeFileHash(ref filePath) ?? throw new FileLoadException($"Unable to calculate hash of file :{filePath}");

            if (fileHash.Equals(hash, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        //

        private static String ComputeFileHash(ref String filePath)
        {
            FileStream fileStream;

            try
            {
                fileStream = File.OpenRead(filePath);
            }
            catch (Exception ex)
            {
                throw new FileLoadException($"Unable to open file :{filePath}\n{ex.Message}");
            }

            Byte[] rawHash;

            SHA256 sha256 = SHA256.Create();
            rawHash = sha256.ComputeHash(fileStream);

            sha256.Clear();
            sha256.Dispose();

            fileStream.Close();
            fileStream.Dispose();

            return BitConverter.ToString(rawHash).Replace("-", String.Empty).ToLower();
        }
    }
}