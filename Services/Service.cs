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
    }
}
