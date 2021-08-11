﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class AddShapeRequestData{
    public string shape;
    public float x, y, z;
    public string sceneID;

    public AddShapeRequestData(string shape, Vector3 position){
        this.shape = shape;
        this.x = position.x;
        this.y = position.y;
        this.z = position.z;
        this.sceneID = Session.getConnectedSceneID();
    }
}

[Serializable]
public class SceneUpdateMessage{
    public string action, shape;
    public float x, y, z;
}

[Serializable]
public class ShapeInfo{
    public string kind;
    public float x, y, z;
}


public class SceneEditor
{
    public static GameObject shapesContainer;       // Stores shapes in the scene   
    public static List<Shape> shapesToAddList;      // List os shapes pending to add to the scene


    public static void Start()
    {
        if(shapesContainer == null)
            shapesContainer = GameObject.Find("ShapesContainer");
    
        if(shapesToAddList == null)
            shapesToAddList = new List<Shape>();
    }


    public static void Update()
    {
        if(shapesToAddList.Count > 0){              // If shapes pending to add -> add new shapes and clear the list
            foreach(Shape shape in shapesToAddList)
                addShape(shape);

            shapesToAddList.Clear();
        }
            
    }

    public static void requestAddShape(String shape, Vector3 position){
        AddShapeRequestData addShapeRequestData = new AddShapeRequestData(shape, position);
        Client.sendData("addShape", JsonUtility.ToJson(addShapeRequestData));
    }

    public static void handleAddShapeResponse(string response){
        if(response == "OK")
            AddShapeCanvas.disable();
        else
            Debug.Log("Error añadiendo una figura");
    }


    public static void handleSceneUpdateMessage(string message){

        try{
            SceneUpdateMessage sceneUpdateMessage = JsonUtility.FromJson<SceneUpdateMessage>(message);

            if(sceneUpdateMessage.action == "added")        // If new shape -> add to toAdd list 
                shapesToAddList.Add(new Shape(sceneUpdateMessage));

        }catch(Exception e){
            Debug.Log(e);
        }

    }


    private static void addShape(Shape shape){
        GameObject newObject = null;                        // Object to instantiate

        if(shape.shape == "Cube")
            newObject = GameObject.CreatePrimitive(PrimitiveType.Cube); 

        if(newObject != null){                              // Set parent and position
            newObject.transform.SetParent(shapesContainer.transform);            
            newObject.transform.localPosition = new Vector3(shape.x, shape.y, shape.z);    
        }

    }

    public static void loadScene(string info){

        try{

            if(shapesToAddList == null)
                shapesToAddList = new List<Shape>();

            info = info.Replace("[", "");
            info = info.Replace("]", "");
            info = info.Replace("'", "\"");

            bool elementDetected = false;
            string element = "";

            for( int i = 0 ; i < info.Length ; ++i){

                if(info[i] == '{')      
                    elementDetected = true;

                if(elementDetected)
                    element += info[i];

                if(info[i] == '}' && elementDetected){  
                    ShapeInfo shape = JsonUtility.FromJson<ShapeInfo>(element);
                    shapesToAddList.Add(new Shape(shape.kind, shape.x, shape.y, shape.z));

                    elementDetected = false;
                    element = "";
                }

            }

        }catch(Exception exception){
            Debug.Log("Error cargando la escena : " + exception );
        }


    }

}
