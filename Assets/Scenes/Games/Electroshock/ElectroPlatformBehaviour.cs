using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroPlatformBehaviour : MonoBehaviour
{

    public Sprite activated, deactivated;
    public new SpriteRenderer renderer;
    public GameObject thunderPrefab;

    void Start()
    {
        renderer.sprite = activated;
    }

    public void Execute(bool active, float time)
    {
        renderer.sprite = (active) ? activated : deactivated;
        if (active && !GameManager.Instance.IsGameEnded()) StartCoroutine(CreateThunder(time));
    }

    IEnumerator CreateThunder(float waitingTime)
    {
        yield return new WaitForSeconds(waitingTime);
        Instantiate(thunderPrefab, new Vector3(this.transform.position.x, this.transform.position.y + 1.88f, this.transform.position.z), Quaternion.identity);
    }

}
