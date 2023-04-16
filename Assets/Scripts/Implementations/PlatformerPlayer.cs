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
    [SerializeField] private LayerMask groundTag;
    private IGamepad gamepad;

    #endregion
    #region Class variables
    private Vector2 movementData = Vector2.zero;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
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

    }

    #endregion

    #region IPlayer implementation

    public void SetGamepad(IGamepad gamepad)
    {
        this.gamepad = gamepad;
        if (this.gamepad == null) Debug.LogError("Player doesn't have a gamepad!");
        else Debug.Log("Gamepad loaded");
        OnGamepadDeconnected();
    }

    public void SetGamepadByAssociation(PlayerControllerAssociationDto pcaDto)
    {
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
        throw new System.NotImplementedException();
    }

    public void OnGameEnds()
    {
        throw new System.NotImplementedException();
    }

    public void OnDeath()
    {
        throw new System.NotImplementedException();
    }

    public void OnSpawn()
    {
        throw new System.NotImplementedException();
    }

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

    #endregion

}
