using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class AddShapeRequestData{
    public string shape;
    public float x, y, z;

    public AddShapeRequestData(string shape, Vector3 position){
        this.shape = shape;
        this.x = position.x;
        this.y = position.y;
        this.z = position.z;
    }
}


public class SceneEditor
{


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void requestAddShape(String shape, Vector3 position){
        AddShapeRequestData addShapeRequestData = new AddShapeRequestData(shape, position);
        Client.sendData("addShape", JsonUtility.ToJson(addShapeRequestData));
    }

    public static void handleAddShapeResponse(string response){

        // TODO mostrar la figura añadida en el lado de cliente si la respuesta es correcta

        if(response == "OK")
            AddShapeCanvas.disable();
        else
            Debug.Log("Error añadiendo una figura");
    }
}
