using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_Lazer : MonoBehaviour
{
    [SerializeField] private Stage005 stage;
    private WaitForSeconds wait = new WaitForSeconds(0.05f);
    [SerializeField] private Vector3 angle;
    [SerializeField] private Vector3 lastAngle;
    [SerializeField] private Vector3 lastHitPos;
    [SerializeField] private LineRenderer line;
    [SerializeField] private Vector3[] vec;
    [SerializeField] private List<Vector3> list = new List<Vector3>();
    private int count = 0;
    [SerializeField] private int colorType = 0;
    public bool isAnswer = false;

    [SerializeField] private GameObject targetParticle;
    private void OnEnable()
    {
        StartCoroutine(CoLazer());
    }

    private IEnumerator CoLazer()
    {
        line.enabled = true;
        while(true)
        {
            //line.SetPositions;
            list.Clear();
            list.Add(transform.position);
            count = 0;
            lastAngle = angle;
            lastHitPos = transform.position;
            isAnswer = false;
            if(Shoot(transform.position, angle))
            {
                count += 1;
                list.Add(lastHitPos + lastAngle * 40.0f);
            }
            if(isAnswer)
            {
                if(!targetParticle.activeSelf)
                {
                    targetParticle.SetActive(true);
                    SoundManager.Instance.StartSound("UI_Success2", 1.0f);
                }
                
            }
            else
            {
                targetParticle.SetActive(false);
            }
            //if(count == 0)
            //{
            //    count += 1;
            //    list.Add(transform.position + lastAngle * 40.0f);
            //}

            
            if (line.positionCount != count + 1)
                line.positionCount = count + 1;
            for(int i = 0; i < count + 1; i++)
            {
                //Debug.Log("@" + i + ":" + list[i]);
                line.SetPosition(i, list[i]);
            }
            
            yield return wait;
        }
    }

    private bool Shoot(Vector3 start, Vector3 ang)
    {
        if (count > 1000)
            this.gameObject.SetActive(false);
        if (Physics.Raycast(start, ang, out RaycastHit hit, 40.0f, LayerMask.GetMask("LayerObj", "Player")))
        {
            count += 1;
            list.Add(hit.point);
            if (hit.transform.TryGetComponent<Obj_LazerMirror>(out Obj_LazerMirror mirror))
            {
                //Debug.Log("@" + hit.transform.gameObject.name + "/" +mirror.angle);
                if(mirror.isEndWall)
                {
                    if(mirror.colorType == colorType)
                    {
                        isAnswer = true;
                    }
                }

                lastHitPos = hit.point;
                lastAngle = mirror.CalAngle(ang);
                if(lastAngle == Vector3.zero)
                {
                    return false;
                }
                //lastAngle = mirror.angle;
                if (Shoot(hit.point, lastAngle))
                    return true;
                else
                    return false;
            }
            else if(hit.transform.CompareTag("Player"))
            {
                stage.ReStage();
                SoundManager.Instance.StartSound("SE_Lazer_Die", 1.0f);
                return false;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    public void SwtLaser(bool b)
    {
        line.enabled = b;
    }
}
