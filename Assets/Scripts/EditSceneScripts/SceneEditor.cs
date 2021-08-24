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
    public string action, id, shape;
    public float    x , y , z,
                    sx, sy, sz,
                    rx, ry, rz;
}

[Serializable]
public class ShapeInfo{
    public string id, kind;
    public float    x , y , z,
                    sx, sy, sz,
                    rx, ry, rz;
}

[Serializable]
public class UpdateShapeRequest{
    public string shapeID;
    public Vector3 position, scale, rotation;
    public string sceneID;

    public UpdateShapeRequest(string shapeID, Vector3 position, Vector3 scale, Vector3 rotation){
        this.shapeID = shapeID;
        this.position = position;
        this.scale = scale;
        this.rotation = rotation;
        this.sceneID = Session.getConnectedSceneID();
    }
}


public class ShapeData{
    public Vector3 position, scale, rotation;

    public ShapeData(Vector3 position, Vector3 scale, Vector3 rotation){
        this.position = position;
        this.scale = scale;
        this.rotation = rotation;
    }
}

public class SceneEditor
{
    public static GameObject shapesContainer;       // Stores shapes in the scene   

    public static Dictionary<Shape, GameObject> shapes;         // Map of shapes 
    public static List<Shape> shapesToAddList;                  // List of shapes pending to add to the scene
    public static Dictionary<GameObject, ShapeData> toUpdate;   // Map of shapes pending to update to the scene

    static bool placingShapeMode;                   // placingShapeMode enabled/disabled
    static string placingShapeKind;                 // shape kind 
    static GameObject placingShapeTemplateObject;   // template aux object
    static GameObject shapesTemplateContainer;      // stores the templace shape
    static float placingShapeDistance;              // distance to the placing object

    private static GameObject camera;               // Main Camera


    public static void Start()
    {
        shapesContainer = GameObject.Find("ShapesContainer");
        
        shapes = new Dictionary<Shape, GameObject>();
        if(shapesToAddList == null)                 
            shapesToAddList = new List<Shape>();
        toUpdate = new Dictionary<GameObject, ShapeData>();
    
        shapesTemplateContainer = GameObject.Find("ShapesTemplateContainer");

        placingShapeMode = false;

        camera = GameObject.Find("Camera"); 
    }


    public static void Update()
    {
        if(shapesToAddList.Count > 0){              // If shapes pending to add -> add new shapes and clear the list
            foreach(Shape shape in shapesToAddList){
                GameObject newObject = addShape(shape);        //  Create instance at the scene
                shapes.Add(shape, newObject);
            }

            shapesToAddList.Clear();
        }

        if(toUpdate.Count > 0){                     // If shapes pending to update -> update data

            foreach(KeyValuePair<GameObject, ShapeData> iterator in toUpdate){
                GameObject gameObject = iterator.Key;

                if(gameObject != null){
                    ShapeData data = iterator.Value;

                    gameObject.transform.localPosition = data.position;
                    gameObject.transform.localScale = data.scale;
                    gameObject.transform.eulerAngles = data.rotation;
                }
            }

            toUpdate.Clear();
        }

        if(placingShapeMode){                       
            Vector3 forward = camera.transform.TransformPoint(Vector3.forward * placingShapeDistance);
            placingShapeTemplateObject.transform.localPosition = forward;   // Update template object position
        
            if(Input.GetKeyDown(KeyCode.Return)){                           // ENTER KEY + placingShapeMode -> requestAddShape 
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

            if(sceneUpdateMessage.action == "added")            // New shape -> add to toAdd list 
                shapesToAddList.Add(new Shape(sceneUpdateMessage));
            
            else if(sceneUpdateMessage.action == "updated")     // Updated shape -> update client data
                updateShape(sceneUpdateMessage);

        }catch(Exception e){
            Debug.Log(e);
        }

    }

    /*
        Update Shape Request
    */
    public static void requestUpdateShape(GameObject updatedObject, string shapeID){
        Vector3 position = updatedObject.transform.position;
        Vector3 scale = updatedObject.transform.localScale;
        Vector3 rotation = updatedObject.transform.rotation.eulerAngles;

        Client.sendData("updateShape", JsonUtility.ToJson( new UpdateShapeRequest(shapeID, position, scale, rotation)) );
    } 

    public static void handlUpdateShapeResponse(string response){
        // TODO si se recibe un mensaje de error devolver a su estado anterior la modificación 
    }

    /*
        Update Shape Message (Broadcast)
    */
    private static void updateShape(SceneUpdateMessage updateMessage){

        // Update scene
        foreach( KeyValuePair<Shape, GameObject> shape in shapes){
            Shape key = shape.Key;
            GameObject sceneObject = shape.Value;

            if(key.id == updateMessage.id){
                Vector3 position = new Vector3(updateMessage.x, updateMessage.y, updateMessage.z);
                Vector3 scale = new Vector3(updateMessage.sx, updateMessage.sy, updateMessage.sz);
                Vector3 rotation = new Vector3(updateMessage.rx, updateMessage.ry, updateMessage.rz);

                toUpdate.Add(sceneObject, new ShapeData(position, scale, rotation));
                break;
            }
        }
    }


    private static GameObject addShape(Shape shape){
        GameObject newObject = null;                        // Object to instantiate

        if(shape.shape == "Cube")
            newObject = GameObject.CreatePrimitive(PrimitiveType.Cube); 

        if(newObject != null){                              // Set parent and position
            newObject.transform.SetParent(shapesContainer.transform);            
            newObject.transform.localPosition = shape.getPosition();    
            newObject.transform.localScale = shape.getScale();
            newObject.transform.eulerAngles = shape.getRotation();
        }

        return newObject;
    }


    public static void enablePlacingShapeMode(string shapeKind){

        placingShapeMode = true;
        placingShapeKind = shapeKind;
        placingShapeDistance = 3f;

        if(placingShapeKind == "Cube")
            placingShapeTemplateObject = GameObject.CreatePrimitive(PrimitiveType.Cube); 

        if(placingShapeTemplateObject != null)              // Set parent
            placingShapeTemplateObject.transform.SetParent(shapesTemplateContainer.transform);   

        InfoCanvas.setTipsText("ENTER to add shape, ESCAPE to cancel, MOUSEWHEEL to change the distance");          
    }

    public static void disablePlacingShapeMode(){
        placingShapeMode = false;                           // Disable placing mode and delete aux object
        UnityEngine.Object.Destroy(shapesTemplateContainer.transform.GetChild(0).gameObject);

        InfoCanvas.setTipsText("");
    }



    public static void loadScene(string info){

        try{
            Debug.Log("Loading the scene");

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
                    shapesToAddList.Add(new Shape(shape.id, shape.kind, shape.x, shape.y, shape.z, shape.sx, shape.sy, shape.sz, shape.rx, shape.ry, shape.rz));

                    elementDetected = false;
                    element = "";
                }

            }

        }catch(Exception exception){
            Debug.Log("Error cargando la escena : " + exception );
        }


    }

    public static string getShapeId(GameObject gameObject){

        foreach(KeyValuePair<Shape, GameObject> iterator in shapes)
            if(iterator.Key.getPosition() == gameObject.transform.position)
                return iterator.Key.id;

        return null;
    }

    
    public static bool getPlacingShapesMode(){          // Called from ObjectSelector.cs y FreeCamera.cs
        return placingShapeMode;
    }
}
