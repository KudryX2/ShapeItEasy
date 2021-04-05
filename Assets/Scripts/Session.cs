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

[Serializable]
public class SignInRequest{
    public string userName, userEmail, userPassword;

    public SignInRequest(string userName, string userEmail, string userPassword){
        this.userName = userName;
        this.userEmail = userEmail;
        this.userPassword = userPassword;
    }
}


public class Session : MonoBehaviour
{

    private string userName, userEmail, userPassword, userToken = "";   // User Credentials
    
    private Canvas loginCanvas, signInCanvas;
    private bool showLoginCanvas, showSignInCanvas;
 
    Scenes scenesManager;
    Client client;


    void Start()
    {
        loginCanvas = GameObject.Find("LoginCanvas").GetComponent<Canvas>();
        signInCanvas = GameObject.Find("SignInCanvas").GetComponent<Canvas>();

        showSignInCanvas = false;
        showLoginCanvas = true;

        scenesManager = GetComponent<Scenes>();
        client = GetComponent<Client>();
    }

    void Update()
    {
        loginCanvas.enabled = showLoginCanvas;
        signInCanvas.enabled = showSignInCanvas;
    }

    /*
        Log In Request and response handler
    */
    public void logIn(){
        userName = GameObject.Find("UserName").GetComponent<InputField>().text;
        userPassword = GameObject.Find("UserPassword").GetComponent<InputField>().text;

        UserCredentials requestUserToken = new UserCredentials(userName, userPassword);
        
        client.sendData("requestUserToken", JsonUtility.ToJson(requestUserToken));        
    }


    public void handleLogInResponse(string receivedToken){

        if(receivedToken != ""){                                    // Successful connection
            Debug.Log("Usuario autentificado correctamente");

            userToken = receivedToken;                                  // Save the token

            showLoginCanvas = false;

            scenesManager.setScenesListCanvasVisibility(true);        // Show scenes panel
            scenesManager.requestScenesList();                  // Request scenes list
        }else   
            Debug.Log("No se ha podido autentificar al usuario");

    }


    /*
        Sign In Request and response Handler
    */
    public void signIn(){

        userName = GameObject.Find("UserName").GetComponent<InputField>().text;
        userEmail = GameObject.Find("Email").GetComponent<InputField>().text;
        userPassword = GameObject.Find("UserPassword").GetComponent<InputField>().text;
        string userRepeatedPassword = GameObject.Find("RepeatUserPassword").GetComponent<InputField>().text;

        // TODO comprobar los campos y que no contengan carácteres no válidos

        if(String.Compare(userPassword, userRepeatedPassword) == 0){                        // Check if the passwords match
            SignInRequest signInRequest = new SignInRequest(userName, userEmail, userPassword);
            client.sendData("signInRequest", JsonUtility.ToJson(signInRequest));
        }
        else{
            GameObject.Find("UserPassword").GetComponent<InputField>().text = "";           // Clear password fields if not match
            GameObject.Find("RepeatUserPassword").GetComponent<InputField>().text = "";
        }
        
    }

    public void handleSignInResponse(string response){
        
        // TODO procesar la respuesta
        if(true){
            // TODO si la respuesta es favorable contendrá un token asi que lo guardamos

            showSignInCanvas = false;                           // Hide SignInCanvas

            scenesManager.setScenesListCanvasVisibility(true);  // Show scenes panel
            scenesManager.requestScenesList();                  // Request scenes list
        }else{
            
            // TODO dependiendo de la respuesta limpiar los campos afectados


        }


    }


    /*
        UI Methods
    */
    public void makeSignInCanvasVisible(){
        showSignInCanvas = true; 
        showLoginCanvas = false;
    }

    public void cancelButton(){
        showSignInCanvas = false; 
        showLoginCanvas = true;
    }


    /*
        Getters
    */
    public string getUserToken(){
        return userToken;
    }

}
