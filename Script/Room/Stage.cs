using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �� ���������� �θ� Ŭ����.
/// �������� �̵�, ����, ���� ���� Ȯ��, ���������� ��Ʈ�� ����.
/// </summary>

public enum ColorBlock
{
    Black = 0,
    Red = 1,
    Green = 2,
    Yellow = 3,
    Blue = 4,
    Purple = 5,
    Emerald = 6,
    White = 7,
    End = 8,
}
public class Stage : MonoBehaviour
{
    [SerializeField] private GameObject[] obj_soloStage;
    [SerializeField] protected Transform startPos;
    public Transform Obj_stage;
    public Elevator startElevator;
    public Elevator endElevator;
    [SerializeField] private int stageNum = 0;
    [SerializeField] protected int hintIndex = 0;
    [SerializeField] private GameObject firstDetector;
    protected void Start()
    {
        if(GameManager.Exist())
        {
            if(startPos != null)
            {
                GameManager.Instance.AddStartPos(stageNum, startPos);
            }
            if(stageNum >=0)
                GameManager.Instance.AddStage(stageNum, this);
        }
        else
        {
            //soloStage
            foreach(GameObject obj in obj_soloStage)
            {
                obj.SetActive(true);
                if (startElevator != null)
                {
                    startElevator.GetDoor().SwtLockSound(false);
                    startElevator.GetDoor().Lock(false);
                    startElevator.GetDoor().SwtLockSound(true);
                }
            }
        }
        //if (stageNum != 1001)
        //    this.gameObject.SetActive(false);
    }

    public virtual void ReStage()
    {
        if (PlayerUnit.player != null)
            PlayerUnit.player.Revive();
    }

    public int GetStageNum()
    {
        return stageNum;
    }

    public Transform GetStartPos()
    {
        return startPos;
    }

    public void SwtFirstDetector(bool b)
    {
        if (firstDetector != null)
            firstDetector.SetActive(b);
    }

    public virtual void Hint()
    {
        UIManager.Toast(Language.Instance.Get("UI_Option_NoHint"));
    }

    public virtual void OnEnter()
    {

    }

    public virtual void OnExit()
    {

    }

    public virtual void OnAnswer()
    {

    }
}
