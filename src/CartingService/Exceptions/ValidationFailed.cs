using FluentValidation.Results;

namespace CartingService.Exceptions;

public record ValidationFailed(IEnumerable<ValidationFailure> Errors) {
    public ValidationFailed(ValidationFailure error) : this(new[] { error }) { 
    }
}