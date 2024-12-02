using Application.Features.DTOs;
using Application.Features.Interfaces.IService;
using Domain.Enum;
using OpenAI_API;
using OpenAI_API.Completions;


namespace Application.Features.Queries.Services.Implementation
{
    public class TextGenerator : ITextGenerator
    {
        private readonly string apiKey = "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";
        private readonly OpenAIAPI api;
        public async Task<BaseResponse<string>> GenerateTextAssessmnet(TextGeneratorRequestModel request)
        {
            var openai = new OpenAIAPI(apiKey);
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
                prompt = firstPart + lastPart;
            }
            else if (request.Type == QuestionType.TrueOrFalse)
            {
                var firstPart = $"Generate {request.NumberOfQuestion} true or false questions in JSON format based on the following document:" +
                    $"\n\n{string.Join("\r\n", request.Content)} " +
                    $"Each question should have a statement, a boolean value indicating the correct answer, and an explanation. ";

                var lastPart = $"The JSON structure should be as follows:" +
                    @"{
               'questiontext': 'Tokyo is the capital of Japan.',
                'answer': true,
                'explanation': 'Tokyo is indeed the capital city of Japan.'
           }";
                prompt = firstPart + lastPart;
            }

            var requestBody = new CompletionRequest
            {
                Prompt = prompt,
                MaxTokens = 600,
                Temperature = 0.7
            };

            try
            {
                var response = await openai.Completions.CreateCompletionAsync(requestBody);

                var generatedText = response.Completions.FirstOrDefault()?.Text;

                if (!string.IsNullOrEmpty(generatedText))
                {
                    return new BaseResponse<string>
                    {
                        IsSuccessful = true,
                        Result = generatedText
                    };
                }
                else
                {
                    return new BaseResponse<string>
                    {
                        IsSuccessful = false,
                        Result = "An Error Occured No text was generated."
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
