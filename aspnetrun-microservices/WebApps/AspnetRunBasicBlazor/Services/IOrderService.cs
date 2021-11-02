

using AspnetRunBasicBlazor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspnetRunBasicBlazor.Services;
public interface IOrderService
{
    Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName);
}
