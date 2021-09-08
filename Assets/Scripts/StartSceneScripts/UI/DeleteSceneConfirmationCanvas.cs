using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class DeleteSceneConfirmationCanvas : ScriptableObject{

    static Canvas canvas;
    static bool enabled;

    static Button confirmButton, cancelButton;


    public static void Start(){
        canvas = GameObject.Find("DeleteSceneConfirmationCanvas").GetComponent<Canvas>();

        confirmButton = GameObject.Find("ConfirmDeleteScene").GetComponent<Button>();
        confirmButton.onClick.AddListener(() => Scenes.requestDeleteScene() );
        
        cancelButton = GameObject.Find("CancelDeleteScene").GetComponent<Button>();
        cancelButton.onClick.AddListener(() => ScenesListCanvas.enable() );
    }


    public static void Update(){
        canvas.enabled = enabled;
    }


    public static void enable(){
        enabled = true;
        EditSceneCanvas.disable();
    }

    public static void disable(){
        enabled = false;
    }

}