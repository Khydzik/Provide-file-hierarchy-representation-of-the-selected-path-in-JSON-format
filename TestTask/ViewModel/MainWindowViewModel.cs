using System;
using TestTask.Operations;
using Prism.Commands;
using Prism.Mvvm;
using System.IO;
using System.Windows.Forms;
using TestTask.Model;
using Newtonsoft.Json;

namespace TestTask.ViewModel
{
    public class MainWindowViewModel: BindableBase
    {
        private string _pathToFolder;
        private OperationFile _operationFile;
        private bool _isEnableSelectFolderButton;
        private bool _isEnableRecordInJsonButton;
        private string _errorLabel;

        public DelegateCommand<Object> RecordInJsonCommandButton { get; set; }
        public DelegateCommand<Object> SelectFolderCommandButton { get; set; }

        public MainWindowViewModel()
        {
            RecordInJsonCommandButton = new DelegateCommand<Object>(RecordButton);
            SelectFolderCommandButton = new DelegateCommand<Object>(SelectFolderButton);
            _operationFile = OperationFile.GetInstance();
            IsEnableSelectFolderButton = true;
            IsEnableRecordInJsonButton = true;
        }

        public string ErrorLabel
        {
            get { return this._errorLabel; }
            set { SetProperty(ref _errorLabel, value); }
        }

        public bool IsEnableSelectFolderButton
        {
            get { return _isEnableSelectFolderButton; }
            set { SetProperty(ref _isEnableSelectFolderButton, value); }
        }

        public bool IsEnableRecordInJsonButton
        {
            get { return _isEnableRecordInJsonButton; }
            set { SetProperty(ref _isEnableRecordInJsonButton, value); }
        }

        public string PathToFolder
        {
            get { return this._pathToFolder; }
            set { SetProperty(ref _pathToFolder, value); }
        }

        private async void SelectFolderButton(Object pbj)
        {
            IsEnableSelectFolderButton = false;

            FolderBrowserDialog FBD = new FolderBrowserDialog();
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                PathToFolder = FBD.SelectedPath;
                IsEnableRecordInJsonButton = true;
            }

            IsEnableSelectFolderButton = true;
        }

        private async void RecordButton(Object obj)
        {
            ErrorLabel = "";
            IsEnableSelectFolderButton = false;
            IsEnableRecordInJsonButton = false;
           
            if (PathToFolder != String.Empty && Directory.Exists(PathToFolder))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(PathToFolder);
                Folder parent = new Folder();
                parent.Name = directoryInfo.Name;
                parent.DataCreated = Directory.GetCreationTime(directoryInfo.FullName).ToString("dd-MMM-yy HH:mm tt");

                await _operationFile.FullFilesList(directoryInfo, parent);                 

                string json = JsonConvert.SerializeObject(parent);
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "JSON Files (*.json)|*.json";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog.FileName, json);
                }
            }
            else
            {
                ErrorLabel = " You have not selected a directory or a directory does not exist !!! Enter path to folder, please...";
                IsEnableSelectFolderButton = true;
                return;
            }

            IsEnableSelectFolderButton = true;
            IsEnableRecordInJsonButton = true;

        }
    }
}

