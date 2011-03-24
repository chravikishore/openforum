using System.Linq;
using HtmlAgilityPack;

namespace OpenForum.Core
{
	public static class HtmlExtensions
	{
		public static void ValidateHtml(this string self)
		{
			HtmlDocument document = new HtmlDocument();
			document.LoadHtml(self ?? "");

			if (document.ParseErrors.Count > 0)
			{
				throw new OpenForumException("Malformed html.");
			}
		}

		public static string FixUpHtml(this string self)
		{
			string[] validElements = { "a", "b", "i", "p", "span", "br", "ul", "ol", "li" };
			string[] validAttributes = { "style", "href" };
			return self.FixUpHtml(validElements, validAttributes);
		}

		// TODO: unit test
		public static string RemoveAllMarkup(this string self)
		{
			return self.FixUpHtml(new string[0], new string[0]);
		}

		private static string FixUpHtml(this string self, string[] validElements, string[] validAttributes)
		{
			HtmlDocument document = new HtmlDocument();
			document.LoadHtml(self ?? "");

			FixUpElement(document.DocumentNode, validElements, validAttributes);

			return document.DocumentNode.OuterHtml;
		}

		private static void FixUpElement(HtmlNode item, string[] validElements, string[] validAttributes)
		{
			if (item.NodeType == HtmlNodeType.Element)
			{
				if (validElements.Contains(item.Name))
				{
					for (int i = item.Attributes.Count - 1; i >= 0; i--)
					{
						if (!validAttributes.Contains(item.Attributes[i].Name))
						{
							item.Attributes.RemoveAt(i);
						}
						else if (item.Attributes[i].Name == "style" && (item.Attributes[i].Value.Contains("background") || item.Attributes[i].Value.Contains("/*")))
						{
							item.Attributes.RemoveAt(i);
						}
						else if (item.Attributes[i].Name == "href" && item.Attributes[i].Value.Trim().StartsWith("javascript"))
						{
							item.Attributes.RemoveAt(i);
						}
					}

					if (item.Name == "a")
					{
						item.Attributes.Append("rel", "nofollow");
					}
				}
				else
				{
					HtmlDocument document = new HtmlDocument();
					document.LoadHtml(item.InnerHtml);
					HtmlNode newHtml = document.DocumentNode;
					item.ParentNode.ReplaceChild(newHtml, item);
					item = newHtml;
				}
			}

			foreach (var child in item.ChildNodes)
			{
				FixUpElement(child, validElements, validAttributes);
			}
		}
	}
}
