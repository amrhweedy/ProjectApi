﻿namespace SurveyBasket.Api.Models;

public class Poll
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public static implicit operator PollResponse(Poll poll)
    {
        return new PollResponse()
        {
            Id = poll.Id,
            Title = poll.Title,
            Description = poll.Description
        };
    }
}
