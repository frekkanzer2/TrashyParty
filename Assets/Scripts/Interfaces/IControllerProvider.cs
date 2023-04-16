using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControllerProvider
{
    PlayerControllerAssociationDto ControllerAssociation { get; set; }
}
