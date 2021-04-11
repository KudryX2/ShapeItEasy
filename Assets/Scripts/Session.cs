using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine.UI;

using HybridWebSocket;


[Serializable]
public class UserCredentials{
    public string email;
    public string password;

    public UserCredentials(string email, string password){
        this.email = email;
        this.password = password;
    }
}

[Serializable]
public class SignInRequest{
    public string name, email, password;

    public SignInRequest(string name, string email, string password){
        this.name = name;
        this.email = email;
        this.password = password;
    }
}

[Serializable]
public class SignInResponse{
    public string result, message;
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
        userEmail = GameObject.Find("UserEmail").GetComponent<InputField>().text;
        userPassword = GameObject.Find("UserPassword").GetComponent<InputField>().text;

        UserCredentials requestUserToken = new UserCredentials(userEmail, userPassword);
        client.sendData("logInRequest", JsonUtility.ToJson(requestUserToken));        
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

        userName = GameObject.Find("newUserName").GetComponent<InputField>().text;
        userEmail = GameObject.Find("newUserEmail").GetComponent<InputField>().text;
        userPassword = GameObject.Find("newUserPassword").GetComponent<InputField>().text;
        string userRepeatedPassword = GameObject.Find("RepeatUserPassword").GetComponent<InputField>().text;

        if(String.Compare(userPassword, userRepeatedPassword) == 0){                        // Check if the passwords match
            SignInRequest signInRequest = new SignInRequest(userName, userEmail, userPassword);
            client.sendData("signInRequest", JsonUtility.ToJson(signInRequest));
        }
        else{
            GameObject.Find("newUserPassword").GetComponent<InputField>().text = "";           // Clear password fields if not match
            GameObject.Find("RepeatUserPassword").GetComponent<InputField>().text = "";
            Debug.Log("Contraseñas no coinciden");
        }
        
    }

    public void handleSignInResponse(string response){
        
        SignInResponse signInResponse = JsonUtility.FromJson<SignInResponse>(response);

        if(signInResponse.result == "success"){
            userToken = signInResponse.message;
        
            showSignInCanvas = false;                           // Hide SignInCanvas

            scenesManager.setScenesListCanvasVisibility(true);  // Show scenes panel
            scenesManager.requestScenesList();                  // Request scenes list
        
        }else{

            Debug.Log(signInResponse.message);
            // TODO dependiendo de la respuesta limpiar los campos afectados

        }


    }


    /*
        Log out Request and response handler
    */
    public void logOut(){
        client.sendData("logOutRequest", " ");
    }

    public void handleLogOutResponse(string response){

        if(response == "OK"){
            userToken = "";                                     // Clear the actual token
            showLoginCanvas = true;                             // Show Log In canvas
            scenesManager.setScenesListCanvasVisibility(false); // Hide scenes list canvas
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
