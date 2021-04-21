using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lms.Api.Data;
using Lms.Core.Entities;
using Lms.Core.Repositories;
using Lms.Data.Repositories;
using AutoMapper;
using Lms.Core.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Lms.Api.ResourceParameters;

namespace Lms.Api.Controllers
{
    [Route("api/courses")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IUnitOfwork uofwork;
        private readonly IMapper mapper;

        public CoursesController( IUnitOfwork uofwork, IMapper mapper)
        {
            this.uofwork = uofwork;
            this.mapper = mapper;
        }

        // GET: api/Courses
        //[HttpGet]
        //[HttpHead]
        //public async Task<ActionResult<IEnumerable<CourseDto>>> GetAllCourses([FromQuery]string title, string searchQuery,bool includeModules)
        //{
        //    //throw new Exception("test exception");

        //    var res = await uofwork.CourseRepository.GetAllCourses(title, includeModules,searchQuery);
        //    var courseDto = mapper.Map<IEnumerable<CourseDto>>(res);
        //    return Ok(courseDto);
        //}
        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetAllCourses([FromQuery]CourseResourceParameters courseResourceParameters,bool includeModules)
        {
            //throw new Exception("test exception");

            var res = await uofwork.CourseRepository.GetAllCourses(courseResourceParameters, includeModules);
            var courseDto = mapper.Map<IEnumerable<CourseDto>>(res);
            return Ok(courseDto);
        }

       // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourse(int id, bool includeModules = true)
        {
            var result = await uofwork.CourseRepository.GetCourse(id, includeModules);

            if (result is null) return NotFound();

            var dto = mapper.Map<CourseDto>(result);

            return Ok(dto);
        }
        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, CourseDto dto)
        {
            var course = await uofwork.CourseRepository.GetCourse(id, false);

            if (course is null) return StatusCode(StatusCodes.Status404NotFound);

            mapper.Map(dto, course);

            // repo.Update(eventday);
            if (await uofwork.CourseRepository.SaveAsync())
            {
                return Ok(mapper.Map<CourseDto>(course));
           // return NoContent();
            }
            else
            {
                return StatusCode(500);
            }
        }

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CourseDto>> PostCourse(CourseDto dto)
        {
            if (await uofwork.CourseRepository.GetCourse(dto.Id,false) != null)
            {
                ModelState.AddModelError("Title", "Course is  in use");
                return BadRequest(ModelState);
            }
            var course = mapper.Map<Course>(dto);
            await uofwork.CourseRepository.AddAsync(course);
            if (await uofwork.CourseRepository.SaveAsync())
            {
                var model = mapper.Map<CourseDto>(course);
                return CreatedAtAction(nameof(GetCourse), new { id = model.Id }, model);
            }
            else
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await uofwork.CourseRepository.GetCourse(id,false);
            if (course == null)
            {
                return NotFound();
            }

            uofwork.CourseRepository.DeleteAsync(course);
            await uofwork.CompleteAsync();
            return NoContent();
        }

        private bool CourseExists(int id)
        {
            return uofwork.CourseRepository.CourseExists(id);
        }
        [HttpPatch("{id}")]
        public async Task<ActionResult<CourseDto>> PatchECourse(int id, JsonPatchDocument<CourseDto> patchDocument)
        {

            var course = await uofwork.CourseRepository.GetCourse(id, true);

            if (course is null) return NotFound();

            var dto = mapper.Map<CourseDto>(course);

            patchDocument.ApplyTo(dto, ModelState);

            if (!TryValidateModel(dto))
                return BadRequest(ModelState);

            mapper.Map(dto, course);

            if (await uofwork.CourseRepository.SaveAsync())
                return Ok(mapper.Map<CourseDto>(course));
            else
                return StatusCode(500);

        }
    }
}
