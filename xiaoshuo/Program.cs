// Console.WriteLine("Hello, World!");
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;
using RestSharp;
using xiaozaixiaoshuo.Libs;


// 注册编码
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);


// await SuiYue.GetList("横推永生，从神象镇狱劲开始", false, "/read/59269098.html");

await SuiYue.GetList("洪荒二郎传", "/read/54239.html", true, "第四百一十章 逆天凤翎燃！本命神通现！");




