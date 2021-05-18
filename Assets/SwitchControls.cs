using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchControls : MonoBehaviour
{
    [SerializeField]
    private Sprite controllerLayout;

    // Start is called before the first frame update
    void Start()
    {
        if(Input.GetJoystickNames().Length > 0)
        {
            GetComponent<Image>().sprite = controllerLayout;
            GameObject.Find("Menus").SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
