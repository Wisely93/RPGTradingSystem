using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SimpleJSON;

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager instance;
    Action<string> _CreateItemsCallBack;
    // Start is called before the first frame update
    void OnEnable()
    {
        instance = this;
        CreateItems();
    }

    public void CreateItems()
    {
        StartCoroutine(LoadProfile());
    }

   private IEnumerator LoadProfile()
    {
        _CreateItemsCallBack = (jsonArrayString) =>
        {
            print(jsonArrayString);
            StartCoroutine(CreateUserProfile(jsonArrayString));
        };
        yield return new WaitForSeconds(.1f);
        string userID = UserInfo.instance.userID;
        StartCoroutine(GameManager.gm.GetUserInformation(userID, _CreateItemsCallBack));

    }

    IEnumerator CreateUserProfile(string jsonArrayString)
    {
        //Parsing Json array string as an array
        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;

        for (int i = 0; i < jsonArray.Count; i++)
        {
            //Create local variables
            //bool isDone = false;
            JSONObject itemInfoJson = new JSONObject();
            itemInfoJson = jsonArray[0].AsObject;

            //Fill information
            gameObject.transform.Find("Name").GetComponent<Text>().text = itemInfoJson["username"].Value;
            gameObject.transform.Find("Level").GetComponent<Text>().text = itemInfoJson["level"].Value;
            gameObject.transform.Find("Coins").GetComponent<Text>().text = itemInfoJson["coins"].Value;
            // yield return new WaitUntil(() => isDone == true);
            yield return new WaitForEndOfFrame();
        }
    }
}