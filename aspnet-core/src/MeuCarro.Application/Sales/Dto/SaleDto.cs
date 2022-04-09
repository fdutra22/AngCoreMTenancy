using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using MeuCarro.Sales;
using System;

namespace Application.Sales.Dto
{
    [AutoMapFrom(typeof(Sale))]
    [AutoMapTo(typeof(Sale))]
    public class SaleDto : EntityDto<int>, IMustHaveTenant
    {
        public DateTime CreationTime { get; set; }
        public long CreatorUserId { get; set; }
        public int TenantId { get; set; }
        public int CarroId { get; set; }
        public float ValorVendido { get; set; }
        public float MargemVendidaPercentual { get; set; }
    }
}
