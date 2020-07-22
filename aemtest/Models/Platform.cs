using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aemtest.Models
{
    public class Platform : BaseEntity
    {
        [JsonProperty("well")]
        public IEnumerable<Well> Wells { get; set; }
    }

}
