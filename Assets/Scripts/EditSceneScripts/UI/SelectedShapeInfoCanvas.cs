using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AttributeCell{ // Aux class
    
    Button button;
    Text text;
    bool editingMode;
    float scrollSpeed;

    float value;
    bool needUpdate;     // If a value changed -> notify the server


    public AttributeCell(string buttonName, float scrollSpeed){

        button = GameObject.Find(buttonName).GetComponent<Button>();
        button.onClick.AddListener(() => updateEditingMode() );

        text = button.transform.GetChild(0).GetComponent<Text>();

        editingMode = false;
        needUpdate = false;

        this.scrollSpeed = scrollSpeed;
    }

    public void updateEditingMode(){
        editingMode = !editingMode;

        if(editingMode)
            button.GetComponent<Image>().color = new Color32(0, 230, 46,100);
        else
            button.GetComponent<Image>().color = new Color32(101, 100, 117,100);

    }

    public void update(){

        if(editingMode){
            if(Input.mouseScrollDelta.y > 0){
                value += scrollSpeed;
                needUpdate = true;
            }                                        
        
            if(Input.mouseScrollDelta.y < 0){
                value -= scrollSpeed;
                needUpdate = true;
            }
        }

        text.text = value.ToString("0.#");
    }

    public void setValue(float value){
        this.value = value;
    }

    public float getValue(){
        return value;
    }

    public void disableEditingMode(){
        editingMode = false;
        button.GetComponent<Image>().color = new Color32(101, 100, 117,100);
    }

    public bool getNeedUpdate(){    // If one of the objects is updated we update, so if we change the value to false there are no need for another loop setting this value to false after the update
        bool oldNeedUpdate = needUpdate;
        needUpdate = false;    
        return oldNeedUpdate;
    }

}


public class SelectedShapeInfoCanvas : ScriptableObject
{
    private static Canvas canvas;
    private static bool enabled;

    static GameObject selectedObject;    

    static Text shapeKind; 

    static List<AttributeCell> attributes;


    public static void Start(){
        canvas = GameObject.Find("SelectedShapeInfoCanvas").GetComponent<Canvas>();
    
        shapeKind = GameObject.Find("SelectedShapeKind").GetComponent<Text>();

        attributes = new List<AttributeCell>();
        attributes.Add(new AttributeCell("PositionX", 0.1f));  // Position
        attributes.Add(new AttributeCell("PositionY", 0.1f));
        attributes.Add(new AttributeCell("PositionZ", 0.1f));

        attributes.Add(new AttributeCell("ScaleX", 0.1f));    // Scale
        attributes.Add(new AttributeCell("ScaleY", 0.1f));
        attributes.Add(new AttributeCell("ScaleZ", 0.1f));

        attributes.Add(new AttributeCell("RotationX", 1f));   // Rotation
        attributes.Add(new AttributeCell("RotationY", 1f));
        attributes.Add(new AttributeCell("RotationZ", 1f));
    }

    public static void Update(){
        canvas.enabled = enabled;           
       
        bool needUpdate = false;
        foreach (var attributeCell in attributes){
            attributeCell.update();                     
            if(attributeCell.getNeedUpdate())       
                needUpdate = true;
        }

        if(selectedObject != null){                 // Update selected object attributes
            // Position
            float x = attributes[0].getValue();
            float y = attributes[1].getValue();
            float z = attributes[2].getValue();

            // Scale
            float sx = attributes[3].getValue();
            float sy = attributes[4].getValue();
            float sz = attributes[5].getValue();

            // Rotation
            float rx = attributes[6].getValue();
            float ry = attributes[7].getValue();
            float rz = attributes[8].getValue();
            
            selectedObject.transform.position = new Vector3(x,y,z);
            selectedObject.transform.localScale = new Vector3(sx, sy, sz);
            selectedObject.transform.rotation = Quaternion.Euler(rx, ry, rz);

            ObjectSelector.updatePointers();        // Update pointers around the selected object
        }
        
        if(needUpdate && selectedObject != null)    // If an attribute has changes -> notify the server
            SceneEditor.requestModifyObject(selectedObject);
        
    }
  

    public static void setSelectedObject(GameObject gameObject){
        enabled = true;
        selectedObject = gameObject;

        shapeKind.text = selectedObject.gameObject.name;        // Name field
       
        // Position
        attributes[0].setValue(selectedObject.transform.position.x);
        attributes[1].setValue(selectedObject.transform.position.y);
        attributes[2].setValue(selectedObject.transform.position.z);

        // Scale
        attributes[3].setValue(selectedObject.transform.localScale.x);
        attributes[4].setValue(selectedObject.transform.localScale.y);
        attributes[5].setValue(selectedObject.transform.localScale.z);

        // Rotation
        attributes[6].setValue(selectedObject.transform.rotation.eulerAngles.x);
        attributes[7].setValue(selectedObject.transform.rotation.eulerAngles.y);
        attributes[8].setValue(selectedObject.transform.rotation.eulerAngles.z);

        foreach (var attributeCell in attributes)
            attributeCell.disableEditingMode();   

        InfoCanvas.setTipsText("CLICK on attribute to modify it / stop modifying, MOUSEWHEEL to increase or decrease the value");
    }

    public static void disable(){
        enabled = false;
        selectedObject = null;

        InfoCanvas.setTipsText("");
    }

    
}
