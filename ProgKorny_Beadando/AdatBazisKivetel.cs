using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgKorny_Beadando
{
    [Serializable]
    class AdatBazisKivetel : Exception
    {
        public AdatBazisKivetel(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
