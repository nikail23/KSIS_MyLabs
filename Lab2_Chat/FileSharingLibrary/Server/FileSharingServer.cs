using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace FileSharingLibrary
{
    public class FileSharingServer
    {
        private FileStorageManager fileStorageManager;
        private HttpListener httpListener;
        private Thread threadListen;
        private bool isListen;

        public FileSharingServer(string serverUrl)
        {
            fileStorageManager = new FileStorageManager();
            httpListener = new HttpListener();
            isListen = false;
            threadListen = new Thread(Listen);
            threadListen.IsBackground = true;
            httpListener.Prefixes.Add(serverUrl);
        }

        public void Start()
        {
            threadListen.Start();
        }

        private void Listen()
        {
            try
            {
                httpListener.Start();
                isListen = true;
                while (isListen)
                {
                    var context = httpListener.GetContext();
                    var task = Task.Run(() => HandleContext(ref context));
                }
            }
            catch
            {
                isListen = false;
            }
        }

        private void HandleContext(ref HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            if (request.HttpMethod == "GET")
            {
                HandleDownloadFileRequest(request, ref response);
            }

            if (request.HttpMethod == "POST")
            {
                HandleFileLoadToServerRequest(request, ref response);
            }

            if (request.HttpMethod == "HEAD")
            {
                HandleFileInfoRequest(request, ref response);
            }

            if (request.HttpMethod == "DELETE")
            {
                HandleDeleteFileRequest(request, ref response);
            }
        }

        private void HandleDeleteFileRequest(HttpListenerRequest request, ref HttpListenerResponse response)
        {
            var fileId = int.Parse(request.Url.LocalPath.Substring(1));

            if (fileStorageManager.DeleteFile(fileId))
            {
                response.StatusCode = 200;
                response.StatusDescription = "OK";
            }
            else
            {
                response.StatusCode = 404;
                response.StatusDescription = "File with such id not found!";
            }

            response.OutputStream.Close();
        }

        private void HandleFileInfoRequest(HttpListenerRequest request, ref HttpListenerResponse response)
        {
            var fileId = int.Parse(request.Url.LocalPath.Substring(1));
            var fileName = "";
            var fileSize = 0;

            if (fileStorageManager.GetFileInfo(fileId, ref fileName, ref fileSize))
            {
                response.Headers.Add("FileName", fileName);
                response.Headers.Add("FileSize", fileSize.ToString());
                response.StatusCode = 200;
                response.StatusDescription = "OK";
            }
            else
            {
                response.StatusCode = 404;
                response.StatusDescription = "File with such id not found!";
            }

            response.OutputStream.Close();
        }

        private void HandleDownloadFileRequest(HttpListenerRequest request, ref HttpListenerResponse response)
        {
            var fileId = int.Parse(request.Url.LocalPath.Substring(1));
            var fileName = fileStorageManager.GetFileNameById(fileId);

            if (fileStorageManager.IsExistedFileCheck(fileId) && fileName != null)
            {
                response.AddHeader("FileName", fileName);

                var downloadFileBytes = fileStorageManager.GetDownloadFileBytes(fileId);
                using (var outputStream = response.OutputStream)
                {
                    outputStream.Write(downloadFileBytes, 0, downloadFileBytes.Length);
                }

                response.StatusCode = 200;
                response.StatusDescription = "OK";
            }
            else
            {
                response.StatusCode = 404;
                response.StatusDescription = "File with such id not found!";
            }

            response.OutputStream.Close();
        }

        private byte[] GetFileBytesByRequest(HttpListenerRequest request)
        {
            var streamReader = new StreamReader(request.InputStream, request.ContentEncoding);
            var resultString = streamReader.ReadToEnd();
            return request.ContentEncoding.GetBytes(resultString);
        }

        private void HandleFileLoadToServerRequest(HttpListenerRequest request, ref HttpListenerResponse response)
        {
            var fileName = request.Headers.Get("FileName");
            var fileBytes = GetFileBytesByRequest(request);
            var fileId = 0;

            if (fileStorageManager.AddNewFile(fileName, fileBytes, ref fileId))
            {
                response.StatusCode = 200;
                response.StatusDescription = "OK";
                response.Headers.Add("FileId", fileId.ToString());
            }
            else
            {
                response.StatusCode = 409;
                response.StatusDescription = "File already exists on the server!";
            }

            response.OutputStream.Close();
        }

        public void Close()
        {
            isListen = false;
            if (httpListener != null)
            {
                httpListener.Stop();
                httpListener = null;
            }
            threadListen.Abort();
        }
    }
}
