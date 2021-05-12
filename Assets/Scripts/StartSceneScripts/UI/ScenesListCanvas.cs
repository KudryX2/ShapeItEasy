using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEngine.UI;


public class ScenesListCanvas : ScriptableObject
{
    private static Canvas canvas;
    private static bool enabled;

    static Button logOutButton, newSceneButton, addSceneButton;

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

        addSceneButton = GameObject.Find("AddSceneButton").GetComponent<Button>();
        addSceneButton.onClick.AddListener(() => AddSceneCanvas.enable() );

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

        int listItemHeight = 80; 

        scenesListContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(0, scenesList.Count * listItemHeight + 5);    // Resize slider container

        int x = (int)scenesListScroll.GetComponent<RectTransform>().rect.width / 2; 
        int y = - listItemHeight/2 - 5  ;                                                                                       // Starting position for elements at y axis

        foreach(Scene scene in scenesList){

            GameObject listItem = GameObject.Instantiate(scenesListItem);                                           // Instantiate a list item
            listItem.transform.SetParent(scenesListContainer.transform, false);                                     // Set listContainer as parent

            listItem.transform.GetChild(0).GetComponent<Text>().text = scene.name;                                  // Set scene name
            listItem.transform.GetChild(1).GetComponent<Text>().text = scene.description;                           // Set scene description
            listItem.transform.GetChild(2).GetComponent<Text>().text = scene.permissions;                           // Set scene permission

            listItem.GetComponent<RectTransform>().localPosition = new Vector3(x, y, 0);                            // Change the position
            y -= listItemHeight;

            if(String.Compare(scene.permissions, "owner") != 0 ){                                                   // If the user is not the owner
                listItem.transform.GetChild(3).gameObject.SetActive(false);                                             // Disable share button 
                listItem.transform.GetChild(4).gameObject.SetActive(false);                                             // Disable edit button
            }else{
                listItem.transform.GetChild(5).gameObject.SetActive(false);                                             // Disable delete button
            }
        }

        updateScenesContainer = false;

    }


    public static void enable(){        // Make visible
        
        enabled = true;
        SignUpCanvas.disable();
        LogInCanvas.disable();
        CreateSceneCanvas.disable();
        EditSceneCanvas.disable();
        ShareSceneCanvas.disable();
        AddSceneCanvas.disable();
    }

    public static void disable(){       // Hide
        enabled = false;
    }

    public static void updateScenesList(){
        updateScenesContainer = true;
    }
}
