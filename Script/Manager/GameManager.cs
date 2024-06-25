using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 게임 상태, 스테이지 총괄 클래스
/// 게임 저장, 로그, 필드 생성 기능.
/// </summary>

public enum GameState
{
    None,
    Loading,
    Start,
    End,
}
public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int currentStage = 1;
    private Dictionary<int, Transform> stageStartPos = new Dictionary<int, Transform>();
    private Dictionary<int, Stage> dic_stage = new Dictionary<int, Stage>();
    private static GameState state = GameState.None;

    private float gameStartTime = 0;

    private int ForceEnableStage = -1;
    public static int AutoLoad = -1;
    
    private void Awake()
    {
        Instance = this;
        gameStartTime = Time.time;
    }
    IEnumerator Start()
    {
        yield return null;
        yield return null;
        SetState(GameState.Start);
        yield return null;
        //스테이지 생성
        yield return SceneManager.LoadSceneAsync("Scene_Stage001", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("Scene_Stage002", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("Scene_Stage003", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("Scene_Stage004", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("Scene_Stage005", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("Scene_Stage006", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("Scene_Stage007", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("Scene_Stage008", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("Scene_Stage009", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("Scene_Stage010", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("Scene_Stage011", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("Scene_Stage012", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("Scene_Stage013", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("Scene_Stage014", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("Scene_Stage015", LoadSceneMode.Additive);

        //스테이지 연결
        yield return null;
        dic_stage[1001].endElevator.SetTarget(dic_stage[1002].startElevator);
        dic_stage[1002].endElevator.SetTarget(dic_stage[1].startElevator);
        dic_stage[1].endElevator.SetTarget(dic_stage[1003].startElevator);
        dic_stage[1003].endElevator.SetTarget(dic_stage[11].startElevator);
        dic_stage[11].endElevator.SetTarget(dic_stage[1004].startElevator);
        dic_stage[1004].endElevator.SetTarget(dic_stage[3].startElevator);
        dic_stage[3].endElevator.SetTarget(dic_stage[1005].startElevator);
        dic_stage[1005].endElevator.SetTarget(dic_stage[2].startElevator);
        dic_stage[2].endElevator.SetTarget(dic_stage[1006].startElevator);
        dic_stage[1006].endElevator.SetTarget(dic_stage[4].startElevator);
        dic_stage[4].endElevator.SetTarget(dic_stage[14].startElevator);
        dic_stage[14].endElevator.SetTarget(dic_stage[1009].startElevator);
        dic_stage[1009].endElevator.SetTarget(dic_stage[5].startElevator);
        dic_stage[5].endElevator.SetTarget(dic_stage[6].startElevator);
        dic_stage[6].endElevator.SetTarget(dic_stage[7].startElevator);
        dic_stage[7].endElevator.SetTarget(dic_stage[8].startElevator);
        dic_stage[8].endElevator.SetTarget(dic_stage[1010].startElevator);
        dic_stage[1010].endElevator.SetTarget(dic_stage[9].startElevator);
        dic_stage[9].endElevator.SetTarget(dic_stage[10].startElevator);
        dic_stage[10].endElevator.SetTarget(dic_stage[1011].startElevator);
        dic_stage[1011].endElevator.SetTarget(dic_stage[13].startElevator);
        dic_stage[13].endElevator.SetTarget(dic_stage[1013].startElevator);
        dic_stage[1013].endElevator.SetTarget(dic_stage[15].startElevator);
        dic_stage[15].endElevator.SetTarget(dic_stage[1014].startElevator);
        dic_stage[1014].endElevator.SetTarget(dic_stage[1015].startElevator);

        //Water Stage. Not Use
        //dic_stage[1007].endElevator.SetTarget(dic_stage[6].startElevator);
        //dic_stage[1008].endElevator.SetTarget(dic_stage[7].startElevator);
        //dic_stage[1012].endElevator.SetTarget(dic_stage[11].startElevator);      
        //dic_stage[12].endElevator.SetTarget(dic_stage[1014].startElevator);

        yield return null;
        yield return new WaitForSeconds(0.1f);
        //현재 있는 스테이지만 활성화
        foreach (KeyValuePair<int, Stage> data in dic_stage)
        {
            if (data.Key != 1001 && data.Key != ForceEnableStage)
                data.Value.gameObject.SetActive(false);
        }
    }


    public static GameState GetState()
    {
        return state;
    }

    public static void SetState(GameState _state)
    {
        state = _state;
    }

    public void AddStartPos(int stage, Transform trans)
    {
        stageStartPos[stage] = trans;
    }
    public void AddStage(int stageNum, Stage stage)
    {
        if (dic_stage.ContainsKey(stageNum))
            return;
        dic_stage[stageNum] = stage;
    }

    public Stage GetStage(int stageNum)
    {
        return dic_stage[stageNum];
    }

    public void SetCurrentStage(int stage)
    {
        currentStage = stage;
        if(stage > 1001 && stage < 1015)
            PlayerPrefs.SetInt("AutoSave", stage);
    }

    public int GetCurrentStage() 
    {
        return currentStage; 
    }
    public void StageRestart(bool isForce = false)
    {
        if (isForce)
            StartCoroutine(CoStageRestart());
        else
            dic_stage[currentStage].ReStage();
    }

    // 강제 스테이지 리셋. 사용하지 않는 기능.
    public IEnumerator CoStageRestart()
    {
        string str = "Scene_Stage" + currentStage.ToString("D3");
        yield return SceneManager.UnloadSceneAsync(str);
        yield return SceneManager.LoadSceneAsync(str, LoadSceneMode.Additive);
        yield return new WaitForSeconds(0.01f);
        PlayerUnit.player.SetPos(stageStartPos[currentStage].position);
        PlayerUnit.player.StageReStart();
    }

    //비밀 스테이지 오픈.
    public void TrueRouteOn()
    {
        if (dic_stage[currentStage].endElevator.GetIsTrueRoute())
            return;
        if (currentStage == 1016 || currentStage == 1017)
            return;
        UIManager.Toast(Language.Instance.Get("UI_Toast_SOS"));
        dic_stage[currentStage].endElevator.SetIsTrueRoute(true);
    }

    public float GetPlayStartTime()
    {
        return gameStartTime;
    }

    //세이브 데이터 로드시, 해당 스테이지에서 시작.
    public void CheckAutoLoad()
    {
        if (AutoLoad < 0)
            return;
        currentStage = AutoLoad;
        ForceEnableStage = currentStage;
        AutoLoad = -1;
        PlayerUnit player = PlayerUnit.player;
        dic_stage[currentStage].SwtFirstDetector(false);
        dic_stage[currentStage].gameObject.SetActive(true);
        player.SetPos(stageStartPos[currentStage].position);
        dic_stage[1001].gameObject.SetActive(false);
        UIManager.Instance.FadeOut(3.0f, () =>
        {
            player.SetIsCanControl(true);
            if(dic_stage[currentStage].startElevator != null)
            {
                dic_stage[currentStage].startElevator.GetDoor().SwtLockSound(false);
                dic_stage[currentStage].startElevator.GetDoor().Lock(false);
                dic_stage[currentStage].startElevator.GetDoor().SwtLockSound(true);
            }
        });
    }
}
