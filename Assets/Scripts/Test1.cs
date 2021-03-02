using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : MonoBehaviour
{

    void Start()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);                                                                           // Object to instantiate
        cube.transform.SetParent(transform);

        int x, y, z;

        for(int i = 0 ;  i < 100 ; ++i){
            x = Random.Range(-25, 25);
            y = Random.Range(-25, 25);
            z = Random.Range(-25, 25);

            GameObject newObject = Instantiate(cube, new Vector3(x,y,z), Quaternion.Euler(0, 90, 0));                                               // Create copy
            newObject.transform.SetParent(transform);                                                                                               // Parent
            newObject.transform.localScale = new Vector3(Random.Range(1,4), Random.Range(1,4), Random.Range(1,4));                                  // Scale
            newObject.transform.Rotate(0, Random.Range(0, 180), 0, Space.Self);                                                                     // Rotate
            newObject.transform.GetComponent<Renderer>().material.color = new Color(Random.Range(0f,1f), Random.Range(0f,1f), Random.Range(0f,1f)); // Color
        }
    }

}
