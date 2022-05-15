using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Automarket1
{
    public partial class Car
    {
        public Car()
        {
            Sales = new HashSet<Sale>();
        }

        public int Id { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "{0} повинен починатись з {1}")]
        [Display(Name = "Серійний номер")]
        [Required(ErrorMessage = "Не залишай мене порожнім! :)")]
        [MinLength(5, ErrorMessage = "Замало символів - не менше {1} символів")]
        [MaxLength(50)]
        public string SerialNumber { get; set; } = null!;
        

        [DisplayFormat(DataFormatString = "{0:# ₴}")]
        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Тільки додатні цілі числа")]
        [Display(Name = "Ціна")]
        [Required(ErrorMessage = "Не залишай мене порожнім! :)")]
        public decimal Price { get; set; }


        [Required(ErrorMessage = "Не залишай мене порожнім! :)")]
        [Display(Name = "Модель")]
        public int ModelId { get; set; }

        [Display(Name = "Модель")]
        public virtual Model Model { get; set; } = null!;
        [Display(Name = "Чек")]
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
