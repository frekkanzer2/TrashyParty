using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdyBullBehaviour : MonoBehaviour
{

    public SpriteRenderer Renderer;
    public Sprite calm;
    public Sprite angry;
    public float movementSpeed;
    private IPlayer target = null;
    private bool isMoving = false;
    private int iteration = 0;

    public void StartBehaviour()
    {
        StartCoroutine(RepeatBehaviour());
    }

    private IEnumerator RepeatBehaviour()
    {
        float waitingTime;
        if (iteration > 0 && iteration <= 22) waitingTime = 2 - MathfFunction.Exponential(1.4f, iteration) / 1000;
        else if (iteration == 0) waitingTime = 2;
        else waitingTime = 0.5f;
        yield return new WaitForSeconds(waitingTime);
        this.target = ChooseTarget();
        if (target == null) yield break;
        ChangeSprite(angry);
        StartCoroutine(MoveToTarget());
        while (isMoving)
            yield return null;
        ChangeSprite(calm);
        iteration++;
        StartCoroutine(RepeatBehaviour());
    }

    private IPlayer ChooseTarget()
    {
        if (GameManager.Instance.IsGameEnded()) return null;
        List<TeamDto> tDtos = GameManager.Instance.Teams.FindAll(t => t.GetAlivePlayers().Count > 0);
        List<IPlayer> alivePlayers = new();
        foreach (TeamDto tDto in tDtos) alivePlayers.Add(tDto.players[0]);
        alivePlayers.Sort(new PlayerDistanceComparer(this.transform));
        int indexToReturn = 0;
        int randomValue = Random.Range(1, 101);
        switch (alivePlayers.Count)
        {
            case 1: indexToReturn = 0; break;
            case 2:
                if (randomValue <= 65) indexToReturn = 0;
                else indexToReturn = 1;
                break;
            case 3:
                if (randomValue <= 50) indexToReturn = 0;
                else if (randomValue <= 70) indexToReturn = 1;
                else indexToReturn = 2;
                break;
            case 4:
                if (randomValue <= 40) indexToReturn = 0;
                else if (randomValue <= 65) indexToReturn = 1;
                else if (randomValue <= 85) indexToReturn = 2;
                else indexToReturn = 3;
                break;
            default:
                if (randomValue <= 35) indexToReturn = 0;
                else if (randomValue <= 60) indexToReturn = 1;
                else if (randomValue <= 80) indexToReturn = 2;
                else if (randomValue <= 95) indexToReturn = 3;
                else indexToReturn = 4;
                break;
        }
        return alivePlayers[indexToReturn];
    }

    private void ChangeSprite(Sprite s) => Renderer.sprite = s;
    private void ChangeSpriteOrientation(bool isMovingRight, bool isSpriteCalm)
    {
        if (isMovingRight) {
            if (isSpriteCalm) Renderer.flipX = false;
            else Renderer.flipX = true;
        } else
        {
            if (isSpriteCalm) Renderer.flipX = true;
            else Renderer.flipX = false;
        }
    }

    private IEnumerator MoveToTarget()
    {
        if (target.IsDead())
        {
            this.target = ChooseTarget();
            StartCoroutine(MoveToTarget());
            yield break;
        }
        isMoving = true;
        float effectiveSpeed = (iteration > 0) ? movementSpeed + MathfFunction.SquareRoot(iteration) * 3 : movementSpeed;
        Vector3 destination = ((this.target as MonoBehaviour).gameObject.transform.position);
        if (destination.x < this.transform.position.x)
            ChangeSpriteOrientation(false, false);
        else
            ChangeSpriteOrientation(true, false);
        while (Vector2.Distance(this.transform.position, destination) > 0.5f)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, destination, Time.deltaTime * effectiveSpeed);
            yield return null;
        }
        isMoving = false;
        if (destination.x < this.transform.position.x)
            ChangeSpriteOrientation(false, true);
        else
            ChangeSpriteOrientation(true, true);
    }

}