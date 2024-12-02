using Application.Features.DTOs;
using Application.Features.Interfaces.IRepositores;
using Application.Features.Interfaces.IService;
using Application.Features.Queries.Services.Implementation;
using Domain.Entities;
using Domain.Enum;
using Domain.RoleConst;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Application.Features.Command.Create.Assesment
{
    

    public class DocumentAssessment
    {
        public record class DocumentRequest(string Title, IFormFile Document) : IRequest<IResult>;

        public class Handler(IUserService _user, IDocumentRepository _document, UserManager<ApplicationUser> _role, IOptions<ConfigPath> _storageConfig) : IRequestHandler<DocumentRequest, IResult>
        {
            public async Task<IResult> Handle(DocumentRequest request, CancellationToken cancellationToken)
            {
                var loggedInUser = await _user.GetCurrentUser();
                var checkDocument = await _document.isExist(D => request.Title.Equals(D.Title, StringComparison.OrdinalIgnoreCase) && D.UserId == loggedInUser.Id);

                if (checkDocument)
                {
                    throw new Exception("Document Already Exist");
                }

                if (request.Document == null || request.Document.Length == 0)
                {
                    throw new Exception("Invalid Document");
                }

                double fileSizeInMB = request.Document.Length / (1024.0 * 1024.0);

                string[] allowedExtensions = { "txt", "doc", "docx", "pdf", "ppt", "pptx" };
                var currentDocumentExtension = Path.GetExtension(request.Document.FileName)?.TrimStart('.').ToLower();

                if (!allowedExtensions.Contains(currentDocumentExtension) || string.IsNullOrEmpty(currentDocumentExtension))
                {
                    throw new Exception("Unsupported Format");
                }

                if(await _role.IsInRoleAsync(loggedInUser, RoleConst.Basic))
                {
                    if (fileSizeInMB > 5)
                    {
                        throw new Exception("Basic user can only upload Document below 5mb");
                    }
                    if (currentDocumentExtension != "txt")
                    {
                        throw new Exception("Can only upload txt document format");
                    }
                }
                else if(await _role.IsInRoleAsync(loggedInUser, RoleConst.Classic))
                {
                    if (fileSizeInMB > 20)
                    {
                        throw new Exception("Classic user can only upload Document below 20mb");
                    }
                    string[] standardRestrictedExtensions = { "txt", "doc", "docx" };
                    if (!standardRestrictedExtensions.Contains(currentDocumentExtension))
                    {
                        throw new Exception("Can only upload document of \"txt\", \"doc\", \"docx\" format");
                    }
                }
                else
                {
                    if (fileSizeInMB > 50)
                    {
                        throw new Exception("File too large");

                    }
                }

                try
                {
                    var documentUrl = await _document.UploadAsync(request.Document);
                    var documentContent = File.ReadAllLines($"{_storageConfig.Value.Path}\\Documents\\{documentUrl}");

                }
                catch
                {
                    throw;
                }
                throw new Exception();
            }
        }

       
    }

   
}
