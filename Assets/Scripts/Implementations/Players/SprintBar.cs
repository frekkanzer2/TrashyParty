using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TopDownPlayer))]
public class SprintBar : GenericBar
{

    public float RecoveryValue;
    public float SprintConsumingValue;
    private TopDownPlayer connectedPlayer;
    private Vector2 backupSpeeds = Vector2.zero;

    public override void OnMaxValueReached() {
        if (!connectedPlayer.CanSprintBySprintBar)
            connectedPlayer.ChangePlayerStats(backupSpeeds.x, backupSpeeds.y);
        connectedPlayer.CanSprintBySprintBar = true;
    }
    public override void OnMinValueReached() {
        connectedPlayer.CanSprintBySprintBar = false;
        backupSpeeds = connectedPlayer.Speeds;
        connectedPlayer.ChangePlayerStats(backupSpeeds.x - 3, backupSpeeds.y - 4);
    }

    public void Recover() => Increase(RecoveryValue);
    public void UseSprint() => Decrease(SprintConsumingValue);

    void Start()
    {
        connectedPlayer = this.gameObject.GetComponent<TopDownPlayer>();
        GenericStart();
    }

    void Update()
    {
        GenericUpdate();
    }

}
