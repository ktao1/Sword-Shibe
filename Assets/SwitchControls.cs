using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchControls : MonoBehaviour
{
    [SerializeField]
    private Sprite controllerLayout;

    [SerializeField]
    private Sprite keyboardLayout;

    // Start is called before the first frame update
    void Start()
    {
        if(Input.GetJoystickNames().Length > 0 && Input.GetJoystickNames()[0].Length > 0)
        {
            GetComponent<Image>().sprite = controllerLayout;
            if(GameObject.Find("Menus") != null)
                GameObject.Find("Menus").SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetJoystickNames().Length > 0 && Input.GetJoystickNames()[0].Length > 0)
        {
            GetComponent<Image>().sprite = controllerLayout;
            if (GameObject.Find("Menus") != null)
                GameObject.Find("Menus").SetActive(false);
        }
        else
        {
            GetComponent<Image>().sprite = keyboardLayout;
            if (GameObject.Find("Menus") != null)
                GameObject.Find("Menus").SetActive(true);
        }
    }
}
