using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CreditCards.Models;

namespace CreditCards.Data
{
    public class CreditCardsContext : DbContext
    {
        public CreditCardsContext (DbContextOptions<CreditCardsContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<CreditCards.Models.CreditCard> CreditCard { get; set; } = default!;
    }
}
