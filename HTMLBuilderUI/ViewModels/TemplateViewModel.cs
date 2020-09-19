using HTMLBuilderUI.HTML.Models;
using HTMLBuilderUI.ViewModels.Parents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLBuilderUI.ViewModels
{
    class TemplateViewModel : NotificationBase
    {
        private TemplateModel _templateModel;

        public TemplateModel TemplateModel
        {
            get { return this._templateModel; }
            set
            {
                this._templateModel = value;
                this.OnPropertyChanged(nameof(this.TemplateModel));
            }
        }

        public TemplateViewModel()
        {
            this.TemplateModel = new TemplateModel();
            this.TemplateModel.Elements.Add(new ElementModel("h1", "<a href=\"https://google.com\">Test</a>"));

            using (StreamWriter sw = new StreamWriter("../../Storage/test.html"))
            {
                sw.WriteLine(this.TemplateModel);
            }

            System.Diagnostics.Process.Start("C:\\Users\\a7xnc\\source\\repos\\HTMLBuilder\\HTMLBuilderUI\\Storage\\test.html");
        }
    }
}
