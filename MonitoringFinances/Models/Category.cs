using MonitoringFinances.Models.AdminModels;
using MonitoringFinances.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MonitoringFinances.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public string UserId { get; set; }
        
        [Display(Name ="Type")]
        public int CategoryTypeId { get; set; }
        
        [ForeignKey("CategoryTypeId")]
        public virtual CategoryType CategoryType { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
