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
        public IActionResult Index()
        {
           var list = _sellersService.FindAll();

            return View(list);
        }
        public IActionResult Create()
        {
            var depts = _departmentService.FindAll(); 
            var viewModel = new SellerFormViewModel { Departments= depts };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = _departmentService.FindAll();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }
            _sellersService.Insert(seller);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if(id == null)
            {
                return RedirectToAction(nameof(Error), new {Message = "Id not provided"});
            }
            
            var seller = _sellersService.FindById(id.Value);
            
            if(seller == null)
            {
                return RedirectToAction(nameof(Error), new { Message = "Id not found" });
            }

            return View(seller);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellersService.Remove(id); //Remove o seller pela id
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Details(int? id)
        {
            if(id == null)
            {
                return RedirectToAction(nameof(Error), new { Message = "Id not provided" });
            }

            var seller = _sellersService.FindById(id.Value);
            if(seller == null)
            {
                return RedirectToAction(nameof(Error), new { Message = "Id not found" });
            }

            return View(seller);
        }
        public IActionResult Edit(int? id) 
        {
            if(id ==null)
            {
                return RedirectToAction(nameof(Error), new { Message = "Id not provided" });
            }
            var seller = _sellersService.FindById(id.Value);
            if(seller == null)
            {
                return RedirectToAction(nameof(Error), new { Message = "Id not found" });
            }
            List<Department> departments = _departmentService.FindAll();
            SellerFormViewModel viewModel = new SellerFormViewModel{ Seller = seller, Departments = departments};
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = _departmentService.FindAll();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }
            if(id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { Message = "Id mismatch" });
            }
            try
            {
                _sellersService.Update(seller);
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
