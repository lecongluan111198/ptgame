using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine;
using System.IO;
using System.Collections;

namespace Assets.Scripts.Logic.Network
{
    public class APIRequest
    {
        private static APIRequest _instance;
        public static APIRequest Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new APIRequest();
                }
                return _instance;
            }
        }
        /// <summary>
        /// Send Request to server with jsonbody
        /// </summary>
        /// <param name="url">API link</param>
        /// <param name="bodyJson">json body</param>
        /// <returns></returns>
        public IEnumerator doPost(string url, string bodyJson,Action<APIReponse> callBack)
        {
            using (var request = new UnityWebRequest(url, "POST"))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJson);
                request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                request.SendWebRequest();
                while (!request.isDone)
                {
                    yield return null;
                }
                if (request.responseCode == 200)
                {
                    string reponseJson = request.downloadHandler.text;
                    callBack(APIReponse.textToReponse(reponseJson));
                }
                else
                {
                    Debug.Log(API.ERROR_CONNECT);
                    callBack(null);
                }
            }
        }

        

        /// <summary>
        /// send request to server with api link
        /// </summary>
        /// <param name="url">api url</param>
        /// <returns></returns>
        public IEnumerator doGet(string url,Action<APIReponse> callBack)
        {
            using (var request = UnityWebRequest.Get(url))
            {
                Debug.Log(url);
                request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                request.SendWebRequest();

                while (!request.isDone)
                {
                    yield return null;
                }

                if (request.responseCode == 200)
                {
                    string reponseJson = request.downloadHandler.text;
                    callBack(APIReponse.textToReponse(reponseJson));
                }
                else
                {
                    Debug.Log(API.ERROR_CONNECT);
                }
            }
        }


        /// <summary>
        /// Dowload file from server
        /// </summary>
        /// <param name="url">API link</param>
        /// <param name="path">path to save file</param>
        /// <returns></returns>
        public IEnumerator downloadFile(string url, string path,Action<bool> callBack)
        {
            using (var request = UnityWebRequest.Get(url))
            {
                request.downloadHandler = new DownloadHandlerBuffer();

                yield return request.SendWebRequest();

                if (request.isNetworkError || request.isHttpError)
                {
                    Debug.Log(API.ERROR_CONNECT);
                    callBack(false);
                }
                else
                {
                    //write byte to file
                    byte[] results = request.downloadHandler.data;
                    using (Stream file = File.OpenWrite(path))
                    {
                        file.Write(results, 0, results.Length);
                    }
                    callBack(true);
                }
            }
        }

        public IEnumerator uploadFile(string url, string path, Action<bool> callBack)
        {
            WWWForm form = new WWWForm();

            UnityWebRequest file = new UnityWebRequest();
            file = UnityWebRequest.Get(path);
            yield return file.SendWebRequest();

            form.AddBinaryData("file", file.downloadHandler.data, Path.GetFileName(path));

            UnityWebRequest req = UnityWebRequest.Post(url, form);

            yield return req.SendWebRequest();

            if (req.isHttpError || req.isNetworkError)
            {
                Debug.Log(req.error);
                callBack(false);
            }
            else
            {
                Debug.Log(API.SUCCES);
                callBack(true);
            }
        }
    }
}
