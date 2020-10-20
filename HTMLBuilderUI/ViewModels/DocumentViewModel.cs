using HTMLBuilderUI.HTML.Models;
using HTMLBuilderUI.Models.Parents;
using HTMLBuilderUI.ViewModels.Commands;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        private bool _openInBrowserAvailable;

        public bool OpenInBrowserAvailable
        {
            get { return this._openInBrowserAvailable; }
            set
            {
                this._openInBrowserAvailable = value;
                this.OnPropertyChanged(nameof(this.OpenInBrowserAvailable));
            }
        }

        private Visibility _selectElementButtonVisibility;

        public Visibility SelectElementButtonVisibility
        {
            get { return this._selectElementButtonVisibility; }
            set
            {
                this._selectElementButtonVisibility = value;
                this.OnPropertyChanged(nameof(this.SelectElementButtonVisibility));
            }
        }

        private string _selectElementButtonMargin;

        public string SelectElementButtonMargin
        {
            get { return this._selectElementButtonMargin; }
            set
            {
                this._selectElementButtonMargin = value;
                this.OnPropertyChanged(nameof(this.SelectElementButtonMargin));
            }
        }

        private int _selectElementButtonHeight;

        public int SelectElementButtonHeight
        {
            get { return this._selectElementButtonHeight; }
            set
            {
                this._selectElementButtonHeight = value;
                this.OnPropertyChanged(nameof(this.SelectElementButtonHeight));
            }
        }

        private bool _selectElementAvailable;

        public bool SelectElementAvailable
        {
            get { return this._selectElementAvailable; }
            set
            {
                this._selectElementAvailable = value;
                this.SelectElementButtonVisibility = value ? Visibility.Visible : Visibility.Hidden;
                this.SelectElementButtonMargin = value ? "0,10,0,0" : "0,5,0,0";
                this.SelectElementButtonHeight = value ? 25 : 0;
                this.OnPropertyChanged(nameof(this.SelectElementAvailable));
            }
        }

        private bool _removeElementAvailable;

        public bool RemoveElementAvailable
        {
            get { return this._removeElementAvailable; }
            set
            {
                this._removeElementAvailable = value;
                this.OnPropertyChanged(nameof(this.RemoveElementAvailable));
            }
        }

        public string FilePath { get; set; }

        public ParameterlessCommand NewCommand { get; set; }
        public ParameterlessCommand OpenCommand { get; set; }
        public ParameterlessCommand SaveCommand { get; set; }
        public ParameterlessCommand SaveAsCommand { get; set; }
        public ParameterlessCommand OpenInBrowserCommand { get; set; }
        public ParameterlessCommand AppendElementCommand { get; set; }
        public ParameterlessCommand BuildDocumentCommand { get; set; }
        public ParameterlessCommand RemoveElementCommand { get; set; }

        public Command<ElementModel> SelectElementCommand { get; set; }
        public Command<ElementModel> SwapElementsUpCommand { get; set; }
        public Command<ElementModel> SwapElementsDownCommand { get; set; }
        public Command<ElementModel> InsertElementCommand { get; set; }
        public Command<ElementModel> ExtractElementCommand { get; set; }

        public DocumentViewModel()
        {
            this.NewCommand = new ParameterlessCommand(this.New);
            this.OpenCommand = new ParameterlessCommand(this.Open);
            this.SaveCommand = new ParameterlessCommand(this.Save);
            this.SaveAsCommand = new ParameterlessCommand(this.SaveAs);
            this.OpenInBrowserCommand = new ParameterlessCommand(this.OpenInBrowser);
            this.AppendElementCommand = new ParameterlessCommand(this.AppendElement);
            this.BuildDocumentCommand = new ParameterlessCommand(this.BuildDocument);
            this.RemoveElementCommand = new ParameterlessCommand(this.RemoveElement);

            this.SelectElementCommand = new Command<ElementModel>(this.SelectElement);
            this.SwapElementsUpCommand = new Command<ElementModel>(this.SwapElementsUp);
            this.SwapElementsDownCommand = new Command<ElementModel>(this.SwapElementsDown);
            this.InsertElementCommand = new Command<ElementModel>(this.InsertElement);
            this.ExtractElementCommand = new Command<ElementModel>(this.ExtractElement);

            this.FilePath = string.Empty;

            this.New();
        }

        public void New()
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

            this.SelectElementAvailable = false;
        }

        public void Open()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "HTML Files|*.html"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                this.Elements = new ObservableCollection<ElementModel>();
                using (StreamReader sr = new StreamReader(openFileDialog.FileName))
                {
                    // TODO: Parse HTML
                    this.Document = sr.ReadToEnd();
                }
                this.FilePath = openFileDialog.FileName;
                this.OpenInBrowserAvailable = true;
            }
        }

        public void Save()
        {
            if (this.FilePath != string.Empty)
            {
                File.WriteAllText(this.FilePath, this.Document);
            }
            else
            {
                this.SaveAs();
            }
        }

        public void SaveAs()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "HTML Files|*.html"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                this.FilePath = saveFileDialog.FileName;
                File.WriteAllText(saveFileDialog.FileName, this.Document);
                this.OpenInBrowserAvailable = true;
            }
        }

        public void SelectElement(ElementModel element)
        {
            this.SelectedElement = element;
            this.SelectedElementFieldsString = string.Join(", ", this.SelectedElement.Fields);

            if (this.SelectedElement != this.Elements[0])
            {
                this.RemoveElementAvailable = true;
                this.SelectElementAvailable = true;
            }
            else
            {
                this.RemoveElementAvailable = false;
                this.SelectElementAvailable = false;
            }
        }

        public void BuildDocument()
        {
            if (this.SelectedElementFieldsString != string.Empty)
            {
                this.SelectedElement.Fields = new List<string>(this.SelectedElementFieldsString.Split(','));
            }
            else
                this.SelectedElement.Fields = new List<string>();

            string html = string.Empty;
            foreach (ElementModel element in this.Elements)
            {
                html = $"{html}{element}";
            }
            this.Document = $"<!DOCTYPE html>{html}";
        }

        public void OpenInBrowser()
        {
            this.Save();
            if (this.FilePath != string.Empty)
                System.Diagnostics.Process.Start(this.FilePath);
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

        public void RemoveElement()
        {
            ElementModel tempElement = this.SelectedElement.ParentElement;
            this.SelectedElement.ParentElement.Elements.Remove(this.SelectedElement);
            this.SelectElement(tempElement);

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

        public void InsertElement(ElementModel element)
        {
            int index1 = this.SelectedElement.Elements.IndexOf(element);
            int index2 = index1 + 1;
            if (index2 < this.SelectedElement.Elements.Count)
            {
                this.SelectedElement.Elements[index2].Append(element);
                this.SelectedElement.Elements.Remove(element);
                this.SelectElement(this.SelectedElement.Elements[index1]);
            }
            this.BuildDocument();
        }

        public void ExtractElement(ElementModel element)
        {
            if (this.SelectedElement != this.Elements[0])
            {
                this.SelectedElement.ParentElement.Append(element);
                this.SelectedElement.Elements.Remove(element);
                this.SelectElement(this.SelectedElement.ParentElement);
            }
            this.BuildDocument();
        }
    }
}
