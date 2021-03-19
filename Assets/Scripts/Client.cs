
using UnityEngine;
using System;
using System.Text;
using UnityEngine.UI;

using HybridWebSocket;

public class Client : MonoBehaviour
{
    const string IP = "172.20.67.4";        // Server IP
    const int PORT = 2323;                  // Server Port

    WebSocket webSocket;
             

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
                string messageString = Encoding.UTF8.GetString(data);
                Debug.Log("Recibido : " + messageString);
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

}
