using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Interfaces
{
    public interface IAuditable
    {
        public DateTime? DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? DateDeleted { get; set; }
        public int? DeletedBy { get; set; }
    }
}
