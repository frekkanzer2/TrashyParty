using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject PrefabProjectile;
    public enum ShootDirection
    {
        Up, Down, Right, Left
    }
    public ShootDirection ProjectileDirection;
    public bool CanShoot => canShoot;
    private bool canShoot = true;
    public void Shoot() => StartCoroutine(PrepareForNextShot());
    IEnumerator PrepareForNextShot()
    {
        canShoot = false;
        GameObject projectile = GameObject.Instantiate(PrefabProjectile, this.transform.position, Quaternion.Euler(0, 0, ProjectileDirection switch {
            ShootDirection.Up => 0,
            ShootDirection.Down => 180,
            ShootDirection.Right => -90,
            ShootDirection.Left => 90,
            _ => 0
        }));
        Destroy(projectile, 10);
        projectile.GetComponent<EggShot>().Direction = ProjectileDirection switch
        {
            ShootDirection.Up => new() { x = 0, y = 1 },
            ShootDirection.Down => new() { x = 0, y = -1 },
            ShootDirection.Right => new() { x = 1, y = 0 },
            ShootDirection.Left => new() { x = -1, y = 0 },
            _ => new() { x = 0, y = 0 }
        };
        yield return new WaitForSeconds(1.5f);
        canShoot = true;
    }
}
