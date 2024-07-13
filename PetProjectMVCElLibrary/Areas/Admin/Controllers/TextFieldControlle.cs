using AutoMapper;
using BLL.Services.TextField;
using DAL.Domain;
using DAL.Domain.Entities;
using DAL.Domain.Repository;
using Microsoft.AspNetCore.Mvc;
using PetProjectMVCElLibrary.Service;

namespace PetProjectMVCElLibrary.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TextFieldControlle : Controller
    {
        private readonly AppDbContext _context;
        private readonly TextFieldService textFieldService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TextFieldControlle(IHttpContextAccessor httpContextAccessor, IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            textFieldService = new TextFieldService(context, mapper);
        }
        //public IActionResult Edit(string codeWord)
        //{
        //    var entity = textFieldRepository.GetByCodeWord(codeWord);
        //    return View(entity);
        //}
        //[HttpPost]
        //public IActionResult Edit(TextField model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        textFieldRepository.Save(model);
        //        return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
        //    }
        //    return View(model);
        //}
    }
}
