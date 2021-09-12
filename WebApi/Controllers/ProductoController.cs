using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DTO;
using WebApi.Errors;

namespace WebApi.Controllers
{

    public class ProductoController : BaseApiController
    {
        private readonly IGenericRepository<Producto> _productoRepository;
        private readonly IMapper _mapper;
        public ProductoController(IGenericRepository<Producto> productoRepository, IMapper mapper)
        {
            _productoRepository = productoRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Pagination<ProductoDTO>>>> GetProducto([FromQuery] ProductoSpecificationParams productoParams)
        {
            var spec = new ProductowithCyMSpecification(productoParams);
            var productos = await _productoRepository.GetAllWithSpec(spec);
            var specCount = new ProductoForCountingSpecification(productoParams);
            var TotalProducto = await _productoRepository.CountAsync(specCount);

            var rounded = Math.Ceiling(Convert.ToDecimal(TotalProducto / productoParams.PageSize));
            var TotalPages = Convert.ToInt32(rounded);
            var data = _mapper.Map<IReadOnlyList<Producto>, IReadOnlyList<ProductoDTO>>(productos);

            return Ok(
                new Pagination<ProductoDTO>
                {
                    Count = TotalProducto,
                    Data = data,
                    PageCount = TotalPages,
                    PageIndex = productoParams.PageIndex,
                    PageSize = productoParams.PageSize
                }
                );


        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDTO>> GetProducto(int id)
        {
            var spec = new ProductowithCyMSpecification(id);
            var producto = await _productoRepository.GetByIdWithSpec(spec);
            if (producto == null)
            { return NotFound(new CodeErrorResponse(404, "El producto no existe")); }

            return _mapper.Map<Producto, ProductoDTO>(producto);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<ActionResult<Producto>> Post(Producto producto)
        {
            var resultado = await _productoRepository.Add(producto);
            if (resultado == 0)
            {
                throw new Exception("No se inserto el producto");
            }

            return Ok(producto);
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Producto>> Put(int id, Producto producto)
        {
            producto.Id = id;
            var resultado = await _productoRepository.Update(producto);
            if (resultado == 0)
            {
                throw new Exception("No se pudo actualizar el producto");
            }

            return Ok(producto);
        }
    }
}
