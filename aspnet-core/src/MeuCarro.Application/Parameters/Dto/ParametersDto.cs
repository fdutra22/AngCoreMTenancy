using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using MeuCarro.Parameters;
using System;

namespace Application.Parameters.Dto
{
    [AutoMapFrom(typeof(Parameter))]
    [AutoMapTo(typeof(Parameter))]
    public class ParameterDto : EntityDto<int>, IMustHaveTenant
    {
        public DateTime CreationTime { get; set; }
        public long CreatorUserId { get; set; }
        public int TenantId { get; set; }
        public float MargemIdealPercentual { get; set; }
        public float VendaMinimaPercentual { get; set; }

    }
}
