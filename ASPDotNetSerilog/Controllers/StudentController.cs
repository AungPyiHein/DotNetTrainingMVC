using Microsoft.AspNetCore.Mvc;
using Serilog;


namespace ASPDotNetSerilog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        public StudentController(ILogger<StudentController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet]
        public IActionResult Read()
        {
            _logger.LogInformation("Reading student data...");
            var students = new List<string> { "Alice", "Bob", "Charlie" };
            Log.Information("Student data read successfully: {@Students}", students);
            return Ok(students);
        }
        [HttpPost]
        public IActionResult Create([FromBody] string name)
        {
            _logger.LogInformation("Creating student: {Name}", name);
            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogWarning("Name is empty.");
                return BadRequest("Name cannot be empty.");
            }
            _logger.LogInformation("Student {Name} created successfully.", name);
            return Ok($"Student {name} created successfully.");
        }
        [HttpPost("{id}")]
        public IActionResult GetStudent(int id)
        {
            try
            {
                Log.Information("Getting student with ID: {Id}", id);
                if (id < 0)
                {
                    _logger.LogWarning("Invalid student ID: {Id}", id);
                    return BadRequest("Invalid student ID.");
                }
                var student = $"Student{id}";
                _logger.LogInformation("Student with ID {Id} retrieved successfully: {Student}", id, student);
                return Ok(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving student with ID: {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the student.");
            }
        }
    }
}
