using Application.Features.DTOs;
using Application.Features.Interfaces.IRepositores;
using Application.Features.Interfaces.IService;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Queries.AssessmentResult
{
    public class GetAssessmentResultHandler(IGenericRepository<Question> _questionRepo, IUserService _userService) : IRequestHandler<GetAssesmentResultCommandModel, BaseResponse<AssessmentResultResponseModel>>
    {
        public async Task<BaseResponse<AssessmentResultResponseModel>> Handle(GetAssesmentResultCommandModel request, CancellationToken cancellationToken)
        {
            int score = 0;
            var allQuestion = await _questionRepo.GetAllAsync();
            var question = allQuestion.Where(Q => Q.AssesmentId == request.AssesmentId).ToList();

            var userAnswer = new List<QuestionResponseModel>();

            foreach(var item in request.Answer)
            {
                var currentQuestion = question.FirstOrDefault(c => c.Id == item.Key);
                if (item.Value == currentQuestion.Answer)
                {
                    score++;
                }

                userAnswer.Add(new QuestionResponseModel
                {
                    Answer = currentQuestion.Answer,       
                    Explanation = currentQuestion.Explanation,
                    UserAnswer = item.Value,
                    QuestionText = currentQuestion.QuestionText,
                    Options = currentQuestion.Options.Select(s => new OptionResponseModel
                    {
                        Text = s.Text,
                    }).ToList(),
                });
            }
            var loggedinUser = await _userService.GetCurrentUser();
            var result = new Result
            {
                Score = score,
                AssesmentId = request.AssesmentId,
                ApplicationUserId = loggedinUser.Id
            };

            return new BaseResponse<AssessmentResultResponseModel>
            {
                IsSuccessful = true,
                Result = new AssessmentResultResponseModel
                {
                    Score = score,
                    Questions = userAnswer
                }
            };
        }
    }
}
