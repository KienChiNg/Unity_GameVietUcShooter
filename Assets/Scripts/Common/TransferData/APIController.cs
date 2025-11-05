using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

    public class APIController : MonoBehaviour
    {
        public static APIController instance;


        string URL_Main = "https://ezwork.vn/simbabyapis";
        

        // Use this for initialization
        void Awake()
        {
            if (instance == null)
            {
                DontDestroyOnLoad(gameObject);
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }


        }
        void Start() { instance = this; }


        //currentObject: this.gameobject goi den
        //msg: function nhận response
        //url: url api
        //body: body api
        //isShowLoading: co hien thi thong bao dang loading khong
        
        //timeout: thoi gian timeout

        public IEnumerator PostData(GameObject currentObject, string msg, string url, string body, bool isShowLoading,  float timeout, int actornum = -1)
        {

            if (isShowLoading == true)
            {
                LoadingEff.instance.ShowLoading("Xin chờ trong giây lát...");          
            }
            
            StartCoroutine(PostDataNoTimeoutMsg(currentObject, msg, url, body, timeout, actornum));

            yield return null;
        }

        IEnumerator PostDataNoTimeoutMsg(GameObject currentObject, string msg, string url, string body, float timeout, int actornum = -1)
        {
            string domain = URL_Main;

            if (!Common.instance.isOnline)
            {
              domain = "http://" + Common.instance.ipAddress + ":8080" + "/simbabyapis";
            }
            //
            JSONObject response = new JSONObject();
            response.AddField("actornum", actornum);
            

            //
            print(body);
            float ServerConnectionTimeout = timeout;
            float tempTime = 0;
            WWW download = new WWW(domain + url, System.Text.Encoding.UTF8.GetBytes(body));
            bool isCancelDownload = false;
            // currentWWW = download;
            while (!download.isDone)
            {
                if (tempTime > ServerConnectionTimeout)
                {
                    isCancelDownload = true;
                    break;
                }
                tempTime += Time.deltaTime;
                yield return 0;
            }
            LoadingEff.instance.HideLoading();
            if (isCancelDownload)
            {
                download.Dispose();
                if (currentObject != null)
                {
                    if (actornum != -1)
                    {
                        response.AddField("text", "");
                        response.AddField("body", body);
                        currentObject.SendMessage(msg, response);
                    } else
                    {
                        currentObject.SendMessage(msg, "");
                    }
                }
                    
                yield break;
            }
            if (isCancelDownload == false)
            {
                yield return download;


                if (msg.Length > 0)
                {
                    if (!string.IsNullOrEmpty(download.error))
                    {
                        print(download.error);
                        if (currentObject != null)
                        {
                            if (actornum != -1)
                            {
                                response.AddField("text", "");
                                response.AddField("body", body);
                                currentObject.SendMessage(msg, response);
                            }
                            else
                            {
                                currentObject.SendMessage(msg, "");
                            }
                        }
                         
                    }
                    else
                    {
                    print(download.text);
                    if (currentObject != null)
                    {
                        if (actornum != -1)
                        {
                            response.AddField("text", download.text);
                            response.AddField("body", body);
                            currentObject.SendMessage(msg, response);
                        }
                        else
                        {
                            currentObject.SendMessage(msg, download.text);
                        }
                    }
                          
                    }
                }
            }

        }

    //get method
    public IEnumerator GetData(GameObject currentObject, string msg, string url, bool isShowLoading, float timeout)
    {

        if (isShowLoading == true)
        {
            LoadingEff.instance.ShowLoading("Xin chờ trong giây lát...");
        }

        StartCoroutine(GetDataNoTimeoutMsg(currentObject, msg, url, timeout));

        yield return null;
    }

    public IEnumerator GetDataNoTimeoutMsg(GameObject currentObject, string msg, string url, float timeout)
    {
        string domain = URL_Main;

        if (!Common.instance.isOnline)
        {
            domain = "http://" + Common.instance.ipAddress + ":8080" + "/simbabyapis";
        }

        //
        //print(body);
        float ServerConnectionTimeout = timeout;
        float tempTime = 0;
        UnityWebRequest download = UnityWebRequest.Get(domain + url);
        bool isCancelDownload = false;
        // currentWWW = download;
        yield return download.SendWebRequest();

        while (!download.isDone)
        {
            if (tempTime > ServerConnectionTimeout)
            {
                isCancelDownload = true;
                break;
            }
            tempTime += Time.deltaTime;
            yield return 0;
        }

        LoadingEff.instance.HideLoading();
        if (isCancelDownload)
        {
            download.Dispose();
            if (currentObject != null)
                currentObject.SendMessage(msg, "");
            yield break;
        }

        if (isCancelDownload == false)
        {
            yield return download;


            if (msg.Length > 0)
            {
                if (!string.IsNullOrEmpty(download.error))
                {
                    print(download.error);
                    if (currentObject != null)
                        currentObject.SendMessage(msg, "");
                }
                else
                {
                    print(download.downloadHandler.text);
                    if (currentObject != null)
                        currentObject.SendMessage(msg, download.downloadHandler.text);
                }
            }
        }

    }


    /* IEnumerator PostDataLoadingTimeoutMsg(GameObject currentObject, string msg, string url, string body, float timeout1,bool isShowMsg)
     {
        print(url );
         float ServerConnectionTimeout2 = timeout1;
         float tempTime2 = 0;
         WWW download = new WWW(URL_Main + url, System.Text.Encoding.UTF8.GetBytes(body));
         bool isCancelDownload = false;
         // currentWWW = download;
         while (!download.isDone)
         {
             if (tempTime2 > ServerConnectionTimeout2)
             {
                 isCancelDownload = true;
                 break;
             }
             tempTime2 += Time.deltaTime;
             yield return 0;
         }
         if (isCancelDownload )
         {
             download.Dispose();
             LoadingEff.instance.HideLoading();
             if (isShowMsg == true)
             {
                 MessageBoxController.instance.OnShowReConnect(currentObject, msg, url, body, timeout1);
             }
             else
             {
                 if (currentObject != null)
                      currentObject.SendMessage(msg, "");
     
             }
             yield break;
         }
         if (isCancelDownload == false)
         {
             yield return download;
             MessageBoxController.instance.ClosePanel();
             LoadingEff.instance.HideLoading();
             if (msg.Length > 0)
             {
                 if (!string.IsNullOrEmpty(download.error))
                 {
                     print(download.error + URL_Main + url);
                     if (currentObject != null)
                         currentObject.SendMessage(msg, "");
                 }
                 else
                 {
                     if (currentObject != null)
                         currentObject.SendMessage(msg, download.text);
                 }
             }
         }

     }

    */




}
