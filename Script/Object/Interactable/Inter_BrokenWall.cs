using RayFire;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class Inter_BrokenWall : InteractableObject
{
    [SerializeField] private SenarioTrue stageTrue;
    [SerializeField] private RayfireShatter shatter;
    [SerializeField] private MeshRenderer meshRender;
    [SerializeField] private NavMeshObstacle obstacle;
    private int interactCount = -1;
    private int[] amounts = new int[20] { 30,30,40,40,50,50,60,60,70,70,80,80,90,90,100,100,110,110,120,120};

    private void Start()
    {
        shatter.voronoi.amount = 30;
        shatter.previewScale = 0.01f;
    }
    public override void Interact(Unit u)
    {
        base.Interact(u);
        if (!isCanInteract)
            return;
        interactCount += 1;
        if(interactCount >= amounts.Length)
        {
            isCanInteract = false;
            foreach(GameObject obj in shatter.fragmentsLast)
            {
                RayfireRigid rigid = obj.AddComponent<RayfireRigid>();
                rigid.Initialize();
                rigid.physics.rigidBody.AddForce(new Vector3(Random.Range(-1, 1), Random.Range(0.1f, 1), Random.Range(-1, 1)) * 15.0f, ForceMode.Impulse);
            }
            stageTrue.Event_BrokenWall();
            StartCoroutine(CoDestroy());
            SoundManager.Instance.StartSound("SE_Explosion_2", 1.0f);
            SoundManager.Instance.StopVoice(0.2f);
        }
        else
        {
            meshRender.enabled = false;
            CameraManager.Instance.StopShake();
            CameraManager.Instance.Shake(0.2f, 0.5f, 60);
            shatter.scalePreview = true;
            shatter.previewScale += 0.01f;
            shatter.voronoi.amount = amounts[interactCount];
            shatter.DeleteFragmentsLast();
            shatter.Fragment();
            foreach (GameObject fragment in shatter.fragmentsLast)
                if (fragment != null)
                    fragment.transform.localScale = Vector3.one * (1-shatter.previewScale);
            if(Random.Range(0, 2) == 0)
                SoundManager.Instance.StartSound("SE_Impact_1", 1.0f);
            else
                SoundManager.Instance.StartSound("SE_Impact_2", 1.0f);
            if (interactCount == 2)
            {
                if (DialogManager.Exist())
                    DialogManager.Instance.ShowDialog("Dialog_Senario016_LastWall_1");
            }
            else if(interactCount == 8)
            {
                if (DialogManager.Exist())
                    DialogManager.Instance.ShowDialog("Dialog_Senario016_LastWall_2");
            }
            else if (interactCount == 15)
            {
                if (DialogManager.Exist())
                    DialogManager.Instance.ShowDialog("Dialog_Senario016_LastWall_3");
            }

        }

    }

    private IEnumerator CoDestroy()
    {
        obstacle.enabled = false;
        yield return new WaitForSeconds(7.0f);
        foreach (GameObject obj in shatter.fragmentsLast)
        {
            Destroy(obj);
        }
    }
}
