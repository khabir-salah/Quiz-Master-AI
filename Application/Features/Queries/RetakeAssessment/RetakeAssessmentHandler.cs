using Application.Features.DTOs;
using Application.Features.Interfaces.IRepositores;
using Application.Features.Interfaces.IService;
using Domain.Entities;
using MediatR;


namespace Application.Features.Queries.RetakeAssessment
{
    public class RetakeAssessmentHandler(IAssessmentRepository _assesment, IUserService _user) : IRequestHandler<RetakeAssessmentCommandModel, BaseResponse<ICollection<RetakeAssesmentResponseModel>>>
    {
        public async Task<BaseResponse<ICollection<RetakeAssesmentResponseModel>>> Handle(RetakeAssessmentCommandModel request, CancellationToken cancellationToken)
        {
            var loggedInUser = await _user.GetCurrentUser();
            var userAssesment = await _assesment.GetAsync(a => a.Id == request.AssessmentId && a.ApplicationUserId == loggedInUser.Id) ?? throw new ArgumentException("Assessment Not found");


            return new BaseResponse<ICollection<RetakeAssesmentResponseModel>>
            {
                IsSuccessful = true,
                Result = userAssesment.Question.Select(s => new RetakeAssesmentResponseModel
                {
                    QuestionText = s.QuestionText,
                    Options = s.Options.Select(o => new OptionResponseModel
                    {
                        Text = o.Text,
                    }).ToList(),
                }).ToList(),
            };
        }
    }
}
