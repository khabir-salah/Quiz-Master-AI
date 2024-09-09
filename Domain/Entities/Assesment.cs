using Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Assesment
    {
        public Guid Id { get; set; }    
        public Guid DocumentId { get; set; }
        public ICollection<Question> Question { get; set; }
        public QuestionType QuestionType { get; set; }
        public AssesmentType AssesmentTypes { get; set; }
        public Guid UserId { get; set; }
        public Guid ResultId { get; set; }
        [NotMapped]
        public string ReferalCode => AssesmentTypes == AssesmentType.Private ? null : Id.ToString("N").Substring(0, 6);
        public User User { get; set; }
        public ICollection<Result> Results { get; set; }
        public Document Document { get; set; }
    }
}
