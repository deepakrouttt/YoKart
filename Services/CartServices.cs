using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using YoKart.IServices;
using YoKart.Models;
using YoKartApi.Models;
using static YoKartApi.Models.Order;

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
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", myVar.Token);
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

        public async Task<HttpResponseMessage> AddProductOrder(OrderDetails obj)
        {
            var url = "https://localhost:44373/api/OrderApi/addOrder";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", myVar.Token);
            var orderDetails = new OrderDetails
            {
                UserId = myVar.UserId,
                ProductId = obj.ProductId,
                Quantity = obj.Quantity,
                OrderStatus= obj.OrderStatus
            };

            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(orderDetails), Encoding.UTF8, "application/json");
            var response = _client.PostAsync(url, stringContent).Result;
            return response;
        }

        public async Task<HttpResponseMessage> RemoveProductOrder(int id)
        {
            var url = $"https://localhost:44373/api/OrderApi/RemoveOrder?UserId={myVar.UserId}&ProductId={id}";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", myVar.Token);
            var response = _client.DeleteAsync(url).Result;
            return response;
        }

    }
}

