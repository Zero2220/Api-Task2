using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversityApi.Data;
using UniversityApi.Dtos;

namespace UniversityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly UniDatabase _context;

        public StudentController(UniDatabase context)
        {
            _context = context;
        }

        [HttpGet("")]
        public ActionResult<List<GroupGetDto>> GetStudents()
        {
            List<Student> groups = _context.Students.ToList();

            List<GetStudentDto> result = groups.Select(x => new GetStudentDto
            {
                FullName = x.FullName,
                Email = x.Email,
                BirthDate = x.BirthDate,
            }).ToList();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<GroupGetDto> GetStudentById(int id)
        {
            var data = _context.Students.FirstOrDefault(x => x.Id == id);

            if (data == null)
            {
                return NotFound();
            }

            GetStudentDto dto = new GetStudentDto
            {
                Email = data.Email,
                FullName = data.FullName,
                BirthDate = data.BirthDate,
            };
            return Ok(dto);
        }

        [HttpPost("")]
        public ActionResult Create(CreateStudentDto createDto)
        {
            Student student = new Student
            {
                GroupId = createDto.GroupId,
                FullName = createDto.FullName,
                Email = createDto.Email,
                BirthDate= createDto.BirthDate,
                
            };

            _context.Students.Add(student);
            _context.SaveChanges();
            return StatusCode(201);
        }

        [HttpPut("{id}")]
        public ActionResult Edit(int id, EditStudentDto editDto)
        {
            var student = _context.Students.FirstOrDefault(x => x.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            student.Email = editDto.Email;
            student.FullName = editDto.FullName;
            student.BirthDate = editDto.BirthDate;
            student.GroupId = editDto.GroupId;

            _context.Update(student);
            _context.SaveChanges();
            return StatusCode(204);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Student student = _context.Students.FirstOrDefault(x => x.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            _context.SaveChanges();
            return StatusCode(204);
        }
    }
}
