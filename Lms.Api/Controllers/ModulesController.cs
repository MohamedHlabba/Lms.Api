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
using Lms.Core.Dto;
using AutoMapper;

namespace Lms.Api.Controllers
{
    [Route("api/courses/{CourseId}/modules")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly IUnitOfwork uOfwork;
        private readonly IMapper mapper;

        public ModulesController( IUnitOfwork uOfwork, IMapper mapper)
        {
            this.uOfwork = uOfwork;
            this.mapper = mapper;
        }

        // GET: api/Modules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModuleDto>>> GetAllModules()
        {
            var modules = await uOfwork.ModuleRepository.GetAllModules();
            var moduleDto = mapper.Map<IEnumerable<ModuleDto>>(modules);
            return Ok(moduleDto);
        }

       // GET: api/Modules/5
        [HttpGet("{title}")]
        public async Task<ActionResult<ModuleDto>> GetModule(string title)
        {
            if (string.IsNullOrEmpty(title)) return BadRequest();

            var result = await uOfwork.ModuleRepository.GetModule(title);

            if (result is null) return NotFound();

            var dto = mapper.Map<ModuleDto>(result);

            return Ok(dto);
        }
        //[HttpGet("{title}")]
        //public ActionResult<ModuleDto> GettModuleForCourse(int id, string title)
        //{
        //    if (!uOfwork.CourseRepository.CourseExists(id))
        //    {
        //        return StatusCode(404);
        //    }
        //    var ModuleForCourseFromRepo = uOfwork.ModuleRepository.GetModuleForCourse(id,title);
        //    if (ModuleForCourseFromRepo is null)
        //    {

        //        return StatusCode(404);
        //    }
        //    return Ok(mapper.Map<ModuleDto>(ModuleForCourseFromRepo));
        //}

        // PUT: api/Modules/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{title}")]
        public async Task<IActionResult> PutModule(string title, ModuleDto dto)
        {
            var module =await uOfwork.ModuleRepository.GetModule(title);
            if (module is null)
            {
                return BadRequest();
            }

            mapper.Map(dto, module);
            if (await uOfwork.ModuleRepository.SaveAsync())
            {
                return Ok(mapper.Map<ModuleDto>(module));
            }
            else
            {
                return StatusCode(500);
            }
        }

        // POST: api/Modules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ModuleDto>> PostModule(ModuleDto dto)
        {
            if (await uOfwork.ModuleRepository.GetModule(dto.Title) != null)
            {
                ModelState.AddModelError("Title", "Module is  in use");
                return BadRequest(ModelState);
            }
            var module = mapper.Map<Module>(dto);
            await uOfwork.ModuleRepository.AddAsync(module);
            if (await uOfwork.ModuleRepository.SaveAsync())
            {
                var model = mapper.Map<ModuleDto>(module);
                return CreatedAtAction(nameof(GetModule), new { courseId=model.CourseId, title = model.Title }, model);
            }
            else
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE: api/Modules/5
        [HttpDelete("{title}")]
        public async Task<IActionResult> DeleteModule(string title)
        {
            var module = await uOfwork.ModuleRepository.GetModule(title);
            if (module == null)
            {
                return NotFound();
            }

            uOfwork.ModuleRepository.DeleteAsync(module);
            await uOfwork.CompleteAsync();
            return NoContent();
        }

        private bool ModuleExists(int id)
        {
            return uOfwork.ModuleRepository.ModuleExists(id);
        }
    }
}
