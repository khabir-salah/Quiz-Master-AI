﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Result
    {
        public Guid Id { get; set; }
        public Guid AssesmentId { get; set; }
        public double Score { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public ICollection<UserQuestionResult> QuestionResults { get; set; }
        public Assesment Assesment { get; set; }
    }
}