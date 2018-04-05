using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The projectile script manages the motion of projectiles, primarily for defenders.

public class Projectile : MonoBehaviour {

    public float speed, damage;
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.right*speed*Time.deltaTime);
	}

    /*Note that to use the method below (derived from MonoBehaviour), you have to have a sprite renderer attached to the gameObject.
    Unfortunately in these videos, the instructor attaches the renderer to a child gameObject. So I added an empty renderer to the parent.

    Instead the instructor created a shredder.
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    */


}
