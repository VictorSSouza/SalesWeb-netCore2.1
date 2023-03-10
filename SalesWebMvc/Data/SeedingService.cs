using System;
using Microsoft.EntityFrameworkCore.Internal;
using SalesWebMvc.Models;
namespace SalesWebMvc.Data
{
    public class SeedingService
    {
        private SalesWebMvcContext _context;

        public SeedingService(SalesWebMvcContext context)
        {
            _context = context;
        }
        public void Seed()
        {
            if(_context.Department.Any() 
                || _context.Seller.Any()
                || _context.SalesRecord.Any())
            {
                return; // Bd foi colocado o conteudo
            }

            Department d1 = new Department(1, "Computers");
            Department d2 = new Department(2, "Electronics");
            Department d3 = new Department(3, "Fashion");
            Department d4 = new Department(4, "Books");

            Seller s1 = new Seller(1, "Abner Adao", "abner@gmail.com", new DateTime(1989, 02, 12), 5.100, d1);
        }
    }
}
