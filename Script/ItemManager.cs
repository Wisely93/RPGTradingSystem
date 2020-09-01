using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SimpleJSON;

public class ItemManager : MonoBehaviour
{
   
    Action<string> _CreateItemsCallBack;
    // Start is called before the first frame update

    void OnEnable()
    {
        if(gameObject.transform.childCount > 0)
        {
            for(int i = 0; i < gameObject.transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            StartCoroutine(CreateItems());
        }else
        {
            StartCoroutine(CreateItems());
        }
    }

    public IEnumerator CreateItems()
    {
        _CreateItemsCallBack = (jsonArrayString) =>
        {
            StartCoroutine(CreateItemsRoutines(jsonArrayString));
        };
        yield return new WaitForSeconds(.1f);
        string userID = UserInfo.instance.userID;
        StartCoroutine(GameManager.gm.GetItemsIDs(userID, _CreateItemsCallBack));
        if(gameObject.transform.childCount == 0)
        {
            GameManager.gm.Notice.SetActive(false);
        }
    }

    IEnumerator CreateItemsRoutines(string jsonArrayString)
    {
        //Parsing Json array string as an array
        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;

        for (int i = 0; i < jsonArray.Count; i++)
        {
            //Create local variables
            bool isDone = false;
            string itemID = jsonArray[i].AsObject["ItemID"];
            string ID = jsonArray[i].AsObject["ID"];
            JSONObject itemInfoJson = new JSONObject();

            //Create a callback to get info from GameManager.cs;
            Action<string> getItemInfoCallback = (itemInfo) =>
            {
                isDone = true;
                JSONArray tempArray = JSON.Parse(itemInfo) as JSONArray;
                itemInfoJson = tempArray[0].AsObject;
            };

            StartCoroutine(GameManager.gm.GetItems(itemID, getItemInfoCallback));
            yield return new WaitUntil(() => isDone == true);

            //Instantiate GameObject(item prefab)
            GameObject ItemObject = Instantiate(Resources.Load("Prefab/Item") as GameObject);
            Item item = ItemObject.AddComponent<Item>();
            item.ID = ID;
            item.ItemID = itemID;

            ItemObject.transform.SetParent(this.transform);
            ItemObject.transform.localPosition = Vector3.zero;
            ItemObject.transform.localScale = Vector3.one;

            //Fill information
            ItemObject.transform.Find("Name").GetComponent<Text>().text = itemInfoJson["name"];
            ItemObject.transform.Find("Price").GetComponent<Text>().text = itemInfoJson["price"];
            ItemObject.transform.Find("Description").GetComponent<Text>().text = itemInfoJson["description"];

            //Create a callback to get sprite from GameManager.cs;
            Action<Sprite> getItemIconCallback = (SpriteInfo) =>
            {
                ItemObject.transform.Find("Photo").GetComponent<Image>().sprite = SpriteInfo;
            };
            StartCoroutine(GameManager.gm.GetItemsIcons(itemID, getItemIconCallback));

            //Set Sell button
            ItemObject.transform.Find("SellButton").GetComponent<Button>().onClick.AddListener(() => { StartCoroutine(GameManager.gm.SellItems(ID, itemID, UserInfo.instance.userID)); Destroy(ItemObject.gameObject); });        }

    }
}