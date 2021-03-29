using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine.UI;

using HybridWebSocket;


[Serializable]
public class UserCredentials{
    public string userName;
    public string userPassword;

    public UserCredentials(string userName, string userPassword){
        this.userName = userName;
        this.userPassword = userPassword;
    }
}


public class Session : MonoBehaviour
{

    private string userName, userPassword, userToken = "";   // User Credentials
    
    private Canvas loginCanvas;
    private bool showLoginCanvas = true;

    Scenes scenesManager;
    Client client;

    void Start()
    {
        loginCanvas = GameObject.Find("LoginCanvas").GetComponent<Canvas>();

        scenesManager = GetComponent<Scenes>();
        client = GetComponent<Client>();
    }

    void Update()
    {
        loginCanvas.enabled = showLoginCanvas;
    }

    
    public void requestUserToken(){
        userName = GameObject.Find("UserName").GetComponent<InputField>().text;
        userPassword = GameObject.Find("UserPassword").GetComponent<InputField>().text;

        UserCredentials requestUserToken = new UserCredentials(userName, userPassword);
        
        client.sendData("requestUserToken", JsonUtility.ToJson(requestUserToken));        
    }


    public void handleUserTokenResponse(string receivedToken){

        if(receivedToken != ""){                                    // Successful connection
            Debug.Log("Usuario autentificado correctamente");

            userToken = receivedToken;                                  // Save the token

            showLoginCanvas = false;

            scenesManager.setScenesListCanvasVisibility(true);        // Show scenes panel
            scenesManager.requestScenesList();                  // Request scenes list
        }else   
            Debug.Log("No se ha podido autentificar al usuario");

    }

    public string getUserToken(){
        return userToken;
    }

    
}
