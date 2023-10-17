using SixLabors.ImageSharp.Formats.Jpeg;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class LaserGeneratorDataWave
{
    public List<LaserGeneratorDataItem> Lasers;
    public float TimeToWaitForNextGeneration;
    public LaserGeneratorDataWave()
    {
        Lasers = new();
        TimeToWaitForNextGeneration = 10;
    }
    public LaserGeneratorDataWave(float timeToWaitForNextGeneration)
    {
        Lasers = new();
        TimeToWaitForNextGeneration = timeToWaitForNextGeneration;
    }
}

public class LaserGeneratorDataItem
{
    // between 2 and 5
    public int LaserRays;
    public Vector2 SpawnPosition;
    public List<Vector2> Path;
    public float MovementSpeed;
    public float RotationSpeed;
    public bool CanRotate;
    // 0 or 1
    public int? RotationDirection;
    // between 3 and 20
    public int Scale;
}

public class LaserGenerator : MonoBehaviour
{
    public List<LaserGeneratorDataWave> Waves;
    public float TimerDifferenceForNewCycle;
    public float StartingGrowingSpeed;
    public GameObject PrefabLaser2Rays;
    public GameObject PrefabLaser3Rays;
    public GameObject PrefabLaser4Rays;
    public GameObject PrefabLaser5Rays;

    private int _wave = 0, _waveCycle = 0;
    private bool _init = false;
    private float _timer = 0, _timerDifferenceBetweenWaveCycles = 0;

    private void OnTimerEnds()
    {
        Debug.Log($"Timer ended");
        if (_wave + 1 == Waves.Count)
        {
            Debug.Log($"New cycle: {_waveCycle} with wave {_wave}");
            _wave = 0;
            _waveCycle++;
            _timerDifferenceBetweenWaveCycles = TimerDifferenceForNewCycle * _waveCycle;
            if (_timerDifferenceBetweenWaveCycles > 3) _timerDifferenceBetweenWaveCycles = 2;
        }
        else
        {
            _wave++;
            Debug.Log($"New wave: {_wave}");
        }
        _timer = Waves[_wave].TimeToWaitForNextGeneration - _timerDifferenceBetweenWaveCycles;
        Debug.Log($"Timer set to {_timer}s");
        OnWaveStarts();
    }
    private void OnWaveStarts()
    {
        List<LaserGeneratorDataItem> generationData = this.Waves[_wave].Lasers;
        foreach(LaserGeneratorDataItem laser in generationData)
        {
            GameObject toSpawn = null;
            switch(laser.LaserRays)
            {
                case 2:
                    toSpawn = PrefabLaser2Rays;
                    break;
                case 3:
                    toSpawn = PrefabLaser3Rays;
                    break;
                case 4:
                    toSpawn = PrefabLaser4Rays;
                    break;
                case 5:
                    toSpawn = PrefabLaser5Rays;
                    break;
                default:
                    throw new System.ArgumentException("Illegal number of rays during generation");
            }
            GameObject spawnedGameObject = Instantiate(toSpawn, laser.SpawnPosition, Quaternion.identity);
            ILaser spawnedLaser = spawnedGameObject.GetComponent<ILaser>();
            spawnedLaser.Initialize(new()
            {
                CanRotate = laser.CanRotate,
                RotationDirection = laser.CanRotate && laser.RotationDirection != null ? laser.RotationDirection : null,
                NotAliveTimer = (int) this.Waves[_wave].TimeToWaitForNextGeneration,
                AliveTimer = (int)(this.Waves[_wave].TimeToWaitForNextGeneration * 1.5f) + (_waveCycle <= 5 ? _waveCycle : 5) * 2,
                MaxScaleWhenAlive = laser.Scale + (_waveCycle <= 20 ? _waveCycle : 20),
                GrowingSpeedWhenAlive = StartingGrowingSpeed + (_waveCycle <= 10 ? _waveCycle : 10) * 0.005f,
                RotationSpeed = laser.RotationSpeed + (_waveCycle <= 10 ? _waveCycle : 10) * 0.025f,
                MovementSpeed = laser.MovementSpeed + (_waveCycle <= 5 ? _waveCycle : 5) * 0.5f,
                Path = laser.Path
            });
            spawnedLaser.OnSpawn();
            Debug.Log($"Lasers generated");
        }
    }

    public void StartWaves()
    {
        Debug.Log($"Game started");
        _timer = Waves[0].TimeToWaitForNextGeneration;
        Debug.Log($"Starting timer: {_timer}");
        _init = true;
        OnWaveStarts();
    }

    private void Start()
    {
        Populate();
    }

    private void Update()
    {
        if (!_init) return;
        _timer -= Time.deltaTime;
        if (_timer <= 0) OnTimerEnds();
    }


    #region Laser generation
    private void Populate()
    {
        this.Waves = new();
        this.Waves.Add(GenerateLasersForWave1());
        this.Waves.Add(GenerateLasersForWave2());
        this.Waves.Add(GenerateLasersForWave3());
        this.Waves.Add(GenerateLasersForWave4());
    }
    private LaserGeneratorDataWave GenerateLasersForWave1()
    {
        LaserGeneratorDataWave waveObj = new();
        waveObj.Lasers.Add(new()
        {
            CanRotate = true,
            RotationSpeed = 1f,
            LaserRays = 3,
            RotationDirection = 0,
            Scale = 2,
            SpawnPosition = new(20, 12)
        });
        waveObj.Lasers.Add(new()
        {
            CanRotate = true,
            RotationSpeed = 1f,
            LaserRays = 3,
            RotationDirection = 0,
            Scale = 2,
            SpawnPosition = new(0, 12)
        });
        waveObj.Lasers.Add(new()
        {
            CanRotate = true,
            RotationSpeed = 1f,
            LaserRays = 3,
            RotationDirection = 0,
            Scale = 2,
            SpawnPosition = new(-20, 12)
        });
        waveObj.Lasers.Add(new()
        {
            CanRotate = true,
            RotationSpeed = 1f,
            LaserRays = 3,
            RotationDirection = 1,
            Scale = 2,
            SpawnPosition = new(20, -12)
        });
        waveObj.Lasers.Add(new()
        {
            CanRotate = true,
            RotationSpeed = 1f,
            LaserRays = 3,
            RotationDirection = 1,
            Scale = 2,
            SpawnPosition = new(0, -12)
        });
        waveObj.Lasers.Add(new()
        {
            CanRotate = true,
            RotationSpeed = 1f,
            LaserRays = 3,
            RotationDirection = 1,
            Scale = 2,
            SpawnPosition = new(-20, -12)
        });
        return waveObj;
    }
    private LaserGeneratorDataWave GenerateLasersForWave2()
    {
        LaserGeneratorDataWave waveObj = new();
        List<Vector2> path = new();
        path.Add(new(-25, 10));
        path.Add(new(-25, -10));
        path.Add(new(25, -10));
        waveObj.Lasers.Add(new()
        {
            CanRotate = true,
            RotationSpeed = 0.7f,
            LaserRays = 3,
            RotationDirection = 0,
            Scale = 2,
            SpawnPosition = new(25, 10),
            Path = path,
            MovementSpeed = 3
        });
        path = new();
        path.Add(new(-25, -10));
        path.Add(new(25, -10));
        path.Add(new(25, 10));
        waveObj.Lasers.Add(new()
        {
            CanRotate = true,
            RotationSpeed = 0.7f,
            LaserRays = 3,
            RotationDirection = 0,
            Scale = 2,
            SpawnPosition = new(-25, 10),
            Path = path,
            MovementSpeed = 3
        });
        path = new();
        path.Add(new(25, -10));
        path.Add(new(25, 10));
        path.Add(new(-25, 10));
        waveObj.Lasers.Add(new()
        {
            CanRotate = true,
            RotationSpeed = 0.7f,
            LaserRays = 3,
            RotationDirection = 0,
            Scale = 2,
            SpawnPosition = new(-25, -10),
            Path = path,
            MovementSpeed = 3
        });
        path = new();
        path.Add(new(25, 10));
        path.Add(new(-25, 10));
        path.Add(new(-25, -10));
        waveObj.Lasers.Add(new()
        {
            CanRotate = true,
            RotationSpeed = 0.7f,
            LaserRays = 3,
            RotationDirection = 0,
            Scale = 2,
            SpawnPosition = new(25, -10),
            Path = path,
            MovementSpeed = 3
        });
        waveObj.Lasers.Add(new()
        {
            CanRotate = true,
            RotationSpeed = 0.5f,
            LaserRays = 4,
            RotationDirection = 1,
            Scale = 4,
            SpawnPosition = new(0, 0)
        });
        return waveObj;
    }
    private LaserGeneratorDataWave GenerateLasersForWave3()
    {
        LaserGeneratorDataWave waveObj = new();
        waveObj.Lasers.Add(new()
        {
            CanRotate = false,
            LaserRays = 4,
            Scale = 3,
            SpawnPosition = new(18, 0)
        });
        waveObj.Lasers.Add(new()
        {
            CanRotate = false,
            LaserRays = 4,
            Scale = 3,
            SpawnPosition = new(-18, 0)
        });
        waveObj.Lasers.Add(new()
        {
            CanRotate = false,
            LaserRays = 4,
            Scale = 3,
            SpawnPosition = new(0, 9)
        });
        waveObj.Lasers.Add(new()
        {
            CanRotate = false,
            LaserRays = 4,
            Scale = 3,
            SpawnPosition = new(0, -9)
        });
        return waveObj;
    }
    private LaserGeneratorDataWave GenerateLasersForWave4()
    {
        LaserGeneratorDataWave waveObj = new();
        List<Vector2> path = new();
        path.Add(new(14, 16));
        waveObj.Lasers.Add(new()
        {
            CanRotate = false,
            LaserRays = 2,
            Scale = 6,
            SpawnPosition = new(14, -16),
            Path = path,
            MovementSpeed = 2
        });
        path = new();
        path.Add(new(-14, -16));
        waveObj.Lasers.Add(new()
        {
            CanRotate = false,
            LaserRays = 2,
            Scale = 6,
            SpawnPosition = new(-14, 16),
            Path = path,
            MovementSpeed = 2
        });
        path = new();
        path.Add(new(-30, -14));
        waveObj.Lasers.Add(new()
        {
            CanRotate = true,
            RotationDirection = 0,
            RotationSpeed = 1.2f,
            LaserRays = 3,
            Scale = 4,
            SpawnPosition = new(30, -14),
            Path = path,
            MovementSpeed = 3.25f
        });
        path = new();
        path.Add(new(30, -5));
        waveObj.Lasers.Add(new()
        {
            CanRotate = true,
            RotationDirection = 1,
            RotationSpeed = 1.2f,
            LaserRays = 3,
            Scale = 3,
            SpawnPosition = new(-30, -5),
            Path = path,
            MovementSpeed = 3.25f
        });
        path = new();
        path.Add(new(30, 14));
        waveObj.Lasers.Add(new()
        {
            CanRotate = true,
            RotationDirection = 1,
            RotationSpeed = 1.2f,
            LaserRays = 3,
            Scale = 4,
            SpawnPosition = new(-30, 14),
            Path = path,
            MovementSpeed = 3.25f
        });

        path = new();
        path.Add(new(-30, 5));
        waveObj.Lasers.Add(new()
        {
            CanRotate = true,
            RotationDirection = 0,
            RotationSpeed = 1.2f,
            LaserRays = 3,
            Scale = 3,
            SpawnPosition = new(30, 5),
            Path = path,
            MovementSpeed = 3.25f
        });
        return waveObj;
    }
    #endregion

}
