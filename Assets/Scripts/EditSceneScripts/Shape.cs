using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape
{
    public string shape;
    public float x, y, z;

    public Shape(SceneUpdateMessage sceneUpdateMessage){
        this.shape = sceneUpdateMessage.shape;
        this.x = sceneUpdateMessage.x;
        this.y = sceneUpdateMessage.y;
        this.z = sceneUpdateMessage.z;
    }

    public Shape(string shape, float x, float y, float z){
        this.shape = shape;
        this.x = x;
        this.y = y;
        this.z = z;
    }

}
