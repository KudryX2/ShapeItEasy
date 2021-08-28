using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using HybridWebSocket;


[Serializable]
public class Request{              
    public string kind;
    public string token;
    public string content;

    public Request(string kind, string token, string content){
        this.kind = kind;
        this.token = token;
        this.content = content;
    }

}

[Serializable]
public class ReceivedMessage{       
    public string kind;
    public string content;
}

public class Client : ScriptableObject
{
    private static string IP = "192.168.1.107";       // Server IP
    private static int PORT = 2323;                  // Server Port

    private static WebSocket webSocket;

    private static bool handleConnectionInterrumpedEvent;


    public static void Start(){

        if(webSocket == null){
            createConnection();
        }
    }

    public static void Update(){

        if(handleConnectionInterrumpedEvent){

            handleConnectionInterrumpedEvent = false;

            Cursor.lockState = CursorLockMode.None;                     // Show the cursor
            Cursor.visible = true;                                     

            if(SceneManager.GetActiveScene().name != "StartScene")      // Load Start Scene
                SceneManager.LoadScene("StartScene", LoadSceneMode.Single);

            Session.deleteSessionData();                                // Delete last session data

            LogInCanvas.enable();                                       // Show Log In canvas
            LogInCanvas.setNotificationText("Se ha interrumpido la conexión con el servidor, intentando reconectarse ...");
            createConnection();                                         // Create new connection
        }

    }


    private static void createConnection(){
        
        try{

            webSocket = WebSocketFactory.CreateInstance("ws://" + IP + ":" + PORT);

            webSocket.OnOpen += () => {
                Debug.Log("Conexión con el servidor establecida");
                LogInCanvas.setNotificationText(" ");
            };

            webSocket.OnMessage += (byte[] data) => {
                processMessage(Encoding.UTF8.GetString(data));
            };

            webSocket.OnError += (string errorMessage) =>{
                Debug.Log("WebSocket error : " + errorMessage);
            };

            webSocket.OnClose += (WebSocketCloseCode code) =>{
                Debug.Log("WebSocket cerrado : " + code.ToString());
                handleConnectionInterrumpedEvent = true;
            };
        
            webSocket.Connect();

        }catch(Exception exception){
            Debug.Log("Error " + exception);
        }
    }


    private static void processMessage(string messageString){

        ReceivedMessage receivedMessage = null;
        bool parsedOK = false;

        try {
            receivedMessage = JsonUtility.FromJson<ReceivedMessage>(messageString);
            parsedOK = true;
        } catch (Exception exception){
            Debug.Log("El mensaje recibido no esta en el formato JSON , mensaje : " + messageString + " error " + exception);
        }

        if(parsedOK)
            /*
                Session Callbacks
            */
            if(receivedMessage.kind == "logInCallback")                     
                Session.handleLogInResponse(receivedMessage.content);
                
            else if(receivedMessage.kind == "signUpCallback")
                Session.handleSignUpResponse(receivedMessage.content);

            else if(receivedMessage.kind == "logOutCallback")
                Session.handleLogOutResponse(receivedMessage.content);

            /*
                Scenes managment Callbacks
            */
            else if(receivedMessage.kind == "connectCallback")              
                Scenes.handleConnectResponse(receivedMessage.content);
            
            else if(receivedMessage.kind == "disconnectCallback")
                Scenes.handleDisconnectSceneResponse(receivedMessage.content);

            else if(receivedMessage.kind == "createSceneCallback" || receivedMessage.kind == "editSceneCallback" || receivedMessage.kind == "deleteSceneCallback")
                Scenes.handleScenesModificationResponse(receivedMessage.content);
          
            else if(receivedMessage.kind == "scenesListCallback")
                Scenes.handleScenesListResponse(receivedMessage.content);

            else if(receivedMessage.kind == "addSceneCallback")
                Scenes.handleAddSceneResponse(receivedMessage.content);
                
            /*
                Edit Scene Callbacks
            */
            else if(receivedMessage.kind == "addShapeCallback")             
                SceneEditor.handleAddShapeResponse(receivedMessage.content);

            else if(receivedMessage.kind == "updateShapeCallback")
                SceneEditor.handlUpdateShapeResponse(receivedMessage.content);

            /* 
                Broadcasts
            */
            else if(receivedMessage.kind == "sceneUpdate")
                SceneEditor.handleSceneUpdateMessage(receivedMessage.content);

            else
                Debug.Log("Tipo de mensaje desconocido " + messageString);

    }

    public static void sendData(String kind, String content){

        Request request = new Request(kind, Session.getUserToken(), content);  

        try{
            webSocket.Send(Encoding.UTF8.GetBytes(JsonUtility.ToJson(request)));
        }catch(Exception exception){
            Debug.Log("No se ha podido enviar el mensaje " + exception.ToString());
        }

    }    

    
}
