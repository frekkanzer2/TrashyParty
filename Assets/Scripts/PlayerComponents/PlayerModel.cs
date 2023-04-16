using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{

    public GameObject ModelPrefab;

    void Start()
    {
        GameObject model = Instantiate(ModelPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        if (!model.CompareTag("PlayerModel")) throw new System.ArgumentException("Invalid model for player");
        model.transform.parent = this.transform.GetChild(1).transform;
        model.transform.localPosition = Vector3.zero;
        model.transform.localScale = new Vector3(1, 1, 1);
    }

}
