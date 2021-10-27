using System.Net.Sockets;
using RestSharp;

namespace ArtemisFlyout.Util
{
    public class RestUtil
    {
      
        public static bool CheckIfPortOsOpen(string host, int port, int timeOut = 200, int maxRetryCount = 1)
        {
            var porIsOpen = false;

            var retryCount = 0;
            while (retryCount < maxRetryCount)
            {
                retryCount++;
                using (var socket = new Socket(SocketType.Stream, ProtocolType.Tcp))
                {
                    var result = socket.BeginConnect(host, port, null, null);
                    result.AsyncWaitHandle.WaitOne(timeOut);

                    if (socket.Connected)
                    {
                        socket.EndConnect(result);
                        porIsOpen = true;
                        break;
                    }
                    socket.Close();
                }
            }
            return porIsOpen;

        }

        public static bool RestGetBool(string host, int port, string endPoint, string data = "")
        {
            var client = new RestClient(host + ":" + port);
            var request = new RestRequest(endPoint, Method.POST);
            request.AddParameter("text/plain", data, ParameterType.RequestBody);
            try
            {
                client.Execute(request, Method.POST);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string RestPost(string host, int port, string endPoint, string data = "")
        {
            var result = string.Empty;
            var client = new RestClient(host + ":" + port.ToString());
            var request = new RestRequest(endPoint, Method.POST);
            request.AddParameter("text/plain", data, ParameterType.RequestBody);
            try
            {
                var response = client.Execute(request, Method.POST);
                result = response.Content;

            }
            catch
            {
                // ignored
            }

            return result;
        }
    }
}
