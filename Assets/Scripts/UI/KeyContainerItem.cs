using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyContainerItem : MonoBehaviour
{
    public void SetAction(string name, RuntimeAnimatorController icon)
    {
        this.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = icon;
        this.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = name;
    }
}
