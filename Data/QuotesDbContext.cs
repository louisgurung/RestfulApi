using Microsoft.EntityFrameworkCore;
using QuoteApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteApi.Data
{
    public class QuotesDbContext:DbContext
    {
        public QuotesDbContext(DbContextOptions<QuotesDbContext> options):base(options)
        {

        }
        public DbSet<Quote> Quotes { get; set; }
    }
}
