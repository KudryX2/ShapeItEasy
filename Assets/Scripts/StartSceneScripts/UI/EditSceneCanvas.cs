using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class EditSceneCanvas : ScriptableObject
{
    static Canvas canvas;
    static bool enabled;

    static Button saveButton, cancelEditingButton, deleteButton;    

    static InputField nameInputField, descriptionInputField;
    static bool clearInputFields;


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
        deleteButton.onClick.AddListener(() => DeleteSceneConfirmationCanvas.enable() );

        /*  
            Input Fields 
        */
        nameInputField = GameObject.Find("EditSceneNameField").GetComponent<InputField>();
        descriptionInputField = GameObject.Find("EditSceneDescriptionField").GetComponent<InputField>();

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

    public static void enable(Scene scene){      // Make visible
        nameInputField.text = scene.name;                       // Placeholder for name input field
        descriptionInputField.text = scene.description;         // Placeholder for description input field
        enabled = true;
        ScenesListCanvas.disable();
    }

    public static void disable(){                           // Hide
        enabled = false;
        clearInputFields = true;
    }


    public static string getNameInput(){
        return nameInputField.text;
    }

    public static string getDescriptionInput(){
        return descriptionInputField.text;
    }


}
