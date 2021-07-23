using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShapesDropDown : CustomDropDown
{
    [SerializeField]
    public GameObject addShapePanel;

    public override void clickOnOption(string option){
        
        List<Button> optionsList = getOptionsList();

        if(option == optionsList[0].gameObject.name){   // Add Shape -> show add shape panel
            hideOptions();
            addShapePanel.GetComponent<AddShapePanel>().show();
        }
    }
}
