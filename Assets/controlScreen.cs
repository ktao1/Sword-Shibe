using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controlScreen : MonoBehaviour
{

    private bool screenActive;
    private GameObject controlBackground;
    Image background;

    // Start is called before the first frame update
    void Start()
    {
        controlBackground = GameObject.Find("ControlsBackground");
        background = controlBackground.GetComponent<Image>();
        Color newColor = background.color;
        newColor.a = 0;
        background.color = newColor;
        screenActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("k") && screenActive)
        {
            Color newColor = background.color;
            newColor.a = 0;
            background.color = newColor;
            screenActive = false;
        }
        else if(Input.GetKeyDown("k") && !screenActive)
        {
            Color newColor = background.color;
            newColor.a = 200;
            background.color = newColor;
            screenActive = true;
        }
    }
}
