using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SelectedShapeInfoCanvas : ScriptableObject
{
    private static Canvas canvas;
    private static bool enabled;

    static GameObject selectedObject;    

    static Text shapeKind, shapePosition, shapeScale;


    public static void Start(){
        canvas = GameObject.Find("SelectedShapeInfoCanvas").GetComponent<Canvas>();
    
        shapeKind = GameObject.Find("SelectedShapeKind").GetComponent<Text>();
        shapePosition = GameObject.Find("SelectedShapePosition").GetComponent<Text>();
        shapeScale = GameObject.Find("SelectedShapeScale").GetComponent<Text>();
    }

    public static void Update(){
        canvas.enabled = enabled;
    }

    public static void disable(){
        enabled = false;
    }

    public static void setSelectedObject(GameObject gameObject){
        enabled = true;
        selectedObject = gameObject;

        shapeKind.text = selectedObject.gameObject.name;
        shapePosition.text = "Position : " + selectedObject.transform.position.x.ToString("0.##") + " " + selectedObject.transform.position.y.ToString("0.##") + " " + selectedObject.transform.position.z.ToString("0.##");
        shapeScale.text = "Scale : " + selectedObject.transform.localScale.x + " " + selectedObject.transform.localScale.y + " " + selectedObject.transform.localScale.z;

    }
}
