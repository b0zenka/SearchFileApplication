using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchFileApplication
{
    public class FileHelper : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private static int idCount = 0;

        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        private bool isEnabled;

        //zmiana widzialności w listView
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsEnabled"));
                }
            }
        }

        public static FileHelper BuilderFileHelper(FileInfo fileInfo)
        {
            FileHelper fH = new FileHelper();
            fH.FileName = fileInfo.Name;
            fH.FilePath = fileInfo.FullName;
            fH.Id = idCount++;
            fH.isEnabled = true;

            return fH;
        }

        public static void ResetIdCount()
        {
            idCount = 0;
        }

    }
}
