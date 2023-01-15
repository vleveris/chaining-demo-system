using ChainingAPI.Models;
using ChainingAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChainingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KnowledgeBaseController: ControllerBase
{
    private readonly KnowledgeBaseService _knowledgeBaseService;

    public KnowledgeBaseController(KnowledgeBaseService knowledgeBaseService) =>
        _knowledgeBaseService = knowledgeBaseService;

    [HttpGet("{id}")]
    public async Task<ActionResult<KnowledgeBase>> GetById(string id)
    {
        var knowledgeBase = await _knowledgeBaseService.GetByIdAsync(id);

        if (knowledgeBase is null)
        {
            return NotFound();
        }

        return knowledgeBase;
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<KnowledgeBase>> GetByName(string name)
    {
        var knowledgeBase = await _knowledgeBaseService.GetByNameAsync(name);

        if (knowledgeBase is null)
        {
            return NotFound();
        }

        return knowledgeBase;
    }

    [HttpGet("get-id/name/{name}")]
    public async Task<ActionResult<string>> GetIdByName(string name)
    {
        var knowledgeBase = await _knowledgeBaseService.GetByNameAsync(name);

        if (knowledgeBase is null)
        {
            return NotFound();
        }

        return knowledgeBase.Id;
    }

   [HttpPost]
    public async Task<IActionResult> Post(KnowledgeBase newKnowledgeBase)
    {
        await _knowledgeBaseService.CreateAsync(newKnowledgeBase);

        return CreatedAtAction(nameof(GetById), new { newKnowledgeBase.Id }, newKnowledgeBase);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, KnowledgeBase updatedKnowledgeBase)
    {
        var knowledgeBase = await _knowledgeBaseService.GetByIdAsync(id);

        if (knowledgeBase is null)
        {
            return NotFound();
        }

        updatedKnowledgeBase.Id = knowledgeBase.Id;

        await _knowledgeBaseService.UpdateAsync(id, updatedKnowledgeBase);

        return NoContent();
    }

    [HttpPut("facts/{name}")]
    public async Task<IActionResult> UpdateFacts(string name, List<string> facts)
    {
        var knowledgeBase = await _knowledgeBaseService.GetByNameAsync(name);

        if (knowledgeBase is null)
        {
            return NotFound();
        }

        knowledgeBase.Facts = facts;

        await _knowledgeBaseService.UpdateAsync(knowledgeBase.Id, knowledgeBase);

        return NoContent();
        }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var knowledgeBase = await _knowledgeBaseService.GetByIdAsync(id);

        if (knowledgeBase is null)
        {
            return NotFound();
        }

        await _knowledgeBaseService.RemoveAsync(id);

        return NoContent();
    }
}