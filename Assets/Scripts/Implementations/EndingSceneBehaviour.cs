using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingSceneBehaviour : MonoBehaviour
{

    public SpriteRenderer winnerSprite;
    public SpriteRenderer winnerText;
    public Sprite redWinner, blueWinner, greenWinner, orangeWinner, pinkWinner, skyWinner, yellowWinner, greyWinner;

    bool canPressStart = false;
    public GameObject continueLabel;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("TransitionManager").GetComponent<TransitionManager>().StartAnimationOnRoomOpens();
        RankingDto.Rank winnerRank = (RankingDto.Rank)AppSettings.Get("Winner");
        winnerSprite.sprite = winnerRank.PlayerSprite;
        winnerText.sprite =
            AppSettings.Get("COLOR_RAW_PLAYER" + winnerRank.PlayerId) switch {
                "RED" => redWinner,
                "BLUE" => blueWinner,
                "GREEN" => greenWinner,
                "ORANGE" => orangeWinner,
                "PINK" => pinkWinner,
                "SKY" => skyWinner,
                "YELLOW" => yellowWinner,
                "GREY" => greyWinner,
                _ => throw new System.NotImplementedException("Color not managed when displaying player color name")
            };
        StartCoroutine(EnableEsc());
    }

    IEnumerator EnableEsc()
    {
        yield return new WaitForSeconds(5.5f);
        continueLabel.SetActive(true);
        canPressStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canPressStart)
        {
            if (GamepadManager.Instance.IsButtonPressedFromAnyGamepad(IGamepad.Key.Start, IGamepad.PressureType.Single))
            {
                canPressStart = false;
                continueLabel.SetActive(false);
                GameObject.FindGameObjectWithTag("TransitionManager").GetComponent<TransitionManager>().StartAnimationOnRoomEnds();
                StartCoroutine(Restart());
            }
        }
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(2);
        AppSettings.Reset();
        SceneManager.LoadScene("CharacterSelection", LoadSceneMode.Single);
    }
}
