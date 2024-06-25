using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_LazerTrapGroup2 : MonoBehaviour
{
    [SerializeField] private SenarioTrue stage;
    [SerializeField] private Boss_LazerTrap2 prefab_lazer;

    //public LazerAngle temp;
    //public float temp2;
    //public bool temp3;
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.T))
    //    {
    //        Debug.Log("T");
    //        Attack(temp, temp2, temp3);
    //    }
    //}

    public void Attack(LazerAngle angle, float time, bool isLeft)
    {
        Boss_LazerTrap2 trap = Instantiate(prefab_lazer, this.transform);
        trap.group = this;
        trap.AttackStart(angle, time, isLeft);
    }
    public void Kill()
    {
        stage.ReStage();
    }
}
