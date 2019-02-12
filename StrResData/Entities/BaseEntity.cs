using System;

namespace StrResData.Entities
{
    public abstract class BaseEntity
    {
        public DateTime CreatedTime { get; set; }

        public DateTime ModifiedTime { get; set; }
    }
}