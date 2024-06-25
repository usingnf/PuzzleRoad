using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_LazerTrapGroup : MonoBehaviour
{
    [SerializeField] private SenarioTrue stage;
    [SerializeField] private Boss_LazerTrap prefab_lazer;


    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.T))
    //    {
    //        Debug.Log("T");
    //        Attack(4, LazerAngle.Left, 2.0f);
    //    }
    //}

    public void Attack(float pos, LazerAngle angle, float time, bool isSound)
    {
        Boss_LazerTrap lazer = Instantiate(prefab_lazer, this.transform.position, Quaternion.identity, this.transform);
        lazer.group = this;
        lazer.AttackStart(pos, angle, time, isSound);
    }
    public void Kill()
    {
        stage.ReStage();
    }
}
