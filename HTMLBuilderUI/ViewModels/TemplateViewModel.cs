using HTMLBuilderUI.HTML.Models;
using HTMLBuilderUI.ViewModels.Commands.Template;
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

        public ParameterlessCommand OpenCommand { get; set; }

        public TemplateViewModel()
        {
            this.TemplateModel = new TemplateModel();

            this.OpenCommand = new ParameterlessCommand(this.Open);

            this.TemplateModel.Elements.Add(new ElementModel("html"));
            this.TemplateModel.Elements[0].Depth = 0;
            this.TemplateModel.Elements[0].Append(new ElementModel("head"));
            this.TemplateModel.Elements[0].Elements[0].Append(new ElementModel("title", "Test"));
            this.TemplateModel.Elements[0].Append(new ElementModel("body"));
            this.TemplateModel.Elements[0].Elements[1].Append(new ElementModel("h1"));
            this.TemplateModel.Elements[0].Elements[1].Elements[0].Append(new ElementModel("a", new List<string>() { "href=\"https://google.com\"" }, "Test"));

            using (StreamWriter sw = new StreamWriter("../../Storage/test.html"))
            {
                sw.WriteLine(this.TemplateModel);
            }
        }

        public void Open()
        {
            System.Diagnostics.Process.Start("C:\\Users\\a7xnc\\source\\repos\\HTMLBuilder\\HTMLBuilderUI\\Storage\\test.html");
        }
    }
}
