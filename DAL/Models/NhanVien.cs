namespace DAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NhanVien")]
    public partial class NhanVien
    {
        [Key]
        public int MaNhanVien { get; set; }

        [Required]
        [StringLength(50)]
        public string TenNhanVien { get; set; }

        [StringLength(15)]
        public string DienThoai { get; set; }

        [Column(TypeName = "money")]
        public decimal? Luong { get; set; }

        [StringLength(30)]
        public string ChucVu { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayVaoLam { get; set; }
    }
}
