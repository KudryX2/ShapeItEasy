using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class EditSceneCanvas : ScriptableObject
{
    static Canvas canvas;
    static bool enabled;

    static Button saveButton, cancelEditingButton, deleteButton;    

    static InputField nameInputField;
    static bool clearNameInputField;


    public static void Start()
    {
        canvas = GameObject.Find("EditSceneCanvas").GetComponent<Canvas>();

        /*
            Buttons
        */
        saveButton = GameObject.Find("SaveButton").GetComponent<Button>();
        saveButton.onClick.AddListener(() => Scenes.requestEditScene() );

        cancelEditingButton = GameObject.Find("EditSceneCancelButton").GetComponent<Button>();
        cancelEditingButton.onClick.AddListener(() => ScenesListCanvas.enable() );

        deleteButton = GameObject.Find("DeleteSceneButton").GetComponent<Button>();
        deleteButton.onClick.AddListener(() => Scenes.requestDeleteScene() );

        /*  
            Input Fields 
        */
        nameInputField = GameObject.Find("EditSceneNameField").GetComponent<InputField>();
    }


    public static void Update()
    {
        canvas.enabled = enabled;

        if(clearNameInputField){
            nameInputField.text = "";
            clearNameInputField = false;
        }
    }

    public static void enable(string sceneName){    // Make visible
        nameInputField.text = sceneName;                // Placeholder for name input field
        enabled = true;
        ScenesListCanvas.disable();
    }

    public static void disable(){                   // Hide
        enabled = false;
        clearNameInputField = true;
    }


    public static string getNameInput(){
        return nameInputField.text;
    }


}
