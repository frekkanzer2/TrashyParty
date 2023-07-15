using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockControllerProvider : MonoBehaviour, IControllerProvider
{
    [System.Serializable]
    public class MockAssociationData
    {
        public int PlayerNumber;
        public int ControllerId;
        public Sprite PlayerSprite;
    }
    public PlayerControllerAssociationDto ControllerAssociation { get; set; }
    public MockAssociationData AssociationMock;
    private void Awake()
    {
        ControllerAssociation = new PlayerControllerAssociationDto()
        {
            ControllerId = AssociationMock.ControllerId,
            PlayerNumber = AssociationMock.PlayerNumber,
        };
    }
    private void Start()
    {
        this.GetComponent<IPlayer>().SetAsReady();
        this.GetComponent<IPlayer>().SetCanWalk(true);
    }
}
