using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// When instantiate a Laser object, follow these steps:
/// - Instantiate the gameobject with the implementation component
/// - Call initialize(...)
/// - Call OnSpawn()
/// </summary>
public class Laser : MonoBehaviour, ILaser
{
    #region Private attributes
    private int _team, _aliveTime, _rotationDirection, _pathIndex;
    private bool _isAlive, _canRotate, _isInit, _timeEnded;
    private float _maxScale, _growingSpeedWhenAlive, _timer, _rotationSpeed, _movementSpeed;
    private IEnumerator _executingCoroutine;
    private List<Vector2> _path;
    private const float SPAWN_SPEED = 0.005f;
    #endregion

    #region Laser methods' implementation
    public void Initialize(LaserInitializationDto initDto)
    {
        this._team = -1;
        this._isAlive = false;
        this._timer = initDto.NotAliveTimer;
        this._aliveTime = initDto.AliveTimer;
        this._canRotate = initDto.CanRotate;
        this._maxScale = initDto.MaxScaleWhenAlive;
        this._growingSpeedWhenAlive = initDto.GrowingSpeedWhenAlive;
        this._executingCoroutine = null;
        this._timeEnded = false;
        this._rotationSpeed = initDto.RotationSpeed;
        this._rotationDirection = initDto.RotationDirection ?? Random.Range(1, 11) % 2;
        this._pathIndex = 0;
        this.Path = initDto.Path.IsNullOrEmpty() ? null : initDto.Path;
        this._movementSpeed = initDto.MovementSpeed ?? 0;
        this._isInit = true;
    }

    public void OnPlayerCollision(IPlayer playerCollided)
    {
        if (_timer <= 0 || playerCollided.IsDead()) return;
        Debug.Log("LASER > Detected collision with player");
        if (!this._isAlive)
        {
            this._timer = this._aliveTime;
            int team = 1; // mocked
            CentralCollider.enabled = false;
            StartCoroutine(OnPlayerCollisionScaling(team));
        }
        else playerCollided.OnDeath();
    }

    public void OnSpawn()
    {
        CentralCollider.enabled = true;
        StartCoroutine(OnSpawnScaling());
    }

    public void OnTimerEnds()
    {
        this._timeEnded = true;
        StartCoroutine(OnTimerEndsScaling());
    }

    public void MoveOnPath(Vector2 nextPoint)
    {
        Vector2 direction = nextPoint - this.transform.position.ToVector2();
        if (direction.magnitude <= 0.1f) _pathIndex = _pathIndex + 1 == Path.Count ? 0 : _pathIndex + 1;
        else this.transform.position = Vector2.MoveTowards(this.transform.position, nextPoint, _movementSpeed * Time.deltaTime);
    }

    public int Team => _team;

    public bool IsAlive => _isAlive;

    public bool CanRotate => _canRotate;

    public bool IsInitialized => _isInit;

    public List<Vector2> Path { 
        get => _path;
        set {
            _path = value;
            if (value != null)
            {
                _path.Insert(0, this.transform.position.ToVector2()); // add as first element the actual position
                _pathIndex = 1;
            }
        }
    }
    #endregion

    #region Scaling implementation
    private enum Scaling
    {
        POSITIVE,
        NEGATIVE
    }
    private Scaling GetScalingType(float startingScale, float destinationScale)
        => startingScale == destinationScale ? throw new System.ArgumentException($"Arguments are equals (scaling value: {startingScale}) -> Invalid scaling type")
        : (startingScale < destinationScale ? Scaling.POSITIVE : Scaling.NEGATIVE);
    private IEnumerator ScaleOverTime(float startingScale, float destinationScale, float growingSpeed)
    {
        if (growingSpeed <= 0) throw new System.ArgumentException($"Growing speed is equals or less than zero ({growingSpeed}) -> Growing speed must be greater than zero");
        Scaling scalingType = GetScalingType(startingScale, destinationScale);
        while (scalingType == Scaling.POSITIVE ? startingScale < destinationScale : startingScale > destinationScale)
        {
            foreach (GameObject ray in Rays)
            {
                ray.transform.localScale = ray.transform.localScale.Variation(scalingType == Scaling.POSITIVE ? growingSpeed : growingSpeed * (-1), 0, 0);
                if ((scalingType == Scaling.POSITIVE) ? ray.transform.localScale.x > destinationScale : ray.transform.localScale.x < destinationScale)
                    ray.transform.localScale = new(destinationScale, 1, 1);
                startingScale = ray.transform.localScale.x;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    private IEnumerator OnSpawnScaling()
    {
        yield return new WaitForSeconds(0.001f);
        _executingCoroutine = ScaleOverTime(0, 1, SPAWN_SPEED);
        StartCoroutine(_executingCoroutine);

    }
    private IEnumerator OnPlayerCollisionScaling(int team)
    {
        yield return new WaitForSeconds(0.001f);
        StopCoroutine(_executingCoroutine);
        _executingCoroutine = ScaleOverTime(Rays[0].transform.localScale.x, 0, _growingSpeedWhenAlive);
        yield return StartCoroutine(_executingCoroutine);
        Sprite spriteToApply = GetRaySprite(team);
        foreach (GameObject ray in Rays)
        {
            ray.transform.GetChild(0).GetComponent<CapsuleCollider2D>().enabled = true;
            ray.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = spriteToApply;
        }
        _isAlive = true;
        _executingCoroutine = ScaleOverTime(0, _maxScale, _growingSpeedWhenAlive);
        StartCoroutine(_executingCoroutine);
    }
    private IEnumerator OnTimerEndsScaling()
    {
        _executingCoroutine = ScaleOverTime(_maxScale, 0, _growingSpeedWhenAlive);
        yield return StartCoroutine(_executingCoroutine);
        Destroy(this.gameObject);
    }
    #endregion

    #region MonoBehaviour

    public List<GameObject> Rays;
    public GameObject Center;
    public Sprite[] TeamRaySprites;
    public CircleCollider2D CentralCollider;

    private Sprite GetRaySprite(int team) => TeamRaySprites[team - 1];

    void Awake()
    {
        _isInit = false;
        CentralCollider.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        Center.SetActive(false);
        foreach (GameObject ray in Rays)
        {
            ray.transform.localScale = new(0, 1, 1);
            ray.transform.GetChild(0).GetComponent<CapsuleCollider2D>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsInitialized || _timeEnded) return;
        _timer -= Time.deltaTime;
        if (_timer <= 0) OnTimerEnds();
    }

    private void FixedUpdate()
    {
        if (!IsInitialized) return;
        if (CanRotate) transform.Rotate(0, 0, _rotationDirection == 0 ? _rotationSpeed : _rotationSpeed * -1);
        if (!Path.IsNullOrEmpty() && !_timeEnded) MoveOnPath(Path[_pathIndex]);
    }

    #endregion
}
