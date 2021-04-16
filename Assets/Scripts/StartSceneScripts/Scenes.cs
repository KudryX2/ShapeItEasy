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
}

[Serializable]
public class EditSceneRequest{
    public string id;
    public string newName;

    public EditSceneRequest(string id, string newName){
        this.id = id;
        this.newName = newName;
    }
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

        string sceneName = CreateSceneCanvas.getNameInput();

        if(sceneName != "")
            Client.sendData("requestCreateScene", sceneName);
        else
            Debug.Log("El nombre de la escena no puede estar vacío");

    }

    public static void requestEditScene(){

        string sceneName = EditSceneCanvas.getNameInput();

        if(sceneName != null)
            Client.sendData("requestEditScene",  JsonUtility.ToJson( new EditSceneRequest(selectedSceneID, sceneName)));
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
        if(response == "OK")
            loadEditingScene = true;
        else
            Debug.Log("Se ha producido un error : " + response);
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
        Aux Methods
    */
    private static string getSceneID(string sceneName){
        
        foreach(Scene scene in scenesList)
            if(String.Compare(scene.name, sceneName ) == 0)
                return scene.id;

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
