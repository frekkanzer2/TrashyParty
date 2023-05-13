using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBladeBehaviour : MonoBehaviour
{

    public bool IsLeftRotation;
    public float RotationSpeed;

    // Update is called once per frame
    void FixedUpdate()
    {
        this.gameObject.transform.eulerAngles = new Vector3(
           this.gameObject.transform.eulerAngles.x,
           this.gameObject.transform.eulerAngles.y,
           (IsLeftRotation) ? this.gameObject.transform.eulerAngles.z + RotationSpeed : this.gameObject.transform.eulerAngles.z - RotationSpeed
       );
    }
}
