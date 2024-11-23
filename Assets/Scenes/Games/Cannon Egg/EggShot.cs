using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggShot : MonoBehaviour
{
    public Vector2? Direction { get => direction; set {
            direction = value;
            OnDirectionSet(); 
        } }
    private Vector2? direction = null;
    private void OnDirectionSet() => canMove = true;
    private bool canMove = false;
    private float speed = 18f;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canMove)
        {
            this.transform.position = this.transform.position + Extensions.Vector3.FromVector2(direction ?? Vector2.zero) * speed * Time.deltaTime;
        }
    }
}
