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
        //private static readonly Regex elementOrderExpression = new Regex(@"(\<(.*?)\>)(\w.+)?(?=<)|(\<(.*?)\>)");
        private static readonly Regex elementOrderExpression = new Regex(@"(<.+?>)([^<\n]?)+|(<.+?>)");
        private static readonly Regex elementTypeExpression = new Regex(@"(?<=<)\w+");
        private static readonly Regex elementPropertiesExpression = new Regex("\\w+=\".+?(?<=\")");
        private static readonly Regex elementInnerHTMLExpression = new Regex(@"(?<=>)[\w\s.]+");

        public static List<ElementModel> Parse(string document)
        {
            List<string> elementOrderTree = GetElementOrderTree(document);

            string currentElementType = elementTypeExpression.Match(elementOrderTree[0]).ToString();
            List<string> currentElementProperties = GetElementProperties(elementOrderTree[0]);
            string currentElementInnerHTML = elementInnerHTMLExpression.Match(elementOrderTree[0]).ToString();

            ElementModel currentElement = new ElementModel(currentElementType, currentElementProperties, currentElementInnerHTML);
            currentElement.ParentElement = currentElement;
            elementOrderTree.Remove(elementOrderTree[0]);
            List<ElementModel> elementTree = new List<ElementModel>()
            {
                currentElement
            };

            foreach (string selection in elementOrderTree)
            {
                if (!selection.StartsWith("</"))
                {
                    string type = elementTypeExpression.Match(selection).ToString();
                    List<string> properties = GetElementProperties(selection);
                    string innerHTML = elementInnerHTMLExpression.Match(selection).ToString();

                    ElementModel childElement = new ElementModel(type, properties, innerHTML);
                    currentElement.Append(childElement);
                    if (selection.Contains("/>"))
                    {
                        childElement.IsSelfClosing = true;
                    }
                    else
                        currentElement = childElement;
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
                if (match.ToString() != string.Empty && !match.ToString().Contains("!DOCTYPE"))
                {
                    matches.Add(match.ToString());
                }
            }
            return matches;
        }

        private static List<string> GetElementProperties(string element)
        {
            List<string> elementProperties = new List<string>();
            foreach (Match propertyMatch in elementPropertiesExpression.Matches(element))
                elementProperties.Add(propertyMatch.ToString());
            return elementProperties;
        }
    }
}
