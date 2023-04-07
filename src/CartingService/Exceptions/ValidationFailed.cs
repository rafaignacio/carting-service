using FluentValidation.Results;

namespace CartingService.Exceptions;

public class ValidationFailed {

    public IEnumerable<ValidationFailure> Errors { get; set; }

    public ValidationFailed(IEnumerable<ValidationFailure> errors)
    {
        Errors = errors;
    }

    public ValidationFailed(ValidationFailure error) : this(new[] { error }) { 
    }
}