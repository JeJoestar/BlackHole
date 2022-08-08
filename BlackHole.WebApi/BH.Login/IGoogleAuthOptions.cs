using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackHole.Login
{
    public interface IGoogleAuthOptions
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }
    }
}
