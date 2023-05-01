using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningSceneBehaviour : MonoBehaviour
{

    private bool changed = false;
    private bool canPress = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnableEsc());
    }

    IEnumerator EnableEsc()
    {
        yield return new WaitForSeconds(5f);
        canPress = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GamepadManager.Instance.IsButtonPressedFromAnyGamepad(IGamepad.Key.Start, IGamepad.PressureType.Single) && !changed && canPress)
        {
            changed = true;
            StartCoroutine(ExitFromRoom());
        }
    }

    IEnumerator ExitFromRoom()
    {
        GameObject.FindGameObjectWithTag("TransitionManager").GetComponent<TransitionManager>().StartAnimationOnRoomEnds();
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("CharacterSelection", LoadSceneMode.Single);
    }
}
