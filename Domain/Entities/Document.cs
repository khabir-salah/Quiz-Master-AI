using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Document
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public UploadType DocumentType { get; set; }
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public ICollection<Assesment> Assesments { get; set; }
    }
}
