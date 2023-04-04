using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMvcContext _context;

        public SalesRecordService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var resultq = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue)
            {
                resultq = resultq.Where(x => x.Date >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                resultq = resultq.Where(x => x.Date <= maxDate.Value);
            }
            return await resultq
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }
        public async Task<List<IGrouping<Department,SalesRecord>>> FindByGroupingDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var resultq = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue)
            {
                resultq = resultq.Where(x => x.Date >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                resultq = resultq.Where(x => x.Date <= maxDate.Value);
            }
            return await resultq
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .GroupBy(x => x.Seller.Department)
                .ToListAsync();
        }
    }
}
