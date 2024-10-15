using Application.Features.DTOs;
using Application.Features.Interfaces.IRepositores;
using Domain.Entities;
using Infrastructure.Persistent.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Infrastructure.Persistent.Repositories.Implementation
{
    public class DocumentRepository : GenericRepository<Document>, IDocumentRepository
    {
        private readonly QuizMasterAiDb _context;
        private readonly ConfigPath _config;

        public DocumentRepository(QuizMasterAiDb context, IOptions<ConfigPath> config) : base(context)
        {
            _context = context;
            _config = config.Value;
        }

        public async Task<ICollection<Document>> GetAllDocumentAsync()
        {
            var document = await _context.Document.Include(a => a.Assesments).ToListAsync();
            return document;
        }

        public async Task<ICollection<Question>> GetAllQuestionsAsync()
        {
            var question = await _context.Question.Include(a => a.Options).ToListAsync();
            return question;
        }



        public async Task<string?> UploadAsync(IFormFile? file)
        {
            if (file == null)
            {
                return null;
            }

            var allowedImageExtensions = new HashSet<string> { ".jpg", ".jpeg", ".png", ".gif", ".tif", ".tiff", ".bmp", ".webp", ".svg", ".ico", ".jfif" };
            var allowedDocumentExtensions = new HashSet<string> { ".pdf", ".doc", ".docx", ".ppt", ".pptx", ".txt" };

            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedImageExtensions.Contains(extension) && !allowedDocumentExtensions.Contains(extension))
            {
                throw new Exception("Unsupported file type.");
            }

            var newFileName = $"{Guid.NewGuid().ToString("N").Substring(0, 6)}" + $".{extension}";

            var basePath = _config.Path;
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            string savePath;
            if (allowedImageExtensions.Contains(extension))
            {
                var imagesPath = Path.Combine(basePath, "Images");
                if (!Directory.Exists(imagesPath))
                {
                    Directory.CreateDirectory(imagesPath);
                }
                savePath = Path.Combine(imagesPath, newFileName);
            }
            else
            {
                var documentsPath = Path.Combine(basePath, "Documents");
                if (!Directory.Exists(documentsPath))
                {
                    Directory.CreateDirectory(documentsPath);
                }
                savePath = Path.Combine(documentsPath, newFileName);
            }

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return newFileName;
        }
    }
}
