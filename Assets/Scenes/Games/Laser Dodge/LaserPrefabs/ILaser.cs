using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILaser
{
    public void Initialize(LaserInitializationDto laserInitializationDto);
    public void OnSpawn();
    public void OnPlayerCollision(IPlayer playerCollided);
    public void OnTimerEnds();
    public void MoveOnPath(Vector2 nextPoint);

    public int Team { get; }
    public bool IsAlive { get; }
    public bool CanRotate { get; }
    public bool IsInitialized { get; }
    public List<Vector2> Path { get; set; }
}

public class LaserInitializationDto
{
    public bool CanRotate { get; set; }
    public int? RotationDirection { get; set; }
    public int NotAliveTimer { get; set; }
    public int AliveTimer { get; set; }
    public float MaxScaleWhenAlive { get; set; }
    public float GrowingSpeedWhenAlive { get; set; }
    public float RotationSpeed { get; set; }
    public float? MovementSpeed { get; set; }
    public List<Vector2> Path { get; set; }
}
