using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using HybridWebSocket;


[Serializable]
public class Scene{
    public string id;
    public string name;
    public string description;
    public string permissions;
    public string shareViewID;
    public string shareEditID;

    public Scene(string id, string name, string description, string permissions){
        this.id = id;
        this.name = name;
        this.description = description;
        this.permissions = permissions;
    }

    public Scene(string name, string description){
        this.name = name;
        this.description = description;
    }
}

[Serializable]
public class ConnectResponse{
    public string status;
    public string info;
}



public class Scenes
{

    static List<Scene> scenesList = new List<Scene>();         // Scenes list
    static string selectedSceneID;
    static bool loadEditingScene;


    public static void Update()
    {

        if(loadEditingScene)
            try{
                SceneManager.LoadScene("EditScene", LoadSceneMode.Single);
                Session.setConnectedSceneID(selectedSceneID);
                loadEditingScene = false;
            }catch(Exception exception){
                Debug.Log("Error cambiando de escena " + exception);
            }

    }


    /*
        Create, Edit and Delete scene requests and response handler
    */
    public static void requestCreateScene(){

        string name = CreateSceneCanvas.getNameInput();
        string description = CreateSceneCanvas.getDescriptionInput();

        if(name != ""){
            CreateSceneCanvas.setNotificationText("Loading ...");
            Client.sendData("requestCreateScene",  JsonUtility.ToJson( new Scene(name, description)));
        }else
            CreateSceneCanvas.setNotificationText("Scene name field can´t be empty");
    }

    public static void requestEditScene(){

        string name = EditSceneCanvas.getNameInput();
        string description = EditSceneCanvas.getDescriptionInput();

        if(name != null)
            Client.sendData("requestEditScene",  JsonUtility.ToJson( new Scene(selectedSceneID, name, description, "")));
        else   
            Debug.Log("El nombre de la escena no puede estar vacío");

    }

    public static void requestDeleteScene(){
        Client.sendData("requestDeleteScene", selectedSceneID);
    }

    public static void handleScenesModificationResponse(string response){

        if(response == "OK"){
            ScenesListCanvas.enable();
            Scenes.requestScenesList();
        }else
            Debug.Log("Se ha producido un error " + response);
    }


    /*
        Scenes list request and response handler
    */
    public static  void requestScenesList(){
        Client.sendData("requestScenesList", " ");                   
    }

    public static void handleScenesListResponse(string jsonArrayString){

        try{

            jsonArrayString = jsonArrayString.Replace("[", "");
            jsonArrayString = jsonArrayString.Replace("]", "");

            bool elementDetected = false;
            string element = "";

            scenesList.Clear();

            for( int i = 0 ; i < jsonArrayString.Length ; ++i){

                if(jsonArrayString[i] == '{')      
                    elementDetected = true;

                if(elementDetected)
                    element += jsonArrayString[i];

                if(jsonArrayString[i] == '}' && elementDetected){  

                    scenesList.Add(JsonUtility.FromJson<Scene>(element));

                    elementDetected = false;
                    element = "";
                }

            }

            ScenesListCanvas.updateScenesList();

        }catch(Exception exception){
            Debug.Log("Error parseando una lista json : " + exception);
        }

    }


    /*
        Connect to scene request and response handler
    */
    public static void requestConnect(string sceneName){
        selectedSceneID = getSceneID(sceneName);

        if(selectedSceneID != null)
            Client.sendData("requestConnect", selectedSceneID);
        else
            Debug.Log("Ha ocurrido un error obteniendo el identificador de la escena");
    }

    public static void handleConnectResponse(string response){

        try{
            ConnectResponse connectResponse = JsonUtility.FromJson<ConnectResponse>(response);

            if(connectResponse.status == "OK"){
                loadEditingScene = true;            
                SceneEditor.loadScene(connectResponse.info);
            }else
                Debug.Log("Se ha producido un error : " + response);

        }catch(Exception exception){
            Debug.Log(exception);
        }

    }

    /*
        Disconnect scene request and response handler
    */
    public static void requestDisconnect(){
        Client.sendData("requestDisconnect", selectedSceneID);
    }

    public static void handleDisconnectSceneResponse(string response){

        try{
            if(response == "OK"){
                selectedSceneID = "";
                SceneCanvas.setLoadSceneTrue();
            }else
                Debug.Log("Se ha producido un error : " + response);
            

        }catch(Exception exception ){
            Debug.Log("Error : " + exception);
        }

    }

    /*
        Add scene request and response handler
    */
    public static void requestAddScene(string inputId){

        if(String.Compare(inputId, "") == 0)          // Before sending the request check if input is not empty
            AddSceneCanvas.setNotificationText("The share ID field can´t be empty");
        else{
            AddSceneCanvas.setNotificationText("Loading ...");
            Client.sendData("requestAddScene", inputId);  
        }
    }

    public static void handleAddSceneResponse(string response){

        if(response == "OK"){         
            requestScenesList();
            ScenesListCanvas.enable();
        }else                               
            AddSceneCanvas.setNotificationText(response);
        
    }

    /*
        Aux Methods
    */
    public static string getSceneID(string sceneName){
        
        foreach(Scene scene in scenesList)
            if(String.Compare(scene.name, sceneName ) == 0)
                return scene.id;

        return null;
    }

    public static Scene getScene(string sceneName){
        foreach(Scene scene in scenesList)
            if(String.Compare(scene.name, sceneName ) == 0)
                return scene;

        return null;
    }

    public static void setSelectedSceneID(string sceneName){
        selectedSceneID = getSceneID(sceneName);
    }

    public static string getSelectedSceneID(){
        return selectedSceneID;
    }

    public static List<Scene> getScenesList(){
        return scenesList;
    }

}
