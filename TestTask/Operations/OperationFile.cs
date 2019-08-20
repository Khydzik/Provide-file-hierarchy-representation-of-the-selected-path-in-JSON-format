using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TestTask.Model;

namespace TestTask.Operations
{
    public class OperationFile
    {
        private static OperationFile _instance;

        public static OperationFile GetInstance()
        {
            if (_instance == null)
                _instance = new OperationFile();
            return _instance;
        }

        public async Task FullFilesList(DirectoryInfo parent, Folder folder)
        {
            folder.Children = new List<Folder>();
            folder.Files = new List<FilesInDir>();

            foreach (FileInfo file in parent.GetFiles("*"))
            {
                folder.Files.Add(new FilesInDir { Name = file.Name, Path = file.FullName, Size = file.Length });
            }

            foreach (DirectoryInfo directory in parent.GetDirectories())
            {
                DateTime timeOfDirectory = Directory.GetCreationTime(directory.FullName);
                string date = timeOfDirectory.ToString("dd-MMM-yy HH:mm tt");
                Folder subFolder = new Folder { Name = directory.Name, DataCreated = date };
                folder.Children.Add(subFolder);

               await FullFilesList(directory, subFolder);
            }
        }
    }
}
