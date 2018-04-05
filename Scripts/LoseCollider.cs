using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseCollider : MonoBehaviour {

    GameManager gameManager;
    int enemyCounter = 0;
    public int maxEnemies = 5;

    private void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        enemyCounter++;
        Destroy(collider.gameObject);
        if(enemyCounter >= maxEnemies)
        {
            gameManager.LoadLevel("03b Lose");
        }

    }
}
