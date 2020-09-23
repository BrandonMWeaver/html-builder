using HTMLBuilderUI.HTML.Models;
using HTMLBuilderUI.ViewModels.Commands;
using HTMLBuilderUI.ViewModels.Commands.Template;
using HTMLBuilderUI.ViewModels.Parents;
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
            get 
            {
                string html = string.Empty;
                foreach (ElementModel element in this.Elements)
                {
                    html = $"{html}{element}";
                }
                return $"<!DOCTYPE html>{html}";
            }
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

        public ParameterlessCommand OpenCommand { get; set; }

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
            using (StreamWriter sw = new StreamWriter("../../Storage/test.html"))
            {
                sw.WriteLine(this.Document);
            }

            this.OpenCommand = new ParameterlessCommand(this.Open);

            this.SelectElementCommand = new Command<ElementModel>(this.SelectElement);
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
