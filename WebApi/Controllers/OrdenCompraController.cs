using AutoMapper;
using Core.Entities.OrdenCompra;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi.DTO;
using WebApi.Dtos;
using WebApi.Errors;

namespace WebApi.Controllers
{
    [Authorize]
    public class OrdenCompraController : BaseApiController
    {
        private readonly IOrdenCompraServices _ordenCompraServices;
        private readonly IMapper _mapper;

        public OrdenCompraController(IOrdenCompraServices ordenCompraServices, IMapper mapper)
        {
            _ordenCompraServices = ordenCompraServices;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<OrdenCompraResponseDto>> AddOrdenCompra(OrdenCompraDTO ordenCompraDTO)
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            var direccion = _mapper.Map<DireccionDto, Direccion>(ordenCompraDTO.DireccionEnvio);
            var ordenCompra = await _ordenCompraServices.AddOrdenCompraAsync(email, ordenCompraDTO.TipoEnvio, ordenCompraDTO.CarritoCompraId, direccion);
            if (ordenCompra == null) return BadRequest(new CodeErrorResponse(400, "Error creando la Orden de compra"));

            
            return Ok(_mapper.Map<OrdenCompra, OrdenCompraResponseDto>(ordenCompra));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrdenCompraResponseDto>>> GetOrdenCompras()
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            var ordenCompras = await _ordenCompraServices.GetOrdenCompraByUserAsync(email);
            
            return Ok(_mapper.Map<IReadOnlyList<OrdenCompra>,IReadOnlyList<OrdenCompraResponseDto>>(ordenCompras));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrdenCompraResponseDto>>GetOrdenCompraById(int id)
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            var ordenCompra = await _ordenCompraServices.GetOrdenCompraByID(id, email);
            if (ordenCompra == null) return NotFound(new CodeErrorResponse(404, "No se encontro la orden de Compra"));
            return Ok(_mapper.Map<OrdenCompra, OrdenCompraResponseDto>(ordenCompra));
        }

        [HttpGet("tipoEnvio")]
        public async Task<ActionResult<IReadOnlyList<Envio>>> GetTiposEnvios()
        {
            return Ok(await _ordenCompraServices.GetEnvios());
        }
    }
}
