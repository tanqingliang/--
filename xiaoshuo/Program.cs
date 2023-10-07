// Console.WriteLine("Hello, World!");
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;
using RestSharp;
using xiaoshuo.Libs;


// 注册编码
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);


// await SuiYue.GetList("横推永生，从神象镇狱劲开始", "/read/59269098.html", false);

// await SuiYue.GetList("洪荒二郎传", "/read/54239.html", true);

// await SuiYue.GetList("谍影：命令与征服", "/read/51502089.html", false);

// await SuiYue.GetList("律师本色", "https://www.suiyuexs.com/read/27916896.html", false);

// await BiQu.GetList("从破碎虚空开始", "https://www.biqubao1.com/book/43799/", true);

await XFueDu.GetList("盘龙开端之纵横三界", "http://www.xfuedu.org/bxwx/28764/", true);




