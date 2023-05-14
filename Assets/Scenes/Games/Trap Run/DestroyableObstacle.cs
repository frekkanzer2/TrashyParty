using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObstacle : MonoBehaviour
{
    private int NumberOfCollisions = 0;
    public int MaxNumberOfCollisions = 3;

    public void RegisterCollision()
    {
        this.NumberOfCollisions++;
        if (this.NumberOfCollisions >= this.MaxNumberOfCollisions)
            Destroy(this.gameObject);
    }
}
