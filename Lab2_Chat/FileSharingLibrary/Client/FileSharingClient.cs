using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileSharingLibrary
{
    public delegate void UpdateFilesListDelegate(Dictionary<int, string> filesNameList);

    public class FileSharingClient
    {
        private Dictionary<int, string> files;
        private List<int> filesIdToSendList;

        public event UpdateFilesListDelegate UpdateFilesListEvent;

        public FileSharingClient()
        {
            files = new Dictionary<int, string>();
            filesIdToSendList = new List<int>();
        }

        public void ActivateShowFilesEvent()
        {
            UpdateFilesListEvent(files);
        }

        public async Task<FileInfo> GetFileInfo(int fileId, string url)
        {
            using (var client = new HttpClient())
            {
                var fileInfoRequest = new HttpRequestMessage(HttpMethod.Head, url + fileId);

                var fileInfoResponse = await client.SendAsync(fileInfoRequest);

                if (fileInfoResponse.IsSuccessStatusCode)
                {
                    return GetFileInfoStringByResponse(fileInfoResponse);
                }
                else
                {
                    ShowError(fileInfoResponse);
                    return null;
                }
            }
        }

        public async Task SendFile(string filePath, string url)
        {
            using (var client = new HttpClient())
            {
                var fileLoadRequest = GetPostRequestMessage(filePath, url);

                var fileLoadResponse = await client.SendAsync(fileLoadRequest);
                
                if (fileLoadResponse.IsSuccessStatusCode)
                {
                    var fileId = GetFileIdByResponse(fileLoadResponse);
                    if (fileId != -1)
                    {
                        var fileInfo = await GetFileInfo(fileId, url);
                        files.Add(fileId, fileInfo.FileName + " " + fileInfo.FileSize);     
                        // TODO: также добавлять во временный список Id для отправки 
                    }
                    else
                    {
                        MessageBox.Show("Не найден заголовок FileId в ответе!", "Error");
                    }
                }
                else
                {
                    ShowError(fileLoadResponse);
                }
            }
            ActivateShowFilesEvent();
        }

        public async Task<DownloadFile> DownloadFile(int fileId, string url)
        {
            using (var client = new HttpClient())
            {
                var downloadRequest = new HttpRequestMessage(HttpMethod.Get, url + fileId);
                
                var downloadResponse = await client.SendAsync(downloadRequest);

                if (downloadResponse.IsSuccessStatusCode)
                {
                    var downloadFile = await GetDownloadFileByResponse(downloadResponse);
                    return downloadFile;
                }
                else
                {
                    ShowError(downloadResponse);
                    return null;
                }
            }
        }

        private void ShowError(HttpResponseMessage response)
        {
            MessageBox.Show("Код: " + response.StatusCode + " - " + response.ReasonPhrase + ".", "Error");
        }

        private async Task<DownloadFile> GetDownloadFileByResponse(HttpResponseMessage response)
        {
            var downloadFileStream = await response.Content.ReadAsStreamAsync();
            var downloadFileBytes = new byte[downloadFileStream.Length];
            downloadFileStream.Read(downloadFileBytes, 0, downloadFileBytes.Length);

            IEnumerable<string> fileNameValues;

            if (response.Headers.TryGetValues("FileName", out fileNameValues))
            {
                return new DownloadFile(fileNameValues.First(), downloadFileBytes);
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

        public int GetFileIdByInfo(string fileInfo)
        {
            foreach (KeyValuePair<int, string> file in files)
            {
                if (fileInfo == file.Value)
                {
                    return file.Key;
                }
            }
            return -1;
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

        private FileInfo GetFileInfoStringByResponse(HttpResponseMessage response)
        {
            HttpHeaders responseHeaders = response.Headers;
            IEnumerable<string> fileNameValues;
            IEnumerable<string> fileSizeValues;
            if (responseHeaders.TryGetValues("FileName", out fileNameValues) && responseHeaders.TryGetValues("FileSize", out fileSizeValues))
            {
                return new FileInfo(fileNameValues.First(), int.Parse(fileSizeValues.First()));
            }
            return null;
        }

        private MultipartFormDataContent GetFileLoadRequestMultipartData(string filePath)
        {
            var formData = new MultipartFormDataContent();
            var fileBytesContent = GetFyleBytesContentByFilePath(filePath);
            formData.Add(fileBytesContent);
            return formData;
        }

        private HttpRequestMessage GetPostRequestMessage(string filePath, string url)
        {
            var fileLoadRequest = new HttpRequestMessage(HttpMethod.Post, url);
            var fileName = Path.GetFileName(filePath);
            fileLoadRequest.Headers.Add("FileName", fileName);
            fileLoadRequest.Content = GetFileLoadRequestMultipartData(filePath);
            return fileLoadRequest;
        }

    }
}
