using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerPlayerKagome : PlatformerPlayer
{

    public GameObject KokeshiUp, KokeshiLeft, KokeshiDown, KokeshiRight;
    private Vector3[] Positions = new Vector3[4];
    private KokeshiPositionAssociationDto[] Associations = new KokeshiPositionAssociationDto[4];

    private class KokeshiPositionAssociationDto
    {
        public GameObject Kokeshi; public Vector3 Position; private int Depth;
        public KokeshiPositionAssociationDto(GameObject kokeshi, Vector3 position, int depth)
        {
            this.Kokeshi = kokeshi;
            this.Position = position;
            this.Depth = depth;
            this.Kokeshi.GetComponent<SpriteRenderer>().sortingOrder = this.Depth;
        }
        public void ChangeKokeshi(GameObject kokeshi)
        {
            this.Kokeshi = kokeshi;
            this.Kokeshi.transform.position = this.Position;
            this.Kokeshi.GetComponent<SpriteRenderer>().sortingOrder = this.Depth;
        }
    }

    protected override void VariantStart()
    {
        base.VariantStart();
        KokeshiUp.name = "F";
        KokeshiLeft.name = "ML";
        KokeshiDown.name = "MD";
        KokeshiRight.name = "MR";
        Positions[0] = KokeshiUp.transform.position;
        Positions[1] = KokeshiLeft.transform.position;
        Positions[2] = KokeshiDown.transform.position;
        Positions[3] = KokeshiRight.transform.position;
        Associations[0] = new(KokeshiUp, Positions[0], -10);
        Associations[1] = new(KokeshiLeft, Positions[1], 11);
        Associations[2] = new(KokeshiDown, Positions[2], 11);
        Associations[3] = new(KokeshiRight, Positions[3], 20);
        StartCoroutine(StartDisplay());
    }

    IEnumerator StartDisplay()
    {
        yield return new WaitForSeconds(1);
        this.KokeshiUp.SetActive(true);
        yield return new WaitForSeconds(1);
        this.KokeshiRight.SetActive(true);
        this.KokeshiLeft.SetActive(true);
        yield return new WaitForSeconds(1);
        this.KokeshiDown.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        canRun = true;
    }

    private float time = 0.0f;
    private float interpolationPeriod = 0.095f;
    private bool canRun = false;
    private bool hasStopped = false;
    private GameObject kokeshiStopped = null;
    protected override void VariantUpdate()
    {
        base.VariantUpdate();
        if (canRun && !hasStopped && !GameManager.Instance.IsGameEnded() && !hasStopped)
        {
            time += Time.deltaTime;
            if (time >= interpolationPeriod)
            {
                time = 0.0f;
                GameObject kokeshi0 = Associations[0].Kokeshi;
                Associations[0].ChangeKokeshi(Associations[3].Kokeshi);
                Associations[3].ChangeKokeshi(Associations[2].Kokeshi);
                Associations[2].ChangeKokeshi(Associations[1].Kokeshi);
                Associations[1].ChangeKokeshi(kokeshi0);
            }
        }
        if (gamepad.IsButtonPressed(IGamepad.Key.ActionButtonLeft, IGamepad.PressureType.Single))
        {
            hasStopped = true;
            kokeshiStopped = Associations[0].Kokeshi.gameObject;
            Log.Logger.Write("Stopped with kokeshi " + kokeshiStopped.name + " with tag " + kokeshiStopped.tag);
            if (kokeshiStopped.CompareTag("Finish"))
            {
                List<TeamDto> loserTeams = GameManager.Instance.Teams.FindAll(t => t.GetAlivePlayers().Count > 0 && !t.players[0].Equals(this));
                foreach (TeamDto team in loserTeams)
                    team.players[0].OnDeath();
            }
            else
            {
                if (GameManager.Instance.Teams.FindAll(team => !team.IsEveryoneDead()).Count == 2)
                {
                    Log.Logger.Write("2 teams remaining (dummy included). Removing the dummy team.");
                    GameManager.Instance.Teams.Find(team => team.Id == 100).KillAllPlayers();
                }
                this.OnDeath();
            }
        }
    }

}
