using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Queries.Document
{
    public record class GetAllDocumentCommandModel : IRequest
    {
    }

    public record class DocumentResponseModel
    {
        public Guid AssessmentId { get; set; }              
        public string DocumentName { get; set; }
        public int 
    }
}
