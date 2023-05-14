using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRunGroundBehaviour : MonoBehaviour
{

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("fireball_1_wall_sensitive"))
            if (collision.gameObject.GetComponent<FireballWallableMovement>().CanBeDestroyed)
                Destroy(collision.gameObject);
    }
}
