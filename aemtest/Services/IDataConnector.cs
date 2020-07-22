using AspNetCore.ServiceRegistration.Dynamic.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace aemtest.Services
{
    public interface IDataConnector : IScopedService
    {
        IEnumerable<T> Query<T>(CommandType cmdType, string cmdText) where T : class;
    }
}
