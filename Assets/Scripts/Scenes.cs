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


public class Scenes : MonoBehaviour
{
//    Client client;

    Canvas scenesListCanvas, newSceneCanvas, editSceneCanvas;               // Canvas
    bool showScenesListCanvas, showNewSceneCanvas, showEditSceneCanvas;

    InputField newSceneNameInput, editSceneNameInput;
    bool clearNewSceneNameInputField, clearEditSceneNameInputField;

    List<Scene> scenesList = new List<Scene>();         // Scenes list
    GameObject scenesListScroll;
    public GameObject scenesListItem;
    GameObject scenesListContainer;                 
    bool updateScenesContainer;

    string selectedSceneID;

    bool loadScene;


    void Start()
    {
  //      client = GetComponent<Client>();

        scenesListCanvas = GameObject.Find("ScenesCanvas").GetComponent<Canvas>();
        newSceneCanvas = GameObject.Find("NewSceneCanvas").GetComponent<Canvas>();
        editSceneCanvas = GameObject.Find("EditSceneCanvas").GetComponent<Canvas>();

        scenesListCanvas.enabled = false;
        newSceneCanvas.enabled = false;
        editSceneCanvas.enabled = false;

        newSceneNameInput = GameObject.Find("NewSceneNameField").GetComponent<InputField>();
        editSceneNameInput = GameObject.Find("EditSceneNameField").GetComponent<InputField>();

        scenesListContainer = GameObject.Find("Content");

        scenesListScroll = GameObject.Find("ScenesListScroll");

    }

    void Update()
    {

        scenesListCanvas.enabled = showScenesListCanvas;
        newSceneCanvas.enabled = showNewSceneCanvas;
        editSceneCanvas.enabled = showEditSceneCanvas;

        if(clearNewSceneNameInputField){
            newSceneNameInput.text = "";
            clearNewSceneNameInputField = false;
        }

        if(clearEditSceneNameInputField){
            editSceneNameInput.text = "";
            clearEditSceneNameInputField = false;
        }

        if(updateScenesContainer)
            reloadScenesContainer();

        if(loadScene)
            loadUnityScene();

    }

    /*
        Create, Edit and Delete scene requests and response handler
    */
    public void requestCreateScene(){

        string sceneName = newSceneNameInput.text;

        if(sceneName != "")
            Client.sendData("requestCreateScene", sceneName);
        else
            Debug.Log("El nombre de la escena no puede estar vacío");

    }

    public void requestEditScene(){

        string sceneName = editSceneNameInput.text;

        if(sceneName != null)
            Client.sendData("requestEditScene",  JsonUtility.ToJson( new EditSceneRequest(selectedSceneID, sceneName)));
        else   
            Debug.Log("El nombre de la escena no puede estar vacío");

    }

    public void requestDeleteScene(){
        Client.sendData("requestDeleteScene", selectedSceneID);
    }

    public void handleScenesModificationResponse(string response){

        if(response == "OK"){

            Debug.Log("Modificación en las escenas exitosa");

            showScenesListCanvas = true;
            showNewSceneCanvas = false;
            showEditSceneCanvas = false;
        
            clearNewSceneNameInputField = true;
            clearEditSceneNameInputField = true;
            requestScenesList();
        }
    }

    /*
        Scenes list request and response handler
    */
    public void requestScenesList(){
        Client.sendData("requestScenesList", " ");                   
    }

    public void handleScenesListResponse(string jsonArrayString){

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

            updateScenesContainer = true;

        }catch(Exception exception){
            Debug.Log("Error parseando una lista json : " + exception);
        }

    }

    private void reloadScenesContainer(){

        foreach(Transform transform in scenesListContainer.transform)                                               // Clear the list container
            GameObject.Destroy(transform.gameObject);

        scenesListContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(0, scenesList.Count * 41);        // Resize slider container

        int x = (int)scenesListScroll.GetComponent<RectTransform>().rect.width / 2;                             
        int y = -25 ;                                                                                               // Starting position for elements at y axis

        foreach(Scene scene in scenesList){

            GameObject listItem = GameObject.Instantiate(scenesListItem);                                           // Instantiate a list item
            listItem.transform.SetParent(scenesListContainer.transform);                                            // Set listContainer as parent

            listItem.transform.GetChild(0).GetComponent<Text>().text = scene.name;                                  // Change list item text

            listItem.GetComponent<RectTransform>().localPosition = new Vector3(x, y, 0);                            // Change the position
            y -= 40;
        }

        updateScenesContainer = false;

    }

    /*
        Connect to scene request and response handler
    */
    public void requestConnect(string sceneName){
        selectedSceneID = getSceneID(sceneName);

        if(selectedSceneID != null)
            Client.sendData("requestConnect", selectedSceneID);
        else
            Debug.Log("Ha ocurrido un error obteniendo el identificador de la escena");
    }

    public void handleConnectResponse(string response){

        if(response == "OK")
            loadScene = true;

    }


    private string getSceneID(string sceneName){
        
        foreach(Scene scene in scenesList)
            if(String.Compare(scene.name, sceneName ) == 0)
                return scene.id;

        return null;
    }


    /*
        UI functions
    */
    public void setScenesListCanvasVisibility(bool visibility){
        showScenesListCanvas = visibility;
    }

    public void makeEditSceneCanvasVisible(string sceneName){

        editSceneNameInput.text = sceneName;
        selectedSceneID = getSceneID(sceneName);

        showScenesListCanvas = false;
        showEditSceneCanvas = true;
    }

    public void makeCreateSceneCanvasVisible(){
        showScenesListCanvas = false;
        showNewSceneCanvas = true;
    }

    public void cancellButton(){
        showScenesListCanvas = true;
        showNewSceneCanvas = false;
        showEditSceneCanvas = false;

        clearNewSceneNameInputField = true;
    }


    private void loadUnityScene(){
        try{
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
        }catch(Exception exception){
            Debug.Log("Error cambiando de escena " + exception);
        }
    }

}
