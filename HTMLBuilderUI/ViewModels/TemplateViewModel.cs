using HTMLBuilderUI.HTML.Models;
using HTMLBuilderUI.ViewModels.Commands;
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

        private ElementModel _selectedElement;

        public ElementModel SelectedElement
        {
            get { return this._selectedElement; }
            set
            {
                this._selectedElement = value;
                this.OnPropertyChanged(nameof(this.SelectedElement));
            }
        }

        public ParameterlessCommand OpenCommand { get; set; }
        public Command<ElementModel> SelectElementCommand { get; set; }

        public TemplateViewModel()
        {
            this.TemplateModel = new TemplateModel();

            this.OpenCommand = new ParameterlessCommand(this.Open);
            this.SelectElementCommand = new Command<ElementModel>(this.SelectElement);

            this.TemplateModel.Elements.Add(new ElementModel("html"));
            this.TemplateModel.Elements[0].ParentElement = this.TemplateModel.Elements[0];
            this.TemplateModel.Elements[0].Depth = 0;

            this.SelectedElement = this.TemplateModel.Elements[0];

            this.TemplateModel.Elements[0].Append(new ElementModel("head"));
            this.TemplateModel.Elements[0].Elements[0].Append(new ElementModel("title", "Test"));
            this.TemplateModel.Elements[0].Append(new ElementModel("body"));
            this.TemplateModel.Elements[0].Elements[1].Append(new ElementModel("h1", new List<string>() { "style=\"margin: 0; text-align: center; font-family: monospace;\"" }));
            this.TemplateModel.Elements[0].Elements[1].Elements[0].Append(new ElementModel("a", new List<string>() { "href=\"https://google.com\"" }, "Test"));
            this.TemplateModel.Elements[0].Elements[1].Append(new ElementModel("button", new List<string>() { "style=\"margin: 0 auto;\"" }, "Test"));
            this.TemplateModel.Elements[0].Elements[1].Append(new ElementModel(
                "script",
                "document.querySelector(\"button\").addEventListener(\"click\", () => document.body.style.backgroundColor = \"#F00\");"
            ));
            using (StreamWriter sw = new StreamWriter("../../Storage/test.html"))
            {
                sw.WriteLine(this.TemplateModel);
            }
        }

        public void Open()
        {
            System.Diagnostics.Process.Start("C:\\Users\\a7xnc\\source\\repos\\HTMLBuilder\\HTMLBuilderUI\\Storage\\test.html");
        }

        public void SelectElement(ElementModel element)
        {
            this.SelectedElement = element;
        }
    }
}
