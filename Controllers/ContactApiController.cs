using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactAPISqlite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactAPISqlite.Controllers
{
    [ApiController]
    [Route("api/contacts")]
    [Produces("application/json")]
    public class ContactApiController : ControllerBase
    {
        private readonly ContactDbContext _context;

        public ContactApiController(ContactDbContext context) => _context = context;

        [HttpGet]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IEnumerable<Contact>), StatusCodes.Status200OK)]
        public ActionResult Get()
        {
            if (!_context.Contacts.Any())
                return NoContent();

            return Ok(_context.Contacts.ToList());
        }

        [HttpPost]
        [ProducesResponseType(typeof(Contact), StatusCodes.Status201Created)]
        public ActionResult Create(Contact contact)
        {
            _context.Contacts.Add(contact);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Get), contact);
        }

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
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public ActionResult Delete(int id)
        {
            if (!ContactExists(id))
                return BadRequest();

            return Ok(_context.Contacts.Find(id));
        }

        private bool ContactExists(int id) => _context.Contacts.Any(c => c.Id == id);
    }
}