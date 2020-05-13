using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace FileSharingLibrary
{
    public class FileInfo
    {
        public string FileName { get; }
        public int FileSize { get; }

        public FileInfo(string fileName, int fileSize)
        {
            FileName = fileName;
            FileSize = fileSize;
        }
    }
}