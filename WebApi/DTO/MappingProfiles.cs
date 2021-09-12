using AutoMapper;
using Core.Entities;
using Core.Entities.OrdenCompra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Dtos;

namespace WebApi.DTO
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Producto, ProductoDTO>()
                .ForMember(p => p.CategoriaNombre, x => x.MapFrom(a => a.Categoria.Nombre))
            .ForMember(p => p.MarcaNombre, x => x.MapFrom(a => a.Marca.Nombre));

            CreateMap<Core.Entities.Direccion, DireccionDto>().ReverseMap();
            CreateMap<Usuario, UsuarioDto>().ReverseMap();
            CreateMap<DireccionDto, Core.Entities.OrdenCompra.Direccion>();
            CreateMap<OrdenCompra, OrdenCompraResponseDto>()
                .ForMember(o => o.TipoEnvio, x => x.MapFrom(i => i.TipoEnvio.Nombre))
                .ForMember(o => o.TipoEnvioPrecio, x => x.MapFrom(i => i.TipoEnvio.Precio));
            CreateMap<OrderItem, OrdenItemResponseDto>()
                .ForMember(o => o.ProductoId, x => x.MapFrom(i => i.ItemOrder.ProductoItemId))
                .ForMember(o => o.ProductoNombre, x => x.MapFrom(i => i.ItemOrder.ProductoNombre))
                .ForMember(o => o.ProductoImagen, x => x.MapFrom(i => i.ItemOrder.ImagenUrl));
        }
    }
}
