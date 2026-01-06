using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DAL.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<DICHVU> DICHVUs { get; set; }
        public virtual DbSet<MAYTINH> MAYTINHs { get; set; }
        public virtual DbSet<NapTien> NapTiens { get; set; }
        public virtual DbSet<NhanVien> NhanViens { get; set; }
        public virtual DbSet<SuDungDichVu> SuDungDichVus { get; set; }
        public virtual DbSet<SuDungMay> SuDungMays { get; set; }
        public virtual DbSet<TaiKhoanNguoiChoi> TaiKhoanNguoiChois { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DICHVU>()
                .Property(e => e.DonGia)
                .HasPrecision(19, 4);

            modelBuilder.Entity<DICHVU>()
                .HasMany(e => e.SuDungDichVus)
                .WithRequired(e => e.DICHVU)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MAYTINH>()
                .Property(e => e.GiaTheoGio)
                .HasPrecision(19, 4);

            modelBuilder.Entity<MAYTINH>()
                .HasMany(e => e.SuDungMays)
                .WithRequired(e => e.MAYTINH)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NapTien>()
                .Property(e => e.SoTien)
                .HasPrecision(19, 4);

            modelBuilder.Entity<NhanVien>()
                .Property(e => e.DienThoai)
                .IsUnicode(false);

            modelBuilder.Entity<NhanVien>()
                .Property(e => e.Luong)
                .HasPrecision(19, 4);

            modelBuilder.Entity<SuDungDichVu>()
                .Property(e => e.ThanhTien)
                .HasPrecision(19, 4);

            modelBuilder.Entity<SuDungMay>()
                .Property(e => e.TongTien)
                .HasPrecision(19, 4);

            modelBuilder.Entity<SuDungMay>()
                .HasMany(e => e.SuDungDichVus)
                .WithRequired(e => e.SuDungMay)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TaiKhoanNguoiChoi>()
                .Property(e => e.SoDu)
                .HasPrecision(19, 4);

            modelBuilder.Entity<TaiKhoanNguoiChoi>()
                .HasMany(e => e.NapTiens)
                .WithRequired(e => e.TaiKhoanNguoiChoi)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TaiKhoanNguoiChoi>()
                .HasMany(e => e.SuDungMays)
                .WithRequired(e => e.TaiKhoanNguoiChoi)
                .WillCascadeOnDelete(false);
        }
    }
}
