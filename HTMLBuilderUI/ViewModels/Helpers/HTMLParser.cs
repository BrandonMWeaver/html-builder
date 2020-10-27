using HTMLBuilderUI.HTML.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HTMLBuilderUI.ViewModels.Helpers
{
    static class HTMLParser
    {
        private static readonly Regex elementOrderExpression = new Regex("<[^!].*?>[\\s\\S]*?(?=(?<!\"[\\s\\w]*|\"[\\s\\w]*<.*>[\\s\\w]*)</?\\w+.*?>)|(<[^!].*>)");
        private static readonly Regex elementTypeExpression = new Regex(@"(?<=<)\w+");
        private static readonly Regex elementPropertiesExpression = new Regex("\\w+=\"[\\s\\S]+?(?<=\")");
        private static readonly Regex elementInnerHTMLExpression = new Regex(@"(?<=>)[\s\S]*?(?=\s*</?\w)");

        public static List<ElementModel> Parse(string document)
        {
            List<string> elementOrderTree = GetElementOrderTree(document);

            string currentElementType = GetElementType(elementOrderTree[0]);
            List<string> currentElementProperties = GetElementProperties(elementOrderTree[0]);
            string currentElementInnerHTML = GetElementInnerHTML(elementOrderTree[0]);
            ElementModel currentElement = new ElementModel(currentElementType, currentElementProperties, currentElementInnerHTML);

            currentElement.Depth = 0;
            currentElement.ParentElement = currentElement;

            elementOrderTree.Remove(elementOrderTree[0]);

            List<ElementModel> elementTree = new List<ElementModel>()
            {
                currentElement
            };

            foreach(string selection in elementOrderTree)
            {
                if (!selection.StartsWith("</"))
                {
                    ElementModel nextElement = new ElementModel(GetElementType(selection), GetElementProperties(selection), GetElementInnerHTML($"{selection}</a"));
                    currentElement.Append(nextElement);

                    if (!selection.Split('>')[1].StartsWith("\n"))
                        nextElement.IsInlineParent = true;

                    if (ElementIsSelfClosing(selection))
                        nextElement.IsSelfClosing = true;
                    else
                        currentElement = nextElement;
                }
                else
                    currentElement = currentElement.ParentElement;
            }
            return elementTree;
        }

        private static List<string> GetElementOrderTree(string document)
        {
            List<string> matches = new List<string>();
            foreach (Match match in elementOrderExpression.Matches(document))
            {
                matches.Add(match.ToString());
            }
            return matches;
        }

        private static string GetElementType(string selection)
        {
            return elementTypeExpression.Match(selection).ToString();
        }

        private static List<string> GetElementProperties(string selection)
        {
            List<string> elementProperties = new List<string>();
            foreach (Match propertyMatch in elementPropertiesExpression.Matches(selection))
                elementProperties.Add(propertyMatch.ToString());
            return elementProperties;
        }

        private static string GetElementInnerHTML(string selection)
        {
            return elementInnerHTMLExpression.Match(selection).ToString();
        }

        private static bool ElementIsSelfClosing(string selection)
        {
            return selection.Contains("/>")
                || selection.StartsWith("<area")
                || selection.StartsWith("<base")
                || selection.StartsWith("<br")
                || selection.StartsWith("<col")
                || selection.StartsWith("<embed")
                || selection.StartsWith("<hr")
                || selection.StartsWith("<img")
                || selection.StartsWith("<input")
                || selection.StartsWith("<link")
                || selection.StartsWith("<meta")
                || selection.StartsWith("<param")
                || selection.StartsWith("<source")
                || selection.StartsWith("<track")
                || selection.StartsWith("<wbr");
        }
    }
}
