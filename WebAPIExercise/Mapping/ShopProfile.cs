using AutoMapper;

using InOrder = WebAPIExercise.Input.Order;
using InProduct = WebAPIExercise.Input.Product;
using InOrderItem = WebAPIExercise.Input.OrderItem;

using OutOrder = WebAPIExercise.Output.Order;
using OutProduct = WebAPIExercise.Output.Product;
using OutOrderItem = WebAPIExercise.Output.OrderItem;

using DbOrder = WebAPIExercise.Data.Models.Order;
using DbProduct = WebAPIExercise.Data.Models.Product;
using DbOrderItem = WebAPIExercise.Data.Models.OrderItem;


namespace WebAPIExercise.Mapping
{
    public class ShopProfile : Profile
    {
        public ShopProfile()
        {
            CreateMap<InOrder, DbOrder>();
            CreateMap<InOrderItem, DbOrderItem>();
            CreateMap<InProduct, DbProduct>();

            CreateMap<DbOrder, OutOrder>();
            CreateMap<DbOrderItem, OutOrderItem>();
            CreateMap<DbProduct, OutProduct>();
        }
    }
}
