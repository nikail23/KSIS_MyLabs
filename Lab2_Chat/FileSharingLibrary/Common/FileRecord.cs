using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace FileSharingLibrary
{
    [Serializable]
    public class FileRecord
    {
        public string FileName { get; set; }
        public byte[] FileBytes { get; set; }

        public FileRecord() { }

        public FileRecord(string fileName, byte[] fileBytes)
        {
            FileName = fileName;
            FileBytes = fileBytes;
        }
    }
}