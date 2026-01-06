namespace DAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SuDungMay")]
    public partial class SuDungMay
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SuDungMay()
        {
            SuDungDichVus = new HashSet<SuDungDichVu>();
        }

        [Key]
        public int MaSD { get; set; }

        public int MaMay { get; set; }

        public int MaTK { get; set; }

        public DateTime ThoiGianBatDau { get; set; }

        public DateTime? ThoiGianKetThuc { get; set; }

        [Column(TypeName = "money")]
        public decimal? TongTien { get; set; }

        public virtual MAYTINH MAYTINH { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SuDungDichVu> SuDungDichVus { get; set; }

        public virtual TaiKhoanNguoiChoi TaiKhoanNguoiChoi { get; set; }
    }
}
