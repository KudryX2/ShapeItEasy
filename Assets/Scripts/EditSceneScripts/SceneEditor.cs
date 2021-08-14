using System.Collections;
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

    static bool placingShapeMode;                   // placingShapeMode enabled/disabled
    static string placingShapeKind;                 // shape kind 
    static GameObject placingShapeTemplateObject;   // template aux object
    static GameObject shapesTemplateContainer;      // stores the templace shape
    static float placingShapeDistance;              // distance to the placing object

    private static GameObject camera;               // Main Camera


    public static void Start()
    {
        if(shapesContainer == null)
            shapesContainer = GameObject.Find("ShapesContainer");
    
        if(shapesToAddList == null)
            shapesToAddList = new List<Shape>();
    
        if(shapesTemplateContainer == null)
            shapesTemplateContainer = GameObject.Find("ShapesTemplateContainer");

        placingShapeMode = false;

        camera = GameObject.Find("Camera"); 
    }


    public static void Update()
    {
        if(shapesToAddList.Count > 0){              // If shapes pending to add -> add new shapes and clear the list
            foreach(Shape shape in shapesToAddList)
                addShape(shape);

            shapesToAddList.Clear();
        }
            

        if(placingShapeMode){                       
            Vector3 forward = camera.transform.TransformPoint(Vector3.forward * placingShapeDistance);
            placingShapeTemplateObject.transform.localPosition = forward;   // Update template object position
        
            if(Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)){    // ENTER KEY + placingShapeMode -> requestAddShape 
                requestAddShape(placingShapeKind, forward);                 // Request
                disablePlacingShapeMode();
            }

            if(Input.GetKeyDown(KeyCode.Escape))                                    // ESCAPE KEY + placingShapeMode -> cancel 
                disablePlacingShapeMode();

            if(Input.mouseScrollDelta.y > 0)                                        // MOUSE SCROLL -> distance 
                placingShapeDistance += 0.1f;
        
            if(Input.mouseScrollDelta.y < 0 && placingShapeDistance > 1.5f)
                placingShapeDistance -= 0.1f;

        }

    }

    /*
        Add Shape Request
    */
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

    /*
        Update Scene Message
    */
    public static void handleSceneUpdateMessage(string message){

        try{
            SceneUpdateMessage sceneUpdateMessage = JsonUtility.FromJson<SceneUpdateMessage>(message);

            if(sceneUpdateMessage.action == "added")        // If new shape -> add to toAdd list 
                shapesToAddList.Add(new Shape(sceneUpdateMessage));

        }catch(Exception e){
            Debug.Log(e);
        }

    }

    /*
        Modify Object Request
    */
    public static void requestModifyObject(GameObject updatedObject){
        Vector3 position = updatedObject.transform.position;
        Vector3 scale = updatedObject.transform.localScale;
        Vector3 rotation = updatedObject.transform.rotation.eulerAngles;

        Debug.Log(position);
    }

    public static void handlModifyObjectResponse(string response){
        Debug.Log(response);

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


    public static void enablePlacingShapeMode(string shapeKind){

        placingShapeMode = true;
        placingShapeKind = shapeKind;
        placingShapeDistance = 3f;

        if(placingShapeKind == "Cube")
            placingShapeTemplateObject = GameObject.CreatePrimitive(PrimitiveType.Cube); 

        if(placingShapeTemplateObject != null)              // Set parent
            placingShapeTemplateObject.transform.SetParent(shapesTemplateContainer.transform);   

        InfoCanvas.setTipsText("ENTER or LEFT CLICK to add shape, ESCAPE to cancel, MOUSEWHEEL to change the distance");          
    }

    public static void disablePlacingShapeMode(){
        placingShapeMode = false;                           // Disable placing mode and delete aux object
        UnityEngine.Object.Destroy(shapesTemplateContainer.transform.GetChild(0).gameObject);

        InfoCanvas.setTipsText("");
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

    
    public static bool getPlacingShapesMode(){          // Called from ObjectSelector.cs y FreeCamera.cs
        return placingShapeMode;
    }
}
