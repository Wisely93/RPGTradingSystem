using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SimpleJSON;

public class ItemCatalogManager : MonoBehaviour
{
 
    Action<string> _CreateItemsCallBack;
    // Start is called before the first frame update
    void OnEnable()
    {
        if (gameObject.transform.childCount > 0)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            StartCoroutine(CreateItems());
        }
        else
        {
            StartCoroutine(CreateItems());
        }
    }
    public IEnumerator CreateItems()
    {
        _CreateItemsCallBack = (jsonArrayString) =>
        {
            StartCoroutine(GetItemsRoutines(jsonArrayString));

        };
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(GameManager.gm.GetItemsCatalog(_CreateItemsCallBack));
    }

    IEnumerator GetItemsRoutines(string jsonArrayString)
    {
        //Parsing Json array string as an array
        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;

        for (int i = 0; i < jsonArray.Count; i++)
        {
            //Create local variables
            string itemID = jsonArray[i].AsObject["ID"];
            string itemName = jsonArray[i].AsObject["name"];
            string itemPrice = jsonArray[i].AsObject["price"];
            string itemDecription = jsonArray[i].AsObject["description"];

            //Convert array into object
            JSONObject itemInfoJson = new JSONObject();
            itemInfoJson = jsonArray[i].AsObject;

            yield return new WaitForEndOfFrame();
            //yield return new WaitUntil(() => isDone == true);

            //Instantiate GameObject(item prefab)
            GameObject ItemObject = Instantiate(Resources.Load("Prefab/ItemCatalog") as GameObject);
            Item item = ItemObject.AddComponent<Item>();

            ItemObject.transform.SetParent(this.transform);
            ItemObject.transform.localPosition = Vector3.zero;
            ItemObject.transform.localScale = Vector3.one;

            //Fill information
            ItemObject.transform.Find("Name").GetComponent<Text>().text = itemInfoJson["name"].Value;
            ItemObject.transform.Find("Price").GetComponent<Text>().text = itemInfoJson["price"].Value;
            ItemObject.transform.Find("Description").GetComponent<Text>().text = itemInfoJson["description"].Value;

            //Create a callback to get sprite from GameManager.cs;
            Action<Sprite> getItemIconCallback = (SpriteInfo) =>
            {
                ItemObject.transform.Find("Photo").GetComponent<Image>().sprite = SpriteInfo;
            };
            StartCoroutine(GameManager.gm.GetItemsIcons(itemID, getItemIconCallback));

            //Buy Sell button
            ItemObject.transform.Find("BuyButton").GetComponent<Button>().onClick.AddListener(() => { StartCoroutine(GameManager.gm.BuyItems(itemID, UserInfo.instance.userID)); });
        }

    }
}