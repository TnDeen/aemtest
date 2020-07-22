using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aemtest.Dto
{
    public class PlatformDto : BaseDto
    {
    }

    public class WellDto : BaseDto
    {
        public int PlatformId { get; set; }
    }

    public class BaseDto
    {
        public int Id { get; set; }
        public string UniqueName { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
