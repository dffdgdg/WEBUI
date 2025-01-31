using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebUI.Migrations;
using Microsoft.AspNetCore.Authorization;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class NewsController(NewsDbContext context, IWebHostEnvironment environment) : Controller
    {
        private readonly NewsDbContext _context = context;
        private readonly IWebHostEnvironment _environment = environment;

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var newsList = await _context.News
                .OrderByDescending(n => n.CreatedAt)
                .Take(6) 
                .ToListAsync();
            return View(newsList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        // Новый метод для получения новостей с пагинацией
        [HttpGet("/api/news")]
        public async Task<IActionResult> GetNews(int skip, int take)
        {
            var news = await _context.News
                .OrderByDescending(n => n.CreatedAt) // Сортируем по убыванию даты создания
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return Ok(news);
        }

        [HttpGet("News/GetImage/{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news?.CoverImage != null)
            {
                string contentType = GetContentType(news.CoverImage);
                return File(news.CoverImage, contentType);
            }

            return NotFound();
        }

        private string GetContentType(byte[] imageData)
        {
            // Проверка первых байтов для определения типа
            if (imageData.Length > 4 && imageData[0] == 0xFF && imageData[1] == 0xD8 && imageData[2] == 0xFF)
            {
                return "image/jpeg";
            }
            if (imageData.Length > 8 && imageData[0] == 0x89 && imageData[1] == 0x50 && imageData[2] == 0x4E && imageData[3] == 0x47 && imageData[4] == 0x0D && imageData[5] == 0x0A && imageData[6] == 0x1A && imageData[7] == 0x0A)
            {
                return "image/png";
            }
            // Добавьте другие проверки для других типов изображений
            return "application/octet-stream"; // Default
        }

        // Метод для удаления новости
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var newsItem = await _context.News.FindAsync(id);
            if (newsItem != null)
            {
                _context.News.Remove(newsItem);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return NotFound();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsCreateModel model)
        {
            if (ModelState.IsValid)
            {
                byte[] coverImageData = null;
                if (model.CoverImage != null && model.CoverImage.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await model.CoverImage.CopyToAsync(memoryStream);
                    coverImageData = memoryStream.ToArray();
                }

                var newsItem = new News
                {
                    Title = model.Title,
                    Content = model.Content,
                    AuthorId = model.AuthorId,
                    CreatedAt = DateTime.Now,
                    ViewsCount = 0,
                    CoverImage = coverImageData
                };

                _context.News.Add(newsItem);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}
