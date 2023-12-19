namespace NguyenVanNguyen__2180606793.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("THAMSO")]
    public partial class THAMSO
    {
        [Key]
        [StringLength(40)]
        public string TenThamSo { get; set; }

        public int? GiaTri { get; set; }
    }
}
