using UnityEngine;
using System;
using UnityEngine.UI;

using UnityEngine.SceneManagement;


public class SceneCanvas : ScriptableObject
{

    static Button disconnectSceneButton;

    static bool loadScene;

    public static void Start(){
        disconnectSceneButton = GameObject.Find("DisconnectSceneButton").GetComponent<Button>();
        disconnectSceneButton.onClick.AddListener(() => Scenes.requestDisconnect());
    }

    public static void Update(){

        if(loadScene){
            SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
            loadScene = false;
        }

    }


    public static void setLoadSceneTrue(){
        loadScene = true;
    }
    


}
