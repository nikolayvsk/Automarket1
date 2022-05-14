using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Automarket1
{
    public partial class Sale
    {
        public class MinimumDayAttribute : ValidationAttribute
        {
            int _minimumDay;

            public MinimumDayAttribute(int minimumDay)
            {
                _minimumDay = minimumDay;
            }

            public override bool IsValid(object value)
            {
                DateTime date;
                if (DateTime.TryParse(value.ToString(), out date))
                {
                    return date.AddDays(_minimumDay) < DateTime.Now;
                }

                return false;
            }
        }

        public Sale()
        {
            Curators = new HashSet<Curator>();
        }

        public int Id { get; set; }

        [Display(Name = "Покупець")]
        [Required(ErrorMessage = "Не залишай мене порожнім! :)")]
        public int CustomerId { get; set; }


        [Display(Name = "Машина")]
        [Required(ErrorMessage = "Не залишай мене порожнім! :)")]
        public int CarId { get; set; }

        
        [MinimumDay(0, ErrorMessage = "Некоректна дата")]
        [Display(Name = "Дата продажу")]
        [Required(ErrorMessage = "Не залишай мене порожнім! :)")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        
        /*
        [DataType(DataType.Date)]
        [Display(Name = "Дата продажу")]

        [Required(ErrorMessage = "Не залишай мене порожнім! :)")]
        */
        public DateTime DateSale { get; set; }

        [Display(Name = "Машина")]
        public virtual Car Car { get; set; } = null!;
        [Display(Name = "Покупець")]
        public virtual Customer Customer { get; set; } = null!;
        [Display(Name = "Куратор")]

        [DataType(DataType.Date)]
        public virtual ICollection<Curator> Curators { get; set; }
    }
}
