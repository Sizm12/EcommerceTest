using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specification
{
    public class UserforCountingSpecification : BaseSpecification<Usuario>
    {
        public UserforCountingSpecification(UserSpecificationParams userParams)
            : base(x =>
            (string.IsNullOrEmpty(userParams.Search) || x.Nombre.Contains(userParams.Search)) &&
            (string.IsNullOrEmpty(userParams.Nombre) || x.Nombre.Contains(userParams.Nombre)) &&
            (string.IsNullOrEmpty(userParams.Apellido) || x.Apellido.Contains(userParams.Apellido))
            )
        {

        }
    }
}
