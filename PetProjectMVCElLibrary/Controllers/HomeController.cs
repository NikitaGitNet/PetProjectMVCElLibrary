using AutoMapper;
using BLL.Models.DTO.TextField;
using BLL.Services.TextField;
using DAL.Domain;
using Microsoft.AspNetCore.Mvc;
using PetProjectMVCElLibrary.Areas.Admin.ViewModel.TextField;

namespace PetProjectMVCElLibrary.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly TextFieldService textFieldService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HomeController(AppDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            textFieldService = new TextFieldService(context, mapper);
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            TextFieldDTO? textFieldDTO = await textFieldService.GetTextFieldByCodeWord("PageIndex");
            return View(_mapper.Map<TextFieldViewModel>(textFieldDTO) ?? new TextFieldViewModel {Text = ""});
        }
        [HttpGet]
        public async Task<IActionResult> Contacts()
        {
            TextFieldDTO? textFieldDTO = await textFieldService.GetTextFieldByCodeWord("PageContacts");
            return View(_mapper.Map<TextFieldViewModel>(textFieldDTO) ?? new TextFieldViewModel { Text = "" });
        }
    }
}
