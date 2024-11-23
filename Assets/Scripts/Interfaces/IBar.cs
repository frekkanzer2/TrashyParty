public interface IBar
{
    public bool CanView { get; set; }
    public float Value { get; set; }
    public float MaxValue { get; set; }
    public float MinValue { get; set; }
    public void OnMaxValueReached();
    public void OnMinValueReached();
    public void Increase(float variation);
    public void Decrease(float variation);
}
