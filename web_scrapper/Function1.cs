using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using HtmlAgilityPack;
using System.Reflection.Metadata;
using System.Xml.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch.Internal;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace web_scrapper
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;
             async Task<List<string[] >> amazon(string name1){

                using (var client = new HttpClient())
                {
                    var amzoresponse = await client.GetAsync($"https://www.amazon.com.tr/s?k={name1}&i=specialty-aps&srs=20467303031&__mk_tr_TR=%C3%85M%C3%85%C5%BD%C3%95%C3%91&ref=nb_sb_noss");
                    amzoresponse.EnsureSuccessStatusCode();

                    var amzocontent = await amzoresponse.Content.ReadAsStringAsync();


                    var doc = new HtmlDocument();
                    doc.LoadHtml(amzocontent);

                    // XPath ile ilgili elementleri seçin
                    var amznprice_nodes = doc.DocumentNode.SelectNodes("//span[@class=\"a-offscreen\"]");
                    var amznimg_nodes = doc.DocumentNode.SelectNodes("//img[@class=\"s-image\"]");

                    List<string> para = new List<string>();
                    List<string> resim = new List<string>();

                    if (amznprice_nodes != null && amznimg_nodes != null)
                    {
                        foreach (var node in amznprice_nodes)
                        {

                            para.Add(node.InnerText);
                        }
                        foreach (var node in amznimg_nodes)
                        {
                            resim.Add(node.Attributes["src"].Value);


                        }

                       
                        var result = para.Zip(resim, (a, b) => new string[] { a, b }).ToList();
                        return result;
                    }
                    
                    return null;
                    
                  

                }
            }

            async Task<List<string[]>> hepsiburada(string name1)
            {
                using (var client1 = new HttpClient())
                {
                    var response1 = await client1.GetAsync($"https://www.hepsiburada.com/ara?q={name1}");
                    response1.EnsureSuccessStatusCode();

                    var content1 = await response1.Content.ReadAsStringAsync();


                    var doc1 = new HtmlDocument();
                    doc1.LoadHtml(content1);

                    // XPath ile ilgili elementleri seçin 

                    var nodes2 = doc1.DocumentNode.SelectNodes("//div[@data-test-id='price-current-price']");
                    var nodes12 = doc1.DocumentNode.SelectNodes("//img[@class=\"moria-ProductCard-dglYMa byYtL svglefferlp\"]");
                    List<string> para1 = new List<string>();
                    List<string> resim1 = new List<string>();


                    if (nodes2 != null && nodes12 != null)
                    {
                        foreach (var node in nodes2)
                        {

                            para1.Add(node.InnerText);
                        }
                        foreach (var node in nodes12)
                        {
                            resim1.Add(node.Attributes["src"].Value);


                        }
                        var result2 = para1.Zip(resim1, (a, b) => new string[] { a, b }).ToList();
                        return result2;

                    }
                    return null;
                    

                }

            }


           var selam = await amazon(name);
           // var selam2 = await hepsiburada(name);


            return new OkObjectResult(selam);
        }
           
        }
        

    }

           
        
   


