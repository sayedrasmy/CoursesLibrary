using CourseLibrary.API.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.Models
{
    [CourseTitleMustBeDifferentFromDescriptionAttribute(ErrorMessage = "العنوان لازم يكون مختلف عن الشرح التفصيلي ")]
    public abstract class CourseForManipulationDto
    {
        [Required(ErrorMessage = "You must fill the Title field")]
        [MaxLength(100, ErrorMessage = "Title shouldn't have more than 100 letters")]
        public string Title { get; set; }
        [MaxLength(1500, ErrorMessage = "Description shouldnt have more than 1500 letters")]
        public virtual string Description { get; set; }
    }
}
