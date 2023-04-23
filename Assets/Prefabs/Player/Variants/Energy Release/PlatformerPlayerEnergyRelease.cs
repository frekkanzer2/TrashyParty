using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerPlayerEnergyRelease : PlatformerPlayer
{
    public GameObject EnergyBlastPreset;
    protected bool isWaitingEnergyRecharge = false;
    private int bonusPower = 0;
    private float timeFromLastPower = 0;
    protected override void VariantUpdate()
    {
        if (gamepad.IsConnected())
        {
            if (!isWaitingEnergyRecharge && gamepad.IsButtonPressed(IGamepad.Key.ActionButtonLeft, IGamepad.PressureType.Single))
            {
                SoundsManager.Instance.PlayPlayerSound(ISoundsManager.PlayerSoundType.EnergyRelease);
                isWaitingEnergyRecharge = true;
                GameObject spawned = Instantiate(EnergyBlastPreset, new Vector3(this.transform.position.x, this.transform.transform.position.y + 1, 0), Quaternion.identity);
                spawned.GetComponent<EnergyReleaseCreator>().Summoner = this;
                Transform spawnedTransform = spawned.transform;
                if (timeFromLastPower < 5) bonusPower = 0;
                else if (timeFromLastPower >= 5 && timeFromLastPower < 10) bonusPower = 3;
                else if (timeFromLastPower >= 10 && timeFromLastPower < 20) bonusPower = 6;
                else if (timeFromLastPower >= 20 && timeFromLastPower < 30) bonusPower = 10;
                else if (timeFromLastPower >= 30 && timeFromLastPower < 60) bonusPower = 14;
                else if (timeFromLastPower >= 60) bonusPower = 20;
                spawnedTransform.localScale = new Vector3(5 + bonusPower, 5 + bonusPower, 1);
                bonusPower = 0;
                StartCoroutine(WaitingEnergyRecharge());
            }
            if (isWaitingEnergyRecharge)
            {
                canJump = false;
                canWalk = false;
            }
            else
            {
                canJump = true;
                canWalk = true;
            }
        }
    }

    protected override void VariantFixedUpdate()
    {
        if (GameManager.Instance.IsGameStarted() && !GameManager.Instance.IsGameEnded())
        {
            timeFromLastPower += Time.deltaTime;
        }
    }

    private IEnumerator WaitingEnergyRecharge() {
        yield return new WaitForSeconds(1);
        isWaitingEnergyRecharge = false;
        timeFromLastPower = 0;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            if (!collision.gameObject.GetComponent<EnergyReleaseCreator>().Summoner.Equals(this) && !this.IsDead())
                this.OnDeath();
        }
    }

}
