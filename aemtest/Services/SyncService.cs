using aemtest.Contexts;
using aemtest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aemtest.Services
{
    public class SyncService : ISyncService
    {
        private readonly IRestClient _restClient;
        private readonly AemContext _aemContext;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public SyncService(IRestClient restClient, AemContext aemContext, IConfiguration configuration)
        {
            _restClient = restClient;
            _aemContext = aemContext;
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<bool> SyncPlatformAndWell()
        {
            var isSync = false;
            var platforms = await _restClient.GetPlatformWellActual();

            if (platforms.Any())
            {
                var runningTask = new List<Task>();
                var allTask = new List<Task>();

                var platformTaskList = platforms.Select(a => new Task(delegate { ProcessPlatformData(a); })).AsEnumerable();
                allTask.AddRange(platformTaskList);

                var wells = GetWells(platforms);
                if (wells.Any())
                {
                    var wellTaskList = wells.Select(a => new Task(delegate { ProcessWellData(a); })).AsEnumerable();
                    allTask.AddRange(wellTaskList);
                }

                Parallel.ForEach(allTask, task =>
                {
                    task.Start();
                    runningTask.Add(task);
                });


                try
                {
                    Task.WaitAll(runningTask.ToArray());
                    isSync = true;
                }
                catch (Exception ex)
                {

                    throw ex;
                }


            }

            return isSync;
        }

        private IEnumerable<Well> GetWells(IEnumerable<Platform> platforms)
        {
            var result = new List<Well>();
            if(platforms.Any())
            {
                platforms.ToList().ForEach(platform =>
                {
                    if (platform.Wells != null && platform.Wells.Any())
                    {
                        result.AddRange(platform.Wells);
                    }

                });
            }
            return result.AsEnumerable();
        }

        private AemContext CreateContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AemContext>();
            optionsBuilder.UseSqlServer(_connString);

            return new AemContext(optionsBuilder.Options);

        }

        private async void ProcessPlatformData(Platform platform)
        {
            var context = CreateContext();

            var syncId = platform.Id;
            var localPlatform = context.Platforms.Where(a => a.SyncId == syncId).FirstOrDefault();

            //skip sync
            if(localPlatform != null && localPlatform.UpdatedAt > platform.UpdatedAt)
            {
                return;
            }

            if (localPlatform != null)
            {
                context.Entry(localPlatform).State = EntityState.Modified;
                localPlatform.Latitude = platform.Latitude;
                localPlatform.Longitude = platform.Longitude;
                localPlatform.UniqueName = platform.UniqueName;
                localPlatform.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                platform.Id = 0;
                platform.SyncId = syncId;
                platform.Wells = null;
                context.Platforms.Add(platform);
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }
        }

        private async void ProcessWellData(Well well)
        {
            var context = CreateContext();

            var syncId = well.Id;
            var localWell = context.Wells.Where(a => a.SyncId == syncId).FirstOrDefault();

            //skip sync
            if (localWell != null && localWell.UpdatedAt > well.UpdatedAt)
            {
                return;
            }

            if (localWell != null)
            {
                context.Entry(localWell).State = EntityState.Modified;
                localWell.PlatformId = well.PlatformId;
                localWell.Latitude = well.Latitude;
                localWell.Longitude = well.Longitude;
                localWell.UniqueName = well.UniqueName;
                localWell.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                well.Id = 0;
                well.SyncId = syncId;
                context.Wells.Add(well);
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }

        }
    }
}
