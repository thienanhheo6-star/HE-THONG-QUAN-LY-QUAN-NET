namespace DAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NapTien")]
    public partial class NapTien
    {
        [Key]
        public int MaNap { get; set; }

        public int MaTK { get; set; }

        [Column(TypeName = "money")]
        public decimal SoTien { get; set; }

        [Column("NgayNap")]
        public DateTime ThoiGian { get; set; }

        public virtual TaiKhoanNguoiChoi TaiKhoanNguoiChoi { get; set; }
    }
}
