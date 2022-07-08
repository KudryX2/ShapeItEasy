using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using System;
using System.Text;
using System.Threading;

using HybridWebSocket;


public class PingTest : ScriptableObject{

    private static string IP = "172.20.67.163";       // Server IP
    private static int PORT = 2323;                  // Server Port

    private static WebSocket webSocket;

    private static double requestTime;   // Momento en el que se lanza la petición


    public static void Start(){

        Debug.Log("Start Ping Test");

        webSocket = new WebSocket("ws://" + IP + ":" + PORT);

        try{
            webSocket.OnOpen += () => {
                requestPing();
            };

            webSocket.OnMessage += (byte[] data) => {
                Debug.Log(DateTime.Now.TimeOfDay.TotalMilliseconds - requestTime);
                Thread.Sleep(1000);
                requestPing();
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

    private static void requestPing(){

        requestTime = DateTime.Now.TimeOfDay.TotalMilliseconds;

        try{
            webSocket.Send(Encoding.UTF8.GetBytes(JsonUtility.ToJson(new Request("pingRequest", "", ""))));
        }catch(Exception e){
            Debug.Log("Error " + e);
        }
    }

    
}
