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
        private static readonly Regex elementInnerHTMLExpression = new Regex(@"(?<=>)[\s\S]*");

        public static List<ElementModel> Parse(string document)
        {
            List<string> elementOrderTree = GetElementOrderTree(document);

            string currentElementType = elementTypeExpression.Match(elementOrderTree[0]).ToString();
            List<string> currentElementProperties = GetElementProperties(elementOrderTree[0]);
            string currentElementInnerHTML = elementInnerHTMLExpression.Match(elementOrderTree[0]).ToString();

            ElementModel currentElement = new ElementModel(currentElementType, currentElementProperties, currentElementInnerHTML);
            currentElement.ClosingTag = GetElementClosingString(0, elementOrderTree);
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
                    if (ElementIsSelfClosing(selection))
                    {
                        childElement.IsSelfClosing = true;
                    }
                    else
                    {
                        string closingString = GetElementClosingString(elementOrderTree.IndexOf(selection), elementOrderTree);
                        childElement.ClosingTag = closingString;
                        currentElement = childElement;
                    }
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

        private static List<string> GetElementProperties(string element)
        {
            List<string> elementProperties = new List<string>();
            foreach (Match propertyMatch in elementPropertiesExpression.Matches(element))
                elementProperties.Add(propertyMatch.ToString());
            return elementProperties;
        }

        private static string GetElementClosingString(int selectionIndex, List<string> elementOrderString)
        {
            string selectionType = elementTypeExpression.Match(elementOrderString[selectionIndex]).ToString();
            System.Diagnostics.Debug.WriteLine(selectionType);
            int i = selectionIndex + 1;
            while (!elementOrderString[i].StartsWith($"</{selectionType}"))
            {
                if (elementOrderString[i].StartsWith($"<{selectionType}"))
                {
                    i++;
                }
                i++;
            }
            return elementOrderString[i];
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
