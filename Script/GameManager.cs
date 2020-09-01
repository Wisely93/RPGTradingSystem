using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public InputField _username, _password, _RegUserName, _RegPassword;
    public GameObject UserProfile, LoginPage, Notice, Warning;
    void Start()
    {
        gm = this;
        //StartCoroutine(GetRequest("http://localhost/UnityBackend/GetDate.php"));
        //StartCoroutine(Register("Sofea", "sofea123"));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
            }
        }
    }

    IEnumerator Login(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/UnityBackend/Login.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.downloadHandler.text.Contains("Wrong username or password. Please try again.") || www.downloadHandler.text.Contains("User does not exist"))
                {
                    Warning.SetActive(true);
                    print("Try Again");
                }else
                {
                    UserProfile.SetActive(true);
                    LoginPage.SetActive(false);
                    UserInfo.instance.SetCredential(username, password);
                    UserInfo.instance.SetID(www.downloadHandler.text);
                    print(www.downloadHandler.text);
                }
            }
        }
    }

    public IEnumerator GetUserInformation(string ID, Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", ID);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/UnityBackend/GetUsers.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                callback(www.downloadHandler.text);
            }
        }
    }

    IEnumerator Register(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/UnityBackend/Registeration.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);    //return message from php
            }
        }
    }

    public IEnumerator GetItemsIDs(string userID, Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", userID);
       
        using (UnityWebRequest webRequest = UnityWebRequest.Post("http://localhost/UnityBackend/GetItemsIDs.php", form))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log(": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);
                if (webRequest.downloadHandler.text != "0 results")
                {
                    string jsonArray = webRequest.downloadHandler.text;
                    callback(jsonArray);
                }else
                {
                    Notice.SetActive(true);
                }
            }
        }
    }

    public IEnumerator GetItemsIcons(string itemID, Action<Sprite> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("ItemID", itemID);

        using (UnityWebRequest webRequest = UnityWebRequest.Post("http://localhost/UnityBackend/GetItemsIcon.php", form))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            { 
                Debug.Log(": Error: " + webRequest.error);
            }
            else
            {
                //results are byte array
                byte[] bytes = webRequest.downloadHandler.data;

                //Create 2Dtexture
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(bytes);

                //Create sprite (in UI)
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f,0.5f));      
                callback(sprite);
            }
        }
    }

    public IEnumerator GetItems(string itemID, Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("ItemID", itemID);

        using (UnityWebRequest webRequest = UnityWebRequest.Post("http://localhost/UnityBackend/GetItem.php", form))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log(": Error: " + webRequest.error);
            }
            else
            {
                //Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);
                string jsonArray = webRequest.downloadHandler.text;
                callback(jsonArray);
            }
        }
    }

    public IEnumerator GetItemsCatalog(Action<string> callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("http://localhost/UnityBackend/GetItemCatalogue.php"))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log(": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);
                string jsonArray = webRequest.downloadHandler.text;
                callback(jsonArray);
            }
        }
    }

    public IEnumerator SellItems(string ID, string ItemID, string userID)
    {
        WWWForm form = new WWWForm();
        form.AddField("ID", ID);
        form.AddField("ItemID", ItemID);
        form.AddField("userID", userID);

        using (UnityWebRequest webRequest = UnityWebRequest.Post("http://localhost/UnityBackend/SellItem.php", form))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log(": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);
                ProfileManager.instance.CreateItems();
            }
        }
    }

    public IEnumerator BuyItems(string ItemID, string userID)
    {
        WWWForm form = new WWWForm();
        form.AddField("ItemID", ItemID);
        form.AddField("userID", userID);

        using (UnityWebRequest webRequest = UnityWebRequest.Post("http://localhost/UnityBackend/BuyItem.php", form))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log(": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);
            }
        }
    }

    public void Login()
    {
        if (_username.text == "" || _password.text == "")
        {
            Warning.GetComponent<Text>().text = "Please key in username and password";

        }
        else
        {
            StartCoroutine(Login(_username.text,_password.text));
        }
    }

    public void Register()
    {
        if (_RegUserName.text == "" || _RegPassword.text == "")
        {
            Warning.SetActive(true);
            //Warning.GetComponent<Text>().text = "Please key in username and password";
        }
        else
        {
            StartCoroutine(Register(_RegUserName.text, _RegPassword.text));
        }
    }
}
