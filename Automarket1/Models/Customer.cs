using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Automarket1
{
    public partial class Customer
    {
        public class MinimumAgeAttribute : ValidationAttribute
        {
            int _minimumAge;

            public MinimumAgeAttribute(int minimumAge)
            {
                _minimumAge = minimumAge;
            }

            public override bool IsValid(object value)
            {
                DateTime date;
                if (DateTime.TryParse(value.ToString(), out date))
                {
                    return date.AddYears(_minimumAge) < DateTime.Now;
                }

                return false;
            }
        }

        public Customer()
        {
            Sales = new HashSet<Sale>();
        }

        public int Id { get; set; }
        [Display(Name = "П.І.П.")]
        [Required(ErrorMessage = "Не залишай мене порожнім! :)")]
        [MinLength(5, ErrorMessage = "Замало символів - не менше {1} символів")]
        [MaxLength(50)]
        public string FullName { get; set; } = null!;

        [MinimumAge(18, ErrorMessage = "Менше 18 років")]
        [Display(Name = "День народження")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateBirth { get; set; }


        [Display(Name = "Номер паспорта")]
        [Required(ErrorMessage = "Не залишай мене порожнім! :)")]
        [MinLength(5, ErrorMessage = "Замало символів - не менше {1} символів")]
        [MaxLength(15)]
        public string PassportNumber { get; set; } = null!;

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Номер телефону")]
        [Required(ErrorMessage = "Не залишай мене порожнім! :)")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Неправильний формат номеру телефону")]
        public string PhoneNumber { get; set; } = null!;

        [Display(Name = "Чек")]
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
