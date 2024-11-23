using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericBar : MonoBehaviour, IBar
{
    public bool CanView { get; set; }
    public float Value { get; set; }
    public float MaxValue { get; set; }
    public float MinValue { get; set; }

    public void Decrease(float variation)
    {
        if (Value == MinValue) return;
        Value -= variation;
        if (Value < MinValue) Value = MinValue;
        OnValueChanged();
    }

    public void Increase(float variation)
    {
        if (Value == MaxValue) return;
        Value += variation;
        if (Value > MaxValue) Value = MaxValue;
        OnValueChanged();
    }

    public abstract void OnMaxValueReached();
    public abstract void OnMinValueReached();

    public GameObject ComplessiveBar;
    public GameObject BarToManipulate;
    public float StartValue, MinimumValue, MaximumValue;

    // Start is called before the first frame update
    protected void GenericStart()
    {
        if (ComplessiveBar is null || BarToManipulate is null) throw new System.ArgumentNullException("Bar gameobjects are not initialized");
        Value = StartValue;
        MaxValue = MaximumValue;
        MinValue = MinimumValue;
        OnValueChanged();
    }

    protected void GenericUpdate()
    {
        if (!ComplessiveBar.activeSelf && CanView) ComplessiveBar.SetActive(true);
        else if (ComplessiveBar.activeSelf && !CanView) ComplessiveBar.SetActive(false);
    }

    void OnValueChanged()
    {
        float realValue = this.Value - this.MinValue;
        float realMaxValue = this.MaxValue - this.MinValue;
        float perc = 100 * realValue / realMaxValue;
        float horizontalScale = perc / 100;
        BarToManipulate.transform.localScale = new Vector3(horizontalScale, 1, 1);
        if (realValue == realMaxValue)
        {
            CanView = false;
            OnMaxValueReached();
        }
        else CanView = true;
        if (realValue == 0)
            OnMinValueReached();
    }

}
