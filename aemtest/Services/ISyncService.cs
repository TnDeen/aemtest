using AspNetCore.ServiceRegistration.Dynamic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aemtest.Services
{
    public interface ISyncService : IScopedService
    {
        Task<bool> SyncPlatformAndWell();
        Task<bool> SyncDummyPlatformAndWell();
    }
}
