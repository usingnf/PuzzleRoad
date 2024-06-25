using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_RockTrapGroup : MonoBehaviour
{
    [SerializeField] private SenarioTrue stage;
    [SerializeField] private Boss_RockTrap prefab_trap;

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.T))
    //    {
    //        Debug.Log("T");
    //        Attack(Vector3.zero, 1.5f);
    //    }
    //}
    public void Attack(Vector3 pos, float t)
    {
        if (pos.x > this.transform.position.x + 8.5f)
            pos.x = this.transform.position.x + 8.5f;
        else if(pos.x < this.transform.position.x - 8.5f)
            pos.x = this.transform.position.x - 8.5f;
        if (pos.z > this.transform.position.z + 8.25f)
            pos.z = this.transform.position.z + 8.25f;
        else if (pos.z < this.transform.position.z - 8.25f)
            pos.z = this.transform.position.z - 8.25f;
        pos.y = this.transform.position.y;
        Boss_RockTrap trap = Instantiate(prefab_trap, pos, Quaternion.identity, this.transform);
        //trap.transform.position = this.transform.position;
        trap.group = this;
        trap.Falling(t);
    }

    public void Kill()
    {
        stage.ReStage();
        CameraManager.Instance.StopShake();
    }
}
