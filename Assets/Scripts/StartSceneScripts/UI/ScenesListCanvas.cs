using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class ScenesListCanvas : ScriptableObject
{
    private static Canvas canvas;
    private static bool enabled;

    static Button logOutButton, newSceneButton;

    // Scenes List
    static GameObject scenesListItem, scenesListScroll, scenesListContainer;                 
    static bool updateScenesContainer;


    public static void Start(GameObject newScenesListItem)
    {
        canvas = GameObject.Find("ScenesListCanvas").GetComponent<Canvas>();

        /*
            Buttons
        */
        logOutButton = GameObject.Find("LogOutButton").GetComponent<Button>();
        logOutButton.onClick.AddListener(() => Session.logOut());

        newSceneButton = GameObject.Find("NewSceneButton").GetComponent<Button>();
        newSceneButton.onClick.AddListener(() => CreateSceneCanvas.enable() );

        /*
            Scenes List
        */
        scenesListItem = newScenesListItem;
        scenesListContainer = GameObject.Find("Content");
        scenesListScroll = GameObject.Find("ScenesListScroll");

    }

    public static void Update()
    {
        canvas.enabled = enabled;

        if(updateScenesContainer)
            reloadScenesContainer();
    }


    private static void reloadScenesContainer(){

        foreach(Transform transform in scenesListContainer.transform)                                               // Clear the list container
            GameObject.Destroy(transform.gameObject);

        List<Scene> scenesList = Scenes.getScenesList();

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

    public static void enable(){        // Make visible
        
        enabled = true;
        SignUpCanvas.disable();
        LogInCanvas.disable();
        CreateSceneCanvas.disable();
        EditSceneCanvas.disable();

        Scenes.requestScenesList();
    }

    public static void disable(){       // Hide
        enabled = false;
    }

    public static void updateScenesList(){
        updateScenesContainer = true;
    }
}
