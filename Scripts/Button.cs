using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//The button class is responsible for illuminating selected sprites, while deluminating others, on the selection panel at the top of the core game space.

public class Button : MonoBehaviour {

    public delegate void ClickAction();
    public static event ClickAction ClickEvent;

    public static GameObject selectedDefender;
    public GameObject defenderPrefab;
    public bool activated = true;

    private Text cost;
    
    SpriteRenderer spriteRenderer;

    // Use this for initialization

    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cost = GetComponentInChildren<Text>();
        if (activated)
        {
            spriteRenderer.color = Color.cyan;
            cost.text = defenderPrefab.GetComponent<Defender>().starCost.ToString();
            if (!cost) { Debug.LogWarning(name + " cost not found!"); }
        }
        else
        {
            spriteRenderer.color = Color.black;
            cost.gameObject.SetActive(false);
        }

	}

    void OnEnable()
    {
        if(activated) ClickEvent += ChangeColour;
    }

    void OnDisable()
    {
       if(activated) ClickEvent -= ChangeColour;
    }

    void ChangeColour()
    {
        spriteRenderer.color = Color.cyan;
    }

    // Another way to light one sprite up and darken the rest, is to use: "private Button[] buttonArray;" in the outermost scope.
    // Then in the start method, set buttonArray = GameObject.FindObjectsOfType<Button>();. While you won't know the order, it won't be important anyway.
    // Next you can use a "foreach Button thisButton in buttonArray" to loop through each button and set them to black.

    // Note that this only works when the mouse button is over a GUIElement or Collider.
    private void OnMouseDown()
    {
        if (activated)
        {
            if (spriteRenderer.color != Color.white)
            {
                ClickEvent();
                spriteRenderer.color = Color.white;
                selectedDefender = defenderPrefab;
            }
        }
    }
}
