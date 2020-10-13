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
using System.Collections.Generic;

namespace WebAPIExercise.Mapping
{
    /// <summary>
    /// <para>This profile automagically converts the following:</para>
    /// <list type="bullet">
    /// <item>Order input POCO => Order DB Entity</item>
    /// <item>Product input POCO => Product DB Entity</item>
    /// <item>OrderItem input POCO => OrderItem DB Entity</item>
    /// <item>Order DB Entity => Order output POCO (with custom mapping)</item>
    /// <item>Product DB Entity => Product output POCO</item>
    /// </list>
    /// </summary>
    public class ShopProfile : Profile
    {
        //FIXME: Filthy workaround, idk how to Inject a dependency into an autoscanned Automapper profile
        public ShopProfile() : this(new DictionaryCompanyTotalConverter()) {}


        public ShopProfile(ICompanyTotalConverter converter)
        {
            //IN => DB
            CreateMap<InOrder, DbOrder>();
            CreateMap<InOrderItem, DbOrderItem>();
            CreateMap<InProduct, DbProduct>();

            //DB => OUT
            CreateMap<DbProduct, OutProduct>();
            CreateMap<DbOrder, OutOrder>()
                .ForMember(
                    outputOrder => outputOrder.Total,
                    mapping => mapping.MapFrom(dbOrder => ComputeTotalFrom(converter, dbOrder))
                )
                .ForMember(
                    outputOrder => outputOrder.OrderedProducts,
                    mapping => mapping.MapFrom(dbOrder => GetOrderItemsFrom(dbOrder))
                );
        }

        private static double ComputeTotalFrom(ICompanyTotalConverter converter, DbOrder dbOrder) => 
            converter.ComputeTotalFor(
                dbOrder.CompanyCode, 
                dbOrder.OrderItems.Sum(item => item.OrderedQuantity * item.Product.UnitPrice)
            );

        private static ICollection<OutOrderItem> GetOrderItemsFrom(DbOrder dbOrder) => 
            dbOrder.OrderItems
                .Select(item => new OutOrderItem
                {
                    ProductId = item.Product.Id,
                    OrderedQuantity = item.OrderedQuantity,
                    UnitPrice = item.Product.UnitPrice
                }).ToImmutableList();
    }
}
