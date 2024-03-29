﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine.UI;

using HybridWebSocket;


[Serializable]
public class UserCredentials{
    public string name;
    public string email;
    public string password;

    public UserCredentials(string name, string email, string password){
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

    private static string connectedSceneID, userToken = "";                             // User Credentials

    public static void Start()
    {

        if(userToken == ""){            // If user not logged in -> show log in canvas
            LogInCanvas.enable();
        }else{                          // If user logged in -> show scenes list
            ScenesListCanvas.enable();
            Scenes.requestScenesList();
        }  
    
    }


    /*
        Log In Request and response handler
    */
    public static void logIn(){
        
        string userEmail = GameObject.Find("UserEmail").GetComponent<InputField>().text;
        if(userEmail == ""){
            LogInCanvas.setNotificationText("User email is required");
            return;
        }

        string userPassword = GameObject.Find("UserPassword").GetComponent<InputField>().text;
        if(userPassword == ""){
            LogInCanvas.setNotificationText("User password is required");
            return;
        }

        LogInCanvas.setNotificationText("Loading ..."); 

        UserCredentials requestUserToken = new UserCredentials("",userEmail, userPassword);
        Client.sendData("logInRequest", JsonUtility.ToJson(requestUserToken));        
    }


    public static void handleLogInResponse(string receivedToken){

        if(receivedToken != ""){                                    // Successful connection
            userToken = receivedToken;                                  // Save the token
            ScenesListCanvas.enable();                                  // Show scenes list panel
            Scenes.requestScenesList();                                 // Request scenes list
        }else
            LogInCanvas.setNotificationText("No se ha podido autentificar al usuario, datos incorrectos");

    }


    /*
        Sign In Request and response Handler
    */
    public static void signIn(){

        string userName = GameObject.Find("newUserName").GetComponent<InputField>().text;
        string userEmail = GameObject.Find("newUserEmail").GetComponent<InputField>().text;
        string userPassword = GameObject.Find("newUserPassword").GetComponent<InputField>().text;
        string userRepeatedPassword = GameObject.Find("RepeatUserPassword").GetComponent<InputField>().text;

        if(String.Compare(userPassword, userRepeatedPassword) == 0){                        // Check if the passwords match
            UserCredentials UserCredentials = new UserCredentials(userName, userEmail, userPassword);
            Client.sendData("signUpRequest", JsonUtility.ToJson(UserCredentials));
        }
        else{
            GameObject.Find("newUserPassword").GetComponent<InputField>().text = "";           // Clear password fields if not match
            GameObject.Find("RepeatUserPassword").GetComponent<InputField>().text = "";
            SignUpCanvas.setNotificationText("Las contraseñas no coinciden");
        }
        
    }

    public static void handleSignUpResponse(string response){
        
        SignInResponse signInResponse = JsonUtility.FromJson<SignInResponse>(response);

        if(signInResponse.result == "success"){
            userToken = signInResponse.message;
        
            ScenesListCanvas.enable();                      // Show scenes list panel
            Scenes.requestScenesList();                     // Request scenes list
        
        }else{
            SignUpCanvas.setNotificationText(signInResponse.message);
            // TODO dependiendo de la respuesta limpiar los campos afectados
        }


    }


    /*
        Log out Request and response handler
    */
    public static void logOut(){
        Client.sendData("logOutRequest", " ");
    }

    public static void handleLogOutResponse(string response){

        if(response == "OK"){
            userToken = "";                                     // Clear the actual token
            LogInCanvas.enable();                               // Show Log In canvas
        }
    }


    public static void deleteSessionData(){
        userToken = "";
        connectedSceneID = "";
    }

    /*
        Getters
    */
    public static string getUserToken(){
        return userToken;
    }

    public static void setConnectedSceneID(string newConnectedSceneID){
        connectedSceneID = newConnectedSceneID;
    }

    public static string getConnectedSceneID(){
        return connectedSceneID;
    }

}
