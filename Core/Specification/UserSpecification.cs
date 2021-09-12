using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specification
{
    public class UserSpecification : BaseSpecification<Usuario>
    {
        public UserSpecification(UserSpecificationParams UserParams)
            : base(x =>
            (string.IsNullOrEmpty(UserParams.Search) || x.Nombre.Contains(UserParams.Search)) &&
            (string.IsNullOrEmpty(UserParams.Nombre) || x.Nombre.Contains(UserParams.Nombre)) &&
            (string.IsNullOrEmpty(UserParams.Apellido) || x.Apellido.Contains(UserParams.Apellido))
            )
        {
            ApplyPaging(UserParams.PageSize * (UserParams.PageIndex - 1), UserParams.PageSize);

            if(!string.IsNullOrEmpty(UserParams.Sort))
            {
                switch(UserParams.Sort)
                {
                    case "nombreAsc":
                        AddOrderBy(u => u.Nombre);
                        break;

                    case "nombreDesc":
                        AddOrderByDescending(u => u.Nombre);
                        break;
                    case "emailAsc":
                        AddOrderBy(u => u.Email);
                        break;

                    case "emailDesc":
                        AddOrderByDescending(u => u.Email);
                        break;
                    default:
                        AddOrderBy(u => u.Apellido);
                        break;
                }
            }
        }
    }
}
