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
        Associations[1] = new(KokeshiLeft, Positions[1], 1);
        Associations[2] = new(KokeshiDown, Positions[2], 1);
        Associations[3] = new(KokeshiRight, Positions[3], 10);
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
        StartCoroutine(Rotation(0.614f));
    }

    IEnumerator Rotation(float time)
    {
        yield return new WaitForSeconds(time);
        GameObject kokeshi0 = Associations[0].Kokeshi;
        Associations[0].ChangeKokeshi(Associations[3].Kokeshi);
        Associations[3].ChangeKokeshi(Associations[2].Kokeshi);
        Associations[2].ChangeKokeshi(Associations[1].Kokeshi);
        Associations[1].ChangeKokeshi(kokeshi0);
        if (!GameManager.Instance.IsGameEnded() && !hasStopped) StartCoroutine(Rotation(time > 0.114f ? time - 0.1f : time));
    }

    private bool hasStopped = false;
    private GameObject kokeshiStopped = null;
    protected override void VariantUpdate()
    {
        base.VariantUpdate();
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
            else this.OnDeath();
        }
    }

    protected override void PreVariantUpdate()
    {
        base.PreVariantUpdate();
        if (kokeshiStopped is not null)
        {
            switch (kokeshiStopped.name)
            {
                case "F":
                    Associations[0].ChangeKokeshi(KokeshiUp);
                    Associations[1].ChangeKokeshi(KokeshiLeft);
                    Associations[2].ChangeKokeshi(KokeshiDown);
                    Associations[3].ChangeKokeshi(KokeshiRight);
                    break;
                case "MR":
                    Associations[0].ChangeKokeshi(KokeshiRight);
                    Associations[1].ChangeKokeshi(KokeshiUp);
                    Associations[2].ChangeKokeshi(KokeshiLeft);
                    Associations[3].ChangeKokeshi(KokeshiDown);
                    break;
                case "MD":
                    Associations[0].ChangeKokeshi(KokeshiDown);
                    Associations[1].ChangeKokeshi(KokeshiRight);
                    Associations[2].ChangeKokeshi(KokeshiUp);
                    Associations[3].ChangeKokeshi(KokeshiLeft);
                    break;
                case "ML":
                    Associations[0].ChangeKokeshi(KokeshiLeft);
                    Associations[1].ChangeKokeshi(KokeshiDown);
                    Associations[2].ChangeKokeshi(KokeshiRight);
                    Associations[3].ChangeKokeshi(KokeshiUp);
                    break;
            }
        }
    }

}
