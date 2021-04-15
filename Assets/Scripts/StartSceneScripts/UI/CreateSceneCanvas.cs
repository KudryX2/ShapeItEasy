using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class CreateSceneCanvas : ScriptableObject
{

    private static Canvas canvas;
    private static bool enabled;

    private static Button createSceneButton, createSceneCancelButton;


    private static InputField nameInputField;
    private static bool clearNameInputField;


    public static void Start()
    {
        canvas = GameObject.Find("NewSceneCanvas").GetComponent<Canvas>();

        /*
            Buttons
        */
        createSceneButton = GameObject.Find("CreateSceneButton").GetComponent<Button>();
        createSceneButton.onClick.AddListener(() => Scenes.requestCreateScene() );

        createSceneCancelButton = GameObject.Find("CreateSceneCancelButton").GetComponent<Button>();
        createSceneCancelButton.onClick.AddListener(() => ScenesListCanvas.enable() );

        /*
            Input fields 
        */
        nameInputField = GameObject.Find("NewSceneNameField").GetComponent<InputField>();

    }

    public static void Update()
    {
        canvas.enabled = enabled;

        if(clearNameInputField){
            nameInputField.text = "";
            clearNameInputField = false;
        }
    }


    public static void enable(){        // Make visible
        enabled = true;
        ScenesListCanvas.disable();
    }

    public static void disable(){       // Hide
        enabled = false;
        clearNameInputField = true;
    }


    private static void cancelButton(){
        ScenesListCanvas.enable();
        clearNameInputField = true;
    }


    public static string getNameInput(){
        return nameInputField.text;
    }
}
