using System;
using System.Collections.Generic;
using System.Text;

namespace StrResApiLib
{
    public class Tenant
    {
        public long TenantId { get; set; }
        public string Name { get; set; }
        public string AccessToken { get; set; }
        public List<Resource> Resources { get; set; }
    }

    public class Resource
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
    }
}