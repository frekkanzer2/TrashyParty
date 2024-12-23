using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerPlayer : MonoBehaviour, IGamepadEventHandler, IPlayer
{

    #region Initialization

    public bool IsInitialized { get { return _isInit; } }

    protected bool _isInit = false;

    #endregion
    #region Components

    [SerializeField] protected new Rigidbody2D rigidbody;
    [SerializeField] protected Transform foots;
    [SerializeField] protected Transform body;
    [SerializeField] protected Transform head;
    [SerializeField] protected GameObject confusionEffect;
    public GameObject GetHead() => head.gameObject;
    public GameObject GetFoots() => foots.gameObject;
    [SerializeField] protected LayerMask groundTag;
    protected IGamepad gamepad;
    protected Sprite birdSprite, deathSprite;

    #endregion
    #region Class variables
    public int JumpLimit = 1;
    private int jumpCount = 0;
    protected Vector2 movementData = Vector2.zero;
    protected bool isDead = false, canPlay = false, canJump = true, canWalk = true;
    protected float originalGravity;
    private bool isWaitingRejump = false;
    private bool canKillOtherBirds = false;
    private bool _isConfused = false;
    public bool IsConfused { get { return _isConfused; } }
    protected bool canConfuseOtherBirds = false;
    private Vector2 lastPositionBeforeConfusing = Vector2.zero;
    private float movementSpeed = Constants.PLAYER_MOVEMENT_SPEED, jumpingPower = Constants.PLAYER_JUMPING_POWER;
    #endregion

    public void ChangePlayerStats(float movementSpeed, float jumpingPower)
    {
        this.movementSpeed = movementSpeed;
        this.jumpingPower = jumpingPower;
    }

    public override bool Equals(object other)
    {
        GameObject toCheck = null;
        if (other is PlatformerPlayer) toCheck = ((PlatformerPlayer)other).gameObject;
        if (other is GameObject) toCheck = (GameObject)other;
        if (toCheck != null && ((GameObject)toCheck).GetComponent<IPlayer>() != null)
            return toCheck.GetComponent<IPlayer>().CheckName(this.gameObject.GetComponent<IPlayer>().GetName());
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerEnterOverridable(collision);
    }

    protected virtual void OnTriggerEnterOverridable(Collider2D collision)
    {

        if (this.isDead) return;

        int layer = collision.gameObject.layer;
        Transform collisionTransform = collision.gameObject.transform;

        if (layer == Constants.LAYER_DEADZONE && !GameManager.Instance.IsGameEnded())
            this.OnDeath();
        if (collision.gameObject.CompareTag("Repels") && !GameManager.Instance.IsGameEnded())
        {
            this.ApplyForce(new Vector2(0, 100));
            this.jumpCount = 0;
        }
        if (collision.gameObject.CompareTag("Teleport") && !GameManager.Instance.IsGameEnded())
        {
            GameObject toTp = collision.gameObject.transform.GetChild(0).gameObject;
            if (toTp is null) throw new System.NullReferenceException("No teleport point set");
            this.transform.position = toTp.transform.position;
        }

        PlatformerPlayer collidedPlayer;
        try
        {
            collidedPlayer = collisionTransform.parent.gameObject.GetComponent<PlatformerPlayer>();
        }
        catch (System.NullReferenceException)
        {
            return;
        }
        if (layer == Constants.LAYER_PLAYERHEAD && this.transform.position.y >= collisionTransform.position.y && !GameManager.Instance.IsGameEnded())
        {
            if (collidedPlayer != null && !collidedPlayer.isDead && GameManager.Instance.IsGameStarted())
            {
                if (this.canKillOtherBirds) collidedPlayer.OnDeath();
                else if (this.canConfuseOtherBirds && !this.IsConfused && !collidedPlayer.IsConfused) collidedPlayer.SetConfusion(true);
                jumpCount = 0;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Initialize", 0.5f);
        deathSprite = GetComponent<PlayerModel>().deathSprite;
        birdSprite = GetComponent<PlayerModel>().ModelPrefab.GetComponent<SpriteRenderer>().sprite;
        originalGravity = rigidbody.gravityScale;
        VariantStart();
    }

    protected virtual void VariantStart() { }

    // Update is called once per frame
    void Update()
    {
        if (_isConfused)
            this.transform.position = new Vector3(this.lastPositionBeforeConfusing.x, this.transform.position.y, 0);
        if (foots.gameObject.activeInHierarchy == true && isDead && isGrounded())
        {
            rigidbody.gravityScale = 0;
            foots.gameObject.SetActive(false);
        }
        if (!IsInitialized || isDead || !canPlay)
        {
            rigidbody.velocity = Vector2.zero;
            return;
        }
        if (gamepad == null) throw new System.NullReferenceException("No gamepad is connected");
        if (gamepad.IsConnected())
        {
            ExecuteMovement();
            ExecuteJump();
            flipPlayerAnimation();
        }
        VariantUpdate();
    }

    protected void ExecuteMovement()
    {
        try
        {
            if (canWalk) movementData = gamepad.GetAnalogMovement(IGamepad.Analog.Left);
            else movementData = Vector2.zero;
        } catch (System.InvalidOperationException)
        {
            Log.Logger.Write(ILogManager.Level.Important, $"Gamepad disconnected from player {this.GetName()}, cannot retrieve movement data.");
            OnGamepadDeconnected();
            movementData = Vector2.zero;
        }
    }
    protected void ExecuteJump()
    {
        if (canJump && !_isConfused)
            if (gamepad.IsButtonPressed(IGamepad.Key.ActionButtonDown, IGamepad.PressureType.Single) && jumpCount < JumpLimit && !isWaitingRejump)
            {
                jumpCount++;
                SoundsManager.Instance.PlayPlayerSound(ISoundsManager.PlayerSoundType.Jump);
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpingPower);
                StartCoroutine(StartWaitingRejump());
            }
        if (isGrounded() && !isWaitingRejump)
            jumpCount = 0;
    }

    private IEnumerator StartWaitingRejump()
    {
        isWaitingRejump = true;
        yield return new WaitForSeconds(0.05f);
        isWaitingRejump = false;
    }

    protected virtual void VariantUpdate() { }

    void FixedUpdate()
    {
        if (!IsInitialized || isDead || !canPlay || _isConfused) return;
        rigidbody.velocity = new Vector2(movementData.x * movementSpeed, rigidbody.velocity.y);
        VariantFixedUpdate();
    }

    protected virtual void VariantFixedUpdate() { }

    public void SetCanKillOtherBirds(bool canKill) => this.canKillOtherBirds = canKill;
    public void SetCanConfuseOtherBirds(bool canConfuse) => this.canConfuseOtherBirds = canConfuse;

    public void SetConfusion(bool active)
    {
        if (this.isDead || !this.canConfuseOtherBirds) return;
        lastPositionBeforeConfusing = this.transform.position;
        confusionEffect.SetActive(active);
        body.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipY = active;
        _isConfused = active;
        if (active)
        {
            rigidbody.velocity = Vector2.zero;
            StartCoroutine(TimerConfusion());
        }
    }

    IEnumerator TimerConfusion()
    {
        yield return new WaitForSeconds(2);
        SetConfusion(false);
    }

    #region IGamepadEventHandler implementation

    public void OnButtonContinuePression(IGamepad.Key key)
    {

    }

    public void OnButtonSinglePression(IGamepad.Key key)
    {

    }

    public void OnGamepadConnected()
    {

    }

    public void OnGamepadDeconnected()
    {
        Log.Logger.Write(ILogManager.Level.Important, "Gamepad deconnected");
        //OnDeath();
    }

    #endregion

    #region IPlayer implementation

    protected int? _team = null;
    public int? Team { get => _team; set => _team = value; }
    public string GetName()
    {
        string _name = this.body.GetChild(0).gameObject.name;
        if (_name.ToLower().Contains("red")) return "RED";
        if (_name.ToLower().Contains("blue")) return "BLUE";
        if (_name.ToLower().Contains("green")) return "GREEN";
        if (_name.ToLower().Contains("orange")) return "ORANGE";
        if (_name.ToLower().Contains("pink")) return "PINK";
        if (_name.ToLower().Contains("yellow")) return "YELLOW";
        if (_name.ToLower().Contains("sky")) return "SKY";
        if (_name.ToLower().Contains("grey")) return "GREY";
        return null;
    }

    public bool CheckName(string name)
    {
        string _name = GetName();
        return name.ToUpper().Equals(_name.ToUpper());
    }

    public Vector3 RespawnPosition { get; set; }
    public int Id { get; set; }

    public void IgnoreCollisionsWithOtherPlayers(bool active)
    {
        Physics2D.IgnoreLayerCollision(30, 30, active);
        Physics2D.IgnoreLayerCollision(30, 31, active);
        Physics2D.IgnoreLayerCollision(31, 31, active);
    }
    public void ApplyForce(Vector2 force) {
        this.rigidbody.AddForce(force, ForceMode2D.Impulse);
    }
    private bool canApplyForce = true;
    public void ApplyForce(Vector2 force, float countdownInSeconds)
    {
        if (canApplyForce)
        {
            canApplyForce = false;
            ApplyForce(force);
            StartCoroutine(ForceWaiting(countdownInSeconds));
        }
    }
    IEnumerator ForceWaiting(float t)
    {
        yield return new WaitForSeconds(t);
        canApplyForce = true;
    }
    public void SetJumpLimit(int limit) {
        this.JumpLimit = limit;
    }
    public void SetCanWalk(bool b) => this.canWalk = b;
    public void SetCanJump(bool b) => this.canJump = b;
    public void SetGamepad(IGamepad gamepad)
    {
        this.gamepad = gamepad;
        if (this.gamepad == null)
        {
            Log.Logger.Write(ILogManager.Level.Warning, "Player doesn't have a gamepad!");
            OnGamepadDeconnected();
        }
        else
        {
            this.gamepad.SetGamepadEventHandler(this);
        }
    }

    public void SetGamepadByAssociation(PlayerControllerAssociationDto pcaDto)
    {
        Log.Logger.Write($"Setting gamepad {pcaDto.ControllerId} to player {pcaDto.PlayerNumber} via association");
        SetGamepad(GamepadManager.Instance.GetGamepadByAssociation(pcaDto));
    }

    public void Initialize()
    {
        if (IsInitialized) return;
        _isInit = true;
        FetchControllerFromProvider();
    }

    public void OnGameStarts()
    {

    }

    public void OnGameEnds()
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnDeath()
    {
        if (isDead) return;
        SetConfusion(false);
        SoundsManager.Instance.PlayPlayerSound(ISoundsManager.PlayerSoundType.Dead);
        isDead = true;
        body.GetChild(0).gameObject.GetComponent<Animator>().enabled = false;
        changeSprite(deathSprite);
        head.gameObject.SetActive(false);
        GameManager.Instance.OnPlayerDies();
    }

    public void OnSpawn()
    {
        isDead = false;
        SetConfusion(false);
        changeSprite(birdSprite);
        body.GetChild(0).gameObject.GetComponent<Animator>().enabled = true;
        head.gameObject.SetActive(true);
        foots.gameObject.SetActive(true);
        rigidbody.gravityScale = originalGravity;
        GameManager.Instance.OnPlayerSpawns();
    }

    public bool IsDead() => isDead;

    public void SetAsReady() => canPlay = true;
    public void SetAsNotReady() => canPlay = false;
    public Sprite GetBirdSprite() => this.birdSprite;

    #endregion

    #region protected methods - resource management

    protected void FetchControllerFromProvider()
    {
        IControllerProvider controllerProvider = GetComponent<IControllerProvider>();
        if (controllerProvider == null) throw new System.NullReferenceException($"No ControllerProvider component found on gameobject {this.gameObject.name}");
        SetGamepadByAssociation(controllerProvider.ControllerAssociation);
    }

    #endregion

    #region protected methods - behaviour

    protected bool isGrounded() => Physics2D.OverlapCircle(foots.position, 0.2f, groundTag);

    protected void flipPlayerAnimation()
    {
        if (_isConfused) return;
        if (this.movementData.x > 0.2f)
            body.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = false;
        else if (this.movementData.x < -0.2f)
            body.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = true;
    }

    protected void changeSprite(Sprite s) => body.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = s;

    #endregion

}
