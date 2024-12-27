﻿
namespace SurveyBasket.Api.Errors;

public static class PollErrors
{
    public static readonly Error PollNotFound =
        new("Poll.NotFound", "No poll was found with the given ID", StatusCodes.Status400BadRequest);

    public static readonly Error DuplicatedPollTitle =
       new("Poll.DuplicatedTitle", "Another poll with the same title already exists", StatusCodes.Status409Conflict);

}
