using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace xiaoshuo.Libs
{
	public class XFueDu
	{

		// 域名
		const string domian = "http://www.xfuedu.org/";

		// 字符集
		readonly static Encoding encoding = Encoding.GetEncoding("UTF-8");


		// 获取文章列表
		public static async Task GetList(string name, string url, bool isTitle = true, string startTitle = null)
		{
			string fileName = name + ".txt";

			// 开始读取文章
			bool isRead = string.IsNullOrEmpty(startTitle);

			if (isRead)
			{
				// 判断存在删除
				if (File.Exists(fileName))
				{
					File.Delete(fileName);
				}

				await File.AppendAllLinesAsync(fileName, new List<string>() { name, string.Empty });
			}

			// From Web
			var web = new HtmlWeb();

			if (url.IndexOf("://") < 0)
			{
				url = domian + url;
			}
			var doc = await web.LoadFromWebAsync(url, encoding);

			var list = doc.DocumentNode.SelectNodes("//div[@class='section-box'][2]/ul[@class='section-list fix']/li/a");


			foreach (var item in list)
			{
				Console.WriteLine($"{item.InnerText} -> {item.Attributes["href"].Value}");

				if (!isRead && item.InnerText != startTitle) continue;

				isRead = true;

				await GetInfo(fileName, isTitle, item.InnerText, url + item.Attributes["href"].Value);
			}

		}



		// 获取文章详细
		public static async Task GetInfo(string fileName, bool isTitle, string title, string url)
		{

			try
			{
				// From Web
				var web = new HtmlWeb();
				Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

				var doc = await web.LoadFromWebAsync(domian + url, encoding);

				string content = doc.DocumentNode.SelectSingleNode("//div[@id='content']").InnerText;
				// Console.WriteLine(content);

				content = content
				.Replace("                        章节错误,点此举报(免注册),举报后维护人员会在两分钟内校正章节内容,请耐心等待,并刷新页面。", "")
				.Replace("                        ", "\t")
				.Replace("　　", "\n\t");



				var data = new List<string>() { content, string.Empty };

				if (isTitle) data.Insert(0, title);

				await File.AppendAllLinesAsync(fileName, data);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"****************文章获取异常：{ex.Message}*******************");
			}

		}




	}
}