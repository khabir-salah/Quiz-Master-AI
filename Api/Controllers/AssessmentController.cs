using Application.Features.Command.Create.Assesment;
using Application.Features.DTOs;
using Application.Features.Interfaces.IService;
using Application.Features.Queries.AssessmentResult;
using Application.Features.Queries.RetakeAssessment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/Assessment")]
    [ApiController]
    public class AssessmentController( IMediator _mediator, ITextGenerator _textGenerator, ITextAssessmentService _textService) : ControllerBase
    {
        [HttpPost("RetakeAssesment/{AssesmentId}")]
        [Authorize]
        public async Task<IActionResult> RetakeAssessment([FromQuery] RetakeAssessmentCommandModel query)
        {
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

        [HttpPost("GetTextAssessment")]
        public async Task<IActionResult> GetTextAssessment()
        {
            var textDocument = await _textService.GetAllTextAssessments();
            if(!textDocument.IsSuccessful) return BadRequest(textDocument.Message);
            return Ok(textDocument.Result);
        }

        [HttpPost("TakeAssesment/{AssesmentId}")]
        public async Task<IActionResult> TakeAssessment([FromQuery] Guid AssesmentId)
        {
            var result = await _textService.TakeAssessment(AssesmentId);
            if(!result.IsSuccessful) return BadRequest(result.Message);
            return Ok(result.Result);
        }
    }
}
