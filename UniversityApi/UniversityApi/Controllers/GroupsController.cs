using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using UniversityApi.Data;
using UniversityApi.Dtos;

namespace UniversityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly UniDatabase _context;

        public GroupsController(UniDatabase context)
        {
            _context = context;
        }

        [HttpGet("")]
        public ActionResult<List<GroupGetDto>> GetGroup()
        {
            List<Group> groups = _context.Groups.Where(x=>x.IsDeleted ==false).ToList();

            List<GroupGetDto> result = groups.Select(x => new GroupGetDto
            {
                No = x.No,
                Limit = x.Limit
            }).ToList();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<GroupGetDto> GetById(int id)
        {
            var data = _context.Groups.Where(x=>x.IsDeleted==false).FirstOrDefault(x => x.Id == id);

            if (data == null)
            {
                return NotFound();
            }

            GroupGetDto dto = new GroupGetDto
            {
                No = data.No,
                Limit = data.Limit
            };
            return Ok(dto);
        }

        [HttpPost("")]
        public ActionResult Create(CreateDto createDto)
        {
            Group group = new Group
            {
                No = createDto.No,
                Limit = createDto.Limit,
                CreatedAt = DateTime.Now
            };

            _context.Groups.Add(group);
            _context.SaveChanges();
            return StatusCode(201);
        }

        [HttpPut("{id}")]
        public ActionResult Edit(int id, EditDto editDto)
        {
            var group = _context.Groups.FirstOrDefault(x => x.Id == id);

            if (group == null)
            {
                return NotFound();
            }

            group.No = editDto.No;
            group.Limit = editDto.Limit;
            group.ModifiedAt = DateTime.Now;

            _context.Update(group);
            _context.SaveChanges();
            return StatusCode(204); 
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Group group = _context.Groups.FirstOrDefault(x => x.Id == id);

            if (group == null)
            {
                return NotFound();
            }
            group.IsDeleted = true;

            _context.Groups.Update(group);
            _context.SaveChanges();
            return StatusCode(204); 
        }
    }
}

