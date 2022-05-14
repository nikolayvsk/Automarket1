using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Automarket1
{
    public partial class Model
    {
        public Model()
        {
            Cars = new HashSet<Car>();
        }

        public int Id { get; set; }


        [Required(ErrorMessage = "Не залишай мене порожнім! :)")]
        [MinLength(5, ErrorMessage = "Замало символів - не менше {1} символів")]
        [MaxLength(30)]
        [Display(Name = "Модель")]
        public string Model1 { get; set; }

        [Display(Name = "Машина")]
        public virtual ICollection<Car> Cars { get; set; }
    }
}
