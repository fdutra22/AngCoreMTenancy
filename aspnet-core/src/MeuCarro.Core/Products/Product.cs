using Abp.Domain.Entities;
using System;

namespace MeuCarro.Products
{
    public class Product : Entity<int>, IMustHaveTenant
    {
        public DateTime CreationTime { get; set; }
        public long CreatorUserId { get; set; }
        public string Name { get; set; }
        public float Value { get; set; }
        public int TenantId { get; set; }
        public string Branch { get; set; }
    }
}
