using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace xiaoshuo.Libs
{
	/// <summary>
	/// 笔趣E
	/// </summary>
	public class BiQu
	{

		// 域名
		const string domian = "https://www.biqubao1.com/";



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
			var doc = await web.LoadFromWebAsync(url, Encoding.GetEncoding("UTF-8"));

			var list = doc.DocumentNode.SelectNodes("//div[@id='list']/dl/dd/a");


			foreach (var item in list)
			{
				Console.WriteLine($"{item.InnerText} -> {item.Attributes["href"].Value}");

				if (!isRead && item.InnerText != startTitle) continue;

				isRead = true;

				await GetInfo(fileName, isTitle, item.InnerText, item.Attributes["href"].Value);
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

				var doc = await web.LoadFromWebAsync(domian + url, Encoding.GetEncoding("UTF-8"));

				string content = doc.DocumentNode.SelectSingleNode("//div[@id='content']").InnerText;

				var index = content.IndexOf("看最新章节内容下载爱阅小说app");
				Console.WriteLine(index);
				if (index > 0)
				{
					content = content.Substring(0, index);
				}

				content = content
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