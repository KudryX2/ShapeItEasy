using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine.UI;

using HybridWebSocket;


[Serializable]
public class Scene{
    public string name;
}


public class Scenes : MonoBehaviour
{
    Client client;

    List<Scene> scenesList = new List<Scene>();

    private GameObject scenesPanel, createScenePanel; 

    
    private Canvas scenesListCanvas, newSceneCanvas;
    private bool showScenesListCanvas, showNewSceneCanvas;

    InputField newSceneNameInput;
    private bool clearNewSceneNameInputField;

    GameObject scenesListContainer;
    private bool updateScenesContainer;

    GameObject scenesListScroll;


    void Start()
    {
        client = GetComponent<Client>();

        scenesListCanvas = GameObject.Find("ScenesListCanvas").GetComponent<Canvas>();
        newSceneCanvas = GameObject.Find("NewSceneCanvas").GetComponent<Canvas>();

        scenesListCanvas.enabled = false;
        newSceneCanvas.enabled = false;
        

        newSceneNameInput = GameObject.Find("SceneNameField").GetComponent<InputField>();

        scenesPanel = GameObject.Find("ScenesPanel");
        createScenePanel = GameObject.Find("CreateScenePanel");

        scenesListContainer = GameObject.Find("Content");

        scenesListScroll = GameObject.Find("ScenesListScroll");

    }

    void Update()
    {

        scenesListCanvas.enabled = showScenesListCanvas;
        newSceneCanvas.enabled = showNewSceneCanvas;

        if(clearNewSceneNameInputField){
            newSceneNameInput.text = "";
            clearNewSceneNameInputField = false;
        }

        if(updateScenesContainer)
            reloadScenesContainer();

    }

    /*
        Create scene request and response handler
    */
    public void requestCreateScene(){

        string sceneName = newSceneNameInput.text;

        if(sceneName != "")
            client.sendData("requestCreateScene", sceneName);
        else
            Debug.Log("El nombre de la escena no puede estar vacío");

    }

    public void handleCreateSceneResponse(string response){

        if(response == "OK"){
            showScenesListCanvas = true;
            showNewSceneCanvas = false;
        
            clearNewSceneNameInputField = true;
            requestScenesList();
        }
    }


    /*
        Scenes list request and response handler
    */
    public void requestScenesList(){
        client.sendData("requestScenesList", " ");                   
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

        foreach(Transform transform in scenesListContainer.transform){
            GameObject.Destroy(transform.gameObject);
        }

        scenesListContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(0, scenesList.Count * 40);

        int y = -25 ;
        int x = (int)scenesListScroll.GetComponent<RectTransform>().rect.width / 2; 

        foreach(Scene scene in scenesList){
            Debug.Log(scene.name);

            GameObject listItem = GameObject.Instantiate(GameObject.Find("ListItem"));
            listItem.GetComponent<Image>().enabled = true;

            listItem.transform.GetChild(0).GetComponent<Text>().text = scene.name;

            RectTransform transform = listItem.GetComponent<RectTransform>();

            transform.anchorMin = new Vector2(0.5f, 0.5f);
            transform.anchorMax = new Vector2(0.5f, 0.5f);
            transform.pivot = new Vector2(0.5f, 0.5f);


            listItem.transform.SetParent(scenesListContainer.transform);

            listItem.GetComponent<RectTransform>().localPosition = new Vector3(x, y, 0);                     
            y -= 40;
        }

        updateScenesContainer = false;

    }


    /*
        UI functions
    */
    public void setScenesListCanvasVisibility(bool visibility){
        showScenesListCanvas = visibility;
    }

    public void makeCreateScenePanelVisible(){
        showScenesListCanvas = false;
        showNewSceneCanvas = true;
    }

    public void cancellButton(){
        showScenesListCanvas = true;
        showNewSceneCanvas = false;

        clearNewSceneNameInputField = true;
    }
}
