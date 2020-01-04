using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WorkingWithJson.Models;

namespace WorkingWithJson.Services
{
    public class JsonFileProductService
    {
        //the idea is that webhostenironment will identify the path from where to get our products from Json file
        public IWebHostEnvironment WebHostEnvironment { get; }
        public JsonFileProductService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }
        //getting the json file path here
        private string JsonFileName
        {
            get
            {
                return Path.Combine(WebHostEnvironment.WebRootPath, "data", "products.json");
               
            }
      
        }
        public IEnumerable<Product> GetProducts()
        {
            using (var jsonFileReader=File.OpenText(JsonFileName))
            {
                return JsonSerializer.Deserialize<Product[]>(jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
        }

        public void AddRating(string productId, int rating)
        {
            var products = GetProducts();
            var query = products.First(p => p.Id == productId);
            if (query.Ratings==null)
            {
                //if it is null. then will initialize and set it
                query.Ratings = new int[] { rating };
            }
            else
            {
                //if there are rating then will do the following
                var ratings = query.Ratings.ToList();
                ratings.Add(rating);
                query.Ratings = ratings.ToArray();

            }


        }
    }
}
