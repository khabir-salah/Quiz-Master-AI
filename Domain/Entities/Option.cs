using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Option
    {
        public Guid Id { get; set; }    
        public string Text { get; set; }
        public Guid QuestionId { get; set; }    
        public Question Question { get; set; }
    }
}
