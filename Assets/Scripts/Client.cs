
using UnityEngine;
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
    const string IP = "172.20.67.4";        // Server IP
    const int PORT = 2323;                  // Server Port

    WebSocket webSocket;

    string clientName = "Kudry";


    void Start(){
        createConnection();
    }


    public void createConnection(){
        
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


    public void processMessage(string messageString){
        
        Debug.Log(messageString);

        try {
            ReceivedMessage receivedMessage = JsonUtility.FromJson<ReceivedMessage>(messageString);

        } catch (Exception exception){
            Debug.Log("El mensaje recibido no esta en el formato JSON , mensaje : " + messageString);
        }
    }

    public void sendData(String kind, String content){
        Request request = new Request(kind, clientName, content);  
        
        try{
            webSocket.Send(Encoding.UTF8.GetBytes(JsonUtility.ToJson(request)));
        }catch(Exception exception){
            Debug.Log("No se ha podido enviar el mensaje " + exception.ToString());
        }
    }

}
