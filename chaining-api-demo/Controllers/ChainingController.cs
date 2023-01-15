using ChainingAPI.Models;
using ChainingAPI.Services;
using ChainingAPI.Helpers;
using Microsoft.AspNetCore.Mvc;
namespace ChainingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChainingController : ControllerBase
{
    private readonly ForwardChainingService _forwardChainingService;
    private readonly BackwardChainingService _backwardChainingService;
    private readonly KnowledgeBaseService _knowledgeBaseService;
    public ChainingController(ForwardChainingService forwardChainingService, BackwardChainingService backwardChainingService, KnowledgeBaseService knowledgeBaseService)
    {
        _forwardChainingService = forwardChainingService;
        _knowledgeBaseService = knowledgeBaseService;
        _backwardChainingService = backwardChainingService;
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ExecutionResult>> Execute(string id, [FromQuery] string goal, [FromQuery] string inferenceMethod, [FromQuery] bool trace)
    {
        var knowledgeBase = await _knowledgeBaseService.GetByIdAsync(id);

        if (knowledgeBase is null)
        {
            return NotFound();
        }
        if (!ChainingHelper.CheckKnowledgeBaseValidity(knowledgeBase))
        {
            return BadRequest("Invalid knowledge base.");
        }
        IChainingService chainingService;
        if (inferenceMethod == "ForwardChaining")
        {
            chainingService = _forwardChainingService;
                    }
        else
        {
            chainingService = _backwardChainingService;
        }
        return chainingService.Execute(knowledgeBase, goal, trace);
    }

    [HttpGet("execute-only/{name}")]
    public async Task<ActionResult<bool>> Execute(string name, [FromQuery] string goal, [FromQuery] string inferenceMethod)
    {
        var knowledgeBase = await _knowledgeBaseService.GetByNameAsync(name);

        if (knowledgeBase is null)
        {
            return NotFound();
        }
        if (!ChainingHelper.CheckKnowledgeBaseValidity(knowledgeBase))
        {
            return BadRequest("Invalid knowledge base.");
        }
        IChainingService chainingService;
        if (inferenceMethod == "ForwardChaining")
        {
            chainingService = _forwardChainingService;
        }
        else
        {
            chainingService = _backwardChainingService;
        }
        var result = chainingService.Execute(knowledgeBase, goal, false);
        return result.GoalAchieved;
    }

}
