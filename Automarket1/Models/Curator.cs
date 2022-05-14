using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Automarket1
{
    public partial class Curator
    {
        public int Id { get; set; }
        [Display(Name = "Працівник")]
        [Required(ErrorMessage = "Не залишай мене порожнім! :)")]

        public int EmployeeId { get; set; }
        [Display(Name = "Номер продажу")]
        [Required(ErrorMessage = "Не залишай мене порожнім! :)")]
        public int SaleId { get; set; }

        [Display(Name = "Працівник")]
        public virtual Employee Employee { get; set; } = null!;
        [Display(Name = "Номер продажу")]
        public virtual Sale Sale { get; set; } = null!;
    }
}
