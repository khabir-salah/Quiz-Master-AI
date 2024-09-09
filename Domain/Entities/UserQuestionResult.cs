using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserQuestionResult
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string UserAnswer {  get; set; }
        public Guid ResultId { get; set; }
        public Result Result { get; set; }
    }
}
