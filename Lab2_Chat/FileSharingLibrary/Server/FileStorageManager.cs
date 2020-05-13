﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileSharingLibrary
{
    public class FileStorageManager
    {
        private static readonly string FileStoragePath = Directory.GetCurrentDirectory() + "\\FileStorage\\";
        private const int StartFileId = 0;

        private Dictionary<int, string> filesList;

        public FileStorageManager()
        {
            filesList = new Dictionary<int, string>();
            CheckStorageFolderCondition();
        }

        public bool DeleteFile(int fileId)
        {
            if (IsExistedFileCheck(fileId))
            {
                var filePath = FileStoragePath + GetFileNameById(fileId);
                File.Delete(filePath);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GetFileInfo(int fileId, ref string fileName, ref int fileSize)
        {
            if (IsExistedFileCheck(fileId))
            {
                fileName = GetFileNameById(fileId);
                var filePath = FileStoragePath + fileName;
                using (var fileStream = new FileStream(filePath, FileMode.Open))
                {
                    fileSize = (int)fileStream.Length;
                }
                return true;
            }
            return false;
        }

        public bool IsExistedFileCheck(string fileName)
        {
            foreach (KeyValuePair<int, string> dictionaryFileName in filesList)
            {
                if (fileName == dictionaryFileName.Value)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsExistedFileCheck(int fileId)
        {
            foreach (KeyValuePair<int, string> dictionaryFileName in filesList)
            {
                if (fileId == dictionaryFileName.Key)
                {
                    return true;
                }
            }
            return false;
        }

        private bool WriteFileToFileStorage(byte[] fileBytes, string filePath)
        {
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    fileStream.Write(fileBytes, 0, fileBytes.Length);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private int GetNewFileId()
        {
            if (filesList.Count > 0)
            {
                return filesList.Keys.Last() + 1;
            }
            else
            {
                return StartFileId;
            }
        }

        public bool AddNewFile(string fileName, byte[] fileBytes, ref int id)
        {
            if (!(IsExistedFileCheck(fileName)))
            {
                string filePath = FileStoragePath + fileName;
                if (WriteFileToFileStorage(fileBytes, filePath))
                {
                    id = GetNewFileId();
                    filesList.Add(id, fileName);
                    return true;
                }            
            }
            return false;
        }

        public string GetFileNameById(int fileId)
        {
            foreach (KeyValuePair<int, string> file in filesList)
            {
                if (fileId == file.Key)
                {
                    return file.Value;
                }
            }
            return null;
        }

        public byte[] GetDownloadFileBytes(int fileId) 
        {
            string filePath = FileStoragePath + GetFileNameById(fileId);
            if (IsExistedFileCheck(fileId))
            {
                using (FileStream fstream = File.OpenRead(filePath))
                {
                    byte[] fileBytes = new byte[fstream.Length];
                    fstream.Read(fileBytes, 0, fileBytes.Length);
                    return fileBytes;
                }
            }
            return null;
        }

        private void CheckStorageFolderCondition()
        {
            if (!(Directory.Exists(FileStoragePath)))
            {
                Directory.CreateDirectory(FileStoragePath);
            }
            else
            {
                var fileStorageDirectory = new DirectoryInfo(FileStoragePath);
                foreach (var file in fileStorageDirectory.GetFiles())
                {
                    file.Delete();
                }
            }
        }
    }
}
