using Application.Features.DTOs;
using Application.Features.Queries.RetakeAssessment;

namespace Application.Features.Interfaces.IService
{
    public interface ITextAssessmentService
    {
        Task<BaseResponse<ICollection<TextAssessmentModel>>> GetAllTextAssessments();
        Task<BaseResponse<RetakeAssesmentResponseModel>> TakeAssessment(Guid AssessmentId);
    }
}
