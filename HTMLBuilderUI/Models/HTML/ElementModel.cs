using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLBuilderUI.HTML.Models
{
    class ElementModel
    {
        public string Type { get; set; }
        public string InnerHTML { get; set; }

        public ElementModel(string type, string innerHTML = "")
        {
            this.Type = type;
            this.InnerHTML = innerHTML;
        }

        public override string ToString()
        {
            return $"<{this.Type}>{InnerHTML}</{Type}>";
        }
    }
}
