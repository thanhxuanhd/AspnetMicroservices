

using AspnetRunBasicBlazor.Models;

namespace AspnetRunBasicBlazor.Services;
public interface IOrderService
{
    Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName);
}
