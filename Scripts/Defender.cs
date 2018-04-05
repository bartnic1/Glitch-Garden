using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The Defender script is responsible for instantiating new projectiles, using a prefabricated weapon defined in the Unity editor.
//It also determines whether the defender is attacking or idle, based on whether there are any attackers in its lane.

public class Defender : MonoBehaviour {

    public GameObject weapon;
    public int starCost = 1;
    private bool canAttack = false;
    private AttackerSpawner myLaneSpawner;
    private AttackerSpawner[] attackerSpawners;
    private StarDisplay starDisplay;
    GameObject projectiles;
    Animator animator;


    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
        starDisplay = GameObject.FindObjectOfType<StarDisplay>();
        projectiles = GameObject.Find("Projectiles");

        //This foreach loop checks whether the defender has a parameter called "isAttacking"
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.name == "isAttacking")
            {
                canAttack = true;
            }
        }

        //Creates an empty projectiles gameobject to hold projectile clones, if necessary
        if(projectiles == null)
        {
            projectiles = new GameObject("Projectiles");
        }

        //Find the attacking lane spawner for this defender.
        SetMyLaneSpawner();

        //If can atack, then continually check to see whether an attacker exists in the defender's lane.
        //If so, change animation state to attacking.
        if (canAttack)
        {
            InvokeRepeating("CheckState", 0.1f, 1f);
        }
        
    }

    void CheckState()
    {
        if (isAttackerAhead())
        {
            if (canAttack)
            {
                animator.SetBool("isAttacking", true);
            }
        }
        else
        {
            if (canAttack)
            {
                animator.SetBool("isAttacking", false);
            }
        }
    }

    bool isAttackerAhead()
    {
        foreach (Transform child in myLaneSpawner.transform)
        {
            if (child.position.x > transform.position.x)
            {
                return true;
            }
        }
        return false;
    }

    public void ThrowProjectile()
    {
        GameObject projectile = Instantiate(weapon, transform.position + Vector3.right * 0.5f, Quaternion.identity);
        projectile.transform.parent = projectiles.transform;
    }


    void SetMyLaneSpawner()
    {
        attackerSpawners = GameObject.FindObjectsOfType<AttackerSpawner>();

        foreach (AttackerSpawner spawner in attackerSpawners)
        {
            if (spawner.transform.position.y == transform.position.y)
            {
                myLaneSpawner = spawner;
                //Remember that break will merely exit the loop. Return not only exits the loop, but also the function as well.
                return;
            }
        }
        Debug.LogError("No attacking lane spawner found for defender: " + name);
    }

    public void AddStars(int amount)
    {
        starDisplay.AddStars(amount);
    }


}
