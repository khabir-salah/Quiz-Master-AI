using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Question
    {
        public Guid Id { get; set; }
        public Guid AssesmentId { get; set; }
        public string QuestionText { get; set; }
        public ICollection<Option> Options { get; set; }
        public string Answer {  get; set; } 
        public string Explanation {  get; set; } 
    }
}
