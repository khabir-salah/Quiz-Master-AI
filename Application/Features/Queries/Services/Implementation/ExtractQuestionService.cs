using Application.Features.DTOs;
using Application.Features.Interfaces.IRepositores;
using Application.Features.Interfaces.IService;
using Domain.Entities;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Application.Features.Queries.Services.Implementation
{
    public class ExtractQuestionService(IGenericRepository<Question> _document) : IExtractQuestionService
    {

        public async Task<ICollection<Question>> ExtractMultipleChoiceQuestions(string questionText, Guid assessmentId)
        {
            List<Question> questions = new List<Question>();

            string questionPattern = @"'question':\s*'([^']*)'";  // Captures question text
            string optionsPattern = @"'options':\s*\[(.*?)\]";    // Captures options array
            string answerPattern = @"'answer':\s*'([^']*)'";      // Captures answer
            string explanationPattern = @"'explanation':\s*'([^']*)'";   // Matches the explanation (e.g., "Explanation: ...")

            string fullPattern = @"'question':\s*'([^']*)',\s*'options':\s*\[(.*?)\],\s*'answer':\s*'([^']*)',\s*'explanation':\s*'([^']*)'";

            // Find all matches
            var matches = Regex.Matches(questionText, fullPattern, RegexOptions.Singleline);


            foreach (Match match in matches)
            {
                var question = new Question
                {
                    QuestionText = match.Groups[1].Value.Trim(),
                    Options = new List<Option>(),
                    Answer = match.Groups[3].Value.Trim(),
                    Explanation = match.Groups[4].Value.Trim(),
                    AssesmentId = assessmentId
                };

                var optionsRaw = match.Groups[2].Value.Trim();  
                var optionsList = optionsRaw.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);  

                foreach (var optionText in optionsList)
                {
                    question.Options.Add(new Option
                    {
                        Text = optionText.Trim(),
                    });
                }

                questions.Add(question);

            }

            await _document.AddRangeAsync(questions);
            await _document.SaveAsync();

            return questions;
        }



        public async Task<ICollection<Question>> ExtractTrueFalseQuestions(string rawResponse, Guid assesmentId)
        {
            var jsonObjects = new List<string>();

            var regex = new Regex(@"{[^}]+}");
            var matches = regex.Matches(rawResponse);

            foreach (Match match in matches)
            {
                jsonObjects.Add(match.Value);
            }

            var questions = new List<Question>();

            foreach (var json in jsonObjects)
            {
                try
                {
                    var question = JsonConvert.DeserializeObject<Question>(json);
                    if (question != null)
                    {
                        question.AssesmentId = assesmentId;
                        questions.Add(question);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to deserialize: " + ex.Message);
                }
            }

            await _document.AddRangeAsync(questions);
            await _document.SaveAsync();

            return questions;
        }


    }
}
