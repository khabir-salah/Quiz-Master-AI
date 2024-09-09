using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Features.DTOs;
using Domain.Enum;
using System.Net.Http.Headers;

namespace Application.Features.Command.Create.Assesment
{
    public class TextQuestionHandler : IRequestHandler<TextQuestionCommandModel, BaseResponse<string>>
    {
        private static readonly string apiKey = "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";
        //private static readonly string apiEndpoint = "https://api-inference.huggingface.co/models/valhalla/t5-small-qg-hl";
        private static readonly string apiEndpoint = "https://api-inference.huggingface.co/models/t5-base";
        public async Task<BaseResponse<string>> Handle(TextQuestionCommandModel request, CancellationToken cancellationToken)
        {
            using(HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                string prompt = string.Empty;
                
                if (request.Type == QuestionType.MultipleChoice)
                {
                    var firstPart = $"Generate {request.NumberOfQuestion} multiple-choice questions in JSON format based on the following document:" +
                        $"\n\n{string.Join("\r\n", request.Content)}" +
                        $"\n\n Each question should have a question text, four options, the correct answer, and an elucidation. ";

                    var lastPart = $"The JSON structure should be as follows:" +
                        @"{
                'question': 'What is the capital of Japan?',
                'options': ['A. Seoul', 'B. Beijing', 'C. Tokyo', 'D. Bangkok'],
                'answer': 'C. Tokyo',
                'explanation': 'Tokyo is the capital city of Japan.'
            }";
                    prompt = firstPart  + lastPart ;
                }
                else if (request.Type == QuestionType.TrueOrFalse)
                {
                    var firstPart = $"Generate {request.NumberOfQuestion} true or false questions in JSON format based on the following document:" +
                        $"\n\n{string.Join("\r\n", request.Content)} " +
                        $"Each question should have a statement, a boolean value indicating the correct answer, and an explanation. ";

                    var lastPart = $"The JSON structure should be as follows:" +
                        @"{
               'statement': 'Tokyo is the capital of Japan.',
                'answer': true,
                'explanation': 'Tokyo is indeed the capital city of Japan.'
           }";
                    prompt = firstPart + lastPart;
                }

                var requestBody = new
                {
                    input = prompt,
                };

                var requestContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiEndpoint, requestContent);
                response.EnsureSuccessStatusCode();

                if(response.IsSuccessStatusCode)
                {
                    return new BaseResponse<string>
                    {
                        IsSuccessful = true,
                        Result = await response.Content.ReadAsStringAsync()
                    };
                }
                else return new BaseResponse<string>()
                {
                    IsSuccessful = false,
                    Result = string.Empty
                };
            }
        }
    }

    //internal static async Task<T> ReadContentAs<T>(this HttpResponseMessage response)
    //{
    //    var responseAsString = await response.Content.ReadAsStringAsync();
    //    var responseObject = JsonSerializer.Deserialize<T>(responseAsString, new JsonSerializerOptions
    //    {
    //        PropertyNameCaseInsensitive = true,
    //        ReferenceHandler = ReferenceHandler.Preserve
    //    });
    //    return responseObject;
    //}
}
