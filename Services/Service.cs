using Domain.Global;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class Service
    {
        public static List<Category> PagingCategory(List<Category> categories, int? page)
        {
            YokartVar.pageSize = 3;
            YokartVar.pageCount = (int)Math.Ceiling(categories.Count / (double)YokartVar.pageSize);
            YokartVar.currentPage = page ?? 1;
            var tempCategory = categories.Skip((YokartVar.currentPage - 1) * YokartVar.pageSize).Take(YokartVar.pageSize).ToList();

            return tempCategory;
        }
        public static Category PagingSubCategory(Category category, int? page)
        {
            YokartVar.pageSize = 3;
            YokartVar.pageCount = (int)Math.Ceiling(category.SubCategories.Count / (double)YokartVar.pageSize);
            YokartVar.currentPage = page ?? 1;
            var tempSubCategory = category.SubCategories.Skip((YokartVar.currentPage - 1) * YokartVar.pageSize).Take(YokartVar.pageSize).ToList();
            var tempCategory = new Category
            {
                CategoryId = category.CategoryId,
                SubCategories = tempSubCategory
            };

            return tempCategory;
        }
        public static string BuildOrderEmailBody(Order order)
        {
            const string ImageBaseUrl = "http://yokart.somee.com/images/products/";

            var itemsHtml = string.Join("", order.OrderItems.Select(item => $@"
        <tr>
            <td>
                <img src='{ImageBaseUrl}{item.Products.ProductImage}' width='80' style='border-radius:6px'>
            </td>
            <td>
                <strong>{item.Products.ProductName}</strong><br>
                <span style='color:#666;font-size:14px'>{item.Products.ProductDescription}</span><br>
                <strong>Unit Price:</strong> ₹ {item.UnitPrice}<br>
                <strong>Qty:</strong> {item.Quantity}<br>
                <strong>Total:</strong> ₹ {item.Price}
            </td>
        </tr>
    "));

            return $@"
        <h2>Your Order Has Been Successfully Placed</h2>
        <p>Thank you for shopping with YoKart.</p>

        <h3>Order Summary</h3>
        <p><strong>Order ID:</strong> {order.OrderId}</p>
        <p><strong>Total Amount:</strong> ₹ {order.TotalPrice}</p>
        <hr>

        <h3>Items</h3>
        <table width='100%' cellpadding='10' cellspacing='0' style='border:1px solid #ddd'>
            {itemsHtml}
        </table>

        <p style='margin-top:20px;font-size:12px;color:#666'>
            You will receive further updates as your order is processed.
        </p>
    ";
        }


    }
}
