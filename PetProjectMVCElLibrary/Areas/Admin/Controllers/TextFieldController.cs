using AutoMapper;
using BLL.Interfaces;
using BLL.Models.DTO.Author;
using BLL.Models.DTO.Book;
using BLL.Models.DTO.Genre;
using BLL.Models.DTO.TextField;
using BLL.Services.Author;
using BLL.Services.Book;
using BLL.Services.Genre;
using BLL.Services.TextField;
using DAL.Domain;
using Microsoft.AspNetCore.Mvc;
using PetProjectMVCElLibrary.Areas.Admin.ViewModel.TextField;
using PetProjectMVCElLibrary.Interfaces.Book;
using PetProjectMVCElLibrary.Service;
using PetProjectMVCElLibrary.ViewModel.Book;

namespace PetProjectMVCElLibrary.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TextFieldController : Controller
    {
        private readonly AppDbContext _context;
        private readonly TextFieldService textFieldService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TextFieldController(AppDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            textFieldService = new TextFieldService(context, mapper);
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string codeWord)
        {
            TextFieldDTO textFieldDTO = codeWord == default ? new TextFieldDTO() : (await textFieldService.GetTextFieldByCodeWord(codeWord) ?? new TextFieldDTO());
            return View(_mapper.Map<TextFieldViewModel>(textFieldDTO));
        }
        [HttpPost]
        public IActionResult Edit(TextFieldViewModel textFieldViewModel)
        {
            if (ModelState.IsValid)
            {
                TextFieldDTO textFieldDTO = _mapper.Map<TextFieldDTO>(textFieldViewModel);
                textFieldService.SaveTextField(textFieldDTO);
                return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
            }
            return View(textFieldViewModel);
        }
    }
}
