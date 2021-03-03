using Microsoft.AspNetCore.Mvc;
using OxfordBookShoppe.Data.Models;
using OxfordBookShoppe.Data.Services;

namespace OxfordBookShoppe.Api.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class BookController : ControllerBase
    {
        private readonly IBookData _bookData;

        public BookController(IBookData bookData)
        {
            _bookData = bookData;
        }

        public IActionResult GetBooks()
        {
            var books = _bookData.GetAll();
            if (books == null)
            {
                return NotFound();
            }
            return Ok(books);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var book = _bookData.GetById(id);
            if (book.Id == 0)
            {
                return NotFound();
            }
            return Ok(book);

        }
        [HttpPost]
        public IActionResult Create([FromBody] Book book)
        {
            if(ModelState.IsValid)
            {
                _bookData.Add(book);
                return Ok();
            }
            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public IActionResult Edit(int id, [FromBody] Book book)
        {
            var bookToUpdate = _bookData.GetById(id);
            if (bookToUpdate.Id == 0)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _bookData.Update(book);
                return Ok();
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var bookToDelete = _bookData.GetById(id);
            if(bookToDelete.Id == 0)
            {
                return BadRequest();
            }
            if(ModelState.IsValid)
            {
                _bookData.Delete(id);
                return Ok();
            }
            return NotFound();
        }
    }
}

