using MonitoringFinances.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MonitoringFinances.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, 1000000, ErrorMessage = "Amount should be a positive number under $1,000,000.")]
        public decimal Amount { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}
