using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using customconnector.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace customconnector.Controllers
{
    public class HomeController : Controller
    {
        
        private MyDbContext _context;
        private IHostingEnvironment _env;
        public HomeController(MyDbContext context,IHostingEnvironment env)
        {
            _context =context;
            _env =env;
        }
        public IActionResult Index()
        {
            var dict = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(Request.QueryString.Value);
            if(dict.ContainsKey("webhook_url")&& dict.ContainsKey("user_objectId")){
                var url = dict["webhook_url"][0];
                var id = dict["user_objectId"][0];

                _context.Subscriptions.Add(new Subscription(){
                    ObjectID = id,
                    WebHookUrl = url
                });
                _context.SaveChanges();
            }

            return View(_context.Subscriptions.ToArray());
        }

        public IActionResult Push()
        {
            var dict = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(Request.QueryString.Value);
            if(dict.ContainsKey("action")){

                var file = System.IO.File.ReadAllText(Path.Combine(_env.ContentRootPath,"message.txt"));

                foreach (var item in _context.Subscriptions)
                {
                    var client = new HttpClient();
                    var content = new StringContent(file);
                    client.PostAsync(item.WebHookUrl,content);
                }
            }
            return View();
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
