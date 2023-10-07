using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When instantiate a Laser object, follow these steps:
/// - Instantiate the gameobject with the implementation component
/// - Call initialize(...)
/// - Call OnSpawn()
/// </summary>
public class Laser : MonoBehaviour, ILaser
{
    #region Private attributes
    private int _team, _aliveTime, _rotationDirection;
    private bool _isAlive, _canRotate, _isInit, _timeEnded;
    private float _maxScale, _growingSpeedWhenAlive, _timer, _rotationSpeed;
    private IEnumerator _executingCoroutine;
    private const float SPAWN_SPEED = 0.005f;
    #endregion

    #region Laser methods' implementation
    public void Initialize(bool canRotate, int notAliveTimer, int aliveTimer, float maxScaleWhenAlive, float growingSpeedWhenAlive, float rotationSpeed)
    {
        this._team = -1;
        this._isAlive = false;
        this._timer = notAliveTimer;
        this._aliveTime = aliveTimer;
        this._canRotate = canRotate;
        this._maxScale = maxScaleWhenAlive;
        this._growingSpeedWhenAlive = growingSpeedWhenAlive;
        this._executingCoroutine = null;
        this._timeEnded = false;
        this._rotationSpeed = rotationSpeed;
        this._rotationDirection = Random.Range(1, 11) % 2;
        this._isInit = true;
    }

    public void OnPlayerCollision(IPlayer playerCollided)
    {
        if (_timer <= 0 || playerCollided.IsDead()) return;
        Debug.Log("LASER > Detected collision with player");
        if (!this._isAlive)
        {
            // Ray is default status
            this._timer = this._aliveTime;
            /// GETTING PLAYER TEAM ID
            int team = 1; // mocked
            CentralCollider.enabled = false;
            StartCoroutine(OnPlayerCollisionScaling(team));
        }
        else playerCollided.OnDeath();
    }

    public void OnSpawn()
    {
        Debug.Log("LASER > Spawn scaling");
        CentralCollider.enabled = true;
        StartCoroutine(OnSpawnScaling());
    }

    public void OnTimerEnds()
    {
        Debug.Log("LASER > Time ended");
        this._timeEnded = true;
        StartCoroutine(OnTimerEndsScaling());
    }

    public int Team => _team;

    public bool IsAlive => _isAlive;

    public bool CanRotate => _canRotate;

    public bool IsInitialized => _isInit;
    #endregion

    #region Scaling implementation
    private enum Scaling
    {
        POSITIVE,
        NEGATIVE
    }
    private Scaling GetScalingType(float startingScale, float destinationScale)
        => startingScale == destinationScale ? throw new System.ArgumentException("Arguments are equals -> Invalid scaling type")
        : (startingScale < destinationScale ? Scaling.POSITIVE : Scaling.NEGATIVE);
    private IEnumerator ScaleOverTime(float startingScale, float destinationScale, float growingSpeed)
    {
        if (growingSpeed <= 0) throw new System.ArgumentException("Growing speed is equals or less than zero -> Growing speed must be greater than zero");
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
        Debug.Log("LASER > Spawn scaling ended");
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
        Debug.Log("LASER > Collision scaling ended");
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
    }

    #endregion
}
