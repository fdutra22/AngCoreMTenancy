using Abp.Domain.Entities;
using System;

namespace MeuCarro.Sales
{
    public class Sale : Entity<int>, IMustHaveTenant
    {
        public DateTime CreationTime { get; set; }
        public long CreatorUserId { get; set; }      
        public int TenantId { get; set; }      
        public int CarroId { get; set; }
        public float ValorVendido { get; set; }
        public float MargemVendidaPercentual { get; set; }
    }
}
