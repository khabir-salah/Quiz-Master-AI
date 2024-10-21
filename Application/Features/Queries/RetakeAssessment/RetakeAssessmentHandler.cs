using Application.Features.Command.Create.Assesment;
using Application.Features.DTOs;
using Application.Features.Interfaces.IRepositores;
using Application.Features.Interfaces.IService;
using Domain.Entities;
using Domain.Enum;
using MediatR;


namespace Application.Features.Queries.RetakeAssessment
{
    public class RetakeAssessmentHandler(IAssessmentRepository _assesment, IUserService _user) : IRequestHandler<RetakeAssessmentCommandModel, BaseResponse<RetakeAssesmentResponseModel>>
    {
        public async Task<BaseResponse<RetakeAssesmentResponseModel>> Handle(RetakeAssessmentCommandModel request, CancellationToken cancellationToken)
        {
            var loggedInUser = await _user.GetCurrentUser();
            var userAssesment = await _assesment.GetAssesmentAsync(a => a.Id == request.AssessmentId && a.ApplicationUserId == loggedInUser.Id) ?? throw new ArgumentException("Assessment Not found");

            return new BaseResponse<RetakeAssesmentResponseModel>
            {
                IsSuccessful = true,
                Result = new RetakeAssesmentResponseModel
                {
                    AssessmentId = request.AssessmentId,
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
    }
}
