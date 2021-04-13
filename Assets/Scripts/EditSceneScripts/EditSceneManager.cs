using UnityEngine;
using System;
using UnityEngine.UI;

using UnityEngine.SceneManagement;


public class EditSceneManager : ScriptableObject
{

    static Button disconnectSceneButton;

    static bool loadScene;

    public static void Start(){
        disconnectSceneButton = GameObject.Find("DisconnectSceneButton").GetComponent<Button>();
        disconnectSceneButton.onClick.AddListener(() => EditSceneManager.requestDisconnect());
    }

    public static void Update(){

        if(loadScene){
            SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
            loadScene = false;
        }

    }

    public static void requestDisconnect(){
        Client.sendData("requestDisconnect", " ");
    }

    public static void handleDisconnectSceneResponse(string response){

        try{
            if(response == "OK"){
                Session.setConnectedSceneID("");
                loadScene = true;
            }

        }catch(Exception exception ){
            Debug.Log("Error : " + exception);
        }

    }


}
