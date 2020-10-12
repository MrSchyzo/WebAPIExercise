using AutoMapper;
using System.Linq;

using InOrder = WebAPIExercise.Input.Order;
using InProduct = WebAPIExercise.Input.Product;
using InOrderItem = WebAPIExercise.Input.OrderItem;

using OutOrder = WebAPIExercise.Output.Order;
using OutProduct = WebAPIExercise.Output.Product;
using OutOrderItem = WebAPIExercise.Output.OrderItem;

using DbOrder = WebAPIExercise.Data.Models.Order;
using DbProduct = WebAPIExercise.Data.Models.Product;
using DbOrderItem = WebAPIExercise.Data.Models.OrderItem;
using System.Collections.Immutable;

namespace WebAPIExercise.Mapping
{
    public class ShopProfile : Profile
    {
        //Filthy workaround
        public ShopProfile() : this(new DictionaryCompanyTotalConverter()) {}

        public ShopProfile(ICompanyTotalConverter converter)
        {
            CreateMap<InOrder, DbOrder>();
            CreateMap<InOrderItem, DbOrderItem>();
            CreateMap<InProduct, DbProduct>();

            CreateMap<DbOrder, OutOrder>()
                .ForMember(
                    outputOrder => outputOrder.Total, 
                    mapping => mapping.MapFrom(
                        dbOrder => converter.ComputeTotalFor(dbOrder.CompanyCode, dbOrder.OrderItems.Sum(item => item.OrderedQuantity * item.Product.UnitPrice))
                ))
                .ForMember(
                    outputOrder => outputOrder.OrderedProducts,
                    mapping => mapping.MapFrom(
                        dbOrder => dbOrder.OrderItems.Select(item => new OutOrderItem
                        {
                            ProductId = item.Product.Id,
                            OrderedQuantity = item.OrderedQuantity,
                            UnitPrice = item.Product.UnitPrice
                        }).ToImmutableList()
                ));
            CreateMap<DbProduct, OutProduct>();
        }
    }
}
