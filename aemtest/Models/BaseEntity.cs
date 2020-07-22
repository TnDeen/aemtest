using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace aemtest.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public int SyncId { get; set; }

        public string UniqueName { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
