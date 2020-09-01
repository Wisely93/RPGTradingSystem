using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class UserInfo : MonoBehaviour
{

    public string userID { get; private set; }
    private string userName;
    private string userPassword;
    private string userLevel;
    private string userCoins;

    public static UserInfo instance;

    void Awake()
    {
        instance = this;
    }
public void SetCredential(string username, string userpass)
    {
        userName = username;
        userPassword = userpass;
    }
public void SetID(string id)
    {
        userID = id;
    }
}
