using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controlScreen : MonoBehaviour
{

    private bool screenActive;
    private GameObject controls;
    Image background;

    // Start is called before the first frame update
    void Start()
    {
        controls = GameObject.Find("Controls");
        if (controls.activeSelf)
            controls.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("k"))
        {
            controls.SetActive(!controls.activeSelf);
        }
    }
}
