using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Domain.Entities
{
    public class Auto //Модель
    {
        [HiddenInput(DisplayValue=false)]
        [Display(Name="ID")]
        public int Id { get; set; }

        [Display(Name="Название")]
        [Required(ErrorMessage ="Пожалуйста,введите название авто")]
        public string Name { get; set; }

        [Display(Name = "Производитель")]
        [Required(ErrorMessage = "Пожалуйста,введите название производителя")]
        public string Manufacturer { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Пожалуйста,введите описание")]
        public string Description { get; set; }

        [Display(Name = "Тип")]
        [Required(ErrorMessage = "Пожалуйста,введите название марки авто")]
        public string Type { get; set; }

        [Display(Name = "Цена ($)")]
        [Required]
        [Range(0.01,double.MaxValue,ErrorMessage = "Пожалуйста,введите положительную цену")]
        public decimal Price { get; set; }

        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }

    }
}

