using Microsoft.AspNetCore.Http.HttpResults;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartServices(IProductSevices productService, HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _productService = productService;
            _client = client;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Order> Index(int id)
        {
            var url = $"https://localhost:44373/api/OrderApi/GetOrderbyUser?id={id}";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("JWToken"));
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
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("JWToken"));
            var orderDetails = new OrderDetails
            {
                UserId =_httpContextAccessor.HttpContext.Session.GetInt32("UserId").Value,
                ProductId = obj.ProductId,
                Quantity = obj.Quantity,
                OrderStatus = obj.OrderStatus
            };

            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(orderDetails), Encoding.UTF8, "application/json");
            var response = _client.PostAsync(url, stringContent).Result;
            return response;
        }

        public async Task<HttpResponseMessage> RemoveProductOrder(int productId)
        {
            var UserId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");
            var url = $"https://localhost:44373/api/OrderApi/RemoveOrder?UserId={UserId}&ProductId={productId}";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("JWToken"));
            var response = _client.DeleteAsync(url).Result;
            return response;
        }

        public async Task<HttpResponseMessage> UpdateProduct(OrderDetails orderDetails)
        {
            var url = "https://localhost:44373/api/OrderApi/UpdateOrder";
            var _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("JWToken"));
            var content = JsonConvert.SerializeObject(orderDetails);
            StringContent stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(url, stringContent);

            return response;
        }
    }
}

