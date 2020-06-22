using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuoteApi.Data;
using QuoteApi.Models;

namespace QuoteApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotesController : ControllerBase
    {
        private QuotesDbContext _context;

        public QuotesController(QuotesDbContext quotesDbContext) {
            _context = quotesDbContext;
        }
        // GET: api/Quotes                      //api/Quotes?sort=asc    
        [HttpGet]
        //[ResponseCache(Duration = 60, Location =ResponseCacheLocation.Client)]
        public IActionResult Get(string sort)             //IActionResult to return status code instead of IEnumerable
        {

            // if (_context.Quotes.Count() == 3)
            //return StatusCode(StatusCodes.Status204NoContent);
            //for status code 200
            //return StatusCode(200)......//can return status code like this or StatusCode(StatusCodes.)
            // return Ok(_context.Quotes);

            IQueryable<Quote> quotes;
            switch (sort) {
                case "desc":
                    quotes= _context.Quotes.OrderByDescending(q => q.CreatedAt);
                    break;
                case "asc":
                    quotes = _context.Quotes.OrderBy(q => q.CreatedAt);
                    break;
                default:
                    quotes = _context.Quotes;
                    break;
            }

            return Ok(quotes);
        }

        // GET: api/Quotes/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var quotes = _context.Quotes.Find(id);
            if (quotes == null) {
                return NotFound("No record found..");
            }
            return Ok(quotes);
        }

        //[HttpGet("[action]/{id}")]      ==> api/Quotes/Test/1
        //attribute routing
        //public int Test(int id) {
        //    return id;
        //}

        // POST: api/Quotes
        [HttpPost]
        public IActionResult Post([FromBody] Quote quote)
        {
            _context.Quotes.Add(quote);
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);

        }

        // PUT: api/Quotes/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Quote quote)
        {
            var entity = _context.Quotes.Find(id);
            if (entity == null)
            {
                return NotFound("No record found against this id");
            }
            else
            {
                entity.Author = quote.Author;
                entity.Description = quote.Description;
                entity.Title = quote.Title;
                entity.Type = quote.Type;
                entity.CreatedAt = quote.CreatedAt;
                _context.SaveChanges();
                return Ok();
            }
           
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var quote = _context.Quotes.Find(id);
            if (quote == null)
            {
                return NotFound("No record found for this id ");
            }
            else {
                _context.Quotes.Remove(quote);
                _context.SaveChanges();
                return Ok("Quote deleted...");
            }

            
        }

        [HttpGet("[action]")]
        public IActionResult PagingQuote(int? pageNumber, int? pageSize)
        {
            var quotes = _context.Quotes;

            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 5;

            return Ok(quotes.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult SearchQuote(string type) {
            var quotes = _context.Quotes.Where(q => q.Type.StartsWith(type));
            return Ok(quotes);
        }

    }
}
