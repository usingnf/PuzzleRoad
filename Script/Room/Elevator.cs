using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.ParticleSystem;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Stage stage;
    [SerializeField] private Elevator target;
    [SerializeField] private Transform centerPos;
    [SerializeField] private Door door;
    [SerializeField] private bool isUp = true;
    [SerializeField] private GameObject wall;
    [SerializeField] private bool isTrueRoute = false;
    [SerializeField] private ParticleSystem spark;

    [SerializeField] private float moveTime = 3.0f;

    private bool isProgress = false;

    public void Btn_Excute(Unit unit)
    {
        if (isProgress)
            return;
        if (stage == null)
            return;

        if(!GameManager.Exist())
        {
            if(CameraManager.Exist())
                CameraManager.Instance.ResetCamAngle();
            if (SoundManager.Exist())
                SoundManager.Instance.StopMusic();
            ExcuteSoloStage(unit, true);
            return;
        }
        if (isTrueRoute)
        {
            if (CameraManager.Exist())
                CameraManager.Instance.ResetCamAngle();
            if (SoundManager.Exist())
                SoundManager.Instance.StopMusic();
            TrueRoute(unit);
            return;
        }
        if (target == null)
            return;
        else
        {
            if (CameraManager.Exist())
                CameraManager.Instance.ResetCamAngle();
            if (SoundManager.Exist())
                SoundManager.Instance.StopMusic();
            Excute(unit, isUp);
        }    
    }

    public void Excute(Unit unit, bool isUp)
    {
        isProgress = true;
        if (door != null)
        {
            door.Close();
            door.Lock(true);
        }
        target.stage.gameObject.SetActive(true);
        if (target.door != null)
        {
            target.door.Close();
            target.door.Lock(true);
        }
        if (wall != null)
        {
            wall.SetActive(true);
        }
        if(target.wall != null)
            target.wall.SetActive(true);
        //unit.SwtAgent(false);
        unit.transform.parent = this.transform;
        //audioSource.Play();
        SoundManager.Instance.StartSoundFadeLoop("SE_Elevator_Move2", 0.1f, 1.0f, 1.0f, moveTime * 2);
        if (isUp)
            stage.Obj_stage.transform.DOLocalMoveY(stage.Obj_stage.transform.position.y - 15, moveTime).OnComplete(() =>
            {
                NextExcute(unit, isUp);
            }).SetEase(Ease.InCubic);
        else
            stage.Obj_stage.transform.DOLocalMoveY(stage.Obj_stage.transform.position.y + 15, moveTime).OnComplete(() =>
            {
                NextExcute(unit, isUp);
            }).SetEase(Ease.InCubic);
    }

    private void NextExcute(Unit unit, bool isUp)
    {
        if(isUp)
        {
            target.stage.Obj_stage.transform.position += new Vector3(0, 15, 0);
            target.stage.Obj_stage.transform.DOLocalMoveY(target.stage.Obj_stage.transform.position.y - 15.0f, moveTime).OnComplete(() =>
            {
                EndExcute(unit);
            }).SetEase(Ease.OutCubic);
        }
        else
        {
            target.stage.Obj_stage.transform.position += new Vector3(0, -15, 0);
            target.stage.Obj_stage.transform.DOLocalMoveY(target.stage.Obj_stage.transform.position.y + 15.0f, moveTime).OnComplete(() =>
            {
                EndExcute(unit);
            }).SetEase(Ease.OutCubic);
        }
        unit.transform.parent = target.transform;
        unit.SetPos(target.centerPos.position + (unit.transform.position - centerPos.position));
        //unit.transform.position = target.centerPos.position + (unit.transform.position - centerPos.position);
        
    }

    private void EndExcute(Unit unit)
    {
        unit.transform.parent = null;
        //unit.SwtAgent(true);
        if (target.door != null)
        {
            target.door.Close();
            target.door.Lock(false);
        }
        if(target.wall != null)
            target.wall.SetActive(false);
        if (target.stage != null)
            GameManager.Instance.SetCurrentStage(target.stage.GetStageNum());
        stage.gameObject.SetActive(false);
        if (PlayerUnit.player != null && PlayerUnit.player.GetHoldObj() != null)
            PlayerUnit.player.GetHoldObj().Interact(PlayerUnit.player);
        //audioSource.Stop();
        isProgress = false;
    }

    public void SetTarget(Elevator target)
    {
        this.target = target;
    }

    public void SetIsTrueRoute(bool isIsTrueRoute)
    {
        this.isTrueRoute = isIsTrueRoute;
    }

    public bool GetIsTrueRoute()
    {
        return isTrueRoute;
    }

    public bool GetIsProgress()
    {
        return isProgress;
    }

    public void ExcuteSolo(Unit unit, bool isUp)
    {
        isProgress = true;
        if (door != null)
        {
            door.Close();
            door.Lock(true);
        }
        if(target != null)
        {
            if (target.door != null)
            {
                target.door.Close();
                target.door.Lock(true);
            }
        }

        if (wall != null)
        {
            wall.SetActive(true);
        }
        //unit.SwtAgent(false);
        AudioSource audio = SoundManager.Instance.StartSoundFadeLoop("SE_Elevator_Move2", 0.05f, 1.0f, 4.0f, 8.5f);
        if(unit != null)
            unit.transform.parent = this.transform;
        if (isUp)
            stage.Obj_stage.transform.DOLocalMoveY(stage.Obj_stage.transform.position.y - 15, moveTime).OnComplete(() =>
            {
                
            }).SetEase(Ease.InCubic);
        else
            stage.Obj_stage.transform.DOLocalMoveY(stage.Obj_stage.transform.position.y + 15, moveTime).OnComplete(() =>
            {
                
            }).SetEase(Ease.InCubic);
    }
    public void ExcuteSoloBadEnd2(Unit unit, bool isUp)
    {
        isProgress = true;
        if (door != null)
        {
            door.Close();
            door.Lock(true);
        }
        if (target != null)
        {
            if (target.door != null)
            {
                target.door.Close();
                target.door.Lock(true);
            }
        }

        if (wall != null)
        {
            wall.SetActive(true);
        }
        //unit.SwtAgent(false);
        AudioSource audio = SoundManager.Instance.StartSoundFadeLoop("SE_Elevator_Move2", 0.05f, 1.0f, 4.0f, 8.5f);
        unit.transform.parent = this.transform;
        if (isUp)
            stage.Obj_stage.transform.DOLocalMoveY(stage.Obj_stage.transform.position.y - 60, 12).OnComplete(() =>
            {

            }).SetEase(Ease.InCubic);
        else
            stage.Obj_stage.transform.DOLocalMoveY(stage.Obj_stage.transform.position.y + 60, 12).OnComplete(() =>
            {

            }).SetEase(Ease.InCubic);
    }

    public void ExcuteSoloStage(Unit unit, bool isUp)
    {
        isProgress = true;
        if (door != null)
        {
            door.Close();
            door.Lock(true);
        }
        if (target != null)
        {
            if (target.door != null)
            {
                target.door.Close();
                target.door.Lock(true);
            }
        }

        if (wall != null)
        {
            wall.SetActive(true);
        }
        //unit.SwtAgent(false);
        AudioSource audio = SoundManager.Instance.StartSoundFadeLoop("SE_Elevator_Move2", 0.05f, 0.9f, 0.9f, 2.0f);
        if(unit != null)
            unit.transform.parent = this.transform;
        UIManager.Instance.FadeIn(2.0f, Color.black, () => 
        { 
            UIManager.Instance.FadeOut(0, () => 
            {
                SceneManager.LoadScene("Scene_Title");
                UIManager.Instance.SetUIState(UIManager.UIState.Title);
            }); 
        });
        if (isUp)
            stage.Obj_stage.transform.DOLocalMoveY(stage.Obj_stage.transform.position.y - 60, 12).OnComplete(() =>
            {
                
            }).SetEase(Ease.InCubic);
        else
            stage.Obj_stage.transform.DOLocalMoveY(stage.Obj_stage.transform.position.y + 60, 12).OnComplete(() =>
            {
                
            }).SetEase(Ease.InCubic);
    }
    private void TrueRoute(Unit u)
    {
        isProgress = true;
        target = GameManager.Instance.GetStage(1016).startElevator;
        GameManager.Instance.GetStage(1016).gameObject.SetActive(true);
        if (door != null)
        {
            door.Close();
            door.Lock(true);
        }
        if(target != null)
        {
            if (target.door != null)
            {
                target.door.Close();
                target.door.Lock(true);
            }
        }
        
        if (wall != null)
        {
            wall.SetActive(true);
        }
        if(target.wall != null)
        {
            target.wall.SetActive(true);
        }
        u.transform.parent = this.transform;
        AudioSource audio = SoundManager.Instance.StartSoundFadeLoop("SE_Elevator_Move2", 0.1f, 1.0f, 4.0f, 8.5f);

        stage.Obj_stage.transform.DOLocalMoveY(stage.Obj_stage.transform.position.y - 1.5f, 1.0f).OnComplete(() =>
        {          
            SoundManager.Instance.SetPitch(audio, 1.5f, 0.5f);
            stage.Obj_stage.transform.DOLocalMoveY(stage.Obj_stage.transform.position.y + 50.0f, 1.0f).OnComplete(() =>
            {
                IEnumerator coSpark = target.CoSpark();
                target.StartCoroutine(coSpark);

                target.stage.Obj_stage.transform.position += new Vector3(0, -300, 0);
                target.stage.Obj_stage.transform.DOLocalMoveY(target.stage.Obj_stage.transform.position.y + 300.0f, 7.5f).OnComplete(() =>
                {
                    target.StopCoroutine(coSpark);
                    EndExcute(u);
                }).SetEase(Ease.OutQuint);

                u.transform.parent = target.transform;
                u.SetPos(target.centerPos.position + (u.transform.position - centerPos.position));
                //unit.transform.position = target.centerPos.position + (unit.transform.position - centerPos.position);
                stage.gameObject.SetActive(false);

            }).SetEase(Ease.InQuint);            
        }).SetEase(Ease.InCubic);

        Event_TrueEnter();
    }

    private void Event_TrueEnter()
    {
        StartCoroutine(CoEvent_TrueEnter());
    }

    private IEnumerator CoEvent_TrueEnter()
    {
        yield return new WaitForSeconds(0.8f);
        CameraManager.Instance.Shake(0.5f);
        SoundManager.Instance.StartSound("SE_Elevator_Explosion", 0.8f);
        if (DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Senario016_TrueEnter");
    }

    private IEnumerator CoSpark()
    {
        yield return new WaitForSeconds(1.5f);
        float time = 0.05f;
        float speed = 25.0f;
        MainModule main;
        while (true)
        {
            time += time * 0.04f;
            if (speed > 5)
                speed += -time * 2.5f;
            ParticleSystem particle = Instantiate(spark, this.transform.position + new Vector3(0, 0, 15), Quaternion.Euler(270,0,0));
            main = particle.main;
            main.startSpeed = new MinMaxCurve(speed, speed * 1.5f);
            particle.Play();

            ParticleSystem particle2 = Instantiate(spark, this.transform.position + new Vector3(10, 0, 15), Quaternion.Euler(270, 0, 0));
            main = particle2.main;
            main.startSpeed = new MinMaxCurve(speed, speed * 1.5f);

            particle.Play();
            particle2.Play();
            Destroy(particle.gameObject, 5.0f);
            Destroy(particle2.gameObject, 5.0f);
            SoundManager.Instance.StartSound("SE_Spark", 0.25f);
            yield return new WaitForSeconds(time);
        }
    }

    public Door GetDoor()
    {
        return door;
    }
}
