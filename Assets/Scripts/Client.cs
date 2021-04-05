
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine.UI;

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

public class Client : MonoBehaviour
{
    private const string IP = "172.20.66.178";       // Server IP
    private const int PORT = 2323;                  // Server Port

    private WebSocket webSocket;

    Session sessionManager;
    Scenes scenesManager;


    void Start(){
        sessionManager = GetComponent<Session>();
        scenesManager = GetComponent<Scenes>();

        createConnection();
    }


    private void createConnection(){
        
        try{

            webSocket = WebSocketFactory.CreateInstance("wss://" + IP + ":" + PORT);

            webSocket.OnOpen += () => {
                Debug.Log("Conexión con el servidor establecida");
            };

            webSocket.OnMessage += (byte[] data) => {
                processMessage(Encoding.UTF8.GetString(data));
            };

            webSocket.OnError += (string errorMessage) =>{
                Debug.Log("WebSocket error : " + errorMessage);
            };

            webSocket.OnClose += (WebSocketCloseCode code) =>{
                Debug.Log("WebSocket cerrado : " + code.ToString());
            };
        
            webSocket.Connect();

        }catch(Exception exception){
            Debug.Log("Error " + exception);
        }
    }


    private void processMessage(string messageString){

        ReceivedMessage receivedMessage = null;
        bool parsedOK = false;

        try {
            receivedMessage = JsonUtility.FromJson<ReceivedMessage>(messageString);
            parsedOK = true;
        } catch (Exception exception){
            Debug.Log("El mensaje recibido no esta en el formato JSON , mensaje : " + messageString);
        }

        if(parsedOK)
            if(receivedMessage.kind == "tokenCallback")
                sessionManager.handleUserTokenResponse(receivedMessage.content);
                
            else if(receivedMessage.kind == "signInCallback")
                sessionManager.handleSignInResponse(receivedMessage.content);

            else if(receivedMessage.kind == "createSceneCallback" || receivedMessage.kind == "editSceneCallback" || receivedMessage.kind == "deleteSceneCallback")
                scenesManager.handleScenesModificationResponse(receivedMessage.content);
          
            else if(receivedMessage.kind == "scenesListCallback")
                scenesManager.handleScenesListResponse(receivedMessage.content);

            else
                Debug.Log("Tipo de mensaje desconocido " + messageString);

    }

    public void sendData(String kind, String content){

        Request request = new Request(kind, sessionManager.getUserToken(), content);  

        try{
            webSocket.Send(Encoding.UTF8.GetBytes(JsonUtility.ToJson(request)));
        }catch(Exception exception){
            Debug.Log("No se ha podido enviar el mensaje " + exception.ToString());
        }
    }

    

    
}
