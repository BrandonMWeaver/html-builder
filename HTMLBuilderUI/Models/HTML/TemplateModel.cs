using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLBuilderUI.HTML.Models
{
    class TemplateModel
    {
        public List<ElementModel> Elements { get; set; }

        public override string ToString()
        {
            string body = "";
            foreach(ElementModel element in this.Elements)
            {
                body = $"{body}{element}";
            }
            return $"<!DOCTYPE html><html><head><title>Test</title></head><body>{body}</body></html>";
        }
    }
}
