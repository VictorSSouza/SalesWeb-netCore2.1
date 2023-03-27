using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;
using System.Collections.Generic;

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
            _sellersService.Insert(seller);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            
            var seller = _sellersService.FindById(id.Value);
            
            if(seller == null)
            {
                return NotFound();
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
                return NotFound();
            }

            var seller = _sellersService.FindById(id.Value);
            if(seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }
        public IActionResult Edit(int? id) 
        {
            if(id ==null)
            {
                return NotFound();
            }
            var seller = _sellersService.FindById(id.Value);
            if(seller == null)
            {
                return NotFound();
            }
            List<Department> departments = _departmentService.FindAll();
            SellerFormViewModel viewModel = new SellerFormViewModel{ Seller = seller, Departments = departments};
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
            if(id != seller.Id)
            {
                return BadRequest();
            }
            try
            {
                _sellersService.Update(seller);
                return RedirectToAction(nameof(Index));
            }
            catch(NotFoundException)
            {
                return NotFound();
            }
            catch(DbConcurrencyException)
            {
                return BadRequest();
            }
        }
    }
}
