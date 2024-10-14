using Application.Features.Command.Create.Assesment;
using Application.Features.DTOs;
using Application.Features.Interfaces.IService;
using Application.Features.Queries.AssessmentResult;
using Application.Features.Queries.RetakeAssessment;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Api.Controllers
{
    [Route("api/Assessment")]
    [ApiController]
    public class AssessmentController(IConfiguration _configuration, ISeedRole _role, HttpClient _httpClient, IUserService _user, IMediator _mediator, ITextGenerator _textGenerator, ITextAssessmentService _textService) : ControllerBase
    {

        [HttpGet("RetakeAssesment/{AssesmentId}")]
        [Authorize]
        public async Task<IActionResult> RetakeAssessment([FromRoute] Guid AssesmentId)
        {
            var query = new RetakeAssessmentCommandModel
            {
                AssessmentId = AssesmentId,
            };
            var assessment = await _mediator.Send(query);
            if(!assessment.IsSuccessful) return BadRequest(assessment.Message);
            return Ok(assessment.Result);
        }

        [HttpPost("GenerateTextQuestions")]
        [Authorize]
        public async Task<IActionResult> GenerateTextQuestion([FromBody] TextAssessmentCommandModel request)
        {
            var textQuestion = await _mediator.Send(request);
            if(!textQuestion.IsSuccessful) return BadRequest(textQuestion.Message); 
            return Ok(textQuestion.Result);
        }

        [HttpPost("AssessmentResult")]
        [Authorize]
        public async Task<IActionResult> AssessmentResult([FromBody] GetAssesmentResultCommandModel request)
        {
            var result = await _mediator.Send(request);
            return Ok(result.Result);
        }

        [HttpGet("GetTextAssessment")]
        [Authorize]
        public async Task<IActionResult> GetTextAssessment()
        {
            var textDocument = await _textService.GetAllTextAssessments();
            if(!textDocument.IsSuccessful) return Ok(textDocument.Result);
            return Ok(textDocument.Result);
        }

        [HttpGet("TakeAssessment/{AssesmentId}")]
        public async Task<IActionResult> TakeAssessment([FromRoute] Guid AssesmentId)
        {
            var result = await _textService.TakeAssessment(AssesmentId);
            if(!result.IsSuccessful) return Ok(result.Result);
            return Ok(result.Result);
        }

        [HttpPost("Result")]
        public async Task<IActionResult> Result([FromBody] GetAssesmentResultCommandModel request)
        {
            var result = await _textService.Result(request);
            return Ok(result.Result);
        }

        [HttpPost("subscribe")]
        [Authorize]
        public async Task<IActionResult> Subscribe([FromBody] SubscriptionRequest request)
        {
            string? apiKey = _configuration["PayStack:SecretKey"];
            string planType = request.PlanType;
            var amount = planType == "classic" ? 5000 : 7500;
            var loggedInUser = await _user.GetCurrentUser();
            var email = loggedInUser.Email;
            var paystackRequest = new
            {
                email = email,
                amount = amount * 100, 
                callback_url = "https://localhost:7164/payment/callback",
                metadata = new
                {
                    planType = planType  
                }
            };

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://api.paystack.co/transaction/initialize")
            {
                Content = JsonContent.Create(paystackRequest)
            };

            // Set the authorization header with the API key
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var response = await _httpClient.SendAsync(requestMessage);

            // Use HttpClient to call Paystack API
            //var response = await _httpClient.PostAsJsonAsync("https://api.paystack.co/transaction/initialize", paystackRequest);
            if (response.IsSuccessStatusCode)
            {

                var paymentLink = await response.Content.ReadFromJsonAsync<PaystackResponse>();
                return Ok(paymentLink); 
            }

            return BadRequest("Payment initialization failed");
        }


        [HttpGet("payment/callback")]
        public async Task<IActionResult> PaymentCallback([FromQuery] string reference)
        {
            // Log or verify the transaction reference
            if (string.IsNullOrEmpty(reference))
            {
                return BadRequest("Transaction reference is missing.");
            }

            // Call Paystack to confirm the payment using the transaction reference
            var paystackResponse = await _role.VerifyPaystackPayment(reference);

            if (paystackResponse.Status)
            {
                var user = await _role.GetUserByEmail(paystackResponse.Data.Customer.Email);
                if (paystackResponse.Data.Metadata.PlanType == "classic")
                {
                    await _role.AssignClassicRole(user);
                }
                else if (paystackResponse.Data.Metadata.PlanType == "standard")
                {
                    await _role.AssignStandardRole(user);
                }

                return Ok();
            }
            else
            {
                return BadRequest("Payment verification failed.");
            }
        }

        


    }
}
