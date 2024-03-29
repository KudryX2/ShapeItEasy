﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ObjectSelector : ScriptableObject
{
    static Transform transform;                                          

    static RaycastHit raycastHit;                                       // RaycastHit, used to select game objects

    static GameObject selectedObject = null;                            // Selected object
    static Vector3[] vertices;                                          // Selected object vertices list
    
    static GameObject pointerObject;                                    // GameObject used as pointer
    static GameObject pointersContainer;                                // Container for selected object pointers, has same position and rotation as selectedObject
    static List<Vector3> pointersPositionList = new List<Vector3>();    // Selected object pointers positions list


    public static void Start()
    {
        transform = GameObject.Find("ObjectSelector").transform;

        pointersContainer = new GameObject("PointersContainer");            // Container for pointers
        pointersContainer.transform.SetParent(transform);   

        pointerObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);   // GameObject used as pointer
        pointerObject.transform.name = "Pointer";
        pointerObject.transform.localScale = new Vector3(0.5f , 0.5f , 0.5f);                                  
        pointerObject.transform.GetComponent<Renderer>().material.color = Color.blue; 
        pointerObject.transform.SetParent(transform);
        pointerObject.SetActive(false);
    }

    public static void Update(){

        if(Input.GetMouseButtonDown(0) && !SceneEditor.getPlacingShapesMode() && !EventSystem.current.IsPointerOverGameObject())
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit)){
            
                if(!raycastHit.collider.name.Contains("Pointer")){          // Object selected
                    selectedObject = raycastHit.collider.gameObject;                        // Update selected object
                    pointersContainer.SetActive(true);                                      // Show pointers
                    string id = SceneEditor.getShapeId(selectedObject);                     // id of the selected object
                    SelectedShapeInfoCanvas.enable(selectedObject, id);                     // Show selected shape info panel

                    if(pointersContainer.transform.childCount == 0)                     // Create pointers if not created
                        createPointers();

                    else                                                                // Update pointers positions

                        if(selectedObject.GetComponent<MeshFilter>().mesh.vertices.Length != vertices.Length){  // Different kind of object selected       
                            deletePointers();
                            createPointers();
                        }else{                                                                                  // Same kind of object selected
                            updatePointers();   
                        }

                }else{                                                      // Pointer selected

                }
            
            }else{                                          // Clicked on empty space
                selectedObject = null;
                pointersContainer.SetActive(false);                 // Hide the pointers
                SelectedShapeInfoCanvas.disable();                  // Hide selected shape info panel
            }

        if(SceneEditor.getPlacingShapesMode()){     // If placingShapeMode -> disable object selection
            selectedObject = null;
            deletePointers();
        }

    }


    static void createPointers(){                                              // Instantiate new pointers for each vertex 
        vertices = selectedObject.GetComponent<MeshFilter>().mesh.vertices;         

        pointersContainer.transform.position = selectedObject.transform.position;   // Set position
        pointersContainer.transform.rotation = Quaternion.Euler(0,0,0);             // Set rotation

        Vector3 position;
        GameObject newPointerObject;

        foreach(Vector3 vertex in vertices){
            position = Vector3.Scale(transform.TransformPoint(vertex), selectedObject.transform.localScale) + selectedObject.transform.position;

            if(!pointersPositionList.Contains(position)){                           // Only create one pointer for each vertex in same position
                pointersPositionList.Add(position);

                newPointerObject = Instantiate(pointerObject, position, Quaternion.Euler(0,0,0));   // Instantiate
                newPointerObject.transform.SetParent(pointersContainer.transform);                  // Set pointersContainer as parent
                newPointerObject.SetActive(true);                                                   // Show
            }
        }

        pointersContainer.transform.rotation = selectedObject.transform.rotation;

    }


    public static void updatePointers(){                                            // Update pointers positions
        pointersContainer.transform.position = selectedObject.transform.position;       // Update position
        pointersContainer.transform.rotation = Quaternion.Euler(0,0,0);

        for( int i = 0 ; i < pointersContainer.transform.childCount ; ++i)              // Update pointers position
            pointersContainer.transform.GetChild(i).transform.position = Vector3.Scale(transform.TransformPoint(vertices[i]), selectedObject.transform.localScale) + pointersContainer.transform.position;

        pointersContainer.transform.rotation = selectedObject.transform.rotation;       // Update rotation
    }

    public static void deletePointers(){                                            // Delete pointers and clear the pointer position list
        if(pointersPositionList.Count > 0){
            pointersPositionList.Clear();

            foreach(Transform pointer in pointersContainer.transform)
                Destroy(pointer.gameObject);
        }
        
    }

}
