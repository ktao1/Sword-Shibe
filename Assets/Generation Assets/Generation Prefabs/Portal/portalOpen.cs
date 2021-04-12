using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portalOpen : MonoBehaviour
{
    public bool red = false;
    public GameObject portal;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject thePortal = Instantiate(portal, new Vector3(transform.position.x, transform.position.y-.25f, transform.position.z), transform.rotation) as GameObject;
        thePortal.transform.SetParent(transform);
    }
}
