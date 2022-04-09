using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using MeuCarro.Products;
using System;

namespace Application.Products.Dto
{
    [AutoMapFrom(typeof(Product))]
    [AutoMapTo(typeof(Product))]
    public class ProductDto : EntityDto<int>
    {
        public DateTime CreationTime { get; set; }
        public long CreatorUserId { get; set; }
        public string Name { get; set; }
        public float Value { get; set; }
        public int TenantId { get; set; }
        public string Branch { get; set; }
    }
}
