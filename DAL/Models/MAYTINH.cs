namespace DAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MAYTINH")]
    public partial class MAYTINH
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MAYTINH()
        {
            SuDungMays = new HashSet<SuDungMay>();
        }

        [Key]
        public int MaMay { get; set; }

        [Required]
        [StringLength(50)]
        public string TenMay { get; set; }

        [StringLength(20)]
        public string TrangThai { get; set; }

        [Column(TypeName = "money")]
        public decimal GiaTheoGio { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SuDungMay> SuDungMays { get; set; }
    }
}
