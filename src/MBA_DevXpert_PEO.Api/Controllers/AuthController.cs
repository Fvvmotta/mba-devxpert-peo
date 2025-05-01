using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MBA_DevXpert_PEO.Identity;
using MBA_DevXpert_PEO.GestaoDeAlunos.Domain.Entities;
using MBA_DevXpert_PEO.GestaoDeAlunos.Domain.Repositories;
using System.Threading.Tasks;
using MBA_DevXpert_PEO.Api.DTOs;
using MBA_DevXpert_PEO.Core.Communication.Mediator;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.Notifications;
using MediatR;

namespace MBA_DevXpert_PEO.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenService _tokenService;
        private readonly IAlunoRepository _alunoRepository;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            TokenService tokenService,
            IAlunoRepository alunoRepository,
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediatorHandler
        ) : base(notifications, mediatorHandler)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _alunoRepository = alunoRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Criar aluno no domínio
            var aluno = Aluno.CriarComId(user.Id, dto.Nome, dto.Email);

            _alunoRepository.Adicionar(aluno);
            await _alunoRepository.UnitOfWork.Commit();

            var token = _tokenService.GenerateToken(user);
            return Ok(new { token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, true);
            if (!result.Succeeded)
            {
                return BadRequest("Usuário ou senha incorretos.");
            }

            var user = await _userManager.FindByEmailAsync(dto.Email);
            var token = _tokenService.GenerateToken(user);
            return Ok(new { token });
        }
    }
}
