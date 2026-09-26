using LearnForge.Domain.Exceptions;

namespace LearnForge.Domain.Common;

internal static class DescriptionValidator
{
    public static string? Validate(string? description, bool required, string entityLabel)
    {
        return required && string.IsNullOrWhiteSpace(description)
            ? throw new DomainException($"{entityLabel} description is required.")
            : (description?.Trim());
    }
}
