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
    #endregion

    public override bool Equals(object other)
    {
        GameObject toCheck = null;
        if (other is PlatformerPlayer) toCheck = ((PlatformerPlayer)other).gameObject;
        if (other is GameObject) toCheck = (GameObject)other;
        if (toCheck != null && ((GameObject)toCheck).GetComponent<IPlayer>() != null)
            return toCheck.GetComponent<PlayerModel>().ModelPrefab.name == this.gameObject.GetComponent<PlayerModel>().ModelPrefab.name;
        return false;
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

    private void ExecuteMovement()
    {
        if (canWalk) movementData = gamepad.GetAnalogMovement(IGamepad.Analog.Left);
        else movementData = Vector2.zero;
    }
    private void ExecuteJump()
    {
        if (gamepad.IsButtonPressed(IGamepad.Key.ActionButtonDown, IGamepad.PressureType.Single) && canJump && jumpCount < JumpLimit && !isWaitingRejump)
        {
            jumpCount++;
            SoundsManager.Instance.PlayPlayerSound(ISoundsManager.PlayerSoundType.Jump);
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, Constants.PLAYER_JUMPING_POWER);
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
        Debug.Log("Not waiting anymore");
    }

    protected virtual void VariantUpdate() { }

    void FixedUpdate()
    {
        if (!IsInitialized || isDead || !canPlay) return;
        rigidbody.velocity = new Vector2(movementData.x * Constants.PLAYER_MOVEMENT_SPEED, rigidbody.velocity.y);
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
        Debug.LogWarning($"Gamepad deconnected for player '{this.gameObject.name}'");
        OnDeath();
    }

    #endregion

    #region IPlayer implementation

    public void IgnoreCollisionsWithOtherPlayers(bool active)
    {
        Physics2D.IgnoreLayerCollision(30, 30, active);
        Physics2D.IgnoreLayerCollision(30, 31, active);
        Physics2D.IgnoreLayerCollision(31, 31, active);
    }
    public void ApplyForce(Vector2 force) {
        this.rigidbody.AddForce(force, ForceMode2D.Impulse);
    }
    public void SetGamepad(IGamepad gamepad)
    {
        this.gamepad = gamepad;
        if (this.gamepad == null)
        {
            Debug.LogError("Player doesn't have a gamepad!");
            OnGamepadDeconnected();
        }
        else
        {
            Debug.Log("Gamepad loaded");
            this.gamepad.SetGamepadEventHandler(this);
        }
    }

    public void SetGamepadByAssociation(PlayerControllerAssociationDto pcaDto)
    {
        Debug.Log($"Setting gamepad {pcaDto.ControllerId} to player {pcaDto.PlayerNumber} via association");
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

    public void OnDeath()
    {
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
    public Sprite GetWinSprite() => this.birdSprite;

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
        if (this.movementData.x > 0.2f)
            body.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = false;
        else if (this.movementData.x < -0.2f)
            body.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = true;
    }

    protected void changeSprite(Sprite s) => body.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = s;

    #endregion

}
