using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Destroys objects that stray too far from the scene on the horizontal axis.

public class Shredder : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
    }
}
