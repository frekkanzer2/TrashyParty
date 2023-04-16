using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerProvider : MonoBehaviour, IControllerProvider
{
    public PlayerControllerAssociationDto ControllerAssociation { get; set; }
}
