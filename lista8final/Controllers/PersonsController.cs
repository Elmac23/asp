using Microsoft.AspNetCore.Mvc;
using lista8final.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace lista8final.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonsController : ControllerBase
    {
        private static readonly List<Person> _persons = new()
        {
            new Person { Id = 1, FirstName = "Jan", LastName = "Kowalski", Age = 30, Email = "jan.kowalski@example.com" },
            new Person { Id = 2, FirstName = "Anna", LastName = "Nowak", Age = 25, Email = "anna.nowak@example.com" }
        };

        [HttpGet]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<IEnumerable<Person>> GetAll()
        {
            return Ok(_persons);
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<Person> Get(int id)
        {
            var p = _persons.FirstOrDefault(x => x.Id == id);
            if (p == null) return NotFound();
            return Ok(p);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<Person> Create([FromBody] Person person)
        {
            person.Id = _persons.Any() ? _persons.Max(p => p.Id) + 1 : 1;
            _persons.Add(person);
            return CreatedAtAction(nameof(Get), new { id = person.Id }, person);
        }
    }
}
