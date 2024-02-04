using System.ComponentModel.DataAnnotations;

namespace TagsCloudVisualization.CustomAttributes;

public class MustBeEnumNameAttribute : ValidationAttribute
{
    private Type enumType;

    public MustBeEnumNameAttribute(Type enumType)
        : base()
    {
        this.enumType = enumType;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var enumNames = Enum.GetNames(enumType);
        if (value is string valueString && Enum.TryParse(enumType, valueString, true, out var result)
                                        && Enum.IsDefined(enumType, result))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult($"The value for {validationContext.DisplayName} must be in {enumType}. " +
                                    $"Available: {String.Join(", ", enumNames)}");
    }
}