using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Features.DTOs;

namespace Application.Features.Command.Create.Assesment
{
    public class TextQuestionHandler : IRequestHandler<TextQuestionCommandModel, BaseResponse<string>>
    {
        private static readonly string apiKey = "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";
        private static readonly string apiEndpoint = "https://api-inference.huggingface.co/models/valhalla/t5-small-qg-hl";
        public Task<BaseResponse<string>> Handle(TextQuestionCommandModel request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    internal static async Task<T> ReadContentAs<T>(this HttpResponseMessage response)
    {
        var responseAsString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<T>(responseAsString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.Preserve
        });
        return responseObject;
    }
}
