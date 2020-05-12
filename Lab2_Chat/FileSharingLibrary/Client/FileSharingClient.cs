using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FileSharingLibrary
{
    public delegate void UpdateFilesListDelegate(Dictionary<int, string> filesNameList);

    public class FileSharingClient
    {
        public Dictionary<int, string> files;
        private List<int> filesIdToSendList;

        public event UpdateFilesListDelegate UpdateFilesListEvent;

        public FileSharingClient()
        {
            files = new Dictionary<int, string>();
            filesIdToSendList = new List<int>();
        }

        public async Task<string> GetFileInfo(int fileId, string url)
        {
            using (var client = new HttpClient())
            {
                var fileInfoRequest = new HttpRequestMessage(HttpMethod.Head, url + fileId);

                var fileInfoResponse = await client.SendAsync(fileInfoRequest);

                if (fileInfoResponse.IsSuccessStatusCode)
                {
                    return GetFileInfoStringByResponse(fileInfoResponse);
                }
            }
            return null;
        }

        public void ActivateShowFilesEvent()
        {
            UpdateFilesListEvent(files);
        }

        private MultipartFormDataContent GetFileLoadRequestMultipartData(string filePath)
        {
            var formData = new MultipartFormDataContent();
            var fileBytesContent = GetFyleBytesContentByFilePath(filePath);
            formData.Add(fileBytesContent);
            return formData;
        }

        public async Task SendFile(string filePath, string url)
        {
            using (var client = new HttpClient())
            {
                var fileName = Path.GetFileName(filePath); 
                // TODO: по REST нужно имя файла убрать в Multipart и обращаться только по url
                var fileLoadRequest = new HttpRequestMessage(HttpMethod.Post, url + fileName);
                fileLoadRequest.Content = GetFileLoadRequestMultipartData(filePath);

                var fileLoadResponse = await client.SendAsync(fileLoadRequest);

                if (fileLoadResponse.IsSuccessStatusCode)
                {
                    var fileId = GetFileIdByResponse(fileLoadResponse);
                    if (fileId != -1)
                    {
                        var fileInfo = await GetFileInfo(fileId, url);
                        files.Add(fileId, fileInfo);     
                        // TODO: также добавлять во временный список Id для отправки 
                    }
                }
            }
            ActivateShowFilesEvent();
        }

        public async Task<Stream> DownloadFile(int fileId, string url)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url + fileId);

                if (response.IsSuccessStatusCode)
                {
                    var downloadFileStream = await response.Content.ReadAsStreamAsync();
                    return downloadFileStream;
                }
            }
            return null;
        }

        private ByteArrayContent GetFyleBytesContentByFilePath(string filePath)
        {
            using (var fileStream = File.OpenRead(filePath))
            {
                byte[] fileBytes = new byte[fileStream.Length];
                fileStream.Read(fileBytes, 0, fileBytes.Length);
                return new ByteArrayContent(fileBytes);
            }
        }

        private int GetFileIdByResponse(HttpResponseMessage response)
        {
            HttpHeaders responseHeaders = response.Headers;
            IEnumerable<string> fileIdValues;
            if (responseHeaders.TryGetValues("FileId", out fileIdValues))
            {
                return int.Parse(fileIdValues.First());
            }
            return -1;
        }

        private string GetFileInfoStringByResponse(HttpResponseMessage response)
        {
            HttpHeaders responseHeaders = response.Headers;
            IEnumerable<string> fileNameValues;
            IEnumerable<string> fileSizeValues;
            if (responseHeaders.TryGetValues("FileName", out fileNameValues) && responseHeaders.TryGetValues("FileSize", out fileSizeValues))
            {
                return "Name: " + fileNameValues.First() + " Size: " + fileSizeValues.First();
            }
            return null;
        }
    }
}
