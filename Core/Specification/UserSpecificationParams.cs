using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specification
{
    public class UserSpecificationParams
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Sort { get; set; }
        public int PageIndex { get; set; } = 1;
        private const int MaxPageSize = 50;
        private int _PageSize = 3;
        public int PageSize
        {
            get => _PageSize;
            set => _PageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string Search { get; set; }
    }
}
