using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aemtest.Models;
using aemtest.Services;
using Microsoft.AspNetCore.Mvc;

namespace aemtest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        private readonly IRestClient _restClient;
        private readonly ISyncService _syncService;
        private readonly IDataConnector _dataConnector;

        public SyncController(IRestClient restClient, ISyncService syncService, IDataConnector dataConnector)
        {
            _restClient = restClient;
            _syncService = syncService;
            _dataConnector = dataConnector;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<bool>> SyncPlatformAndWell()
        {
            return await _syncService.SyncPlatformAndWell();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IEnumerable<dynamic>> LastUpdatedWell()
        {
            var query = $"select p.UniqueName as PlatformName, p.Id, p.SyncId as PlatformId, w.UniqueName, w.Latitude, w.Longitude, w.CreatedAt, w.UpdatedAt " +
                "from [dbo].[Platforms] p inner join [dbo].[Wells] w on w.PlatformId = p.SyncId " +
                "where w.UpdatedAt in ( " +
                "select Max(w.UpdatedAt) as UpdatedAt from [dbo].[Platforms] p " +
                "left join [dbo].[Wells] w on w.PlatformId = p.SyncId " +
                "where w.UpdatedAt is not null group by p.SyncId) " +
                "order by w.UpdatedAt desc";


            return await Task.FromResult(_dataConnector.Query<dynamic>(System.Data.CommandType.Text, query));
        }

        //[HttpGet]
        //[Route("[action]")]
        //public async Task<IEnumerable<Platform>> GetPlatformWellActual()
        //{
        //    return await _restClient.GetPlatformWellActual();
        //}

        //[HttpGet]
        //[Route("[action]")]
        //public async Task<IEnumerable<Platform>> GetPlatformWellDummy()
        //{
        //    return await _restClient.GetPlatformWellDummy();
        //}
    }
}