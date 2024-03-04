using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using YoKart.IServices;
using YoKart.Models;
using YoKartApi.Models;

namespace YoKart.Services
{
    public class CartServices : ICartServices
    {
        public readonly IProductSevices _productService;
        private readonly HttpClient _client;

        public CartServices(IProductSevices productService, HttpClient client)
        {
            _productService = productService;
            _client = client;
        }

        public async Task<Order> Index(int id)
        {
            var url = $"https://localhost:44373/api/OrderApi/GetOrderbyUser?id={id}";
            var response = await _client.GetAsync(url);
            var orders = new Order();
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<Order>(result);
                if (data != null)
                {
                    orders = data;
                }
            }
            return orders;
        }

        public async Task<HttpResponseMessage> AddProductOrder(int id)
        {
            var url = "https://localhost:44373/api/OrderApi/addOrder";
            var product = await _productService.Edit(id);

            var orderItem = new OrderItem
            {
                ProductId = product.ProductId,
                Products = product,
                Quantity = 1
            };
            var orderProduct = new Order()
            {
                UserId = myVar.UserId,
                OrderItems = new List<OrderItem> { orderItem },
            };

            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(orderProduct), Encoding.UTF8, "application/json");
            var response = _client.PostAsync(url, stringContent).Result;

            return response;
        }

    }
}

