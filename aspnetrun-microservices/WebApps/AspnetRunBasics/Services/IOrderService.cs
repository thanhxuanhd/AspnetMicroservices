

using AspnetRunBasics.Models;

namespace AspnetRunBasics.Services;
public interface IOrderService
{
    Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName);
}
