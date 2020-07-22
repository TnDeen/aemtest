using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace aemtest.Models
{
    public class Well : BaseEntity
    {
        public int PlatformId { get; set; }
    }
}
