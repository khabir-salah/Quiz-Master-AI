using Application.Features.DTOs;
using Application.Features.Interfaces.IRepositores;
using Application.Features.Interfaces.IService;
using Domain.Entities;
using MediatR;


namespace Application.Features.Queries.AssessmentResult
{
    public class GetAssessmentResultHandler(IDocumentRepository _questionRepo, IUserService _userService) : IRequestHandler<GetAssesmentResultCommandModel, BaseResponse<AssessmentResultResponseModel>>
    {
        public async Task<BaseResponse<AssessmentResultResponseModel>> Handle(GetAssesmentResultCommandModel request, CancellationToken cancellationToken)
        {
            int score = 0;
            var correctAnswers = 0;
            var allQuestion = await _questionRepo.GetAllQuestionsAsync() ?? throw new Exception("Empty List");
            var question = allQuestion.Where(Q => Q.AssesmentId == request.AssesmentId).ToList() ?? throw new Exception("Not Found");

            var userAnswer = new List<QuestionResponseModel>();

            foreach(var item in request.Answer)
            {
                var currentQuestion = question.FirstOrDefault(c => c.Id == item.Key);
                if (item.Value == currentQuestion.Answer)
                {
                    score++;
                    correctAnswers++;
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
            var percentage = ((double)correctAnswers / question.Count) * 100;

            var loggedinUser = await _userService.GetCurrentUser();
            var result = new Result
            {
                Score = percentage,
                AssesmentId = request.AssesmentId,
                ApplicationUserId = loggedinUser.Id
            };

            return new BaseResponse<AssessmentResultResponseModel>
            {
                IsSuccessful = true,
                Result = new AssessmentResultResponseModel
                {
                    Score = percentage,
                    Questions = userAnswer,
                    AssesmentId = request.AssesmentId,
                }
            };
        }
    }
}
