using HTMLBuilderUI.Models.Parents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLBuilderUI.HTML.Models
{
    class ElementModel : NotificationBase
    {
        public int Depth { get; set; }
        public string Indentation { get; set; }
        public ElementModel ParentElement { get; set; }

        public string Type { get; set; }
        public string InnerHTML { get; set; }
        public List<string> Fields { get; set; }

        public bool IsSelfClosing { get; set; }
        public bool IsInline { get; set; }
        public bool IsInlineParent { get; set; }

        private ObservableCollection<ElementModel> _elements;

        public ObservableCollection<ElementModel> Elements
        {
            get { return this._elements; }
            set
            {
                this._elements = value;
                this.OnPropertyChanged(nameof(this.Elements));
            }
        }

        public ElementModel(string type, string innerHTML = "")
        {
            this.Indentation = "";
            this.Type = type;
            this.InnerHTML = innerHTML;
            this.Fields = new List<string>();
            this.Elements = new ObservableCollection<ElementModel>();
            this.IsSelfClosing = false;
            this.IsInline = false;
            this.IsInlineParent = false;
        }

        public ElementModel(string type, List<string> fields, string innerHTML = "")
        {
            this.Indentation = "";
            this.Type = type;
            this.InnerHTML = innerHTML;
            this.Fields = fields;
            this.Elements = new ObservableCollection<ElementModel>();
            this.IsSelfClosing = false;
            this.IsInline = false;
            this.IsInlineParent = false;
        }

        public void Append(ElementModel element)
        {
            element.Depth = this.Depth + 1;
            element.Indentation = "";
            for (int i = 0; i < element.Depth; i++)
            {
                element.Indentation = $"{element.Indentation}\t";
            }
            foreach (ElementModel e in element.Elements)
            {
                e.Depth = element.Depth + 1;
                e.Indentation = $"{element.Indentation}\t";
            }
            element.ParentElement = this;
            this.Elements.Add(element);
        }

        public override string ToString()
        {
            string elementChildren = "";
            foreach (ElementModel element in this.Elements)
            {
                if (this.IsInlineParent)
                    element.IsInline = true;
                else
                    element.IsInline = false;
                elementChildren = $"{elementChildren}{element}";
            }
            string elementFields = "";
            foreach (string field in this.Fields)
            {
                elementFields = $"{elementFields} {field}";
            }
            if (this.IsSelfClosing && this.IsInline)
                return $"<{this.Type}{elementFields} />{this.InnerHTML}";
            else if (this.IsSelfClosing)
                return $"\n{this.Indentation}<{this.Type}{elementFields} />{this.InnerHTML}";
            else if (this.IsInline)
                return $"<{this.Type}{elementFields}>{this.InnerHTML}{elementChildren}</{this.Type}>";
            else if (this.IsInlineParent)
                return $"\n{this.Indentation}<{this.Type}{elementFields}>{this.InnerHTML}{elementChildren}</{this.Type}>";
            else
                return $"\n{this.Indentation}<{this.Type}{elementFields}>{this.InnerHTML}{elementChildren}\n{this.Indentation}</{this.Type}>";
        }
    }
}
