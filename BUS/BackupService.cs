using System;
using System.IO;
using System.Text;
using DAL.Models;

namespace QLQuanNet
{
    public class BackupService
    {
        public static void CreateBackup(string backupPath)
        {
            try
            {
                // 1. Nếu không truyền gì → dùng Documents
                if (string.IsNullOrWhiteSpace(backupPath))
                {
                    backupPath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                        "QLQuanNet_Backup"
                    );
                }

                // 2. ĐẢM BẢO backupPath LÀ THƯ MỤC
                if (!Directory.Exists(backupPath))
                {
                    Directory.CreateDirectory(backupPath);
                }

                // 3. TẠO FILE .SQL RIÊNG (CỰC KỲ QUAN TRỌNG)
                string backupFile = Path.Combine(
                    backupPath,
                    $"QLQuanNet_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.sql"
                );

                // 4. GHI FILE
                using (var db = new Model1())
                using (var writer = new StreamWriter(backupFile, false, Encoding.UTF8))
                {
                    writer.WriteLine("-- QLQuanNet BACKUP");
                    writer.WriteLine("-- Created: " + DateTime.Now);
                    writer.WriteLine("SET DATEFORMAT dmy;");
                    writer.WriteLine();

                    // ===== TAI KHOAN =====
                    foreach (var acc in db.TaiKhoanNguoiChois)
                    {
                        writer.WriteLine(
                            $"INSERT INTO TaiKhoanNguoiChoi VALUES ({acc.MaTK},'{acc.TenDangNhap}','{acc.MatKhau}',{acc.SoDu});"
                        );
                    }

                    // ===== MAY =====
                    foreach (var m in db.MAYTINHs)
                    {
                        writer.WriteLine(
                            $"INSERT INTO MAYTINH VALUES ({m.MaMay},'{m.TenMay}','{m.TrangThai}');"
                        );
                    }

                    // ===== DICH VU =====
                    foreach (var s in db.DICHVUs)
                    {
                        writer.WriteLine(
                            $"INSERT INTO DICHVU VALUES ({s.MaDV},'{s.TenDV}',{s.DonGia});"
                        );
                    }
                }

                System.Windows.Forms.MessageBox.Show(
                    "Sao lưu thành công!\n" + backupPath,
                    "Backup",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Lỗi sao lưu:\n" + ex.Message,
                    "Backup lỗi",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error
                );
            }
        }
    }
}
