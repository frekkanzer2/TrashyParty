using System.Collections;
using UnityEngine;

public class TopDownPlayer : MonoBehaviour, IGamepadEventHandler, IPlayer
{

    #region Initialization

    public bool IsInitialized { get { return _isInit; } }

    protected bool _isInit = false;

    #endregion
    #region Components

    [SerializeField] protected new Rigidbody2D rigidbody;
    [SerializeField] protected Transform body;
    protected IGamepad gamepad;
    protected Sprite birdSprite, deathSprite;

    #endregion
    #region Class variables
    protected Vector2 movementData = Vector2.zero;
    protected bool isDead = false, canPlay = false, canWalk = true, canSprint = true, isSprinting = false;
    public bool CanSprintBySprintBar { get; set; }
    private float movementSpeed = Constants.PLAYER_MOVEMENT_SPEED, sprintSpeed = Constants.PLAYER_SPRINT_MOVEMENT_SPEED;
    private SprintBar sprintBar = null;

    #endregion

    public void ChangePlayerStats(float movementSpeed, float sprintSpeed)
    {
        this.movementSpeed = movementSpeed;
        this.sprintSpeed = sprintSpeed;
    }

    public void ChangeSprintBarRecoveryValue(float recoveryValue) => sprintBar.RecoveryValue = recoveryValue;
    public void ChangeSprintBarSprintConsumingValue(float sprintConsumingValue) => sprintBar.SprintConsumingValue = sprintConsumingValue;

    public Vector2 Speeds { get { return new Vector2(this.movementSpeed, this.sprintSpeed); } }

    public override bool Equals(object other)
    {
        GameObject toCheck = null;
        if (other is TopDownPlayer) toCheck = ((TopDownPlayer)other).gameObject;
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

        if (layer == Constants.LAYER_DEADZONE && !GameManager.Instance.IsGameEnded())
            this.OnDeath();
        if (collision.gameObject.CompareTag("Teleport") && !GameManager.Instance.IsGameEnded())
        {
            GameObject toTp = collision.gameObject.transform.GetChild(0).gameObject;
            if (toTp is null) throw new System.NullReferenceException("No teleport point set");
            this.transform.position = toTp.transform.position;
        }

    }

    private void Awake()
    {
        CanSprintBySprintBar = true;
        sprintBar = GetComponent<SprintBar>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Initialize", 0.5f);
        deathSprite = GetComponent<PlayerModel>().deathSprite;
        birdSprite = GetComponent<PlayerModel>().ModelPrefab.GetComponent<SpriteRenderer>().sprite;
        VariantStart();
    }

    protected virtual void VariantStart() { }

    // Update is called once per frame
    void Update()
    {
        if (!IsInitialized || isDead || !canPlay)
        {
            rigidbody.velocity = Vector2.zero;
            return;
        }
        if (gamepad == null) throw new System.NullReferenceException("No gamepad is connected");
        if (gamepad.IsConnected())
        {
            if (canSprint && CanSprintBySprintBar && gamepad.IsButtonPressed(IGamepad.Key.ActionButtonRight, IGamepad.PressureType.Continue))
                isSprinting = true;
            else isSprinting = false;
            ExecuteMovement();
            flipPlayerAnimation();
        }
        VariantUpdate();
    }

    protected void ExecuteMovement()
    {
        if (canWalk) movementData = gamepad.GetAnalogMovement(IGamepad.Analog.Left);
        else movementData = Vector2.zero;
    }

    protected virtual void VariantUpdate() { }

    void FixedUpdate()
    {
        if (!IsInitialized || isDead || !canPlay) return;
        float realMovementSpeed = movementSpeed;
        if (canSprint && isSprinting)
        {
            realMovementSpeed = sprintSpeed;
            sprintBar.UseSprint();
        }
        else if (canSprint && !isSprinting)
        {
            sprintBar.Recover();
        }
        rigidbody.velocity = new Vector2(movementData.x * realMovementSpeed, movementData.y * realMovementSpeed);
        rigidbody.velocity.Normalize();
        VariantFixedUpdate();
    }

    protected virtual void VariantFixedUpdate() { }

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

    public void SetCanWalk(bool b) => this.canWalk = b;
    public void SetCanSprint(bool b) => this.canSprint = b;

    public bool IsSprinting() => this.isSprinting;

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
        try
        {
            Log.Logger.Write($"Setting gamepad {pcaDto.ControllerId} to player {pcaDto.PlayerNumber} via association");
        } catch (System.NullReferenceException)
        {
            Debug.Log($"Setting gamepad {pcaDto.ControllerId} to player {pcaDto.PlayerNumber} via association");
        }
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
        SoundsManager.Instance.PlayPlayerSound(ISoundsManager.PlayerSoundType.Dead);
        isDead = true;
        body.GetChild(0).gameObject.GetComponent<Animator>().enabled = false;
        changeSprite(deathSprite);
        sprintBar.CanView = false;
        GameManager.Instance.OnPlayerDies();
    }

    public void OnSpawn()
    {
        isDead = false;
        changeSprite(birdSprite);
        body.GetChild(0).gameObject.GetComponent<Animator>().enabled = true;
        if (sprintBar.Value < sprintBar.MaxValue) sprintBar.CanView = true;
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
        if (controllerProvider.ControllerAssociation == null) throw new System.NullReferenceException($"No controller association fetched from gamepad provider on gameobject {this.gameObject.name}");
        SetGamepadByAssociation(controllerProvider.ControllerAssociation);
    }

    #endregion

    #region protected methods - behaviour

    protected void flipPlayerAnimation()
    {
        if (this.movementData.x > 0.2f)
            body.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = false;
        else if (this.movementData.x < -0.2f)
            body.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = true;
    }

    protected void changeSprite(Sprite s) => body.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = s;

    #endregion
}
