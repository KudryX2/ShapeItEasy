using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape
{
    public string id, shape;
    public float    x , y , z,  // Position
                    sx, sy, sz, // Scale
                    rx, ry, rz; // Rotation


    public Shape(SceneUpdateMessage sceneUpdateMessage){
        this.id = sceneUpdateMessage.id;
        this.shape = sceneUpdateMessage.shape;

        updateData(sceneUpdateMessage);
    }

    public Shape(string id, string shape, float x, float y, float z, float sx, float sy, float sz, float rx, float ry, float rz){
        this.id = id;
        this.shape = shape;
        
        this.x = x;
        this.y = y;
        this.z = z;

        this.sx = sx;
        this.sy = sy;
        this.sz = sz;

        this.rx = rx;
        this.ry = ry;
        this.rz = rz;
    }

    public void updateData(SceneUpdateMessage sceneUpdateMessage){
        this.x = sceneUpdateMessage.x;
        this.y = sceneUpdateMessage.y;
        this.z = sceneUpdateMessage.z;
    
        this.sx = sceneUpdateMessage.sx;
        this.sy = sceneUpdateMessage.sy;
        this.sz = sceneUpdateMessage.sz;

        this.rx = sceneUpdateMessage.rx;
        this.ry = sceneUpdateMessage.ry;
        this.rz = sceneUpdateMessage.rz;
    }

    public Vector3 getPosition(){
        return new Vector3(x,y,z);
    }

    public Vector3 getScale(){
        return new Vector3(sx, sy, sz);
    }

    public Vector3 getRotation(){
        return new Vector3(rx, ry, rz);
    }

}
