using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi.DTO;
using WebApi.Dtos;
using WebApi.Errors;
using WebApi.Extensions;

namespace WebApi.Controllers
{
    public class UsuarioController : BaseApiController
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        private readonly IGenericSecurityRepository<Usuario> _SecurityRepository;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsuarioController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, ITokenService tokenService, IMapper mapper, IPasswordHasher<Usuario> passwordHasher, IGenericSecurityRepository<Usuario> SecurityRepository, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _SecurityRepository = SecurityRepository;
            _roleManager = roleManager;
        }


        [HttpPost("login")]
        public async Task<ActionResult<UsuarioDto>> Login(LoginDto loginDto)
        {
            var usuario = await _userManager.FindByEmailAsync(loginDto.Email);

            if (User == null)
            {
                return Unauthorized(new CodeErrorResponse(401));
            }

            var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, loginDto.Password, false);

            if (!resultado.Succeeded)
            {
                return Unauthorized(new CodeErrorResponse(401));
            }

            var roles = await _userManager.GetRolesAsync(usuario);

            return new UsuarioDto
            {
                Id = usuario.Id,
                Email = usuario.Email,
                Username = usuario.UserName,
                Token = _tokenService.CreateToken(usuario,roles),
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Imagen=usuario.Imagen,
                Admin = roles.Contains("ADMIN") ? true : false

            };

        }

        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioDto>> Registrar(RegistrarDto registrarDto)
        {

            var usuario = new Usuario
            {
                Email = registrarDto.Email,
                UserName = registrarDto.Username,
                Nombre = registrarDto.Nombre,
                Apellido = registrarDto.Apellido
            };

            var resultado = await _userManager.CreateAsync(usuario, registrarDto.Password);

            if (!resultado.Succeeded)
            {
                return BadRequest(new CodeErrorResponse(400));
            }

            return new UsuarioDto
            {
                Id=usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Token = _tokenService.CreateToken(usuario, null),
                Email = usuario.Email,
                Username = usuario.UserName,
                Admin = false
            };

        }
        [Authorize]
        [HttpPut("actualizar/{id}")]
        public async Task<ActionResult<UsuarioDto>> Actualizar(string id, RegistrarDto registrar)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
            {
                return NotFound(new CodeErrorResponse(404, "El usuario no existe"));

            }
            usuario.Nombre = registrar.Nombre;
            usuario.Apellido = registrar.Apellido;
            usuario.Imagen = registrar.Imagen;
            if (!string.IsNullOrEmpty(registrar.Password))
            {
                usuario.PasswordHash = _passwordHasher.HashPassword(usuario, registrar.Password);

            }
            var response = await _userManager.UpdateAsync(usuario);
            if (!response.Succeeded)
            {
                return BadRequest(new CodeErrorResponse(400, "No se ha actualizado el usuario"));
            }

            var roles = await _userManager.GetRolesAsync(usuario);

            return new UsuarioDto
            {
                Id=usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Username = usuario.UserName,
                Token = _tokenService.CreateToken(usuario,roles),
                Imagen = usuario.Imagen,
                Admin = roles.Contains("ADMIN") ? true : false
            };

        }

        [Authorize(Roles ="ADMIN")]
        [HttpGet("pagination")]
        public async Task<ActionResult<Pagination<UsuarioDto>>> GetUser([FromQuery] UserSpecificationParams userParams)
        {
            var spec = new UserSpecification(userParams);

            var user = await _SecurityRepository.GetAllWithSpec(spec);

            var specCount = new UserforCountingSpecification(userParams);
            var TotalUser = await _SecurityRepository.CountAsync(specCount);

            var rounded = Math.Ceiling(Convert.ToDecimal(TotalUser) / Convert.ToDecimal(userParams.PageSize));
            var Total = Convert.ToInt32(rounded);

            var data = _mapper.Map<IReadOnlyList<Usuario>, IReadOnlyList<UsuarioDto>>(user);

            return Ok(
                new Pagination<UsuarioDto>
                {
                    Count = TotalUser,
                    Data = data,
                    PageCount = Total,
                    PageIndex = userParams.PageIndex,
                    PageSize = userParams.PageSize,
                }
                );
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPut("role/{id}")]
        public async Task<ActionResult<UsuarioDto>> UpdateRole(string id, RoleDto roleparams)
        {
            var role = await _roleManager.FindByNameAsync(roleparams.Nombre);
            if (role == null)
            {
                return NotFound(new CodeErrorResponse(404, "El role no existe"));
            }
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
            {
                return NotFound(new CodeErrorResponse(404, "El usuario no existe"));
            }
            var usuarioDto = _mapper.Map<Usuario, UsuarioDto>(usuario);
            if (roleparams.Status)
            {
                var response = await _userManager.AddToRoleAsync(usuario, roleparams.Nombre);
                if (response.Succeeded)
                {
                    usuarioDto.Admin = true;
                }
                if (response.Errors.Any())
                {
                    if (response.Errors.Where(x => x.Code == "UserAlreadyInRole").Any())
                    {
                        usuarioDto.Admin = true;
                    }
                }
            }
            else
            {
                var response = await _userManager.RemoveFromRoleAsync(usuario, roleparams.Nombre);
                if (response.Succeeded)
                {
                    usuarioDto.Admin = false;
                }
            }
            if (usuarioDto.Admin)
            {
            var roles = new List<string>();
                roles.Add("ADMIN");
                usuarioDto.Token = _tokenService.CreateToken(usuario, roles);
            }
            else
            {
                usuarioDto.Token = _tokenService.CreateToken(usuario, null);
            }
            return usuarioDto;
        }
        [Authorize(Roles = "ADMIN")]
        [HttpGet("account/{id}")]
        public async Task<ActionResult<UsuarioDto>>GetUsuarioBy(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
            {
                return NotFound(new CodeErrorResponse(404, "el usuario no existe"));
            }

            var roles = await _userManager.GetRolesAsync(usuario);

            return new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Username = usuario.UserName,
                Imagen = usuario.Imagen,
                Admin = roles.Contains("ADMIN") ? true : false
            };
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UsuarioDto>> GetUsuario()
        {

            var usuario = await _userManager.BuscarUsuarioAsync(HttpContext.User);
            //var usuario = await _userManager.FindByEmailAsync(email);
            var roles = await _userManager.GetRolesAsync(usuario);

            return new UsuarioDto
            {
                Id= usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Username = usuario.UserName,
                Imagen=usuario.Imagen,
                Token = _tokenService.CreateToken(usuario, roles),
                Admin = roles.Contains("ADMIN") ? true : false

            };
        }

        [HttpGet("emailvalido")]
        public async Task<ActionResult<bool>> ValidarEmail([FromQuery] string email)
        {
            var usuario = await _userManager.FindByEmailAsync(email);

            if (usuario == null) return false;

            return true;
        }

        [Authorize]
        [HttpGet("direccion")]
        public async Task<ActionResult<DireccionDto>> GetDireccion()
        {
            var usuario = await _userManager.BuscarUsuarioConDireccionAsync(HttpContext.User);

            return _mapper.Map<Direccion, DireccionDto>(usuario.Direccion);
        }

        [Authorize]
        [HttpPut("direccion")]
        public async Task<ActionResult<DireccionDto>> UpdateDireccion(DireccionDto direccion)
        {
            var usuario = await _userManager.BuscarUsuarioConDireccionAsync(HttpContext.User);
            usuario.Direccion = _mapper.Map<DireccionDto, Direccion>(direccion);
            var resultado = await _userManager.UpdateAsync(usuario);
            if (resultado.Succeeded) return Ok(_mapper.Map<Direccion, DireccionDto>(usuario.Direccion));

            return BadRequest("No se pudo actualizar la direccion del usuario");
        }

    }
}
