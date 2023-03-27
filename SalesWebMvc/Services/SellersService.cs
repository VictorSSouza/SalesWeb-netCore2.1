using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using System.Collections.Generic;
using System.Linq;

namespace SalesWebMvc.Services
{
    public class SellersService
    {
        private readonly SalesWebMvcContext _context;

        public SellersService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public List<Seller> FindAll()
        {
            return _context.Seller.ToList();
        }
        public void Insert(Seller obj)
        {
            _context.Add(obj);
            _context.SaveChanges();
        }
        public Seller FindById(int id)
        {
            return _context.Seller.Include(obj => obj.Department)
                   .FirstOrDefault(obj => obj.Id == id); // indica a igualdade de valores para procurar o seller 
        }
        public void Remove(int id)
        {
            var obj = this.FindById(id);
            _context.Seller.Remove(obj);
            _context.SaveChanges();
        }
    }
}
