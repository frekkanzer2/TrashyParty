using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDisplayerManager : MonoBehaviour
{

    private bool isActive = false;
    private bool isConfirmed = false;

    public SpriteRenderer renderer;
    private ChoosePlayerManager.ColorMatch prevMatch = null;
    private IGamepad gamepad;

    public ChoosePlayerManager.ColorMatch GetActualColorMatch() => prevMatch;
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
    }

#pragma warning disable CS8632
    public void SetActive(bool b, IGamepad? gamepad)
#pragma warning restore CS8632
    {
        SetConfirm(false);
        this.isActive = b;
        if (b)
        {
            SetSprite(ChoosePlayerManager.Instance.GetFirstFreeColor());
            SetGamepad(gamepad);
        } else
        {
            SetSprite(null);
            SetGamepad(null);
        }
    }

    public void SetConfirm(bool b)
    {
        this.isConfirmed = b;
        this.renderer.color = (b) ? new Color(1, 1, 1, 1) : new Color(0.5f, 0.5f, 0.5f, 0.4f);
    }

    public bool IsActive() => this.isActive;
    public bool IsConfirmed() => this.isConfirmed;

    public int? GetGamepadId() => (gamepad is null) ? null : gamepad.Id;

    private void Start()
    {
        SetActive(false, null);
    }

    private void Update()
    {
        if (this.gamepad == null)
        {
            this.SetConfirm(false);
            this.SetActive(false, null);
            return;
        }
        if (this.gamepad.IsButtonPressed(IGamepad.Key.ActionButtonRight, IGamepad.PressureType.Single)) {
            if (this.IsConfirmed())
                this.SetConfirm(false);
            else this.SetActive(false, null);
        }
        if (this.IsConfirmed()) return;
        if (this.gamepad == null) // Double check because the previous IsButtonPressed check can remove the gamepad
        {
            this.SetConfirm(false);
            this.SetActive(false, null);
            return;
        }
        if (this.gamepad.IsButtonPressed(IGamepad.Key.MovementButtonRight, IGamepad.PressureType.Single) || this.gamepad.GetAnalogMovement(IGamepad.Analog.Left).x > 0)
        {
            ChoosePlayerManager.ColorMatch nextMatch = ChoosePlayerManager.Instance.GetNextFreeColor(prevMatch);
            if (nextMatch != null)
                SetSprite(nextMatch);
        }
        if (this.gamepad.IsButtonPressed(IGamepad.Key.MovementButtonLeft, IGamepad.PressureType.Single) || this.gamepad.GetAnalogMovement(IGamepad.Analog.Left).x < 0)
        {
            ChoosePlayerManager.ColorMatch nextMatch = ChoosePlayerManager.Instance.GetPreviousFreeColor(prevMatch);
            if (nextMatch != null)
                SetSprite(nextMatch);
        }

    }
}
