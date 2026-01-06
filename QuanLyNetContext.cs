using System.Data.Entity;

public class QuanLyNetContext : DbContext
{
    public QuanLyNetContext() : base("name=QLNET") { }

    public DbSet<MAY> MAYs { get; set; }
    public DbSet<TAIKHOAN> TAIKHOANs { get; set; }
    public DbSet<SUDUNG> SUDUNGs { get; set; }
    public DbSet<DICHVU> DICHVUs { get; set; }
    public DbSet<CHITIETDICHVU> CHITIETDICHVUs { get; set; }
}
