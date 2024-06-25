using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RotateTileData
{
    public GameObject outline;
    public bool isEnergy;
    public bool isLeft;
    public bool isRight;
    public bool isUp;
    public bool isDown;

    public RotateTileData(GameObject obj)
    {
        isEnergy = false;
        isLeft = false;
        isRight = false;
        isUp = false;
        isDown = false;
        outline = obj;
    }
}

public class Senario016 : Stage
{
    [SerializeField] private Transform[] obj_stages;
    [SerializeField] private GameObject[] obj_PriNavWall;
    [SerializeField] private GameObject[] obj_NextNavWall;
    private float moveDistance = 20.0f;
    private float moveSpeed = 8.0f;
    private float closeMoveDistance = 1.0f;
    private float closeMoveSpeed = 0.5f;
    private List<int> activedBtnList = new List<int>();

    [SerializeField] private Transform moveCamTarget;
    [SerializeField] private GameObject fireParticle;

    [Header("Entrance - First")]
    [SerializeField] private Inter_Gate gate;
    [SerializeField] private GameObject gate_leftWall;
    [SerializeField] private GameObject gate_rightWall;
    [SerializeField] private Material redMat;
    [SerializeField] private Material greenMat;
    [SerializeField] private Transform[] gateLever;
    [SerializeField] private MeshRenderer[] gateLeverLight;
    [SerializeField] private bool[] gateAnswer;
    [SerializeField] private bool[] gateLeverState;
    private bool isFirstGateOpen = false;

    [Header("Rotate - Second")]
    [SerializeField] private Transform[] rotateRoad;
    [SerializeField] private Transform[] rotateTile;
    [SerializeField] private GameObject[] rotateNavWall;
    [SerializeField] private GameObject[] navRoad;
    [SerializeField] private RotateTileData[,] rotateTileData;
    [SerializeField] private MeshRenderer endCoreRender;
    [SerializeField] private Material endCoreMat;
    [SerializeField] private AudioSource endCoreSound;
    [SerializeField] private Obj_LastDoor secondDoor;
    //private Transform[,] rotateTileArr = new Transform[3,3];
    private bool isSecondRotate = false;
    private bool isSecondAnswer = false;
    private int secondRotateCount = 0;

    [Header("Arrow - Third")]
    [SerializeField] private Transform thirdStartPos;
    [SerializeField] private List<Obj_ArrowTile> arrowRoute = new List<Obj_ArrowTile>();
    private bool isArrowEnd = false;
    private bool isArrowTileMove = false;
    [SerializeField] private Transform[] arrowMoveTilesTrans;
    [SerializeField] private Obj_MoveArrowTile[] arrowMoveTiles;
    private Tweener arrowTileTween = null;
    [SerializeField] private Transform[] arrowTilePosTrans;
    [SerializeField] private GameObject[] arrowNavWall;
    [SerializeField] private GameObject arrowStartNavWall;
    [SerializeField] private Inter_Lever arrowLever;
    private Vector3[,] arrowTilePosVec;
    private Vector2Int[] arrowTilePos;
    private bool[,] isArrowTilePos;
    private bool isFirstThirdAnswer = true;

    [Header("MoveTile - Fourth")]
    [SerializeField] private Inter_HoldColorBlock lastColorBlock;
    [SerializeField] private AudioSource moveTileAudio;
    private HoldState holdState = HoldState.None;
    [SerializeField] private Obj_MoveColorTile[] redMoveTiles;
    [SerializeField] private Obj_MoveColorTile[] greenMoveTiles;
    [SerializeField] private Obj_MoveColorTile[] blueMoveTiles;
    [SerializeField] private Obj_MoveColorTile[] yellowMoveTiles;
    [SerializeField] private Transform[] redTile;
    [SerializeField] private Transform[] greenTile;
    [SerializeField] private Transform[] blueTile;
    [SerializeField] private Transform[] yellowTile;
    [SerializeField] private MeshRenderer[] redRender;
    [SerializeField] private MeshRenderer[] greenRender;
    [SerializeField] private MeshRenderer[] blueRender;
    [SerializeField] private MeshRenderer[] yellowRender;
    [SerializeField] private Material mat_gray;
    [SerializeField] private Material mat_green;
    [SerializeField] private float redFloat = 0;
    [SerializeField] private float greenFloat = 3;
    [SerializeField] private float blueFloat = 4;
    [SerializeField] private float yellowFloat = 3;
    [SerializeField] private float tileMoveSpeed = 1.0f;
    private bool redBool = true;
    private bool greenBool = true;
    private bool blueBool = true;
    private bool yellowBool = true;

    //[SerializeField] private GameObject[] commonMoveTiles;

    [Header("End")]
    [SerializeField] private GameObject lastNoise;
    [SerializeField] private GameObject forest;
    [SerializeField] private ParticleSystem bird;
    [SerializeField] private Transform terrainTarget;
    private bool isLastEnter = false;



    private bool isBadEnd2 = false;
    private bool isCanBackElevator = false;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        foreach (GameObject obj in navRoad)
            obj.SetActive(false);
        gateAnswer = new bool[6] { false, false, false, false, false, false };
        gateLeverState = new bool[6] { false, false, false, false, false, false };
        rotateTileData = new RotateTileData[3, 3];
        endCoreMat = endCoreRender.material;
        int index = -1;
        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                index += 1;
                rotateTileData[x, y] = new RotateTileData(rotateTile[index].GetChild(2).gameObject);
                //rotateTileData[x,y].outline = rotateTile[index].GetChild(2).gameObject;
            }
        }

        rotateTileData[0, 0].isRight = true;
        rotateTileData[1, 0].isLeft = true;
        rotateTileData[1, 0].isDown = true;
        rotateTileData[0, 1].isUp = true;
        rotateTileData[0, 2].isDown = true;
        rotateTileData[1, 1].isUp = true;
        rotateTileData[1, 2].isDown = true;
        rotateTileData[1, 2].isUp = true;
        CheckRotateTile();
        secondDoor.isSound = true;


        isArrowTilePos = new bool[3, 3];
        arrowTilePosVec = new Vector3[3, 3];
        index = -1;
        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                index += 1;
                arrowTilePosVec[x, y] = arrowTilePosTrans[index].position;
                isArrowTilePos[x, y] = false;
            }
        }
        arrowTilePos = new Vector2Int[3];
        arrowTilePos[0] = new Vector2Int(0, 1); //R
        arrowTilePos[1] = new Vector2Int(2, 2); //G
        arrowTilePos[2] = new Vector2Int(2, 0); //B
        isArrowTilePos[0, 1] = true;
        isArrowTilePos[2, 2] = true;
        isArrowTilePos[2, 0] = true;


        foreach (Obj_MoveColorTile tile in redMoveTiles)
            tile.SetLocalPos(redFloat);
        foreach (Obj_MoveColorTile tile in greenMoveTiles)
            tile.SetLocalPos(greenFloat);
        foreach (Obj_MoveColorTile tile in blueMoveTiles)
            tile.SetLocalPos(blueFloat);
        foreach (Obj_MoveColorTile tile in yellowMoveTiles)
            tile.SetLocalPos(yellowFloat);

        //When Test, Disable
        obj_stages[1].localPosition += new Vector3(moveDistance, 0, 0);
        obj_stages[2].localPosition += new Vector3(moveDistance, 0, 0);
        obj_stages[3].localPosition += new Vector3(moveDistance, 0, 0);
        obj_stages[4].localPosition += new Vector3(moveDistance, 0, 0);

        obj_stages[1].gameObject.SetActive(false);
        obj_stages[2].gameObject.SetActive(false);
        obj_stages[3].gameObject.SetActive(false);
        obj_stages[4].gameObject.SetActive(false);
        forest.SetActive(false);
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    Debug.Log("T");
        //    Event_ForestEnter();
        //}

        if (isLastEnter)
            return;

        if (holdState == HoldState.None)
        {
            StopMoveTileSound();
            return;
        }
        if (holdState == HoldState.Red)
        {
            if (redBool)
                redFloat += Time.deltaTime * tileMoveSpeed;
            else
                redFloat += -Time.deltaTime * tileMoveSpeed;
            if (redFloat > 6.0f)
            {
                redFloat = 6.0f;
                redBool = false;
                StopMoveTileSound();
            }
            else if (redFloat < 0.0f)
            {
                redFloat = 0.0f;
                redBool = true;
                StopMoveTileSound();
            }
            else if(redFloat > 1.0f && redFloat < 5.0f)
                StartMoveTileSound();
            else
                StopMoveTileSound();
            MoveColorTile(HoldState.Red);
        }
        else if (holdState == HoldState.Green)
        {
            if (greenBool)
                greenFloat += Time.deltaTime * tileMoveSpeed;
            else
                greenFloat += -Time.deltaTime * tileMoveSpeed;
            if (greenFloat > 6.0f)
            {
                greenFloat = 6.0f;
                greenBool = false;
                StopMoveTileSound();
            }
            else if (greenFloat < 0.0f)
            {
                greenFloat = 0.0f;
                greenBool = true;
                StopMoveTileSound();
            }
            else if (greenFloat > 1.0f && greenFloat < 5.0f)
                StartMoveTileSound();
            else
                StopMoveTileSound();
            MoveColorTile(HoldState.Green);
        }
        else if (holdState == HoldState.Blue)
        {
            if (blueBool)
                blueFloat += Time.deltaTime * tileMoveSpeed;
            else
                blueFloat += -Time.deltaTime * tileMoveSpeed;
            if (blueFloat > 6.0f)
            {
                blueFloat = 6.0f;
                blueBool = false;
                StopMoveTileSound();
            }
            else if (blueFloat < 0.0f)
            {
                blueFloat = 0.0f;
                blueBool = true;
                StopMoveTileSound();
            }
            else if (blueFloat > 1.0f && blueFloat < 5.0f)
                StartMoveTileSound();
            else
                StopMoveTileSound();
            MoveColorTile(HoldState.Blue);
        }
        else if (holdState == HoldState.Yellow)
        {
            if (yellowBool)
                yellowFloat += Time.deltaTime * tileMoveSpeed;
            else
                yellowFloat += -Time.deltaTime * tileMoveSpeed;
            if (yellowFloat > 6.0f)
            {
                yellowFloat = 6.0f;
                yellowBool = false;
                StopMoveTileSound();
            }
            else if (yellowFloat < 0.0f)
            {
                yellowFloat = 0.0f;
                yellowBool = true;
                StopMoveTileSound();
            }
            else if (yellowFloat > 1.0f && yellowFloat < 5.0f)
                StartMoveTileSound();
            else
                StopMoveTileSound();
            MoveColorTile(HoldState.Yellow);
        }
    }

    public override void ReStage()
    {
        base.ReStage();
        PlayerUnit.player.Revive();
        PlayerUnit.player.SetPos(startPos.position);
        SetQuestion();

    }
    public void SetQuestion()
    {

    }

    public void Event_FirstEnter()
    {
        isCanBackElevator = true;
        LightManager.Instance.StartWarning();
        PlayerUnit.player.SetIsCanControl(false);
        PlayerUnit.player.SetDestination(startPos.position);
        StartCoroutine(CoEvent_FirstEnter());
    }

    private IEnumerator CoEvent_FirstEnter()
    {
        yield return new WaitForSeconds(2.0f);
        
        DialogManager.Instance.ShowDialog("Dialog_Senario016_FirstEnter", null, (() =>
        {
            PlayerUnit.player.SetIsCanControl(true);
            StartCoroutine(CoStopWarning());
            if (coMusic != null)
                StopCoroutine(coMusic);
            coMusic = CoMusic(0.1f, 0);
            StartCoroutine(coMusic);
        }));
    }

    public void Event_FirstEnter2()
    {
        DialogManager.Instance.ShowDialog("Dialog_Senario016_FirstEnter2", null, (() =>
        {
            PlayerUnit.player.SetIsCanControl(true);
        }));
    }

    public void Event_OpenGate()
    {
        StartCoroutine(CoEvent_OpenGate());
    }

    private IEnumerator CoEvent_OpenGate()
    {
        yield return new WaitForSeconds(2.0f);
        DialogManager.Instance.ShowDialog("Dialog_Senario016_OpenGate");
    }

    public void Event_FirstEnter3()
    {
        DialogManager.Instance.ShowDialog("Dialog_Senario016_FirstEnter3", null, (() =>
        {
            PlayerUnit.player.SetIsCanControl(true);
        }));
    }

    

    public void Event_ShakeEnter()
    {
        hintIndex = 1;
        StartCoroutine(CoEvent_ShakeEnter());
    }

    private IEnumerator CoEvent_ShakeEnter()
    {
        yield return new WaitForSeconds(0.5f);
        PlayerUnit p = PlayerUnit.player;
        p.SetIsCanControl(false);
        yield return new WaitForSeconds(0.05f);
        SoundManager.Instance.StartSound("SE_Explosion_2", 0.8f);
        CameraManager.Instance.Shake(0.1f, 1.0f, 40);
        yield return new WaitForSeconds(0.02f);
        p.GetAnimator().SetTrigger("backdown");
        yield return new WaitForSeconds(1.85f);
        p.transform.DORotate(new Vector3(0, 270, 0), 1.0f);
        yield return new WaitForSeconds(1.0f);

        //cameraMove
        fireParticle.SetActive(true);
        moveCamTarget.position = p.transform.position;
        CameraManager.Instance.SetTarget(moveCamTarget);
        moveCamTarget.DOMove(startPos.position, 8.0f).OnComplete(() =>
        {
            StartCoroutine(CoEvent_ShakeEnter2());
        }).SetEase(Ease.Linear);
        yield return new WaitForSeconds(1.0f);
        SoundManager.Instance.StartSoundFadeLoop("SE_Fire_Loop2", 1.0f, 0.5f, 0.5f, 15.5f);
    }

    private IEnumerator CoEvent_ShakeEnter2()
    {
        yield return new WaitForSeconds(1.5f);
        PlayerUnit p = PlayerUnit.player;
        moveCamTarget.DOMove(p.transform.position, 8.0f).OnComplete(() =>
        {
            p.SetIsCanControl(true);
            CameraManager.Instance.SetTarget(p.transform);
            obj_stages[0].gameObject.SetActive(false);
            DialogManager.Instance.ShowDialog("Dialog_Senario016_ShakeEnter", null, ()=>
            {
                if (coMusic != null)
                    StopCoroutine(coMusic);
                coMusic = CoMusic(0.1f, 1);
                StartCoroutine(coMusic);
            });
        }).SetEase(Ease.Linear);
    }


    public void Event_SecondRotate()
    {
        DialogManager.Instance.ShowDialog("Dialog_Senario016_SecondRotate");
    }

    public void Event_SecondEnter2()
    {
        DialogManager.Instance.ShowDialog("Dialog_Senario016_SecondEnter2");
    }

    public void Event_ThirdEnter()
    {
        hintIndex = 2;
        DialogManager.Instance.ShowDialog("Dialog_Senario016_ThirdEnter", null, (() =>
        {
            PlayerUnit.player.SetIsCanControl(true);
        }));
    }

    public void Event_ThirdAnswer()
    {
        DialogManager.Instance.ShowDialog("Dialog_Senario016_ThirdAnswer");
    }

    public void Event_ThirdEnter2()
    {
        DialogManager.Instance.ShowDialog("Dialog_Senario016_ThirdEnter2", null, (() =>
        {
            if (coMusic != null)
                StopCoroutine(coMusic);
            coMusic = CoMusic(0.1f, 2);
            StartCoroutine(coMusic);
        }));
    }

    public void Event_FourthEnter()
    {
        hintIndex = 3;
        DialogManager.Instance.ShowDialog("Dialog_Senario016_FourthEnter");
    }

    public void Event_FourthEnter2()
    {
        DialogManager.Instance.ShowDialog("Dialog_Senario016_FourthEnter2");
    }

    public void Event_FourthEnter3()
    {
        DialogManager.Instance.ShowDialog("Dialog_Senario016_FourthEnter3");
    }

    public void Event_FourthEnter4()
    {
        DialogManager.Instance.ShowDialog("Dialog_Senario016_FourthEnter4");
    }

    public void Event_FourthEnter5()
    {
        DialogManager.Instance.ShowDialog("Dialog_Senario016_FourthEnter5");
    }

    public void Event_LastEnter()
    {
        isLastEnter = true;
        lastColorBlock.RemoveHoldOutFunc();
        if (PlayerUnit.player.GetHoldObj() == lastColorBlock)
            lastColorBlock.HoldOut();
        lastColorBlock.SetCanInteract(false);
        lastColorBlock.gameObject.SetActive(false);
        StopMoveTileSound();
        StartCoroutine(CoEvent_LastEnter());
    }

    private IEnumerator CoEvent_LastEnter()
    {
        yield return new WaitForSeconds(1.0f);
        DialogManager.Instance.ShowDialog("Dialog_Senario016_LastEnter", null, ()=>
        {
            SoundManager.Instance.StartSoundFadeLoop("SE_TVNoise_Loop", 0.5f, 0.3f, 0.3f, 2.0f);
            lastNoise.SetActive(true);
        });
    }

    private IEnumerator CoTest()
    {
        PlayerUnit p = PlayerUnit.player;
        yield return new WaitForSeconds(0.05f);
    }
    

    private IEnumerator CoStopWarning()
    {
        yield return new WaitForSeconds(2.0f);
        LightManager.Instance.StopWarning();
    }


    public void Event_BadEnd2()
    {
        if (isBadEnd2)
            return;
        if (!isCanBackElevator)
            return;
        if (startElevator.GetIsProgress())
            return;
        isBadEnd2 = true;
        StartCoroutine(BadEnd2());
        StopMusic();
    }

    private IEnumerator BadEnd2()
    {
        startElevator.ExcuteSoloBadEnd2(PlayerUnit.player, true);
        if (DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Senario016_BadEnd2");
        yield return new WaitForSeconds(2.0f);
        UIManager.Instance.StartBadEnd2();
    }

    public void Btn_MoveElevator(int index)
    {
        if (activedBtnList.Contains(index))
            return;
        if(index == 0)
        {
            startElevator.transform.parent = obj_stages[0];
            StopMusic();
        }
        //AudioSource audio = SoundManager.Instance.StartSoundFadeLoop("SE_LastElevator_Start", 0.8f, 1.0f, 0.1f, moveSpeed);
        SoundManager.Instance.StartSound("SE_LastElevator_Start", 0.8f);
        isCanBackElevator = false;
        activedBtnList.Add(index);
        if(obj_stages.Length >= index)
        {
            obj_PriNavWall[index].SetActive(true);
            obj_stages[index].DOLocalMoveX(obj_stages[index].localPosition.x - moveDistance, moveSpeed).OnComplete(() =>
            {
                //obj_stages[index].gameObject.SetActive(false);
            }).SetEase(Ease.Linear);
        }
        if(obj_stages.Length >= index+1)
        {
            obj_stages[index + 1].gameObject.SetActive(true);
            obj_stages[index + 1].DOLocalMoveX(obj_stages[index + 1].localPosition.x - moveDistance + closeMoveDistance, moveSpeed).OnComplete(() =>
            {
                obj_stages[index + 1].DOLocalMoveX(obj_stages[index + 1].localPosition.x - closeMoveDistance, closeMoveSpeed).OnComplete(() =>
                {
                    obj_NextNavWall[index].SetActive(false);
                    SoundManager.Instance.StartSound("SE_Impact_3", 0.6f);
                }).SetEase(Ease.InCubic);
            }).SetEase(Ease.Linear);
        }
    }

    
    public void Btn_LastMoveElevator()
    {
        if (activedBtnList.Contains(3))
            return;
        //AudioSource audio = SoundManager.Instance.StartSoundFadeLoop("SE_LastElevator_Start", 0.8f, 1.0f, 0.1f, moveSpeed);
        lastColorBlock.gameObject.SetActive(false);
        isCanBackElevator = false;
        activedBtnList.Add(3);
        if (obj_stages.Length >= 3)
        {
            obj_PriNavWall[3].SetActive(true);
            obj_stages[3].DOLocalMoveX(obj_stages[3].localPosition.x - moveDistance, moveSpeed).OnComplete(() =>
            {
                //obj_stages[index].gameObject.SetActive(false);
            }).SetEase(Ease.Linear);
            Event_LastEvent();
        }
        if(SoundManager.Exist())
            SoundManager.Instance.StartSound("SE_LastElevator_Start", 0.8f);
    }

    private void Event_LastEvent()
    {
        StartCoroutine(CoEvent_LastEvent());
    }

    private IEnumerator CoEvent_LastEvent()
    {
        yield return new WaitForSeconds(3.0f);
        DialogManager.Instance.ShowDialog("Dialog_Senario016_LastButton", null, (() =>
        {
            Struct_Choice[] choices = new Struct_Choice[2];
            choices[0] = new Struct_Choice(Language.Instance.Get("Narration_1016_37_1"), Event_LastChoices, 0);
            choices[1] = new Struct_Choice(Language.Instance.Get("Narration_1016_37_2"), Event_LastChoices, 1);
            DialogManager.Instance.ShowChoice(choices);
        }));
    }

    private void Event_LastChoices(int index)
    {
        if(index == 0)
        {
            DialogManager.Instance.ShowDialog("Dialog_Senario016_LastAnswer1", null, () =>
            {
                Event_LastDialog();
            });
        }
        else if(index == 1)
        {
            DialogManager.Instance.ShowDialog("Dialog_Senario016_LastAnswer2", null, () =>
            {
                Event_LastDialog();
            });
        }
    }

    private void Event_LastDialog()
    {
        if (obj_stages.Length >= 4)
        {
            obj_stages[4].gameObject.SetActive(true);
            obj_stages[4].DOLocalMoveX(obj_stages[4].localPosition.x - moveDistance + closeMoveDistance, moveSpeed).OnComplete(() =>
            {
                obj_stages[4].DOLocalMoveX(obj_stages[4].localPosition.x - closeMoveDistance, closeMoveSpeed).OnComplete(() =>
                {
                    obj_NextNavWall[3].SetActive(false);
                    SoundManager.Instance.StartSound("SE_Impact_3", 0.6f);
                }).SetEase(Ease.InCubic);
            }).SetEase(Ease.Linear);
        }
        DialogManager.Instance.ShowDialog("Dialog_Senario016_LastButton2", null, (() =>
        {
            StopMusic();
        }));
    }

    private bool isLastReady = true;
    public void Event_ForestEnter()
    {
        if (!isLastReady)
            return;
        isLastReady = false;
        UI_Ingame.isActive = false;
        UIManager.Instance.CloseGameUI();
        PlayerUnit p = PlayerUnit.player;
        p.SetIsCanControl(false);
        UIManager.Instance.FadeIn(3.0f, Color.white, null);
        SoundManager.Instance.StartSound("SE_Fade_Start", 0.8f);
        StartCoroutine(CoEvent_Forest());
    }

    private IEnumerator CoEvent_Forest()
    {
        PlayerUnit p = PlayerUnit.player;
        yield return new WaitForSeconds(3.0f);
        //obj_stages[4].gameObject.SetActive(false);
        forest.SetActive(true);
        p.SetPos(terrainTarget.position);
        yield return new WaitForSeconds(1.0f);
        p.transform.rotation = Quaternion.Euler(0, 180, 0);
        UIManager.Instance.FadeOut(3.0f, null);
        yield return new WaitForSeconds(1.2f);
        p.GetAnimator().SetTrigger("around");
        p.SetForceWalk(true);
        AudioSource audio = SoundManager.Instance.StartSoundFadeLoop("SE_River_Loop", 0.6f, 1.0f, 0, 100);
        yield return new WaitForSeconds(3.0f);
        SoundManager.Instance.StartSound("SE_Bird_1", 0.6f);
        yield return new WaitForSeconds(2.5f);
        bird.Play();
        yield return new WaitForSeconds(2.0f);
        CameraManager.Instance.SetTarget(null);
        SoundManager.Instance.StartSound("SE_Bird_2", 0.6f);
        p.SetSpeed(2, false);
        p.SetDestination(p.transform.position + new Vector3(0, 0, -3));
        yield return new WaitForSeconds(3.0f);
        p.transform.DORotate(new Vector3(0, 225.0f, 0), 0.25f, RotateMode.FastBeyond360);
        //p.transform.rotation = Quaternion.Euler(0, 225.0f, 0);
        p.GetAnimator().SetTrigger("greet");
        yield return new WaitForSeconds(2.0f);
        SoundManager.Instance.StartSound("SE_Bird_1", 0.6f);
        p.transform.DORotate(new Vector3(0, 180.0f, 0), 0.25f, RotateMode.FastBeyond360);
        yield return new WaitForSeconds(0.25f);
        //p.transform.rotation = Quaternion.Euler(0, 180, 0);
        p.SetForceWalk(false);
        p.SetSpeed(p.DefaultSpeed, false);
        p.SetDestination(p.transform.position + new Vector3(0, 0, -30));
        UIManager.Instance.StartTrueEnd();
        yield return new WaitForSeconds(2.0f);
        SoundManager.Instance.StopSound(audio, 3.0f);
    }


    #region First-Lever
    public void SwtLever(int index)
    {
        if (isFirstGateOpen)
            return;
        SwtLeverProcess(index, !gateLeverState[index]);

        if (gateLeverState[index] && !CheckGateLever())
        {
            // 0 4 2 3 1 5
            if (index == 0)
            {
                SwtLeverProcess(4, false);
            }
            else if (index == 1)
            {
                SwtLeverProcess(5, false);
            }
            else if (index == 2)
            {
                SwtLeverProcess(3, false);
            }
            else if (index == 3)
            {
                SwtLeverProcess(1, false);
            }
            else if (index == 4)
            {
                SwtLeverProcess(2, false);
            }
            else if (index == 5)
            {
                SwtLeverProcess(0, false);
            }
        }

        CheckGateLever();
    }

    private void SwtLeverProcess(int index, bool b)
    {
        gateLeverState[index] = b;
        if (gateLeverState[index])
            gateLever[index].localRotation = Quaternion.Euler(0, 0, 45);
        else
            gateLever[index].localRotation = Quaternion.Euler(0, 0, -45);
        gateAnswer[index] = gateLeverState[index];
    }

    private bool CheckGateLever()
    {
        bool isAnswer = true;
        for (int i = 0; i < gateAnswer.Length; i++)
        {
            if (gateAnswer[i])
            {
                gateLeverLight[i].material = greenMat;
            }
            else
            {
                gateLeverLight[i].material = redMat;
                isAnswer = false;
            }
        }

        //if (gateLeverState[0] && gateLeverState[1])
        //    gate_rightWall.SetActive(true);
        //else
        //    gate_rightWall.SetActive(false);

        //if (gateLeverState[4] && gateLeverState[5])
        //    gate_leftWall.SetActive(true);
        //else
        //    gate_leftWall.SetActive(false);

        if (isAnswer)
        {
            gate.Open();
            isFirstGateOpen = true;
            Event_OpenGate();
        }

        return isAnswer;
    }

    #endregion

    #region Second-Rotate

    public void RotateTile(int index)
    {
        if (isSecondRotate)
            return;

        isSecondRotate = true;
        Transform back = null;
        Transform left = null;
        Transform front = null;
        Transform right = null;
        int backIndex = 0;
        int leftIndex = 0;
        int frontIndex = 0;
        int rightIndex = 0;
        //GameObject backWall = rotateTile[index].GetChild(5).gameObject;
        //GameObject leftWall = rotateTile[index].GetChild(3).gameObject;
        //GameObject frontWall = rotateTile[index].GetChild(2).gameObject;
        //GameObject rightWall = rotateTile[index].GetChild(4).gameObject;
        //GameObject backWall2 = rotateTileArr[1, 1].GetChild(2).gameObject;
        //GameObject leftWall2 = rotateTileArr[1, 1].GetChild(4).gameObject;
        //GameObject frontWall2 = rotateTileArr[1, 1].GetChild(5).gameObject;
        //GameObject rightWall2 = rotateTileArr[1, 1].GetChild(2).gameObject;
        if (index == 0)
        {
            backIndex = 0;
            leftIndex = 3;
            frontIndex = 7;
            rightIndex = 4;
        }
        else if(index == 1)
        {
            backIndex = 1;
            leftIndex = 4;
            frontIndex = 8;
            rightIndex = 5;
        }
        else if (index == 2)
        {
            backIndex = 2;
            leftIndex = 5;
            frontIndex = 9;
            rightIndex = 6;
        }
        else if (index == 3)
        {
            backIndex = 7;
            leftIndex = 10;
            frontIndex = 14;
            rightIndex = 11;
        }
        else if (index == 4)
        {
            backIndex = 8;
            leftIndex = 11;
            frontIndex = 15;
            rightIndex = 12;
        }
        else if (index == 5)
        {
            backIndex = 9;
            leftIndex = 12;
            frontIndex = 16;
            rightIndex = 13;
        }
        else if (index == 6)
        {
            backIndex = 14;
            leftIndex = 17;
            frontIndex = 21;
            rightIndex = 18;
        }
        else if (index == 7)
        {
            backIndex = 15;
            leftIndex = 18;
            frontIndex = 22;
            rightIndex = 19;
        }
        else if (index == 8)
        {
            backIndex = 16;
            leftIndex = 19;
            frontIndex = 23;
            rightIndex = 20;
        }

        back = rotateRoad[backIndex];
        left = rotateRoad[leftIndex];
        front = rotateRoad[frontIndex];
        right = rotateRoad[rightIndex];
        rotateNavWall[backIndex].SetActive(true);
        rotateNavWall[leftIndex].SetActive(true);
        rotateNavWall[frontIndex].SetActive(true);
        rotateNavWall[rightIndex].SetActive(true);

        if (back != null)
            back.parent = rotateTile[index];
        if (left != null)
            left.parent = rotateTile[index];
        if (front != null)
            front.parent = rotateTile[index];
        if (right != null)
            right.parent = rotateTile[index];

        rotateRoad[leftIndex] = back;
        rotateRoad[frontIndex] = left;
        rotateRoad[rightIndex] = front;
        rotateRoad[backIndex] = right;

        rotateTile[index].DORotate(new Vector3(0, 90, 0), 0.5f, RotateMode.WorldAxisAdd).OnComplete(() =>
        {
            isSecondRotate = false;
            if (back != null)
                back.parent = obj_stages[1];
            if (left != null)
                left.parent = obj_stages[1];
            if (front != null)
                front.parent = obj_stages[1];
            if (right != null)
                right.parent = obj_stages[1];
            if (rotateRoad[leftIndex] != null)
                rotateNavWall[leftIndex].SetActive(false);
            if (rotateRoad[frontIndex] != null)
                rotateNavWall[frontIndex].SetActive(false);
            if (rotateRoad[rightIndex] != null)
                rotateNavWall[rightIndex].SetActive(false);
            if (rotateRoad[backIndex] != null)
                rotateNavWall[backIndex].SetActive(false);
            CheckRotateTile();
        });
        SoundManager.Instance.StartSound("SE_Fly_Move", 0.5f);

        secondRotateCount += 1;
        if(secondRotateCount == 5)
        {
            Event_SecondRotate();
        }
    }

    private void CheckRotateTile()
    {
        if (rotateRoad[4] != null)
        {
            rotateTileData[0, 0].isRight = true;
            rotateTileData[1, 0].isLeft = true;
        }
        else
        {
            rotateTileData[0, 0].isRight = false;
            rotateTileData[1, 0].isLeft = false;
        }
        if (rotateRoad[5] != null)
        {
            rotateTileData[1, 0].isRight = true;
            rotateTileData[2, 0].isLeft = true;
        }
        else
        {
            rotateTileData[1, 0].isRight = false;
            rotateTileData[2, 0].isLeft = false;
        }
        if (rotateRoad[11] != null)
        {
            rotateTileData[0, 1].isRight = true;
            rotateTileData[1, 1].isLeft = true;
        }
        else
        {
            rotateTileData[0, 1].isRight = false;
            rotateTileData[1, 1].isLeft = false;
        }
        if (rotateRoad[12] != null)
        {
            rotateTileData[1, 1].isRight = true;
            rotateTileData[2, 1].isLeft = true;
        }
        else
        {
            rotateTileData[1, 1].isRight = false;
            rotateTileData[2, 1].isLeft = false;
        }
        if (rotateRoad[18] != null)
        {
            rotateTileData[0, 2].isRight = true;
            rotateTileData[1, 2].isLeft = true;
        }
        else
        {
            rotateTileData[0, 2].isRight = false;
            rotateTileData[1, 2].isLeft = false;
        }
        if (rotateRoad[19] != null)
        {
            rotateTileData[1, 2].isRight = true;
            rotateTileData[2, 2].isLeft = true;
        }
        else
        {
            rotateTileData[1, 2].isRight = false;
            rotateTileData[2, 2].isLeft = false;
        }

        if (rotateRoad[7] != null)
        {
            rotateTileData[0, 0].isUp = true;
            rotateTileData[0, 1].isDown = true;
        }
        else
        {
            rotateTileData[0, 0].isUp = false;
            rotateTileData[0, 1].isDown = false;
        }
        if (rotateRoad[8] != null)
        {
            rotateTileData[1, 0].isUp = true;
            rotateTileData[1, 1].isDown = true;
        }
        else
        {
            rotateTileData[1, 0].isUp = false;
            rotateTileData[1, 1].isDown = false;
        }
        if (rotateRoad[9] != null)
        {
            rotateTileData[2, 0].isUp = true;
            rotateTileData[2, 1].isDown = true;
        }
        else
        {
            rotateTileData[2, 0].isUp = false;
            rotateTileData[2, 1].isDown = false;
        }
        if (rotateRoad[14] != null)
        {
            rotateTileData[0, 1].isUp = true;
            rotateTileData[0, 2].isDown = true;
        }
        else
        {
            rotateTileData[0, 1].isUp = false;
            rotateTileData[0, 2].isDown = false;
        }
        if (rotateRoad[15] != null)
        {
            rotateTileData[1, 1].isUp = true;
            rotateTileData[1, 2].isDown = true;
        }
        else
        {
            rotateTileData[1, 1].isUp = false;
            rotateTileData[1, 2].isDown = false;
        }
        if (rotateRoad[16] != null)
        {
            rotateTileData[2, 1].isUp = true;
            rotateTileData[2, 2].isDown = true;
        }
        else
        {
            rotateTileData[2, 1].isUp = false;
            rotateTileData[2, 2].isDown = false;
        }

        //rotateTileData[0, 0].isEnergy = true;
        StartRotateRoute(0,0);
    }

    private void StartRotateRoute(int startX, int startY)
    {
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                rotateTileData[x, y].isEnergy = false;
            }
        }
        CheckRotateRoute(startX, startY);

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                rotateTileData[x, y].outline.SetActive(rotateTileData[x, y].isEnergy);
            }
        }

        foreach (Transform trans in rotateRoad)
        {
            if (trans != null)
                trans.GetChild(1).gameObject.SetActive(false);
        }

        if (rotateTileData[0,0].isEnergy)
        {
            if (rotateRoad[0] != null)
                rotateRoad[0].GetChild(1).gameObject.SetActive(rotateTileData[0, 0].isEnergy);
            if (rotateRoad[3] != null)
                rotateRoad[3].GetChild(1).gameObject.SetActive(rotateTileData[0, 0].isEnergy);
            if (rotateRoad[7] != null)
                rotateRoad[7].GetChild(1).gameObject.SetActive(rotateTileData[0, 0].isEnergy);
            if (rotateRoad[4] != null)
                rotateRoad[4].GetChild(1).gameObject.SetActive(rotateTileData[0, 0].isEnergy);
        }

        if (rotateTileData[1,0].isEnergy)
        {
            if (rotateRoad[1] != null)
                rotateRoad[1].GetChild(1).gameObject.SetActive(rotateTileData[1, 0].isEnergy);
            if (rotateRoad[4] != null)
                rotateRoad[4].GetChild(1).gameObject.SetActive(rotateTileData[1, 0].isEnergy);
            if (rotateRoad[8] != null)
                rotateRoad[8].GetChild(1).gameObject.SetActive(rotateTileData[1, 0].isEnergy);
            if (rotateRoad[5] != null)
                rotateRoad[5].GetChild(1).gameObject.SetActive(rotateTileData[1, 0].isEnergy);
        }

        if (rotateTileData[2, 0].isEnergy)
        {
            if (rotateRoad[2] != null)
                rotateRoad[2].GetChild(1).gameObject.SetActive(rotateTileData[2, 0].isEnergy);
            if (rotateRoad[5] != null)
                rotateRoad[5].GetChild(1).gameObject.SetActive(rotateTileData[2, 0].isEnergy);
            if (rotateRoad[9] != null)
                rotateRoad[9].GetChild(1).gameObject.SetActive(rotateTileData[2, 0].isEnergy);
            if (rotateRoad[6] != null)
                rotateRoad[6].GetChild(1).gameObject.SetActive(rotateTileData[2, 0].isEnergy);
        }

        if (rotateTileData[0, 1].isEnergy)
        {
            if (rotateRoad[7] != null)
                rotateRoad[7].GetChild(1).gameObject.SetActive(rotateTileData[0, 1].isEnergy);
            if (rotateRoad[10] != null)
                rotateRoad[10].GetChild(1).gameObject.SetActive(rotateTileData[0, 1].isEnergy);
            if (rotateRoad[14] != null)
                rotateRoad[14].GetChild(1).gameObject.SetActive(rotateTileData[0, 1].isEnergy);
            if (rotateRoad[11] != null)
                rotateRoad[11].GetChild(1).gameObject.SetActive(rotateTileData[0, 1].isEnergy);
        }

        if (rotateTileData[1, 1].isEnergy)
        {
            if (rotateRoad[8] != null)
                rotateRoad[8].GetChild(1).gameObject.SetActive(rotateTileData[1, 1].isEnergy);
            if (rotateRoad[11] != null)
                rotateRoad[11].GetChild(1).gameObject.SetActive(rotateTileData[1, 1].isEnergy);
            if (rotateRoad[15] != null)
                rotateRoad[15].GetChild(1).gameObject.SetActive(rotateTileData[1, 1].isEnergy);
            if (rotateRoad[12] != null)
                rotateRoad[12].GetChild(1).gameObject.SetActive(rotateTileData[1, 1].isEnergy);
        }

        if (rotateTileData[2, 1].isEnergy)
        {
            if (rotateRoad[9] != null)
                rotateRoad[9].GetChild(1).gameObject.SetActive(rotateTileData[2, 1].isEnergy);
            if (rotateRoad[12] != null)
                rotateRoad[12].GetChild(1).gameObject.SetActive(rotateTileData[2, 1].isEnergy);
            if (rotateRoad[16] != null)
                rotateRoad[16].GetChild(1).gameObject.SetActive(rotateTileData[2, 1].isEnergy);
            if (rotateRoad[13] != null)
                rotateRoad[13].GetChild(1).gameObject.SetActive(rotateTileData[2, 1].isEnergy);
        }

        if (rotateTileData[0, 2].isEnergy)
        {
            if (rotateRoad[14] != null)
                rotateRoad[14].GetChild(1).gameObject.SetActive(rotateTileData[0, 2].isEnergy);
            if (rotateRoad[17] != null)
                rotateRoad[17].GetChild(1).gameObject.SetActive(rotateTileData[0, 2].isEnergy);
            if (rotateRoad[21] != null)
                rotateRoad[21].GetChild(1).gameObject.SetActive(rotateTileData[0, 2].isEnergy);
            if (rotateRoad[18] != null)
                rotateRoad[18].GetChild(1).gameObject.SetActive(rotateTileData[0, 2].isEnergy);
        }

        if (rotateTileData[1, 2].isEnergy)
        {
            if (rotateRoad[15] != null)
                rotateRoad[15].GetChild(1).gameObject.SetActive(rotateTileData[1, 2].isEnergy);
            if (rotateRoad[18] != null)
                rotateRoad[18].GetChild(1).gameObject.SetActive(rotateTileData[1, 2].isEnergy);
            if (rotateRoad[22] != null)
                rotateRoad[22].GetChild(1).gameObject.SetActive(rotateTileData[1, 2].isEnergy);
            if (rotateRoad[19] != null)
                rotateRoad[19].GetChild(1).gameObject.SetActive(rotateTileData[1, 2].isEnergy);
        }

        if (rotateTileData[2, 2].isEnergy)
        {
            if (rotateRoad[16] != null)
                rotateRoad[16].GetChild(1).gameObject.SetActive(rotateTileData[2, 2].isEnergy);
            if (rotateRoad[19] != null)
                rotateRoad[19].GetChild(1).gameObject.SetActive(rotateTileData[2, 2].isEnergy);
            if (rotateRoad[23] != null)
                rotateRoad[23].GetChild(1).gameObject.SetActive(rotateTileData[2, 2].isEnergy);
            if (rotateRoad[20] != null)
                rotateRoad[20].GetChild(1).gameObject.SetActive(rotateTileData[2, 2].isEnergy);
        }

        SwtRotateAnswer(rotateTileData[2, 2].isEnergy);
    }

    private void CheckRotateRoute(int x, int y)
    {
        if (x < 0 || y < 0 || x > 2 || y > 2)
            return;
        if (rotateTileData[x, y].isEnergy)
            return;
        rotateTileData[x, y].isEnergy = true;
        if (rotateTileData[x, y].isLeft)
            CheckRotateRoute(x - 1, y);
        if (rotateTileData[x, y].isRight)
            CheckRotateRoute(x + 1, y);
        if (rotateTileData[x, y].isUp)
            CheckRotateRoute(x, y + 1);
        if (rotateTileData[x, y].isDown)
            CheckRotateRoute(x, y - 1);
    }

    private void SwtRotateAnswer(bool b)
    {
        endCoreSound.enabled = b;
        if (b)
        {
            endCoreMat.DOFloat(10.0f, "_EmissionStrength", 1.0f);
            endCoreMat.DOFloat(5.0f, "_UVSpeed", 1.0f);
            if (!isSecondAnswer)
                SoundManager.Instance.StartSound("SE_Core_PowerOn", 0.5f);
            secondDoor.Open();
        }
        else
        {
            endCoreMat.DOFloat(1.0f, "_EmissionStrength", 1.0f);
            endCoreMat.DOFloat(1.0f, "_UVSpeed", 1.0f);
            if(isSecondAnswer)
                SoundManager.Instance.StartSound("SE_Core_PowerOff", 0.4f);
            secondDoor.Close();
        }
        isSecondAnswer = b;
        
    }

    #endregion

    #region Third-Arrow
    public void Btn_ArrowReset()
    {
        ArrowReset(false);
        PlayerUnit.player.SetPos(thirdStartPos.position);
        thirdStartPos.gameObject.SetActive(true);
        StartCoroutine(CoThirdStartDisable());
    }

    private IEnumerator CoThirdStartDisable()
    {
        yield return new WaitForSeconds(0.8f);
        thirdStartPos.gameObject.SetActive(false);
    }
    public void ArrowReset(bool isSound)
    {
        foreach(Obj_ArrowTile arrow in arrowRoute)
        {
            arrow.SetColor(ArrowColor.None);
        }
        if(arrowRoute.Count > 0 && isSound)
            SoundManager.Instance.StartSound("SE_Button_Reset", 1.0f);
        arrowRoute.Clear();
        isArrowEnd = false;
        isArrowTileMove = false;
        if (arrowTileTween != null)
            arrowTileTween.Kill();

        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                isArrowTilePos[x, y] = false;
            }
        }

        arrowMoveTilesTrans[0].position = arrowTilePosVec[0, 1];    //R
        arrowMoveTilesTrans[1].position = arrowTilePosVec[2, 2];    //G
        arrowMoveTilesTrans[2].position = arrowTilePosVec[2, 0];    //B

        arrowTilePos[0] = new Vector2Int(0, 1); //R
        arrowTilePos[1] = new Vector2Int(2, 2); //G
        arrowTilePos[2] = new Vector2Int(2, 0); //B
        isArrowTilePos[0, 1] = true;
        isArrowTilePos[2, 2] = true;
        isArrowTilePos[2, 0] = true;

        foreach(GameObject obj in arrowNavWall)
        {
            obj.SetActive(true);
        }
        arrowStartNavWall.SetActive(true);

        arrowLever.ForceLever(false);

        if (coMoveArrowTile != null)
            StopCoroutine(coMoveArrowTile);
    }


    public void EnterArrowEnd()
    {
        isArrowEnd = true;
    }

    public void EnterArrow(Obj_ArrowTile tile)
    {
        //if (arrowRoute.Contains(tile))
        //    return;

        int index = -1;
        for(int i = 0; i < arrowRoute.Count; i++)
        {
            if (tile == arrowRoute[i])
            {
                index = i;
                break;
            }
        }
        
        if(index < 0)
        {
            if (isArrowEnd)
                return;
            arrowRoute.Add(tile);
            tile.SetColor();
            SoundManager.Instance.StartSound("SE_Tile_Signal", 1.0f);
        }
        else
        {
            for(int i = arrowRoute.Count - 1; i > index; i--)
            {
                arrowRoute[i].SetColor(ArrowColor.None);
                arrowRoute.RemoveAt(i);
            }
            isArrowEnd = false;
            SoundManager.Instance.StartSound("SE_Tile_Do", 1.0f);
        }
    }

    
    public void MoveArrowTile()
    {
        if (isArrowTileMove)
            return;
        isArrowTileMove = true;
        if(coMoveArrowTile != null)
            StopCoroutine(coMoveArrowTile);
        coMoveArrowTile = CoMoveArrowTile();
        StartCoroutine(coMoveArrowTile);
    }

    private IEnumerator coMoveArrowTile = null;

    private IEnumerator CoMoveArrowTile()
    {
        if (!isArrowTileMove)
            yield break;
        arrowStartNavWall.SetActive(true);
        WaitForSeconds wait = new WaitForSeconds(1.0f);
        for(int i = 0; i < arrowRoute.Count; i++)
        {
            yield return wait;
            ArrowAngle ang = arrowRoute[i].GetAngle();
            int color = (int)arrowRoute[i].GetColor();

            if (ang == ArrowAngle.Up)
            {
                if (arrowTilePos[color].y < 2 && !isArrowTilePos[arrowTilePos[color].x, arrowTilePos[color].y + 1])
                {
                    isArrowTilePos[arrowTilePos[color].x, arrowTilePos[color].y] = false;
                    arrowTilePos[color].y += 1;
                    isArrowTilePos[arrowTilePos[color].x, arrowTilePos[color].y] = true;
                    arrowTileTween = arrowMoveTilesTrans[color].DOMove(arrowTilePosVec[arrowTilePos[color].x, arrowTilePos[color].y], 0.5f);
                    SoundManager.Instance.StartSound("SE_Fly_Move", 0.5f);
                }
            }
            else if (ang == ArrowAngle.Down)
            {
                if (arrowTilePos[color].y > 0 && !isArrowTilePos[arrowTilePos[color].x, arrowTilePos[color].y - 1])
                {
                    isArrowTilePos[arrowTilePos[color].x, arrowTilePos[color].y] = false;
                    arrowTilePos[color].y += -1;
                    isArrowTilePos[arrowTilePos[color].x, arrowTilePos[color].y] = true;
                    arrowTileTween = arrowMoveTilesTrans[color].DOMove(arrowTilePosVec[arrowTilePos[color].x, arrowTilePos[color].y], 0.5f);
                    SoundManager.Instance.StartSound("SE_Fly_Move", 0.5f);
                }
            }
            else if (ang == ArrowAngle.Left)
            {
                if (arrowTilePos[color].x > 0 && !isArrowTilePos[arrowTilePos[color].x - 1, arrowTilePos[color].y])
                {
                    isArrowTilePos[arrowTilePos[color].x, arrowTilePos[color].y] = false;
                    arrowTilePos[color].x += -1;
                    isArrowTilePos[arrowTilePos[color].x, arrowTilePos[color].y] = true;
                    arrowTileTween = arrowMoveTilesTrans[color].DOMove(arrowTilePosVec[arrowTilePos[color].x, arrowTilePos[color].y], 0.5f);
                    SoundManager.Instance.StartSound("SE_Fly_Move", 0.5f);
                }
            }
            else if (ang == ArrowAngle.Right)
            {
                if (arrowTilePos[color].x < 2 && !isArrowTilePos[arrowTilePos[color].x + 1, arrowTilePos[color].y])
                {
                    isArrowTilePos[arrowTilePos[color].x, arrowTilePos[color].y] = false;
                    arrowTilePos[color].x += 1;
                    isArrowTilePos[arrowTilePos[color].x, arrowTilePos[color].y] = true;
                    arrowTileTween = arrowMoveTilesTrans[color].DOMove(arrowTilePosVec[arrowTilePos[color].x, arrowTilePos[color].y], 0.5f);
                    SoundManager.Instance.StartSound("SE_Fly_Move", 0.5f);
                }
            }
            arrowMoveTiles[color].Arrow(ang);
        }

        arrowStartNavWall.SetActive(false);
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                arrowNavWall[x + y * 3].SetActive(!isArrowTilePos[x, y]);
            }
        }

        if (!isArrowTilePos[1,0] || !isArrowTilePos[1, 1] || !isArrowTilePos[1, 2])
        {
            if (isFirstThirdAnswer)
            {
                isFirstThirdAnswer = false;
                Event_ThirdAnswer();
            }
        }

        
    }

    #endregion


    #region Fourth-Elevator

    public enum HoldState
    {
        None,
        Red,
        Green,
        Blue,
        Yellow,
    }

    

    public void HoldOnCube(Inter_HoldColorBlock holdObj)
    {
        holdState = HoldState.None;
        foreach(MeshRenderer render in redRender)
            render.material = mat_gray;
        foreach (MeshRenderer render in greenRender)
            render.material = mat_gray;
        foreach (MeshRenderer render in blueRender)
            render.material = mat_gray;
        foreach (MeshRenderer render in yellowRender)
            render.material = mat_gray;
        holdObj.transform.parent = obj_stages[3];
    }

    public void HoldOutCube(Inter_HoldColorBlock holdObj)
    {
        HoldState targetState = HoldState.None;
        Transform target = null;
        float max = 9999;
        float dis = 0;
        int index = 0;     

        for(int i = 0; i < redTile.Length; i++)
        {
            dis = Vector3.Distance(redTile[i].position, holdObj.transform.position);
            if (dis <= 1.25f && dis < max)
            {
                max = dis;
                target = redTile[i];
                targetState = HoldState.Red;
                index = i;
            }
        }

        for (int i = 0; i < greenTile.Length; i++)
        {
            dis = Vector3.Distance(greenTile[i].position, holdObj.transform.position);
            if (dis <= 1.25f && dis < max)
            {
                max = dis;
                target = greenTile[i];
                targetState = HoldState.Green;
                index = i;
            }
        }
        for (int i = 0; i < blueTile.Length; i++)
        {
            dis = Vector3.Distance(blueTile[i].position, holdObj.transform.position);
            if (dis <= 1.25f && dis < max)
            {
                max = dis;
                target = blueTile[i];
                targetState = HoldState.Blue;
                index = i;
            }
        }
        for (int i = 0; i < yellowTile.Length; i++)
        {
            dis = Vector3.Distance(yellowTile[i].position, holdObj.transform.position);
            if (dis <= 1.25f && dis < max)
            {
                max = dis;
                target = yellowTile[i];
                targetState = HoldState.Yellow;
                index = i;
            }
        }

        if (target != null)
        {
            holdObj.transform.position = new Vector3(target.position.x, holdObj.transform.position.y, target.position.z);
            holdObj.transform.parent = target;
        }
        else
        {
            holdObj.transform.parent = obj_stages[3];
        }
        holdState = targetState;
        if(holdState == HoldState.Red)
        {
            redRender[index].material = mat_green;
        }
        else if(holdState == HoldState.Green)
        {
            greenRender[index].material = mat_green;
        }
        else if( holdState == HoldState.Blue)
        {
            blueRender[index].material = mat_green;
        }
        else if(holdState == HoldState.Yellow)
        {
            yellowRender[index].material = mat_green;
        }
    }


    private void StartMoveTileSound()
    {
        if(!moveTileAudio.isPlaying)
            moveTileAudio.Play();
    }

    private void StopMoveTileSound()
    {
        moveTileAudio.Stop();
    }
    private void MoveColorTile(HoldState color)
    {
        if(color == HoldState.Red)
        {
            foreach(Obj_MoveColorTile tile in redMoveTiles)
                tile.SetLocalPos(redFloat);
        }
        else if(color == HoldState.Green)
        {
            foreach (Obj_MoveColorTile tile in greenMoveTiles)
                tile.SetLocalPos(greenFloat);
        }
        else if( color == HoldState.Blue)
        {
            foreach (Obj_MoveColorTile tile in blueMoveTiles)
                tile.SetLocalPos(blueFloat);
        }
        else if(color == HoldState.Yellow)
        {
            foreach (Obj_MoveColorTile tile in yellowMoveTiles)
                tile.SetLocalPos(yellowFloat);
        }
    }



    #endregion


    public override void Hint()
    {
        if (hintIndex == 0)
            UIManager.Toast(Language.Instance.Get("Hint_1016_1"));
        else if(hintIndex == 1)
            UIManager.Toast(Language.Instance.Get("Hint_1016_2"));
        else if (hintIndex == 2)
            UIManager.Toast(Language.Instance.Get("Hint_1016_3"));
        else if (hintIndex == 3)
            UIManager.Toast(Language.Instance.Get("Hint_1016_4"));
        else
            base.Hint();
    }


    private IEnumerator coMusic = null;
    private IEnumerator CoMusic(float t, int index)
    {
        yield return new WaitForSeconds(t);
        if(index == 0)
            SoundManager.Instance.StartMusic("Music_Speed_Force_Loop", 0.2f, true, 1.0f);
        else if(index ==1)
            SoundManager.Instance.StartMusic("Music_Spirit_Of_The_Warrior_Loop", 0.2f, true);
        else if(index == 2)
            SoundManager.Instance.StartMusic("Music_Feel_My_Heart_Beating", 0.2f, true);
    }

    public void StopMusic()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        SoundManager.Instance.StopMusic(0.5f);
    }
}
