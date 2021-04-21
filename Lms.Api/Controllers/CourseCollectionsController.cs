using AutoMapper;
using Lms.Core.Dto;
using Lms.Core.Entities;
using Lms.Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lms.Api.Controllers
{
    [ApiController]
    [Route("api/coursecollections")]
    public class CourseCollectionsController : ControllerBase
    {
        private readonly IUnitOfwork uofwork;
        private readonly IMapper mapper;
        public CourseCollectionsController(IUnitOfwork uofwork, IMapper mapper)
        {
            this.uofwork = uofwork;
            this.mapper = mapper;
        }
        [HttpPost]
        public ActionResult<IEnumerable<CourseDto>> CreateCourseCollection(IEnumerable<CourseForCreationDto> courseCollection)
        {
            var courseEntities = mapper.Map<IEnumerable<Course>>(courseCollection);
            foreach (var course in courseEntities)
            {
                uofwork.CourseRepository.AddAsync(course);
            }
            uofwork.CourseRepository.SaveAsync();
            return Ok();
        }
    }
}
