using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetExam.Services
{
    public interface IAuthService
    {
        public Task<string> Authenticate(string login, string password);
    }
}
