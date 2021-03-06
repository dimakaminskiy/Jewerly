﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Jewerly.Domain
{
    public class Review
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Введите имя")]
        [Display(Name = @"Имя")]
        public string Name { get; set; }
        [ScaffoldColumn(false)]
        [Display(Name = @"Дата")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Введите отзыв")]
        [Display(Name = @"Отзыв")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
    }
}
