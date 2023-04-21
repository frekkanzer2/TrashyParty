using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerPlayer : MonoBehaviour, IGamepadEventHandler, IPlayer
{

    #region Initialization

    public bool IsInitialized { get { return _isInit; } }
    private bool _isInit = false;

    #endregion
    #region Components

    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private Transform foots;
    [SerializeField] private Transform body;
    [SerializeField] private Transform head;
    public GameObject GetHead() => head.gameObject;
    public GameObject GetFoots() => foots.gameObject;
    [SerializeField] private LayerMask groundTag;
    private IGamepad gamepad;
    private Sprite birdSprite, deathSprite;

    #endregion
    #region Class variables
    private Vector2 movementData = Vector2.zero;
    private bool isDead = false, canPlay = false;
    private float originalGravity;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Initialize", 0.5f);
        deathSprite = GetComponent<PlayerModel>().deathSprite;
        birdSprite = GetComponent<PlayerModel>().ModelPrefab.GetComponent<SpriteRenderer>().sprite;
        originalGravity = rigidbody.gravityScale;
    }

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
            movementData = gamepad.GetAnalogMovement(IGamepad.Analog.Left);
            if (isGrounded() && gamepad.IsButtonPressed(IGamepad.Key.ActionButtonDown, IGamepad.PressureType.Continue))
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, Constants.PLAYER_JUMPING_POWER);
            }
            flipPlayerAnimation();
        }
    }

    void FixedUpdate()
    {
        if (!IsInitialized || isDead || !canPlay) return;
        rigidbody.velocity = new Vector2(movementData.x * Constants.PLAYER_MOVEMENT_SPEED, rigidbody.velocity.y);
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
        Debug.LogWarning($"Gamepad deconnected for player '{this.gameObject.name}'");
        OnDeath();
    }

    #endregion

    #region IPlayer implementation

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
        Debug.Log("Player is dead");
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

    #endregion

    #region Private methods - resource management

    private void FetchControllerFromProvider()
    {
        IControllerProvider controllerProvider = GetComponent<IControllerProvider>();
        if (controllerProvider == null) throw new System.NullReferenceException($"No ControllerProvider component found on gameobject {this.gameObject.name}");
        SetGamepadByAssociation(controllerProvider.ControllerAssociation);
    }

    #endregion

    #region Private methods - behaviour

    private bool isGrounded() => Physics2D.OverlapCircle(foots.position, 0.2f, groundTag);

    private void flipPlayerAnimation()
    {
        if (this.movementData.x > 0.2f)
            body.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = false;
        else if (this.movementData.x < -0.2f)
            body.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = true;
    }

    private void changeSprite(Sprite s) => body.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = s;

    #endregion

}
