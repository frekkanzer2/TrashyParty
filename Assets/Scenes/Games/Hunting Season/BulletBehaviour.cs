using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletBehaviour : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate()
    {
        Transform component;
        component = this.gameObject.GetComponent<Transform>();
        component.position = new Vector3(component.position.x-(20f*Time.deltaTime), component.position.y, component.position.z);
    }
}
