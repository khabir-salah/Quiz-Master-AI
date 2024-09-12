using MediatR;
using Application.Features.DTOs;
using Domain.Enum;
using Microsoft.AspNetCore.Identity;
using Application.Features.Interfaces.IRepositores;
using Domain.Entities;
using Application.Features.Interfaces.IService;
using Microsoft.AspNetCore.Http;
using Domain.RoleConst;

namespace Application.Features.Command.Create.Assesment
{
    public class TextAssessmentHandler(IGenericRepository<Document> _documentRepo, IAssessmentService _assessment, IUserService _userService, UserManager<ApplicationUser> _user, IHttpContextAccessor _assessor, ITextGenerator _generator, IExtractQuestionService _extractQuestion) : IRequestHandler<TextAssessmentCommandModel, BaseResponse<AssessmentResponseModel>>
    {
        //private static readonly string apiKey = "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";
        public async Task<BaseResponse<AssessmentResponseModel>> Handle(TextAssessmentCommandModel request, CancellationToken cancellationToken)
        {
            var isTitleExist = await _documentRepo.isExist(D => D.Title == request.Title);
            if(isTitleExist)
            {
                return new BaseResponse<AssessmentResponseModel>
                {
                    IsSuccessful = true,
                    Message = "You Already Created a Document with this title",
                };
            }

            var loggedInUser = await _user.GetUserAsync(_assessor.HttpContext.User);

            if (await _user.IsInRoleAsync(loggedInUser, RoleConst.Basic))
            {
                var NunmberOfAssesmentToday = await _assessment.GetAssessmentGeneratedToday(loggedInUser.Id, DateTime.Now);

                if (NunmberOfAssesmentToday >= 3)
                {
                    return new BaseResponse<AssessmentResponseModel>
                    {
                        IsSuccessful = false,
                        Message = "You have reached your daily limit of 3 Assessment. Please upgrade your subscription."
                    };
                }

                if (request.NumberOfQuestion > 10)
                {
                    return new BaseResponse<AssessmentResponseModel>
                    {
                        IsSuccessful = false,
                        Message = "Basic users can generate up to 10 questions per quiz. Please upgrade your subscription."
                    };
                }
            }

            var model = new TextGeneratorRequestModel
            {
                Content = request.Content,
                NumberOfQuestion = request.NumberOfQuestion,
                Type = request.Type,
            };

            var generateAssessment = await _generator.GenerateTextAssessmnet(model);
            if (!generateAssessment.IsSuccessful)
            {
                return new BaseResponse<AssessmentResponseModel> { IsSuccessful = false, Message = "An Error Occured" };
            }

            var document = new Document
            {
                Title = request.Title,
                Content = request.Content,
                DocumentType = UploadType.Text,
                UserId = loggedInUser.Id,
            };

            var assesment = new Domain.Entities.Assesment
            {
                Date = DateTime.Now,
                QuestionType = request.Type,
                DocumentId = document.Id,
                Document = document,
                ApplicationUserId = loggedInUser.Id,
            };

            await _assessment.StoreQuizGeneration(loggedInUser.Id, assesment.Date);

            if (request.Type == QuestionType.MultipleChoice)
            {
                var extractionMultipleQuestion = await _extractQuestion.ExtractMultipleChoiceQuestions(generateAssessment.Result, assesment.Id);
                return new BaseResponse<AssessmentResponseModel>
                {
                    IsSuccessful = true,
                    Result = new AssessmentResponseModel
                    {
                        Date = assesment.Date,
                        QuestionType = QuestionType.MultipleChoice,
                        Question = extractionMultipleQuestion.Select(s => new QuestionResponseModel
                        {
                            QuestionText = s.QuestionText,
                            Options = s.Options.Select(o => new OptionResponseModel
                            {
                                Text = o.Text,
                            }).ToList(),
                            Answer = s.Answer,
                            Explanation = s.Explanation,
                        }).ToList()
                    }
                };
            }

            var extractionBooleanQuestion = await _extractQuestion.ExtractTrueFalseQuestions(generateAssessment.Result, assesment.Id);
            return new BaseResponse<AssessmentResponseModel>
            {
                IsSuccessful = true,
                Result = new AssessmentResponseModel
                {
                    Date = assesment.Date,
                    QuestionType = QuestionType.TrueOrFalse,
                    Question = extractionBooleanQuestion.Select(s => new QuestionResponseModel
                    {
                        QuestionText = s.QuestionText,
                        Answer = s.Answer,
                        Explanation = s.Explanation,
                    }).ToList()
                }
            };
        }
    }
}
