using Application.Features.DTOs;
using Application.Features.Interfaces.IRepositores;
using Application.Features.Interfaces.IService;
using Application.Features.Queries.AssessmentResult;
using Application.Features.Queries.RetakeAssessment;

namespace Application.Features.Queries.Services.Implementation
{
    public class TextAssessmentService(IDocumentRepository _document, IUserService _user, IAssessmentRepository _assessment) : ITextAssessmentService
    {

        public async Task<BaseResponse<ICollection<TextAssessmentModel>>> GetAllTextAssessments()
        {
            var loggedInUser = await _user.GetCurrentUser();
            var document = await _document.GetAllDocumentAsync();
            var userDocument = document.Where(d => d.UserId == loggedInUser.Id && d.DocumentType == Domain.Enum.UploadType.Text).ToList ();
            if (userDocument.Count > 0)
            {
                return new BaseResponse<ICollection<TextAssessmentModel>>
                {
                    IsSuccessful = true,
                    Result = userDocument.Select(d => new TextAssessmentModel
                    {
                        Id = d.Assesments.Id,
                        Title = d.Title,
                        AssessmentLink = $"https://localhost:7164/TakeAssessment/{d.Assesments.Id}",
                        DateCreated = d.Date
                    }).ToList(),
                };
            }
            return new BaseResponse<ICollection<TextAssessmentModel>>
            {
                IsSuccessful = false,
                Result = new List<TextAssessmentModel> { }
            };
        }
        

        public async Task<BaseResponse<RetakeAssesmentResponseModel>> TakeAssessment(Guid AssessmentId)
        {
            var userAssesment = await _assessment.GetAssesmentAsync(a => a.Id == AssessmentId) ?? throw new ArgumentException("Assessment Not found");

           
            return new BaseResponse<RetakeAssesmentResponseModel>
            {
                IsSuccessful = true,
                Result = new RetakeAssesmentResponseModel
                {
                    AssessmentId = AssessmentId,
                    Question = userAssesment.Question.Select(s => new QuestionModel
                    {
                        QuestionId = s.Id,
                        QuestionText = s.QuestionText,
                        Options = s.Options.Select(o => new OptionModel
                        {
                            OptionId = o.Id,
                            Text = o.Text,
                        }).ToList(),
                    }).ToList()
                }
            };
        }

        public async Task<BaseResponse<AssessmentResultResponseModel>> Result(GetAssesmentResultCommandModel request)
        {
            int score = 0;
            var correctAnswers = 0;
            var allQuestion = await _document.GetAllQuestionsAsync() ?? throw new Exception("Empty List");
            var question = allQuestion.Where(Q => Q.AssesmentId == request.AssesmentId).ToList() ?? throw new Exception("Not Found");

            var userAnswer = new List<QuestionResponseModel>();

            foreach (var item in request.Answer)
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
