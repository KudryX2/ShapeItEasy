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

    static Text notificationTextComponent;
    static string notificationText = "";


    public static void Start()
    {
        canvas = GameObject.Find("CreateSceneCanvas").GetComponent<Canvas>();

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
        nameInputField = GameObject.Find("CreateSceneNameField").GetComponent<InputField>();
        descriptionInputField = GameObject.Find("CreateSceneDescriptionField").GetComponent<InputField>();


        notificationTextComponent = GameObject.Find("CreateSceneNotification").GetComponent<Text>();
        notificationTextComponent.text = "";
    }

    public static void Update()
    {
        canvas.enabled = enabled;

        if(clearInputFields){
            nameInputField.text = "";
            descriptionInputField.text = "";
            clearInputFields = false;
        }

        if(notificationText != ""){
            notificationTextComponent.text = notificationText;
            notificationText = "";
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


    public static void setNotificationText(string text){
        notificationText = text;        // We use the same variable to store the text to update and the need of action
    }

    public static string getNameInput(){
        return nameInputField.text;
    }

    public static string getDescriptionInput(){
        return descriptionInputField.text;
    }
}
