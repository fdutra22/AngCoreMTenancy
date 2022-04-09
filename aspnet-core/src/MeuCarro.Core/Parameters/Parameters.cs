using Abp.Domain.Entities;
using System;

namespace MeuCarro.Parameters
{
    public class Parameter : Entity<int>, IMustHaveTenant
    {
        public DateTime CreationTime { get; set; }
        public long CreatorUserId { get; set; }
        public int TenantId { get; set; }
        public float MargemIdealPercentual { get; set; }
        public float VendaMinimaPercentual { get; set; }
    }
}
