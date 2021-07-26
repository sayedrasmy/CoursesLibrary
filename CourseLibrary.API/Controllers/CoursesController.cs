using System;
using System.Collections.Generic;
using System.Diagnostics;
using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Models;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CourseLibrary.API.Controllers
{
    [Route("api/authors/{authorId}/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseLibraryRepository _repo;
        private readonly IMapper _mapper;

        public CoursesController(ICourseLibraryRepository repo, IMapper mapper)
        {
            _repo = repo ??
                throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public ActionResult GetCourses(Guid authorId)
        {
            if (!_repo.AuthorExists(authorId))
                return NotFound();
            var courseEntites = _mapper.Map<IEnumerable<CourseDto>>(_repo.GetCourses(authorId));
            return Ok(courseEntites);
        }

        [HttpGet("{courseId}", Name = "GetCourse")]
        public ActionResult GetCourse(Guid authorId, Guid courseId)
        {
            if (!_repo.AuthorExists(authorId))
                return NotFound();
            var courseFromRepo = _repo.GetCourse(authorId, courseId);
            if (courseFromRepo == null)
                return NotFound();
            return Ok(_mapper.Map<CourseDto>(courseFromRepo));
        }

        [HttpPost]
        public ActionResult CreateCourseForAutor(Guid authorId, CourseForCreationDto course)
        {
            if (!_repo.AuthorExists(authorId))
            {
                return NotFound();
            }

            var courseEntity = _mapper.Map<Entities.Course>(course);
            _repo.AddCourse(authorId, courseEntity);
            _repo.Save();

            var courseToReturn = _mapper.Map<CourseDto>(courseEntity);
            return CreatedAtRoute("GetCourse",
                new { authorId = authorId, courseId = courseToReturn.Id },
                courseToReturn);
        }

        [HttpPut("{courseId}")]
        public IActionResult UpdateCourseForAutor(Guid authorId, Guid courseId, CourseForUpdateDto course)
        {
            if (!_repo.AuthorExists(authorId))
            {
                return NotFound();
            }

            var courseForAuthorFromRepo = _repo.GetCourse(authorId, courseId);

            if (courseForAuthorFromRepo == null)
            {
                var courseToAdd = _mapper.Map<Course>(course);
                courseToAdd.Id = courseId;

                _repo.AddCourse(authorId, courseToAdd);

                _repo.Save();

                var courseToReturn = _mapper.Map<CourseDto>(courseToAdd);

                return CreatedAtRoute("GetCourse",
                    new { authorId, courseId = courseToReturn.Id },
                    courseToReturn);
            }

            // map the entity to a CourseForUpdateDto
            // apply the updated field values to that dto
            // map the CourseForUpdateDto back to an entity
            _mapper.Map(course, courseForAuthorFromRepo);

            _repo.UpdateCourse(courseForAuthorFromRepo);

            _repo.Save();
            return NoContent();
        }
        [HttpPatch("{courseId}")]
        public ActionResult PartiallyUpdatedCourseForAuthor(Guid authorId,
            Guid courseId,
            JsonPatchDocument<CourseForUpdateDto> patchDocument)
        {
            if (!_repo.AuthorExists(authorId))
            {
                return NotFound();
            }

            var courseForAuthorFromRepo = _repo.GetCourse(authorId, courseId);

            if (courseForAuthorFromRepo == null)
            {
                var courseDto = new CourseForUpdateDto();
                patchDocument.ApplyTo(courseDto);
                var courseToAdd = _mapper.Map<Course>(courseDto);
                courseToAdd.Id = courseId;
                var valid = Validate(courseDto);
                if (valid != null)
                {
                    return valid;
                }
                //if (!TryValidateModel(courseToPatch))
                //{
                //    return ValidationProblem(ModelState);
                //}
                _repo.AddCourse(authorId, courseToAdd);
                _repo.Save();

                var courseToReturn = _mapper.Map<CourseDto>(courseToAdd);

                return CreatedAtRoute("GetCourse",
                    new { authorId = authorId, courseId = courseToReturn.Id },
                    courseToReturn);
            }

            var courseToPatch = _mapper.Map<CourseForUpdateDto>(courseForAuthorFromRepo);
            //ToDo: add validation

            patchDocument.ApplyTo(courseToPatch, ModelState);
            var validCourseToPatch = Validate(courseToPatch);
            if (validCourseToPatch != null)
            {
                return validCourseToPatch;
            }
            
            _mapper.Map(courseToPatch, courseForAuthorFromRepo);
            _repo.UpdateCourse(courseForAuthorFromRepo);
            _repo.Save();
            return NoContent();
        }

        public override ActionResult ValidationProblem(
            [ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices
                .GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }


        [HttpDelete("{courseId}")]
        public ActionResult DeleteCourseForAuthor(Guid authorId, Guid courseId)
        {
            if (!_repo.AuthorExists(authorId))
            {
                return NotFound();
            }
            var courseForAuthorFromRepo = _repo.GetCourse(authorId, courseId);
            if (courseForAuthorFromRepo == null)
            {
                return NotFound();
            }
            _repo.DeleteCourse(courseForAuthorFromRepo);
            _repo.Save();

            return NoContent();

        }




        public ActionResult Validate(CourseForUpdateDto courseToPatch)
        {
            if (!TryValidateModel(courseToPatch))
            {
                return ValidationProblem(ModelState);
            }
            return null;
        }
    }
}