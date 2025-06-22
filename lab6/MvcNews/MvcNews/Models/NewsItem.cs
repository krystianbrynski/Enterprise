using System;
using System.ComponentModel.DataAnnotations;

namespace MvcNews.Models
{
    public class NewsItem
    {
        public int ID { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date")] // opcjonalnie etykieta
        public DateTime Timestamp { get; set; }

        [Required(ErrorMessage = "Treść newsa jest wymagana.")]
        [StringLength(140, MinimumLength = 5, ErrorMessage = "Treść newsa musi mieć od {2} do {1} znaków.")]
        public string Text { get; set; } = string.Empty;

        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }
}
