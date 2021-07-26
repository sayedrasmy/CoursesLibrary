using System;
using System.ComponentModel.DataAnnotations;
using CourseLibrary.API.Models;

namespace CourseLibrary.API.ValidationAttributes
{
    public class CourseTitleMustBeDifferentFromDescriptionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var course = (CourseForManipulationDto)validationContext.ObjectInstance;
            if (course.Title == course.Description)
            {
                return new ValidationResult(
                    /*"The provided description should be diffrent from the Title."*/
                    ErrorMessage, new[] { nameof(CourseForCreationDto) }
                );
            }

            return ValidationResult.Success;
        }
    }
}
