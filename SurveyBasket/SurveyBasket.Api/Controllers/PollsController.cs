﻿using Microsoft.AspNetCore.Authorization;
using SurveyBasket.Api.Contracts.Polls;
using SurveyBasket.Api.Services.Polls;

namespace SurveyBasket.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]  // it means that the user must be authenticated (have a valid token) to access any endpoint in this controller

public class PollsController(IPollService pollService) : ControllerBase
{
    private readonly IPollService _pollService = pollService;


    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var polls = await _pollService.GetAllAsync(cancellationToken);
        var response = polls.Adapt<IEnumerable<PollResponse>>();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var pollResult = await _pollService.GetAsync(id, cancellationToken);

        return pollResult.IsSuccess
            ? Ok(pollResult.Value)
            : pollResult.ToProblem(status: StatusCodes.Status404NotFound);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreatePollRequest request, CancellationToken cancellationToken)
    {
        var newPoll = await _pollService.AddAsync(request, cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll);  // 201


        // one of the rest guidelines says that when you create a new resource in the server the client must know how can access this resource 
        // so if return ok() the client can't know where the new resource is located
        // so when we use CreatedAtAction we let the client know where the new resource is located it returns status code 201
        // This response indicates that a resource has been successfully created, and it includes a location header that points to the URI of the newly created resource.
        //  location: https://localhost:7265/api/Polls/2   => when i create a new poll this location property appears in the response header to know the client how can access this new poll
        // CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll) => will generate a URL for the Get action, passing in the ID of the new poll (newPoll.Id).
        // it means when i access or call this url https://localhost:7265/api/Polls/2 it will access the Get Action to get the new poll
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CreatePollRequest request, CancellationToken cancellationToken)
    {
        var result = await _pollService.UpdateAsync(id, request, cancellationToken);

        return result.IsSuccess
            ? NoContent()  // 204
            : result.ToProblem(status: StatusCodes.Status404NotFound);

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _pollService.DeleteAsync(id, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem(status: StatusCodes.Status404NotFound);
    }


    [HttpPut("{id}/togglePublish")]
    public async Task<IActionResult> TogglePublish([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _pollService.TogglePublishStatusAsync(id, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem(status: StatusCodes.Status404NotFound);
    }

    [HttpGet("test-header")]
    public IActionResult TestHeader([FromHeader(Name = "x-lang")] string lang)
    {
        return Ok(lang);
    }

    [HttpGet("test-query")]
    public IActionResult TestQuery([FromQuery] int[] id)
    {
        return Ok(id);
    }

}
