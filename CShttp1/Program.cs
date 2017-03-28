using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CShttp1
{
    class Program
    {
        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            RunAsync().Wait();
            RunAsyncXML().Wait();
            Console.ReadLine();
        }


        static async Task<Product> GetProductAsync(string path)
        {
            Product product = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<Product>();
            }
            return product;
        }

        static void ShowProduct(Product product)
        {
            Console.WriteLine($"{product.message}: is a JSON with status {product.status} and first value {product.computed} and second value {product.computed2}");
        }

        static void ShowProductXML(ProductXML product)
        {
            Console.WriteLine($"{product.message}: is a XML with status {product.status} and first value {product.computed} and second value {product.computed2}");
        }

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri("http://localhost:3000/getJSONnumberextra/45/89");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                Product product = new Product();

                product = await GetProductAsync(client.BaseAddress.ToString());
                ShowProduct(product);

            }
            catch (Exception)
            {

                throw;
            }

            
        }

        static async Task RunAsyncXML()
        {
            string url = "http://localhost:3000/getXMLnumberextra/45/89";
            Uri uri = new Uri(url);
            WebClient wc = new WebClient();
            var data = await wc.DownloadStringTaskAsync(uri);
            XDocument doc = XDocument.Parse(data);
            var serializer = new XmlSerializer(typeof(ProductXML));
            var productxml = (ProductXML)serializer.Deserialize(doc.CreateReader());
            ShowProductXML(productxml);
            Console.WriteLine(doc);

        }
    }

    public class Product
    {
        public int status { get; set; }
        public string message { get; set; }
        public string computed { get; set; }
        public string computed2 { get; set; }
    }

    [XmlRoot("map")]
    public class ProductXML
    {
        [XmlElement("status")]
        public int status { get; set; }
        [XmlElement("message")]
        public string message { get; set; }
        [XmlElement("computed")]
        public string computed { get; set; }
        [XmlElement("computed2")]
        public string computed2 { get; set; }
    }
}
