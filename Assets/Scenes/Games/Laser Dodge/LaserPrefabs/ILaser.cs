using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILaser
{
    public void Initialize(bool canRotate, int notAliveTimer, int aliveTimer, float maxScaleWhenAlive, float growingSpeedWhenAlive, float rotationSpeed);
    public void OnSpawn();
    public void OnPlayerCollision(IPlayer playerCollided);
    public void OnTimerEnds();
    public int Team { get; }
    public bool IsAlive { get; }
    public bool CanRotate { get; }
    public bool IsInitialized { get; }
}
