using LibraryApi.Domain;
using LibraryApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    [Produces("application/json")]
    public class BooksController : Controller
    {
        LibraryDataContext Context;

        public BooksController(LibraryDataContext context)
        {
            Context = context;
        }

        [HttpPut("/books/{id:int}/genre")]
        public async Task<IActionResult> UpdateGenre(int id,[FromBody] string newGenre)
        {
            var book = await Context.Books
                .Where(b => b.Id == id && b.InInventory)
                .SingleOrDefaultAsync();
            if(book == null)
            {
                return NotFound();
            }
            else
            {
                book.Genre = newGenre;
                await Context.SaveChangesAsync();
                return NoContent();
            }
        }


        /// <summary>
        /// Removes a book from inventory
        /// </summary>
        /// <param name="id">the id of the book you want to remove</param>
        /// <returns></returns>
        [HttpDelete("/books/{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RemoveABook(int id)
        {
            var book = await Context.Books.Where(b => b.Id == id && b.InInventory).SingleOrDefaultAsync();
            if(book != null)
            {
                book.InInventory = false;
                await Context.SaveChangesAsync();
            }
            return NoContent();
        }



        [HttpPost("/Books")]
        [ProducesResponseType(typeof(GetBookResponseDocument), 201)]
        public async Task<IActionResult> AddABook([FromBody] PostBookRequest bookToAdd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = new Book
            {
                Title = bookToAdd.Title,
                Author = bookToAdd.Author,
                Genre = bookToAdd.Genre ?? "Unknown"
            };
            Context.Books.Add(book);
            await Context.SaveChangesAsync();

            var bookToReturn = new GetBookResponseDocument
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Genre = book.Genre
            };

            return CreatedAtRoute("books#getabook", new { id = book.Id }, bookToReturn );
        }

        [HttpGet("/books/{id:int}", Name = "books#getabook")]
        public async Task<IActionResult> GetABook(int id)
        {
            var result = await Context.Books
                .Where(b => b.InInventory == true)
                .Select(b => new GetBookResponseDocument
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Genre = b.Genre
                }).SingleOrDefaultAsync(b => b.Id == id);

            if (result == null)
            {
                return NotFound("That book isn't in our library");
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet("/books")]
        public async Task<IActionResult> GetAllBooks([FromQuery] string genre = "all")
        {
            var response = new GetBooksResponseCollection();
            var allBooks = Context.Books
                .Where(b => b.InInventory == true)
                .Select(b => new BookSummaryItem
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Genre = b.Genre
            });
            if (genre != "all")
            {
                allBooks = allBooks.Where(b => b.Genre == genre);
            }
            response.Books = await allBooks.ToListAsync();
            response.GenreFilter = genre;

            return Ok(response);
        }
    }
}
