using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRG.LeisureCards.Service
{
    public interface IAdminCodeProvider
    {
        bool IsAdminCode(string code);
    }

    public class AdminCodeProvider : IAdminCodeProvider
    {
        private readonly string _code;

        public AdminCodeProvider(string code)
        {
            _code = code.Trim().ToUpper();
        }
        public bool IsAdminCode(string code)
        {
            return !string.IsNullOrWhiteSpace(code) && code.Trim().ToUpper() == _code;
        }
    }
}
