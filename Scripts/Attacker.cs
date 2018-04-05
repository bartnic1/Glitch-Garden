using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The attacker script governs the motion of the attackers, reduces the health and destroys attackers when hit by projectiles, and sets various
//triggers and booleans depending on what defender they come in contact with. It also has a setSpeed function that allows the animator to change
//the motion of the attacker in certain animation clips, as well as a strikeCurrentTarget function which is called in animation clips to allow the
//attacker to strike and reduce the health of its target.

//-------------------------------

// Note that, if you require your script to call another script that must exist on the same game object, you can use the following
// statement as a safety mechanism. In this case, if you were to add the script named Attacker.cs to a gameObject, Unity would automatically
// attach FadeIn.cs as well.

//[RequireComponent (typeof (FadeIn))]

public class Attacker : MonoBehaviour {

    //Use the range command to create a slider in the editor, limiting the variable 'currentSpeed' values to the encoded limits.
    [Range(0, 1.5f)] private float currentSpeed;

    //Note that you can use the following tooltip command to let designers in Unity editor know what the variable means:
    //[Tooltip("Average number of seconds between spawns")]
    bool frozen = false;
    Animator animator;
    Health defenderHealth, attackerHealth;
    Projectile projectile;

    private void OnEnable()
    {
        GameManager.endEvent += EndGame;
    }

    private void OnDisable()
    {
        GameManager.endEvent -= EndGame;
    }

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        attackerHealth = GetComponent<Health>();
	}
	
	// Update is called once per frame
	void Update () {
        // Old way to move gameObjects:
        // transform.position += new Vector3(currentSpeed * Time.deltaTime, 0, 0);
        // Alternative:
        if(frozen == false)
        {
            transform.Translate(Vector3.left * Time.deltaTime * currentSpeed);
        }
	}

    private void OnTriggerEnter2D(Collider2D collider)
    {
        projectile = collider.GetComponent<Projectile>();
        if (projectile)
        {
            //This is probably the best place to manage the health of the attacker. The health script is more of a storage bin for the values
            //and for the health slider. Its more useful to partition it this way, so the same can be done for the defender.
            attackerHealth.health -= projectile.damage;
            attackerHealth.healthSlider.value = attackerHealth.health;
            Destroy(collider.gameObject);
            if (attackerHealth.healthDepleted())
            {
                Destroy(gameObject);
            }
        }
        // Its useful to note that the triggerEnter2D function only runs once - when the colliders first collide. After that, even though 
        // one collider is inside of another, it doesn't count as being entered.
        switch (tag)
        {
            case "Fox":
                // Parameters are assigned integers depending on their order in the animator parameter list:
                // For fox: isAttacking = 0, jumpTrigger = 1.
                // For lizard: isAttacking = 0.

                if (collider.GetComponent<Defender>())
                {
                    if (collider.tag == "Gravestone")
                    {
                        animator.SetTrigger("jumpTrigger");
                    }
                    else
                    {
                        animator.SetBool("isAttacking", true);
                        defenderHealth = collider.GetComponent<Health>();
                    }
                }
                break;

            case "Lizard":
                if (collider.GetComponent<Defender>())
                {
                    animator.SetBool("isAttacking", true);
                    defenderHealth = collider.GetComponent<Health>();
                }
                break;
            default:
                break;
        }
    }

    public void SetSpeed (float speed)
    {
        currentSpeed = speed;
    }

    public void StrikeCurrentTarget (float damage)
    {
        if (defenderHealth)
        {
            defenderHealth.health -= damage;
            defenderHealth.healthSlider.value = defenderHealth.health;
            //print(defenderHealth.gameObject.name + " has " + defenderHealth.health + " health left.");
            if (defenderHealth.healthDepleted())
            {
                // Alternatively, if you have a death animation, you can signal for this transition to occur in the animator.
                // Then, you can create an event in the death animation clip that calls a function in health.cs which destroys the gameObject.
                // Note that this function has not been created yet as no death animations are anticipated.
                Destroy(defenderHealth.gameObject);
                animator.SetBool("isAttacking", false);
            }
            //Put in a special trigger for the gravestone. Its easier to put the trigger in this script, since the gravestone should shake
            //when the attacker strikes it.
            else if (defenderHealth.gameObject.tag == "Gravestone")
            {
                defenderHealth.gameObject.GetComponent<Animator>().SetTrigger("ShakeTrigger");
            }
        }
        //If the defender was destroyed by another target, then no defenderhealth will exist. In this case, exit the attack animation.
        else if (!defenderHealth)
        {
            animator.SetBool("isAttacking", false);
        }

    }

    private void EndGame()
    {
        frozen = true;
        animator.SetBool("isEnd", true);
    }
}
