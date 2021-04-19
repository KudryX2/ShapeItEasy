using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class CreateSceneCanvas : ScriptableObject
{

    private static Canvas canvas;
    private static bool enabled;

    private static Button createSceneButton, createSceneCancelButton;


    private static InputField nameInputField, descriptionInputField;
    private static bool clearInputFields;


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
        descriptionInputField = GameObject.Find("NewSceneDescriptionField").GetComponent<InputField>();

    }

    public static void Update()
    {
        canvas.enabled = enabled;

        if(clearInputFields){
            nameInputField.text = "";
            descriptionInputField.text = "";
            clearInputFields = false;
        }
    }


    public static void enable(){        // Make visible
        enabled = true;
        ScenesListCanvas.disable();
    }

    public static void disable(){       // Hide
        enabled = false;
        clearInputFields = true;
    }


    private static void cancelButton(){
        ScenesListCanvas.enable();
        clearInputFields = true;
    }


    public static string getNameInput(){
        return nameInputField.text;
    }

    public static string getDescriptionInput(){
        return descriptionInputField.text;
    }
}
