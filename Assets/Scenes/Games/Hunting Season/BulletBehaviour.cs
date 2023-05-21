using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Transform component;
        
        component = this.gameObject.GetComponent<Transform>();
        component.position = new Vector3(component.position.x-0.02f, component.position.y, component.position.z);
    }
}
