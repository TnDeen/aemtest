using aemtest.Models;
using AspNetCore.ServiceRegistration.Dynamic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aemtest.Services
{
    public interface IRestClient : IScopedService
    {
        Task<string> Authenticate();

        Task<IEnumerable<Platform>> GetPlatformWellActual();

        Task<IEnumerable<Platform>> GetPlatformWellDummy();
    }
}
