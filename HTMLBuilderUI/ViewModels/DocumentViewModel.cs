using HTMLBuilderUI.HTML.Models;
using HTMLBuilderUI.Models.Parents;
using HTMLBuilderUI.ViewModels.Commands;
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

        private string _selectedElementFieldsString;

        public string SelectedElementFieldsString
        {
            get { return this._selectedElementFieldsString; }
            set
            {
                this._selectedElementFieldsString = value;
                this.OnPropertyChanged(nameof(this.SelectedElementFieldsString));
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

        private string _fields;

        public string Fields
        {
            get { return this._fields; }
            set
            {
                this._fields = value;
                this.OnPropertyChanged(nameof(this.Fields));
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
        public ParameterlessCommand BuildDocumentCommand { get; set; }

        public Command<ElementModel> SelectElementCommand { get; set; }
        public Command<ElementModel> SwapElementsUpCommand { get; set; }
        public Command<ElementModel> SwapElementsDownCommand { get; set; }

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
            this.BuildDocumentCommand = new ParameterlessCommand(this.BuildDocument);

            this.SelectElementCommand = new Command<ElementModel>(this.SelectElement);
            this.SwapElementsUpCommand = new Command<ElementModel>(this.SwapElementsUp);
            this.SwapElementsDownCommand = new Command<ElementModel>(this.SwapElementsDown);

            string html = string.Empty;
            foreach (ElementModel element in this.Elements)
            {
                html = $"{html}{element}";
            }
            this.Document = $"<!DOCTYPE html>{html}";

            this.SelectedElementFieldsString = string.Empty;
            this.Type = string.Empty;
            this.Fields = string.Empty;
            this.InnerHTML = string.Empty;
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
            this.SelectedElementFieldsString = string.Join(", ", this.SelectedElement.Fields);
        }

        public void BuildDocument()
        {
            if (this.SelectedElementFieldsString != string.Empty)
            {
                this.SelectedElement.Fields = new List<string>(this.SelectedElementFieldsString.Split(','));
            }
            string html = string.Empty;
            foreach (ElementModel element in this.Elements)
            {
                html = $"{html}{element}";
            }
            this.Document = $"<!DOCTYPE html>{html}";
        }

        public void AppendElement()
        {
            if (this.Fields != string.Empty)
                this.SelectedElement.Append(new ElementModel(this.Type, new List<string>(this.Fields.Split(',')), this.InnerHTML));
            else
                this.SelectedElement.Append(new ElementModel(this.Type, this.InnerHTML));
            this.Type = string.Empty;
            this.Fields = string.Empty;
            this.InnerHTML = string.Empty;

            this.BuildDocument();
        }

        public void SwapElementsUp(ElementModel element)
        {
            int index1 = this.SelectedElement.Elements.IndexOf(element);
            int index2 = index1 - 1;
            if (index2 >= 0)
            {
                ElementModel tempElement = this.SelectedElement.Elements[index2];
                this.SelectedElement.Elements[index2] = element;
                this.SelectedElement.Elements[index1] = tempElement;
            }
            this.BuildDocument();
        }

        public void SwapElementsDown(ElementModel element)
        {
            int index1 = this.SelectedElement.Elements.IndexOf(element);
            int index2 = index1 + 1;
            if (index2 < this.SelectedElement.Elements.Count)
            {
                ElementModel tempElement = this.SelectedElement.Elements[index2];
                this.SelectedElement.Elements[index2] = element;
                this.SelectedElement.Elements[index1] = tempElement;
            }
            this.BuildDocument();
        }
    }
}
