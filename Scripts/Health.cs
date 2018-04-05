using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Health keeps track of the health remaining for defenders and attackers, and is publicly defined in the editor. It also has a function that can be called
//to test whether the health of a subject has been depleted.

public class Health : MonoBehaviour {

    public float health;
    public Slider healthSlider;

    private void Start()
    {
        //You can't attach the health slider to the parent gameobject, because the slider script depends on there being a rect. transform, which in turn
        //only works when the object is in a canvas.
        //If you instead attach the slider to a child, then the parent maintains its original transform, and the child becomes a canvas-only object.
        
        //What you can then do is spawn the parent object in a canvas. Since you don't necessarily need a rect. transform to do this, it can maintain
        //its original 3d transform. However, its health slider will still be visible. Also note that UI objects which go beyond the limits of the canvas
        //are still visible!
        healthSlider = transform.GetComponentInChildren<Slider>();
        healthSlider.maxValue = health;
        healthSlider.value = health;
    }

    public bool healthDepleted()
    {
        if (health <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
