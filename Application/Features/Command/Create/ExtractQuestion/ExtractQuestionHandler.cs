using Application.Features.DTOs;
using MediatR;


namespace Application.Features.Command.Create.ExtractQuestion
{
    public class ExtractQuestionHandler(IMediator _mediator) : IRequestHandler<ExtractQuestionCommandModel, BaseResponse<string>>
    {
        public Task<BaseResponse<string>> Handle(ExtractQuestionCommandModel request, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
