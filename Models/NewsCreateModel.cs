using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class NewsCreateModel : Controller
    {
        [Required(ErrorMessage = "Заголовок обязателен")]
        [StringLength(255, ErrorMessage = "Заголовок не должен превышать 255 символов")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Контент обязателен")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Автор обязателен")]
        public int AuthorId { get; set; }

        public IFormFile CoverImage { get; set; }
    }
}
