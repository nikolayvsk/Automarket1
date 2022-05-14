using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Automarket1
{
    public partial class Position
    {
        public Position()
        {
            Employees = new HashSet<Employee>();
        }

        public int Id { get; set; }
        [Display(Name = "Посада")]
        [Required(ErrorMessage = "Не залишай мене порожнім! :)")]
        public string Position1 { get; set; } = null!;

        [Display(Name = "Працівник")]
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
