using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ShippingDetails
    {
        [Required(ErrorMessage = "Укажите Ваше Имя")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Укажите данные")]
        [Display(Name = "Адрес доставки")]
        public string Line1 { get; set; }
        [Display(Name = "Номер телефона")]
        public string Line2 { get; set; }
        [Display(Name = "E-mail")]
        public string Line3 { get; set; }

        [Required(ErrorMessage = "Укажите Город")]
        [Display(Name = "Город")]
        public string City { get; set; }
        [Required(ErrorMessage = "Укажите Страну")]
        [Display(Name = "Страна")]
        public string Country { get; set; }

      
    }
}
