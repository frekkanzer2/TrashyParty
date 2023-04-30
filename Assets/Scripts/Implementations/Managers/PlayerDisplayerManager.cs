using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDisplayerManager : MonoBehaviour
{

    private bool isActive = false;

    public SpriteRenderer renderer;
    private ChoosePlayerManager.ColorMatch prevMatch = null;
    private IGamepad gamepad;
    public void SetSprite(ChoosePlayerManager.ColorMatch colorMatch)
    {
        if (colorMatch == null)
        {
            renderer.sprite = null;
            SetPreviousMatch(null);
            return;
        }
        this.renderer.sprite = colorMatch.prefab.GetComponent<SpriteRenderer>().sprite;
        SetPreviousMatch(colorMatch);
    }
    private void SetPreviousMatch(ChoosePlayerManager.ColorMatch match)
    {
        if (prevMatch != null)
            prevMatch.pointed = false;
        if (match != null)
            match.pointed = true;
        this.prevMatch = match;
    }
    public void SetGamepad(IGamepad gamepad)
    {
        this.gamepad = gamepad;
        if (this.gamepad == null)
            SetSprite(null);
        else
        {

        }
    }

#pragma warning disable CS8632
    public void SetActive(bool b, IGamepad? gamepad)
#pragma warning restore CS8632
    {
        this.isActive = b;
        if (b)
        {
            SetSprite(ChoosePlayerManager.Instance.GetNextFreeColor());
            SetGamepad(gamepad);
        } else
        {
            SetSprite(null);
            SetGamepad(null);
        }
    }

    public bool IsActive() => this.isActive;

    public int? GetGamepadId() => (gamepad is null) ? null : gamepad.Id;

    private void Start()
    {
        SetActive(false, null);
    }
}
