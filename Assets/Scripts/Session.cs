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


public class Session : ScriptableObject
{

    private static string userName, userEmail, userPassword, userToken = "";    // User Credentials
    
    private static Canvas loginCanvas;                                          // Canvas 
    private static Canvas signInCanvas;
    private static bool showLoginCanvas, showSignInCanvas;
 
    static Scenes scenesManager;
    static Client client;

    static Button logInButton, logOutButton, signInButton, sendButton, cancelButton;    // Buttons


    public static void Start()
    {
        /*
            Canvas 
        */
        loginCanvas =  GameObject.Find("LoginCanvas").GetComponent<Canvas>();
        signInCanvas = GameObject.Find("SignInCanvas").GetComponent<Canvas>();

        showSignInCanvas = false;
        showLoginCanvas = true;

        scenesManager = GameObject.Find("Client").GetComponent<Scenes>();
        client = GameObject.Find("Client").GetComponent<Client>();
    
        /*
            Buttons click handlers
        */
        logInButton = GameObject.Find("LogInButton").GetComponent<Button>();
        logInButton.onClick.AddListener(() => Session.logIn());

        logOutButton = GameObject.Find("LogOutButton").GetComponent<Button>();
        logOutButton.onClick.AddListener(() => Session.logOut());

        signInButton = GameObject.Find("SignInButton").GetComponent<Button>();
        signInButton.onClick.AddListener(() => Session.makeSignInCanvasVisible());

        sendButton = GameObject.Find("SendButton").GetComponent<Button>();
        sendButton.onClick.AddListener(() => Session.signIn());

        cancelButton = GameObject.Find("CancelButton").GetComponent<Button>();
        cancelButton.onClick.AddListener(() => Session.cancelButtonAction());
    }


    public static void Update()
    {
        loginCanvas.enabled = showLoginCanvas;
        signInCanvas.enabled = showSignInCanvas;
    }


    /*
        Log In Request and response handler
    */
    public static void logIn(){
        userEmail = GameObject.Find("UserEmail").GetComponent<InputField>().text;
        userPassword = GameObject.Find("UserPassword").GetComponent<InputField>().text;

        UserCredentials requestUserToken = new UserCredentials(userEmail, userPassword);
        client.sendData("logInRequest", JsonUtility.ToJson(requestUserToken));        
    }


    public static void handleLogInResponse(string receivedToken){

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
    public static void signIn(){

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

    public static void handleSignInResponse(string response){
        
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
    public static void logOut(){
        client.sendData("logOutRequest", " ");
    }

    public static void handleLogOutResponse(string response){

        if(response == "OK"){
            userToken = "";                                     // Clear the actual token
            showLoginCanvas = true;                             // Show Log In canvas
            scenesManager.setScenesListCanvasVisibility(false); // Hide scenes list canvas
        }
    }


    /*
        UI Methods
    */
    public static void makeSignInCanvasVisible(){
        showSignInCanvas = true; 
        showLoginCanvas = false;
    }

    public static void cancelButtonAction(){
        showSignInCanvas = false; 
        showLoginCanvas = true;
    }


    /*
        Getters
    */
    public static string getUserToken(){
        return userToken;
    }

}
