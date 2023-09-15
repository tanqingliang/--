using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace xiaoshuo.Libs
{
	/// <summary>
	/// 岁月小说网
	/// </summary>
	public class SuiYue
	{

		// 域名
		const string domian = "https://www.suiyuexs.com";



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
			var doc = await web.LoadFromWebAsync(url, Encoding.GetEncoding("gbk"));

			var list = doc.DocumentNode.SelectNodes("//div[@class='book-chapter-list']/ul[2]/li/a");


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

				var doc = await web.LoadFromWebAsync(domian + url, Encoding.GetEncoding("gbk"));

				string content = doc.DocumentNode.InnerHtml;

				var ssid = GetRegexValue(content, "^*var ssid=(.*);;*");
				// Console.WriteLine($"ssid=>{ssid}");

				var bookid = GetRegexValue(content, "^*bookid=(.*);;*");
				// Console.WriteLine($"bookid=>{bookid}");

				var chapterid = GetRegexValue(content, "^*chapterid=(.*);;*");
				// Console.WriteLine($"chapterid=>{chapterid}");


				url = $"/files/article/html{ssid}/{(Convert.ToInt32(bookid) / 1000)}/{bookid}/{chapterid}.html";
				Console.WriteLine(url);

				if (url.IndexOf("://") < 0)
				{
					url = domian + url;
				}
				doc = await web.LoadFromWebAsync(url, Encoding.GetEncoding("gbk"));

				content = doc.DocumentNode.InnerText;
				content = content
				.Replace("&nbsp;&nbsp;&nbsp;&nbsp;", "\n")
				.Replace("&nbsp;", "\t")
				.Replace("var cctxt='", string.Empty);

				content = ReplaceContent(content);

				var data = new List<string>() { content, string.Empty };

				if (isTitle) data.Insert(0, title);

				// Console.WriteLine(content);

				await File.AppendAllLinesAsync(fileName, data);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"****************文章获取异常：{ex.Message}*******************");
			}

		}



		/// <summary>
		/// 正则取值
		/// </summary>
		static string GetRegexValue(string content, string pattern)
		{
			Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
			Match match = regex.Match(content);

			if (match.Success)
			{
				return match.Groups[1].Value;
			}
			return string.Empty;
		}

		/// <summary>
		/// 替换内容
		/// </summary>
		/// <param name="content"></param>
		/// <returns></returns>
		static string ReplaceContent(string content)
		{
			Regex regex = new Regex("^*cctxt=cctxt.replace\\(/(.+)/g,'(.+)'\\);;*", RegexOptions.IgnoreCase);
			Match match = regex.Match(content);

			if (match.Success)
			{
				string text = match.Groups[0].Value;
				string key = match.Groups[1].Value;
				string value = match.Groups[2].Value;

				content = content.Replace(text, string.Empty).Replace(key, value);

				return ReplaceContent(content);
			}

			return content;

		}

	}
}