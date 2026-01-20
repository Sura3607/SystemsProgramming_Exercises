using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Security.Principal;

namespace Tutorial_02.Question_05
{
    internal class Q5_main
    {
        static string FontName = "UBUNTUMONO";
        static string FontFile = @"Question_05\UBUNTUMONO.TTF";

        static PrivateFontCollection privateFonts = null;

        static bool IsFontInstalled(string fontName)
        {
            InstalledFontCollection fonts = new InstalledFontCollection();
            foreach (var f in fonts.Families)
                if (f.Name.Equals(fontName, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        static void LoadPrivateFont()
        {
            privateFonts = new PrivateFontCollection();
            Console.WriteLine("Finding font in: " + Path.GetFullPath(FontFile));
            privateFonts.AddFontFile(FontFile);
            Console.WriteLine("[SUCCESS] Private font loaded.\n");
        }

        static void InstallFont()
        {
            string dest = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
                FontFile
            );

            File.Copy(FontFile, dest, true);

            RegistryKey key = Registry.LocalMachine.CreateSubKey(
                @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts"
            );
            key.SetValue(FontName, FontFile);

            Console.WriteLine("[SUCCESS] System font installed.\n");
        }

        //Kiem tra quyen admin
        static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        static string RenderCard()
        {
            Bitmap bmp = new Bitmap(420, 260);
            Graphics g = Graphics.FromImage(bmp);

            g.Clear(Color.White);

            Font baseFont;

            // Load font
            if (privateFonts != null && privateFonts.Families.Length > 0)
                baseFont = new Font(privateFonts.Families[0], 12);
            else
                baseFont = new Font(FontName, 12);

            Font titleFont = new Font(baseFont.FontFamily, 18, FontStyle.Bold);

            // Card
            g.DrawRectangle(Pens.Black, 10, 10, 400, 240);
            g.DrawString("MEMBERSHIP CARD", titleFont, Brushes.Black, 90, 40);
            g.DrawString("Name: Nguyen Van A", baseFont, Brushes.Black, 60, 110);
            g.DrawString("ID: CRM-00123", baseFont, Brushes.Black, 60, 145);
            g.DrawString("Level: GOLD", baseFont, Brushes.Black, 60, 180);


            // Xác định thư mục "Question_05" bên trong thư mục Output
            string outputFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Question_05");

            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            string fullPath = Path.Combine(outputFolder, "membership_card.png");

            // Lưu file
            bmp.Save(fullPath, ImageFormat.Png);

            Console.WriteLine($"\n[SUCCESS] Exported successful");

            Process.Start("explorer.exe", $"/select,\"{fullPath}\"");

            g.Dispose();
            bmp.Dispose();
            return fullPath;
        }

        public static void Run()
        {
            Console.WriteLine("=== MEMBERSHIP CARD ===\n");

            Console.WriteLine("==> Checking required font...");
            if (!IsFontInstalled(FontName))
            {
                Console.WriteLine("[Warning] Font not found.\n");

                Console.WriteLine("==> Checking administrator permission...");
                if (!IsAdministrator())
                {
                    Console.WriteLine("[Warning] Administrator permission NOT granted.\n");
                    Console.WriteLine("==> Using PrivateFontCollection instead...\n");
                    LoadPrivateFont();
                }
                else
                {
                    Console.WriteLine("[SUCCESS] Administrator permission granted\n");
                    Console.WriteLine("==> Installing system font...");
                    InstallFont();
                }
            }
            else
            {
                Console.WriteLine("[SUCCESS] Font already installed.\n");
            }

            Console.WriteLine("==> Rendering membership card...");
            string path = RenderCard();

            Console.WriteLine($"\nCard exported: {path}");
        }
    }
}