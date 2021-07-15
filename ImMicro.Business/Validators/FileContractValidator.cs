using System.IO;
using FluentValidation;
using ImMicro.Common.Constans; 
using ImMicro.Common.Validation;
using ImMicro.Contract.Dtos;
using Microsoft.AspNetCore.Http;

namespace ImMicro.Business.Validators
{
    public class FileContractValidator : AbstractValidator<FileContract>
    {
        public FileContractValidator()
        {
            RuleSet("importFile", () =>
            {
                RuleFor(x => x.FileData)
                    .NotNull().WithMessage("File can't be null")
                    .Must(IsHaveExtension).WithMessage("Extension can't be null.")
                    .Must(file => IsValidMime(file, new[] {AppConstants.Json})).WithMessage("Wrong file type!");
            });
        }

        private static bool IsValidMime(IFormFile file, string[] fileTypes)
        {
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            var fileBytes = ms.ToArray();
            return MimeValidation.IsValidMime(fileBytes, file.FileName, fileTypes);
        }

        private static bool IsHaveExtension(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            return !string.IsNullOrEmpty(extension);
        }
    }
}