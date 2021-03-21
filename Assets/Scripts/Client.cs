
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
public class RequestUserTokenCredentials{
    public string userName;
    public string userPassword;

    public RequestUserTokenCredentials(string userName, string userPassword){
        this.userName = userName;
        this.userPassword = userPassword;
    }
}


[Serializable]
public class ReceivedMessage{       
    public string kind;
    public string content;
}


public class Client : MonoBehaviour
{
    private const string IP = "172.20.67.4";        // Server IP
    private const int PORT = 2323;                  // Server Port

    private WebSocket webSocket;

    private string userName, userPassword, userToken = "";   // User Credentials

    private GameObject loginPanel; 


    void Start(){
        loginPanel = GameObject.Find("LogInPanel");

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
        
        try {
            ReceivedMessage receivedMessage = JsonUtility.FromJson<ReceivedMessage>(messageString);

            if(receivedMessage.kind == "tokenRequestCallback")
                handleRequestUserTokenResponse(receivedMessage.content);
                
            else
                Debug.Log("Tipo de mensaje desconocido " + messageString);

        } catch (Exception exception){
            Debug.Log("El mensaje recibido no esta en el formato JSON , mensaje : " + messageString);
        }
    }

    public void sendData(String kind, String content){
        Request request = new Request(kind, userToken, content);  
        
        try{
            webSocket.Send(Encoding.UTF8.GetBytes(JsonUtility.ToJson(request)));
        }catch(Exception exception){
            Debug.Log("No se ha podido enviar el mensaje " + exception.ToString());
        }
    }


    public void requestUserToken(){
        userName = GameObject.Find("UserName").GetComponent<InputField>().text;
        userPassword = GameObject.Find("UserPassword").GetComponent<InputField>().text;

        RequestUserTokenCredentials requestUserToken = new RequestUserTokenCredentials(userName, userPassword);
        sendData("requestUserToken", JsonUtility.ToJson(requestUserToken));        
    }

    private void handleRequestUserTokenResponse(string receivedToken){

        if(receivedToken != ""){
            userToken = receivedToken;
            Debug.Log("Usuario autentificado correctamente");
        }else   
            Debug.Log("No se ha podido autentificar al usuario");

    }

}
