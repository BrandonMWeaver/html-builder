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

        public TemplateModel()
        {
            this.Elements = new List<ElementModel>();
        }

        public override string ToString()
        {
            string html = "";
            foreach(ElementModel element in this.Elements)
            {
                html = $"{html}{element}";
            }
            return $"<!DOCTYPE html>{html}";
        }
    }
}
