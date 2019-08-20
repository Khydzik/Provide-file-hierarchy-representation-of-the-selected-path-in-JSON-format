using System.Collections.Generic;

namespace TestTask.Model
{
    public class Folder
    {
        public string Name { get; set; }
        public string DataCreated { get; set; }
        public List<FilesInDir> Files { get; set; }
        public List<Folder> Children { get; set; }
    }
}
