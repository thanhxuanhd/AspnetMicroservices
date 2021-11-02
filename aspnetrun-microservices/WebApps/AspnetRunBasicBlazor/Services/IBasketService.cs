
using AspnetRunBasicBlazor.Models;
using System.Threading.Tasks;

namespace AspnetRunBasicBlazor.Services;
public interface IBasketService
{
    Task<BasketModel> GetBasket(string userName);
    Task<BasketModel> UpdateBasket(BasketModel model);
    Task CheckoutBasket(BasketCheckoutModel model);
}
