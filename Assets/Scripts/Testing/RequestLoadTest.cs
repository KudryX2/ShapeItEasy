using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using HybridWebSocket;

using System.Threading;


public class RequestLoadTest : ScriptableObject {

    private static string IP = "172.20.67.163";       // Server IP
    private static int PORT = 2323;                  // Server Port

    private static string SCENE_ID = "337b9807-d9ad-43b9-8343-f9d19ef69322";
    private static string SHAPE_ID = "26c3cccd-4eb9-42df-adc2-fcf0a2a140f5";
    private static int USERS_AMOUNT = 100;
    private static int REQUESTS_AMOUNT = 100;

    private static int MILLS_MIN_WAIT = 0;
    private static int MILLS_MAX_WAIT = 100;

    private static List<WebSocket> connections = new List<WebSocket>();


    public static void Start(){

        // Creamos instancias de web socket para cada cliente simulado
        for(int i = 0 ; i < USERS_AMOUNT ; ++i)
            connections.Add(new WebSocket("ws://" + IP + ":" + PORT));

        // Con cada cliente simulado iniciamos la sesión y lanzamos una hebra para testear las peticiones
        foreach(WebSocket webSocket in connections){

            try{
                webSocket.OnOpen += () => {
                    try{
                        Request request = new Request("logInRequest", "", JsonUtility.ToJson(new UserCredentials("","user" + connections.IndexOf(webSocket), "pass")));  
                        webSocket.Send(Encoding.UTF8.GetBytes(JsonUtility.ToJson(request)));

                    }catch(Exception e){
                        Debug.Log("Error " + e);
                    }

                };

                webSocket.OnMessage += (byte[] data) => {

                    ReceivedMessage receivedMessage = JsonUtility.FromJson<ReceivedMessage>(Encoding.UTF8.GetString(data));
        
                    if(receivedMessage.kind == "logInCallback"){
                        Thread thread = new Thread(() => testRequests(webSocket, receivedMessage.content));
                        thread.Start();
                    }

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


    public static void testRequests(WebSocket webSocket, string token){

        Debug.Log("Start TestRequests " + token);
        waitRandom();

        sendMessage(webSocket, new Request("requestScenesList", token, ""));
        waitRandom();

        sendMessage(webSocket, new Request("requestConnect", token, SCENE_ID));
        waitRandom();

        for(int i = 0 ; i < REQUESTS_AMOUNT ; ++i){
            sendMessage(webSocket, new Request("updateShape", token,
              JsonUtility.ToJson( new UpdateShapeRequest(SHAPE_ID, new Vector3(0,0,0), new Vector3(1,1,1), new Vector3(0,0,0)))));
            waitRandom();
        }

        sendMessage(webSocket, new Request("requestDisconnect", token, SCENE_ID));
        waitRandom();

        // Mensaje para cerrar sesión
        sendMessage(webSocket, new Request("logOutRequest", token, ""));
    }

    public static void sendMessage(WebSocket webSocket, Request request){
        try{
            webSocket.Send(Encoding.UTF8.GetBytes(JsonUtility.ToJson(request)));
        }catch(Exception exception){
            Debug.Log("No se ha podido enviar el mensaje " + exception.ToString());
        }
    }

    public static void waitRandom(){
        System.Random random = new System.Random();
        Thread.Sleep(random.Next(MILLS_MIN_WAIT, MILLS_MAX_WAIT));
    }

}