using System.Linq;
using HtmlAgilityPack;

namespace OpenForum.Core.Models
{
    public static class HtmlHelper
    {
        private static readonly string[] VALID_ELEMENTS = { "a", "b", "i", "p", "span", "br", "ul", "ol", "li" };
        private static readonly string[] VALID_ATTRIBUTES = { "style", "href" };

        public static void Validate(string html)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html ?? "");

            if (document.ParseErrors.Count > 0)
            {
                throw new OpenForumException("Malformed html.");
            }
        }

        public static string FixUp(string html)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html ?? "");

            FixUpElement(document.DocumentNode);

            return document.DocumentNode.OuterHtml;
        }

        private static void FixUpElement(HtmlNode item)
        {
            if (item.NodeType == HtmlNodeType.Element)
            {

                if (VALID_ELEMENTS.Contains(item.Name))
                {
                    for (int i = item.Attributes.Count - 1; i >= 0; i--)
                    {
                        if (!VALID_ATTRIBUTES.Contains(item.Attributes[i].Name))
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
                FixUpElement(child);
            }
        }
    }
}
