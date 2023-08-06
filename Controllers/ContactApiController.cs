using ContactAPISqlite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace ContactAPISqlite.Controllers
{
    [ApiController]
    [Tags("Contact")]
    [Route("api/contacts")]
    [Produces("application/json")]
    public class ContactApiController : ControllerBase
    {
        private readonly ContactDbContext _context;

        public ContactApiController(ContactDbContext context) => _context = context;

        [HttpGet]
        [SwaggerOperation("Get")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IEnumerable<Contact>), StatusCodes.Status200OK)]
        public ActionResult Get()
        {
            if (!_context.Contacts.Any())
                return NoContent();

            return Ok(_context.Contacts.ToList());
        }

        [HttpPost]
        [SwaggerOperation("Create")]
        [ProducesResponseType(typeof(Contact), StatusCodes.Status201Created)]
        public ActionResult Create(Contact contact)
        {
            _context.Contacts.Add(contact);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Get), contact);
        }

        [SwaggerOperation("GetContact")]
        [HttpGet("{id:int}", Name = "GetContact")]
        [ProducesResponseType(typeof(Contact), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public ActionResult Get(int id)
        {
            Contact model = _context.Contacts.Find(id);

            if (model == null)
                return NotFound();

            return Ok(model);
        }

        [HttpPut("{id:int}")]
        [SwaggerOperation("Update")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public ActionResult Update(int id, [FromBody] Contact contact)
        {
            if (!ContactExists(id))
                return BadRequest();

            contact.Id = id;

            _context.Entry(contact).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [SwaggerOperation("Delete")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public ActionResult Delete(int id)
        {
            if (!ContactExists(id))
                return BadRequest();

            Contact contact = _context.Contacts.Find(id);

            _context.Remove(contact);
            _context.SaveChanges();

            return NoContent();
        }

        private bool ContactExists(int id) => _context.Contacts.Any(c => c.Id == id);
    }
}