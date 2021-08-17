using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape
{
    public string id, shape;
    public float x, y, z;

    public Shape(SceneUpdateMessage sceneUpdateMessage){
        this.id = sceneUpdateMessage.id;
        this.shape = sceneUpdateMessage.shape;
        this.x = sceneUpdateMessage.x;
        this.y = sceneUpdateMessage.y;
        this.z = sceneUpdateMessage.z;
    }

    public Shape(string id, string shape, float x, float y, float z){
        this.id = id;
        this.shape = shape;
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3 getPosition(){
        return new Vector3(x,y,z);
    }

}
