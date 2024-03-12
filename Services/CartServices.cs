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
        private readonly string baseUrl = "https://localhost:44373/api/OrderApi/";

        public CartServices(IProductSevices productService, HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _productService = productService;
            _client = client;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Order> Index(int id)
        {
            var url = $"{baseUrl}GetOrderbyUser?id={id}";
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
            var url = $"{baseUrl}addOrder";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("JWToken"));
            var orderDetails = new OrderDetails
            {
                UserId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId").Value,
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
            var url = $"{baseUrl}RemoveOrder?UserId={UserId}&ProductId={productId}";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("JWToken"));
            var response = _client.DeleteAsync(url).Result;
            return response;
        }

        public async Task<HttpResponseMessage> UpdateProduct(OrderDetails orderDetails)
        {
            var url = $"{baseUrl}UpdateOrder";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("JWToken"));
            var content = JsonConvert.SerializeObject(orderDetails);
            StringContent stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(url, stringContent);

            return response;
        }

        public async Task<bool> Checkout(int UserId)
        {
            var url = $"https://localhost:44373/api/UserApi/{UserId}";
            var response = await _client.GetAsync(url);
            var mailMessage = false;
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(result);
                var order = JsonConvert.SerializeObject(await Index(UserId));

                var PlacedUrl = $"{baseUrl}OrderPlaced";
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("JWToken"));
                var content = new StringContent(JsonConvert.SerializeObject(UserId), Encoding.UTF8, "application/json");
                var responseOrder = await _client.PostAsync(PlacedUrl, content);

                if (user != null && responseOrder.IsSuccessStatusCode)
                {
                    var mailData = new MailData()
                    {
                        EmailToId = user.Email,
                        EmailToName = user.Firstname + " " + user.Lastname,
                        EmailSubject = "Order Placed",
                        EmailBody = order
                    };
                    var mailUrl = "https://localhost:44373/api/MailApi/SendMail";
                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(mailData), Encoding.UTF8, "application/json");
                    var mailResponse = await _client.PostAsync(mailUrl, stringContent);
                    var mailResult = await mailResponse.Content.ReadAsStringAsync();
                    mailMessage = JsonConvert.DeserializeObject<bool>(mailResult);
                }
            }
            return mailMessage;
        }
    }
}

