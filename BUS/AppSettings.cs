using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public static class AppSettings
    {
        public static bool DarkMode { get; set; }

        public static bool AutoLogoutEnabled { get; set; }
        public static int AutoLogoutMinutes { get; set; }

        public static bool AutoBackupEnabled { get; set; }
        public static int BackupIntervalHours { get; set; }
        private static readonly string ConfigFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "QLQuanNet");
        private static readonly string ConfigFile = System.IO.Path.Combine(ConfigFolder, "appsettings.ini");

        public static void Save()
        {
            try
            {
                if (!System.IO.Directory.Exists(ConfigFolder)) System.IO.Directory.CreateDirectory(ConfigFolder);
                using (var sw = new System.IO.StreamWriter(ConfigFile, false))
                {
                    sw.WriteLine($"DarkMode={DarkMode}");
                    sw.WriteLine($"AutoLogoutEnabled={AutoLogoutEnabled}");
                    sw.WriteLine($"AutoLogoutMinutes={AutoLogoutMinutes}");
                    sw.WriteLine($"AutoBackupEnabled={AutoBackupEnabled}");
                    sw.WriteLine($"BackupIntervalHours={BackupIntervalHours}");
                }
            }
            catch { }
        }

        public static void Load()
        {
            try
            {
                if (!System.IO.File.Exists(ConfigFile)) return;
                var lines = System.IO.File.ReadAllLines(ConfigFile);
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line) || !line.Contains('=')) continue;
                    var kv = line.Split(new[] { '=' }, 2);
                    var key = kv[0].Trim();
                    var val = kv[1].Trim();
                    switch (key)
                    {
                        case "DarkMode": DarkMode = bool.TryParse(val, out var dm) ? dm : DarkMode; break;
                        case "AutoLogoutEnabled": AutoLogoutEnabled = bool.TryParse(val, out var al) ? al : AutoLogoutEnabled; break;
                        case "AutoLogoutMinutes": AutoLogoutMinutes = int.TryParse(val, out var am) ? am : AutoLogoutMinutes; break;
                        case "AutoBackupEnabled": AutoBackupEnabled = bool.TryParse(val, out var ab) ? ab : AutoBackupEnabled; break;
                        case "BackupIntervalHours": BackupIntervalHours = int.TryParse(val, out var bh) ? bh : BackupIntervalHours; break;
                    }
                }
            }
            catch { }
        }
    }
}
