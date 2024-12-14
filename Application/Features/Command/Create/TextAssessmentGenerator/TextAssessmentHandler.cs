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
    public class TextAssessmentHandler(IGenericRepository<Document> _documentRepo, IAssessmentService _assessment, IUserService _userService, UserManager<ApplicationUser> _user, IHttpContextAccessor _assessor, ITextGenerator _generator, IExtractQuestionService _extractQuestion, IGenericRepository<Domain.Entities.Assesment> _assessmentRepo) : IRequestHandler<TextAssessmentCommandModel, BaseResponse<AssessmentResponse>>
    {
        //private static readonly string apiKey = "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";
        public async Task<BaseResponse<AssessmentResponse>> Handle(TextAssessmentCommandModel request, CancellationToken cancellationToken)
        {
            var loggedInUser = await _user.GetUserAsync(_assessor.HttpContext.User);

            var isTitleExist = await _documentRepo.isExist(D => D.Title.ToLower() == request.Title.ToLower() && D.UserId == loggedInUser.Id);

            if (isTitleExist)
            {
                return new BaseResponse<AssessmentResponse>
                {
                    IsSuccessful = false,
                    Message = "You Already Created a Document with this title",
                };
            }


            if (await _user.IsInRoleAsync(loggedInUser, RoleConst.Basic))
            {
                var NunmberOfAssesmentToday = await _assessment.GetAssessmentGeneratedToday(loggedInUser.Id, DateTime.Now.Date);

                if (NunmberOfAssesmentToday >= 3)
                {
                    return new BaseResponse<AssessmentResponse>
                    {
                        IsSuccessful = false,
                        Message = "You have reached your daily limit of 3 Assessment. Please upgrade your subscription."
                    };
                }

                if (request.Choice.NumberOfQuestion > 10)
                {
                    return new BaseResponse<AssessmentResponse>
                    {
                        IsSuccessful = false,
                        Message = "Basic users can generate up to 10 questions per quiz. Please upgrade your subscription."
                    };
                }
            }

            var model = new TextGeneratorRequestModel
            {
                Content = request.Content,
                NumberOfQuestion = request.Choice.NumberOfQuestion,
                Type = request.Choice.Type,
            };


            var generateAssessment = await _generator.GenerateTextAssessmnet(model);
            if (!generateAssessment.IsSuccessful)
            {
                return new BaseResponse<AssessmentResponse> { IsSuccessful = false, Message = "An Error Occured" };
            }

            var document = new Document
            {
                Title = request.Title,
                Content = request.Content,
                DocumentType = UploadType.Text,
                UserId = loggedInUser.Id,
                Date = DateTime.Now.Date,
            };

            var assesment = new Domain.Entities.Assesment
            {
                Date = DateTime.Now.Date,
                QuestionType = request.Choice.Type,
                DocumentId = document.Id,
                Document = document,
                ApplicationUserId = loggedInUser.Id,
            };

            await _documentRepo.AddAsync(document);
            await _assessmentRepo.AddAsync(assesment);
            await _documentRepo.SaveAsync();

            await _assessment.StoreQuizGeneration(loggedInUser.Id, assesment.Date);

            if (request.Choice.Type == QuestionType.MultipleChoice)
            {
                var extractionMultipleQuestion = await _extractQuestion.ExtractMultipleChoiceQuestions(request.Content, assesment.Id);
                return new BaseResponse<AssessmentResponse>
                {
                    IsSuccessful = true,
                    Result = new AssessmentResponse
                    {
                        AssessmentId = assesment.Id,
                        Date = assesment.Date,
                        QuestionType = QuestionType.MultipleChoice,
                        Question = extractionMultipleQuestion.Select(s => new QuestionModel
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

            var extractionBooleanQuestion = await _extractQuestion.ExtractTrueFalseQuestions(request.Content, assesment.Id);
            return new BaseResponse<AssessmentResponse>
            {
                IsSuccessful = true,
                Result = new AssessmentResponse
                {
                    AssessmentId = assesment.Id,
                    Date = assesment.Date,
                    QuestionType = QuestionType.TrueOrFalse,
                    Question = extractionBooleanQuestion.Select(s => new QuestionModel
                    {
                        QuestionId = s.Id,
                        QuestionText = s.QuestionText,
                    }).ToList()
                }
            };

        }
    }
}
