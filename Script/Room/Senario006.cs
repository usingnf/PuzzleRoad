using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Senario006 : Stage
{
    public Transform picture;
    public Transform red;
    public Transform green;
    public Transform blue;
    public Transform player;
    public UI_Keypad keypad;
    [SerializeField] private UI_Page page;

    [SerializeField] private SpriteRenderer answerRender;
    [SerializeField] private Sprite[] answerSprite;
    [SerializeField] private int[] answerInt;
    [SerializeField] private Obj_Divide_CheckAngle compass;

    [SerializeField] private Transform[] trans_answer2;
    [SerializeField] private Texture2D[] answer2Texture;
    [SerializeField] private SpriteRenderer[] renders;
    
    [SerializeField] private int[] answerInt2;


    private Vector3 vec;
    private float redAngle = 0;
    public float speed = 1.0f;

    private float redFloat = 0;
    private float greenFloat = 0;
    private float blueFloat = 0;
    private int answer = 65298;

    private bool isEnd = false;

    protected void Start()
    {
        base.Start();
        player = PlayerUnit.player.transform;
        SetQuestion();
    }
    public override void ReStage()
    {
        base.ReStage();
        //PlayerUnit.player.SetPos(startPos.position);
        //PlayerUnit.player.SetIsCanControl(true);
        //PlayerUnit.player.Revive();
        SetQuestion();
    }
    public void SetQuestion()
    {
        greenFloat = Random.Range(0, 360);
        blueFloat = Random.Range(0, 360);
        speed = 60.0f;

        int rand = Random.Range(0, answerSprite.Length);
        answerRender.sprite = answerSprite[rand];
        //int rand2 = Random.Range(0, answerInt2.Length);

        //for (int x = 0; x < 4; x++)
        //{
        //    for (int y = 0; y < 4; y++)
        //    {
        //        Texture2D answer = answer2Texture[rand2];
        //        int width = (int)(answer.width * 0.25f);
        //        int height = (int)(answer.height * 0.25f);
        //        int startWidth = width * x;
        //        int startHeight = height * y;

        //        Texture2D puzzleTexture = new Texture2D(width, height);
        //        puzzleTexture.filterMode = FilterMode.Bilinear;


        //        for (int x1 = 0; x1 < width; x1++)
        //        {
        //            for (int y1 = 0; y1 < height; y1++)
        //            {
        //                puzzleTexture.SetPixel(x1, y1, answer.GetPixel(startWidth + x1, startHeight + y1));
        //            }
        //        }
        //        puzzleTexture.Apply();
        //        Sprite s = Sprite.Create(puzzleTexture, new Rect(0, 0, puzzleTexture.width, puzzleTexture.height), new Vector2(0.5f, 0.5f));
        //        renders[y * 4 + x].sprite = s;
        //        trans_answer2[y * 4 + x].localRotation = Quaternion.Euler(90, Random.Range(0, 4) * 90, -90);
        //        //mats[y * 5 + x].SetTexture("_MainTex", puzzleTexture);
        //    }
        //}

        //keypad.SetAnswer(answerInt[rand] + answerInt2[rand2]);
        //answer = answerInt[rand] + answerInt2[rand2];

        keypad.SetAnswer(answerInt[rand]);
        answer = answerInt[rand];

        page.Clear();
        page.SetTitle("Senario006_Title", true);
        page.AddContent("Senario006_Content_1", true);
        page.AddContent("Senario006_Content_2", true);
        page.AddContent("Senario006_Content_3", true);
        //page.AddContent("Senario006_Content_4", true);
    }

    void Update()
    {
        redAngle += Time.deltaTime * speed;

        vec = player.position - picture.position;
        vec.y = 0;
        vec = vec.normalized;
        green.localRotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(vec.z, vec.x) * Mathf.Rad2Deg + greenFloat));
                
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100.0f, LayerMask.GetMask("Ground")))
        {
            vec = hit.point - picture.position;
            vec.y = 0;
            vec = vec.normalized;
            blue.localRotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(vec.z, vec.x) * Mathf.Rad2Deg + blueFloat));
        }

        red.localRotation = Quaternion.Euler(new Vector3(0, 0, redAngle));

    }

    public void Event_FirstEnter()
    {
        compass.isUpdate = true;
        StartCoroutine(CoEvent_FirstEnter());
    }

    private IEnumerator CoEvent_FirstEnter()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        coMusic = CoMusic(0.1f);
        StartCoroutine(coMusic);
        yield return new WaitForSeconds(1.0f);
        if (DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Senario006_FirstEnter");

        if (coEvent != null)
            StopCoroutine(coEvent);
        coEvent = CoEvent();
        StartCoroutine(coEvent);
    }

    private IEnumerator coEvent = null;
    private IEnumerator CoEvent()
    {
        yield return new WaitForSeconds(60.0f);
        if (!isEnd)
            if (DialogManager.Exist())
                DialogManager.Instance.ShowDialog("Dialog_Senario006_Delay");
    }

    public void Event_End()
    {
        isEnd = true;
        if (coEvent != null)
            StopCoroutine(coEvent);
        if(DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Senario006_End");
        //DialogManager.Instance.StopDialog();
        StopMusic();
    }

    public override void Hint()
    {
        if (hintIndex == 0)
            UIManager.Toast(Language.Instance.Get("Hint_1006_1"));
        else
            base.Hint();
    }

    private IEnumerator coMusic = null;
    private IEnumerator CoMusic(float t)
    {
        yield return new WaitForSeconds(t);
        SoundManager.Instance.StartMusic("Music_The_Greatest_Detective", 0.2f, true);
    }

    public void StopMusic()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        SoundManager.Instance.StopMusic(0.5f);
    }
}
