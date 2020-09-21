using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLBuilderUI.HTML.Models
{
    class ElementModel
    {
        public int Depth { get; set; }
        public string Indentation { get; set; }

        public string Type { get; set; }
        public string InnerHTML { get; set; }
        public List<string> Fields { get; set; }

        public List<ElementModel> Elements { get; set; }

        public ElementModel(string type, string innerHTML = "")
        {
            this.Indentation = "";
            this.Type = type;
            this.InnerHTML = innerHTML;
            this.Fields = new List<string>();
            this.Elements = new List<ElementModel>();
        }

        public ElementModel(string type, List<string> fields, string innerHTML = "")
        {
            this.Indentation = "";
            this.Type = type;
            this.InnerHTML = innerHTML;
            this.Fields = fields;
            this.Elements = new List<ElementModel>();
        }

        public void Append(ElementModel element)
        {
            element.Depth = this.Depth + 1;
            for (int i = 0; i < element.Depth; i++)
            {
                element.Indentation = $"{element.Indentation}\t";
            }
            this.Elements.Add(element);
        }

        public override string ToString()
        {
            string children = "";
            foreach(ElementModel element in this.Elements)
            {
                children = $"{children}{element}";
            }
            string elementFields = "";
            foreach (string field in this.Fields)
            {
                elementFields = $" {elementFields}{field}";
            }
            if (this.InnerHTML != string.Empty && children != string.Empty)
                return $"\n{this.Indentation}<{this.Type}{elementFields}>\n{this.Indentation}\t{this.InnerHTML}\n{this.Indentation}\t{children}\n{this.Indentation}</{Type}>";
            else if (this.InnerHTML != string.Empty)
                return $"\n{this.Indentation}<{this.Type}{elementFields}>\n{this.Indentation}\t{this.InnerHTML}\n{this.Indentation}</{Type}>";
            else if (children != string.Empty)
                return $"\n{this.Indentation}<{this.Type}{elementFields}>{children}\n{this.Indentation}</{Type}>";
            else
                return $"\n{this.Indentation}<{this.Type}{elementFields}>\n{this.Indentation}</{Type}>";
        }
    }
}
