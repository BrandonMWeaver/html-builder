using HTMLBuilderUI.HTML.Models;
using HTMLBuilderUI.Models.Parents;
using HTMLBuilderUI.ViewModels.Commands;
using HTMLBuilderUI.ViewModels.Commands.Template;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLBuilderUI.ViewModels
{
    class DocumentViewModel : NotificationBase
    {
        private string _document;

        public string Document
        {
            get { return this._document; }
            set
            {
                this._document = value;
                this.OnPropertyChanged(nameof(this.Document));
            }
        }

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

        private string _type;

        public string Type
        {
            get { return this._type; }
            set
            {
                this._type = value;
                this.OnPropertyChanged(nameof(this.Type));
            }
        }

        private string _innerHTML;

        public string InnerHTML
        {
            get { return this._innerHTML; }
            set
            {
                this._innerHTML = value;
                this.OnPropertyChanged(nameof(this.InnerHTML));
            }
        }

        public ParameterlessCommand OpenCommand { get; set; }
        public ParameterlessCommand AppendElementCommand { get; set; }

        public Command<ElementModel> SelectElementCommand { get; set; }

        public DocumentViewModel()
        {
            this.Elements = new ObservableCollection<ElementModel>()
            {
                new ElementModel("html")
            };
            this.Elements[0].ParentElement = this.Elements[0];
            this.SelectedElement = this.Elements[0];

            // Temporary
            this.SelectedElement.Append(new ElementModel("head"));
            this.SelectedElement.Elements[0].Append(new ElementModel("title", "Test"));

            this.OpenCommand = new ParameterlessCommand(this.Open);
            this.AppendElementCommand = new ParameterlessCommand(this.AppendElement);

            this.SelectElementCommand = new Command<ElementModel>(this.SelectElement);

            string html = string.Empty;
            foreach (ElementModel element in this.Elements)
            {
                html = $"{html}{element}";
            }
            this.Document = $"<!DOCTYPE html>{html}";
        }

        public void Open()
        {
            using (StreamWriter sw = new StreamWriter("../../Storage/test.html"))
            {
                sw.Write(this.Document);
            }
            System.Diagnostics.Process.Start("C:\\Users\\a7xnc\\source\\repos\\HTMLBuilder\\HTMLBuilderUI\\Storage\\test.html");
        }

        public void SelectElement(ElementModel element)
        {
            this.SelectedElement = element;
        }

        public void AppendElement()
        {
            this.SelectedElement.Append(new ElementModel(this.Type, this.InnerHTML));
            this.Type = string.Empty;
            this.InnerHTML = string.Empty;

            string html = string.Empty;
            foreach (ElementModel element in this.Elements)
            {
                html = $"{html}{element}";
            }
            this.Document = $"<!DOCTYPE html>{html}";
        }
    }
}
