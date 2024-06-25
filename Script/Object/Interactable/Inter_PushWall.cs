using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inter_PushWall : InteractableObject
{
    public Stage006 stage6;
    public int x;
    public int y;
    public override void Interact(Unit u)
    {
        base.Interact(u);
        if (!isCanInteract)
            return;
        stage6.PushWall(u.transform.position, this);
        SoundManager.Instance.StartSound("SE_Box_Push", 0.8f, 0.1f);
    }

    //public void Test(Stage006 s)
    //{
    //    render = this.GetComponent<MeshRenderer>();
    //    coll = this.GetComponent<BoxCollider>();
    //    stage = s.transform;
    //    stage6 = s;
    //}

}
