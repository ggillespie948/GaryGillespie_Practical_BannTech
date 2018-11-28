using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GaryGillespie_Practical.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(255)]
        [Required]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        public double PurchasePrice { get; set; }

        [Required]
        public double SalesPrice { get; set; }

        public int Quantity { get; set; }

        public int LogId { get; set; }

        [ForeignKey("LogId")]
        public virtual OperationLog Log { get; set; }
    }
}
