using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Hosting;
using System.Threading.Tasks;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellersService _sellersService;
        private readonly DepartmentService _departmentService;
        public SellersController (SellersService sellersService, DepartmentService departmentService)
        {
            _sellersService = sellersService;
            _departmentService = departmentService;
        }
        public async Task<IActionResult> Index()
        {
           var list = await _sellersService.FindAllAsync();

            return View(list);
        }
        public async Task<IActionResult> Create()
        {
            var depts = await _departmentService.FindAllAsync(); 
            var viewModel = new SellerFormViewModel { Departments= depts };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }
            await _sellersService.InsertAsync(seller);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return RedirectToAction(nameof(Error), new {Message = "Id not provided"});
            }
            
            var seller = await _sellersService.FindByIdAsync(id.Value);
            
            if(seller == null)
            {
                return RedirectToAction(nameof(Error), new { Message = "Id not found" });
            }

            return View(seller);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _sellersService.RemoveAsync(id); //Remove o seller pela id de forma assincrona
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return RedirectToAction(nameof(Error), new { Message = "Id not provided" });
            }

            var seller = await _sellersService.FindByIdAsync(id.Value);
            if(seller == null)
            {
                return RedirectToAction(nameof(Error), new { Message = "Id not found" });
            }

            return View(seller);
        }
        public async Task<IActionResult> Edit(int? id) 
        {
            if(id ==null)
            {
                return RedirectToAction(nameof(Error), new { Message = "Id not provided" });
            }
            var seller = await _sellersService.FindByIdAsync(id.Value);
            if(seller == null)
            {
                return RedirectToAction(nameof(Error), new { Message = "Id not found" });
            }
            List<Department> departments = await _departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel{ Seller = seller, Departments = departments};
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }
            if(id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { Message = "Id mismatch" });
            }
            try
            {
                await _sellersService.UpdateAsync(seller);
                return RedirectToAction(nameof(Index));
            }
            catch(ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { Message = e.Message });
            }
            
        }
        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(viewModel);
        }
    }
}
