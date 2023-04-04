using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Services.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Services
{
    public class SellersService
    {
        private readonly SalesWebMvcContext _context;

        public SellersService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();
        }
        public async Task InsertAsync(Seller obj)
        {
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }
        public async Task<Seller> FindByIdAsync(int id) // Procura a Id no bd
        {
            return await _context.Seller
                    .Include(obj => obj.Department)
                   .FirstOrDefaultAsync(obj => obj.Id == id); 
        }
        public async Task RemoveAsync(int id)
        {
            try
            {
                var obj = await this.FindByIdAsync(id);
                _context.Seller.Remove(obj);
                _context.SaveChanges();
            }
            catch(DbUpdateException ) 
            {
                throw new IntegrityException("Can´t delete seller because he/she has sales");            
            }
        }
        public async Task UpdateAsync(Seller obj) 
        {
            bool hasAny = await _context.Seller.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny)
            {
                throw new NotFoundException("Id not found!");
            }
            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message); // excecoes de nivel de acesso de dados sao capturadas pelos servicos
            }
        }
    }
}
