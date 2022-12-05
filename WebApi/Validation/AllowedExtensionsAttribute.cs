using System.ComponentModel.DataAnnotations;

namespace WebApi.Validation;

public class AllowedExtensionsAttribute : ValidationAttribute
{
    private readonly string[] _extensions;

    public AllowedExtensionsAttribute(string[] extensions)
    {
        _extensions = extensions;
    }

    //Not the safest way to do this but will do
    protected override ValidationResult IsValid(
        object? value, ValidationContext validationContext)
    {
        switch (value)
        {
            case IFormFile file:
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower())) return new ValidationResult(GetErrorMessage());
                break;
            }
            case IEnumerable<IFormFile> files:
            {
                foreach (var f in files)
                {
                    var extension = Path.GetExtension(f.FileName);
                    if (!_extensions.Contains(extension.ToLower())) return new ValidationResult(GetErrorMessage());
                }

                break;
            }
        }

        return ValidationResult.Success;
    }

    public string GetErrorMessage()
    {
        return "This file extension is not allowed";
    }
}