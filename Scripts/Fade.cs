using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Allows the start screen to fade in over time. Must be attached to a panel, which in turn is a part of the canvas, in order to work.

[RequireComponent (typeof (Image))]
public class Fade : MonoBehaviour {

    Image baseImage;
    float alpha;
    public float fadeTime;
    public float fadeDelay;
    public bool fadeIn;
    private float invokePeriod = 0.01f;
    private float alphaShift;


    //If you define the reference variable baseColor = gameObject.GetComponent<Image>().color, you won't be able to modify the alpha
    //value directly. While you can set baseColor = new Color(r, g, b, a), that won't actually do anything in the game.

    //I think the reason is that, when you go a step too far and specify color, the script instantiates an object with an undefined color as
    //baseColor. However, this object is not the same as the gameObject that the script is a component of. Another user has remarked on something
    //similar happening in his case, and it was confirmed by Nina - that he ended up creating a color object instead of modifying the gameobject color.

    //Now if you set the reference variable to baseImage = gameObject.GetComponent<Image>(), then you can set the color by using 
    //baseImage.color = new Color(r, g, b, a).
    //Note that you still can't specify the alpha value; you must enter the entire color tuple instead.

    void Start()
    {
        baseImage = gameObject.GetComponent<Image>();
        alphaShift = invokePeriod / fadeTime;

        if (fadeIn)
        {
            alpha = 1f;
            InvokeRepeating("FadeIn", fadeDelay, invokePeriod);
        }
        else
        {
            alpha = 0;
            InvokeRepeating("FadeOut", fadeDelay, invokePeriod);
        }
    }
    
    void FadeIn()
    {
        //alpha goes from 1 to 0. Gradually, the alphaShift variable decreases the alpha to zero.
        if (alpha - alphaShift > 0)
        {
            alpha -= alphaShift;
            baseImage.color = new Color(baseImage.color.r, baseImage.color.g, baseImage.color.b, alpha);
        }
        else
        {
            alpha = 0;
            gameObject.GetComponent<Image>().color = new Color(baseImage.color.r, baseImage.color.g, baseImage.color.b, alpha);
            CancelInvoke("FadeIn");
            gameObject.SetActive(false);
        }
    }

    void FadeOut()
    {
        //alpha goes from 0 to 1. Gradually, the alphaShift variable decreases the alpha to zero.
        if (alpha + alphaShift < 1)
        {
            alpha += alphaShift;
            baseImage.color = new Color(baseImage.color.r, baseImage.color.g, baseImage.color.b, alpha);
        }
        else
        {
            alpha = 1;
            gameObject.GetComponent<Image>().color = new Color(baseImage.color.r, baseImage.color.g, baseImage.color.b, alpha);
            CancelInvoke("FadeOut");
            gameObject.SetActive(false);
        }
    }



}
