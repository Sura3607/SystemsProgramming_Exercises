using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Collections;

namespace Tutorial_04.Question_05
{
    internal class Q5_main
    {
        static readonly string InputFile =
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Question_05", "Data", "input.txt");

        static readonly string EncryptedCompressedFile = "secure.bin";
        static readonly string OutputFile = "output.txt";

        static readonly byte[] Key = Encoding.UTF8.GetBytes("1234567890ABCDEF1234567890ABCDEF");
        static readonly byte[] IV = Encoding.UTF8.GetBytes("ABCDEF1234567890");

        public static void Run()
        {
            Console.WriteLine("=== Secure and Efficient File Storage ===\n");

            PrintFileInfo("Original file", InputFile);

            EncryptAndCompress();

            PrintFileInfo("Encrypted + Compressed file", EncryptedCompressedFile);

            DecompressAndDecrypt();

            PrintFileInfo("Decrypted + Decompressed file", OutputFile);

            VerifyResult();

            Console.WriteLine("\n=== Final Result ===");
            Console.WriteLine($"Input file : {InputFile}");
            Console.WriteLine($"Secure file: {Path.GetFullPath(EncryptedCompressedFile)}");
            Console.WriteLine($"Output file: {Path.GetFullPath(OutputFile)}");
            Console.WriteLine("Status     : SUCCESS");
        }

        static void EncryptAndCompress()
        {
            byte[] plainText = File.ReadAllBytes(InputFile);

            Console.WriteLine($"Read input        : {plainText.Length} bytes");

            using Aes aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            using FileStream fs = new FileStream(EncryptedCompressedFile, FileMode.Create);
            using GZipStream gzip = new GZipStream(fs, CompressionMode.Compress);
            using CryptoStream crypto =
                new CryptoStream(gzip, aes.CreateEncryptor(), CryptoStreamMode.Write);

            crypto.Write(plainText, 0, plainText.Length);

            Console.WriteLine("Encryption done");
            Console.WriteLine($"Saved secure file : {new FileInfo(EncryptedCompressedFile).Length} bytes\n");
        }

        static void DecompressAndDecrypt()
        {
            using Aes aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            using FileStream fs = new FileStream(EncryptedCompressedFile, FileMode.Open);
            using GZipStream gzip = new GZipStream(fs, CompressionMode.Decompress);
            using CryptoStream crypto =
                new CryptoStream(gzip, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using FileStream output = new FileStream(OutputFile, FileMode.Create);

            crypto.CopyTo(output);

            Console.WriteLine("Decryption completed");
            Console.WriteLine($"Recovered output  : {new FileInfo(OutputFile).Length} bytes\n");
        }

        static void PrintFileInfo(string title, string path)
        {
            FileInfo fi = new FileInfo(path);
            string hash = Convert.ToHexString(
                SHA256.HashData(File.ReadAllBytes(path))
            );

            Console.WriteLine($"[{title}]");
            Console.WriteLine($"Name   : {fi.Name}");
            Console.WriteLine($"Size   : {fi.Length} bytes");
            Console.WriteLine($"SHA256 : {hash}\n");
        }

        static void VerifyResult()
        {
            byte[] h1 = SHA256.HashData(File.ReadAllBytes(InputFile));
            byte[] h2 = SHA256.HashData(File.ReadAllBytes(OutputFile));

            Console.WriteLine("=== Verification ===");
            Console.WriteLine(
                StructuralComparisons.StructuralEqualityComparer.Equals(h1, h2)
                ? "SUCCESS: Decrypted file matches original\n"
                : "FAIL: Files are different\n"
            );
        }
    }
}
