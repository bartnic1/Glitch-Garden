using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof (Text))]
public class StarDisplay : MonoBehaviour {

    Text text;
    public int stars = 10;

    //Note that enums are automatically considered as static members of the class that they belong to.
    public enum Status { SUCCESS, FAILURE };

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        text.text = stars.ToString();
	}
	
    public void AddStars(int amount)
    {
        stars += amount;
        text.text = stars.ToString();
    }

    public Status UseStars(int amount)
    {
        if (stars >= amount)
        {
            stars -= amount;
            text.text = stars.ToString();
            return Status.SUCCESS;
        }
        return Status.FAILURE;
    }
}
