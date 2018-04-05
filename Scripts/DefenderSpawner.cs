using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//DefenderSpawner allows the player to create new defenders on the core game space by selecting them from a pane at the top of the screen.
//If necessary, an empty game object labelled "defenders" is created in the hierarchy to store newly instantiated defenders.

public class DefenderSpawner : MonoBehaviour {

    GameObject defenders;
    StarDisplay starDisplay;
    Canvas levelCanvas;
    private bool letSpawn = false;

    // Use this for initialization
    void Start() {
        levelCanvas = GameObject.Find("Level Canvas").GetComponent<Canvas>();
        defenders = GameObject.Find("Defenders");
        starDisplay = GameObject.FindObjectOfType<StarDisplay>();

        if (defenders == null)
        {
            defenders = new GameObject("Defenders");
            defenders.transform.SetParent(levelCanvas.transform);
        }
    }

    private void OnEnable()
    {
        GameManager.startEvent += AllowSpawning;
    }

    private void OnDisable()
    {
        GameManager.startEvent -= AllowSpawning;
    }

    void AllowSpawning()
    {
        letSpawn = true;
    }

    //Note: You can only use onmousedown if the gameobject has a collider or GUI element
    private void OnMouseDown()
    {
        if (letSpawn)
        {
            Vector3 mousePixelPos = Input.mousePosition;

            if (Button.selectedDefender)
            {
                //First get the position of the mouse cursor in world units, snapped to the nearest grid point.
                Vector2 potentialPos = SnapToGrid(CalculateWorldPointOfMouseClick(mousePixelPos));
                //Cycle through the existing defenders. If one exists at that grid point, then prevent a new defender from spawning there.
                foreach (Transform defender in defenders.transform)
                {
                    Vector2 defenderPosition = defender.position;
                    if (potentialPos == new Vector2(defenderPosition.x, defenderPosition.y))
                    {
                        Debug.Log("Can't stack defenders!");
                        return;
                    }
                }
                //Otherwise, the code continues. If enough stars exist in the star bank, then allow the purchase and placement to occur.
                if (starDisplay.UseStars(Button.selectedDefender.GetComponent<Defender>().starCost) == StarDisplay.Status.SUCCESS)
                {
                    GameObject newDefender = Instantiate(Button.selectedDefender, potentialPos, Quaternion.identity);
                    newDefender.transform.parent = defenders.transform;
                }
                else
                {
                    Debug.Log("Insufficient Stars!");
                }
            }
        }
    }

    Vector2 CalculateWorldPointOfMouseClick(Vector3 mouseInput)
    {
        //You can define a vector 2 as a vector 3 value - unity simply ignores the z component automatically.
        //Also note that in the instructors code, he entered a distance to world camera for the z value. This might be important for a perspective camera.
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(mouseInput);
        return worldPoint;
    }

    Vector2 SnapToGrid (Vector2 rawWorldPos)
    {
        int roundedX = Mathf.RoundToInt(rawWorldPos.x);
        int roundedY = Mathf.RoundToInt(rawWorldPos.y);
        return new Vector2 (roundedX, roundedY);
    }
}
