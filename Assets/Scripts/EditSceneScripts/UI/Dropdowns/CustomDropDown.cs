using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;


public class CustomDropDown : MonoBehaviour
{

    Button optionTemplateButton;        // Template for options 
    public int optionHeight;            // Height of a option given from unity

    [SerializeField]
    public string[] options;            // Options to include given from unity 
    
    GameObject optionsContainer;        // Container to store options 
    List<Button> optionsList;           // List of options (buttons)

    bool overDropdown;                  // Mouse is over the dropdown menu
    

    void Start()
    {
        addMouseListener(gameObject);

        optionTemplateButton = transform.GetChild(1).GetComponent<Button>();    // Option template (Second child)
        optionTemplateButton.gameObject.SetActive(false);


        optionsContainer = new GameObject("OptionsContainer");
        optionsContainer.transform.parent = transform;
        optionsContainer.transform.localPosition = new Vector3(Math.Abs(optionTemplateButton.GetComponent<RectTransform>().rect.x), (float) - (optionHeight * 0.5f) ,0);   // If using anchors
        //optionsContainer.transform.localPosition = new Vector3(0, 0, 0);        

        overDropdown = false;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0)){

            GameObject selectedOption = getSelectedOption();    // Get selected option based on position

            if(selectedOption != null && selectedOption.name == gameObject.name)    // Click on action button -> hide/show options
                if(isDropdownDeployed())
                    hideOptions();
                else
                    showOptions();

            else if(!overDropdown)                                                  // Click out of dropdown -> hide options
                hideOptions();

            else                                                                    // Click on a option 
                clickOnOption(selectedOption.name);

        }

    }

    // Method controlling the click on options, can be overrided
    public virtual void clickOnOption(string option){
        Debug.Log(option);
    }


    void showOptions(){         // Shows dropdown menu options
        if(optionsList == null)     // If doesn´t exist -> init
            initOptions();
        else
            foreach(var option in optionsList)
                option.gameObject.SetActive(true);
    }

    public void hideOptions(){         // Hides dropdown menu options
        if(optionsList != null)
            foreach(var option in optionsList)
                option.gameObject.SetActive(false);
    }

    
    
    void initOptions(){
        optionsList = new List<Button>();
        
        for(int i = 0 ; i < options.Length ; ++i){
            var option = Instantiate(optionTemplateButton, new Vector3(0, 0, 0), new Quaternion(),  optionsContainer.transform);   // Instantiate
            option.transform.GetChild(0).GetComponent<Text>().text = options[i];                                                    // Set title 
            option.transform.localPosition = new Vector3(0, - optionHeight * (i + 1) ,0);                                           // Set position
            option.gameObject.name = options[i];
            addMouseListener(option.gameObject);
            option.gameObject.SetActive(true);


            optionsList.Add(option);
        }

    }


    void addMouseListener(GameObject element){      // Add mouse listener to a gameobject (dropdown button)
        element.AddComponent<EventTrigger>();                                   // Add event trigger

        EventTrigger.Entry eventPointerEnter = new EventTrigger.Entry();        // Mouse enter event
        eventPointerEnter.eventID = EventTriggerType.PointerEnter;
        eventPointerEnter.callback.AddListener((eventData) => { overDropdown = true; });
        element.GetComponent<EventTrigger>().triggers.Add(eventPointerEnter);

        EventTrigger.Entry eventPointerExit = new EventTrigger.Entry();         // Mouse exit event
        eventPointerExit.eventID = EventTriggerType.PointerExit;
        eventPointerExit.callback.AddListener((eventData) => { overDropdown = false; });
        element.GetComponent<EventTrigger>().triggers.Add(eventPointerExit);
    }


    GameObject getSelectedOption(){             // Return the clicked menu option 

        GameObject selectedOption = null;

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position =  Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll( eventData, raysastResults );

        foreach (var eventData2 in raysastResults)
            selectedOption = eventData2.gameObject;

        return selectedOption;
    }

    bool isDropdownDeployed(){                  // Finds out if dropdown is deployed or not
        if(optionsContainer.transform.childCount == 0)      // In case it isnt initialized yet
            return false;
        else
            return optionsContainer.transform.GetChild(0).gameObject.activeSelf;
    }

    public List<Button> getOptionsList(){
        return optionsList;
    }

}
