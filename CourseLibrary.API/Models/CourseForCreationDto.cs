using System.ComponentModel.DataAnnotations;
using CourseLibrary.API.ValidationAttributes;

namespace CourseLibrary.API.Models
{

    public class CourseForCreationDto : CourseForManipulationDto //: IValidatableObject
    {

        // public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        // {
        //     if(Title == Description){
        //         yield return new ValidationResult(
        //             "The provided description should be diffrent from the Title.", new [] {nameof(CourseForCreationDto)}
        //         );
        //     }
        // }
        
        public override string Description { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}