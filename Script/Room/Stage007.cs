using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Stage007 : Stage
{
    [SerializeField] private Obj_PlayerDetectFunc[] defaultTiles;
    private Transform[,] tiles;
    private List<List<Vector2Int>> questions;

    [SerializeField] private int questIndex = -1;
    [SerializeField] private int playerIndex = -1;
    [SerializeField] private Door door;
    [SerializeField] private UI_Page page;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    private bool isEnd = false;


    protected void Start()
    {
        base.Start();
        tiles = new Transform[6, 6];
        for(int x = 0; x < 6; x++)
        {
            for(int y = 0; y < 6; y++)
            {
                tiles[x, y] = defaultTiles[x + y * 6].transform;
            }
        }

        //SetQuestion();
    }

    public override void ReStage()
    {
        base.ReStage();
        if (coViewRoad != null)
            StopCoroutine(coViewRoad);
        SetQuestion();
        PlayerUnit.player.SetPos(startPos.position);
    }
    public void SetQuestion()
    {
        isEnd = false;
        questIndex = -1;
        playerIndex = -1;
        questions = new List<List<Vector2Int>>();
        AddQuestion(new Vector2Int(Random.Range(0, 6), 0), 6);
        for (int i = 0; i < 3; i++)
        {
            AddQuestion(GetNextQuestTile(questions[^1][^1]), 6+i);
        }

        //questions.Add(new List<Vector2Int>());
        //for(int i = 0; i < 6; i++)
        //{
        //    questions[0].Add(new Vector2Int(i, 0));
        //}
        //questions.Add(new List<Vector2Int>());
        //for (int i = 0; i < 6; i++)
        //{
        //    questions[1].Add(new Vector2Int(5-i, 1));
        //}
        NextQuestion();
        page.Clear();
        page.SetTitle("Stage007_Memorize", true);
        page.AddContent("Stage007_Content_1", true);
        page.AddContent("Stage007_Content_2", true);
        page.AddContent("Stage007_Content_3", true);
        page.AddContent("Stage007_Content_4", true);
    }

    public void NextQuestion()
    {
        questIndex += 1;
        playerIndex = -1;
        if(questIndex > 0 && questIndex < questions.Count)
            SoundManager.Instance.StartSound("SE_Tile_Success", 0.8f);

        if (questIndex >= questions.Count)
            EndQuestion();
        else
            ViewRoad();
    }

    public void ViewRoad()
    {
        if(coViewRoad != null)
            StopCoroutine(coViewRoad);
        coViewRoad = CoViewRoad();
        StartCoroutine(coViewRoad);
    }

    private IEnumerator coViewRoad = null;
    private IEnumerator CoViewRoad()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        WaitForSeconds wait2 = new WaitForSeconds(1.25f);
        while (true)
        {
            for (int i = questions[questIndex].Count - 1; i >= 0; i--)
            {
                if(i == questions[questIndex].Count - 1)
                {
                    ActiveTile(questions[questIndex][i], 2);
                    yield return wait;
                }
                else if(i == 0)
                {
                    ActiveTile(questions[questIndex][i], 1);
                    yield return wait2;
                }
                else
                {
                    ActiveTile(questions[questIndex][i], 0);
                    yield return wait;
                }
                
            }
        }
        
    }

    public void EnterTile(Transform tile)
    {
        if (isEnd)
            return;
        if (coViewRoad != null)
            StopCoroutine(coViewRoad);
        playerIndex += 1;
        if(tiles[questions[questIndex][playerIndex].x, questions[questIndex][playerIndex].y] != tile)
        {
            ReStage();
            SoundManager.Instance.StartSound("SE_Button_Reset2", 0.8f);
        }
        else
        {
            if (playerIndex == questions[questIndex].Count-1 && tile == tiles[questions[questIndex][^1].x, questions[questIndex][^1].y])
            {
                NextQuestion();
            }
            else
            {
                SoundManager.Instance.StartSound("UI_Success2", 0.5f);
            }
        }
        
        
    }

    public void ActiveTile(Vector2Int pos, int startEnd)
    {
        if (startEnd == 0)
        {
            defaultTiles[pos.x + pos.y * 6].Active(defaultColor);
            SoundManager.Instance.StartSound("SE_Tile_Signal", 0.8f);
        }
        else if (startEnd == 1)
        {
            defaultTiles[pos.x + pos.y * 6].Active(startColor);
            SoundManager.Instance.StartSound("SE_Tile_Signal_Start", 0.8f);
        }
        else if (startEnd == 2)
        {
            defaultTiles[pos.x + pos.y * 6].Active(endColor);
            SoundManager.Instance.StartSound("SE_Tile_Signal_End", 0.8f);
        }
    }

    private Vector2Int GetNextQuestTile(Vector2Int vec)
    {
        int angle = Random.Range(0, 9);
        if (angle >= 0 && angle <=1)
        {
            //right
            if (vec.x >= 5)
                return GetNextQuestTile(vec);
            return new Vector2Int(vec.x + 1, vec.y);
        }
        else if (angle == 8 || (angle >= 2 && angle <= 3))
        {
            //up
            if (vec.y >= 5)
                return GetNextQuestTile(vec);
            return new Vector2Int(vec.x, vec.y + 1);
        }
        else if (angle >= 4 && angle <= 5)
        {
            //left
            if (vec.x <= 0)
                return GetNextQuestTile(vec);
            return new Vector2Int(vec.x - 1, vec.y);
        }
        else if (angle >= 6 && angle <= 7)
        {
            //down
            if (vec.y <= 0)
                return GetNextQuestTile(vec);
            return new Vector2Int(vec.x, vec.y - 1);
        }
        return vec;
    }
    private void AddQuestion(Vector2Int startPos, int count)
    {
        List<Vector2Int> list = new List<Vector2Int>();
        list.Add(startPos);
        for(int i = 0; i < count; i++)
        {
            list.Add(GetNextQuestTile(list[i]));
        }
        questions.Add(list);

    }
    private void EndQuestion()
    {
        isEnd = true;
        door.Lock(false);
        door.Open();
        foreach(Obj_PlayerDetectFunc tile in defaultTiles)
        {
            tile.SetEndColor();
        }
        StartCoroutine(CoEndQuestion());
    }

    private IEnumerator CoEndQuestion()
    {
        yield return new WaitForSeconds(2.0f);
        if (DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Stage007_End");
    }

    public void Event_FirstEnter()
    {
        SetQuestion();
        StartCoroutine(CoEvent_FirstEnter());
    }

    private IEnumerator CoEvent_FirstEnter()
    {
        yield return new WaitForSeconds(1.0f);
        if (DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Stage007_FirstEnter");
    }
}
