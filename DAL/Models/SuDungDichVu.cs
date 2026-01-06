namespace DAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SuDungDichVu")]
    public partial class SuDungDichVu
    {
        [Key]
        public int MaSDDV { get; set; }

        public int MaSD { get; set; }

        public int MaDV { get; set; }

        public int SoLuong { get; set; }

        [Column(TypeName = "money")]
        public decimal? ThanhTien { get; set; }

        public virtual DICHVU DICHVU { get; set; }

        public virtual SuDungMay SuDungMay { get; set; }
    }
}
