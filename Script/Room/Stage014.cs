using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class Stage014 : Stage
{
    public enum Color14
    {
        None = 0,
        Red = 1,
        Green = 2,
        Blue = 3,
        Yellow = 4,
    }

    [SerializeField] private UI_Page page;
    [SerializeField] private Door door;
    [SerializeField] private MeshRenderer[] renders;
    private Material[,] mats;
    private Color14[,] tiles;
    private Vector4 answer_red;
    private Vector4 answer_green;
    private Vector4 answer_blue;
    private Vector4 answer_yellow;

    private Color14 selectColor;
    private bool isGetColor = false;

    private bool isFirstAnswer = true;
    protected void Start()
    {
        base.Start();
        mats = new Material[7, 7];
        tiles = new Color14[7, 7];
        int index = -1;
        for(int y = 0; y < 7; y++)
        {
            for(int x = 0; x < 7; x++)
            {
                index += 1;
                mats[x, y] = renders[index].material;
            }
        }
        answer_red.x = 2;
        answer_red.y = 5;
        answer_red.z = 4;
        answer_red.w = 1;
        answer_green.x = 5;
        answer_green.y = 3;
        answer_green.z = 4;
        answer_green.w = 6;
        answer_blue.x = 5;
        answer_blue.y = 1;
        answer_blue.z = 5;
        answer_blue.w = 5;
        answer_yellow.x = 3;
        answer_yellow.y = 3;
        answer_yellow.z = 5;
        answer_yellow.w = 6;
        SetQuestion();
    }

    public override void ReStage()
    {
        base.ReStage();
        SetQuestion();
    }
    public void SetQuestion()
    {
        tiles[2, 5] = Color14.Red;
        mats[2, 5].SetColor("_Color", Color.red);
        tiles[4, 1] = Color14.Red;
        mats[4, 1].SetColor("_Color", Color.red);

        tiles[5, 3] = Color14.Green;
        mats[5, 3].SetColor("_Color", Color.green);
        tiles[4, 6] = Color14.Green;
        mats[4, 6].SetColor("_Color", Color.green);

        tiles[5, 1] = Color14.Blue;
        mats[5, 1].SetColor("_Color", Color.blue);
        tiles[5, 5] = Color14.Blue;
        mats[5, 5].SetColor("_Color", Color.blue);

        tiles[3, 3] = Color14.Yellow;
        mats[3, 3].SetColor("_Color", Color.yellow);
        tiles[5, 6] = Color14.Yellow;
        mats[5, 6].SetColor("_Color", Color.yellow);



        page.Clear();
        page.SetTitle("Stage014_ColorLine", true);
        page.AddContent("Stage014_Content_1", true);
        page.AddContent("Stage014_Content_2", true);
    }

    private int maxLoop = 1000;
    List<Vector2Int> checkList = new List<Vector2Int>();
    private void CheckAnswer()
    {
        maxLoop = 1000;
        checkList.Clear();
        if (!CheckColor(Color14.Red, (int)answer_red.x, (int)answer_red.y, (int)answer_red.z, (int)answer_red.w))
        {
            Incorrect();
            return;
        }
        checkList.Clear();
        if (!CheckColor(Color14.Green, (int)answer_green.x, (int)answer_green.y, (int)answer_green.z, (int)answer_green.w))
        {
            Incorrect();
            return;
        }
        checkList.Clear();
        if (!CheckColor(Color14.Blue, (int)answer_blue.x, (int)answer_blue.y, (int)answer_blue.z, (int)answer_blue.w))
        {
            Incorrect();
            return;
        }
        checkList.Clear();
        if (!CheckColor(Color14.Yellow, (int)answer_yellow.x, (int)answer_yellow.y, (int)answer_yellow.z, (int)answer_yellow.w))
        {
            Incorrect();
            return;
        }
        door.Lock(false);
        door.Open();
        if(isFirstAnswer)
        {
            Event_End();
            isFirstAnswer = false;
            SoundManager.Instance.StartSound("UI_Success3", 1.0f);
            StopMusic();
        }
    }

    

    private void Incorrect()
    {
        door.Close();
        door.Lock(true);
    }

    private bool CheckColor(Color14 c, int x, int y, int endX, int endY)
    {
        //Debug.Log(c.ToString() + "/" + x + ":" + y + "/" + endX + ":" + endY);
        if (maxLoop < 0)
            return false;
        maxLoop -= 1;
        
        if(x < 0)
            return false;
        if(x >= 7)
            return false;
        if (y < 0)
            return false;
        if (y >= 7)
            return false;
        if (tiles[x, y] != c)
            return false;
        if (checkList.Contains(new Vector2Int(x, y)))
            return false;
        if (x == endX && y == endY)
            return true;
        checkList.Add(new Vector2Int(x, y));
        if (CheckColor(c, x + 1, y, endX, endY))
            return true;
        if (CheckColor(c, x - 1, y, endX, endY))
            return true;
        if (CheckColor(c, x, y + 1, endX, endY))
            return true;
        if (CheckColor(c, x, y - 1, endX, endY))
            return true;

        return false;
    }

    public void SetColor(int color)
    {
        selectColor = (Color14)color;
    }

    public void SetIsGet(bool b)
    {
        isGetColor = b;
    }

    public void EnterTile(int index)
    {
        if (index < 0)
            return;
        if (!isGetColor)
            return;
        if(selectColor == Color14.None) 
            return;

        if(selectColor == Color14.Red)
            mats[index % 7, (int)(index / 7)].SetColor("_Color", Color.red);
        else if(selectColor == Color14.Green)
            mats[index % 7, (int)(index / 7)].SetColor("_Color", Color.green);
        else if (selectColor == Color14.Blue)
            mats[index % 7, (int)(index / 7)].SetColor("_Color", Color.blue);
        else if (selectColor == Color14.Yellow)
            mats[index % 7, (int)(index / 7)].SetColor("_Color", Color.yellow);
        Color14 tc = tiles[index % 7, (int)(index / 7)];
        tiles[index % 7, (int)(index / 7)] = selectColor;
        if (tc != selectColor)
            CheckAnswer();
        SoundManager.Instance.StartSound("SE_Tile_Signal", 1.0f);
    }

    public void Event_FirstEnter()
    {
        StartCoroutine(CoEvent_FirstEnter());
    }

    private IEnumerator CoEvent_FirstEnter()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        coMusic = CoMusic(0.5f);
        StartCoroutine(coMusic);
        yield return new WaitForSeconds(1.0f);
        if (DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Stage014_FirstEnter");
    }

    private void Event_End()
    {
        if (!isFirstAnswer)
            return;
        StartCoroutine(CoEvent_End());
    }
    private IEnumerator CoEvent_End()
    {
        yield return new WaitForSeconds(1.0f);
        if (DialogManager.Exist())
        {
            DialogManager.Instance.ShowDialog("Dialog_Stage014_End");
        }
    }

    private IEnumerator coMusic = null;
    private IEnumerator CoMusic(float t)
    {
        yield return new WaitForSeconds(t);
        SoundManager.Instance.StartMusic("Music_First_Steps", 0.2f, true);
    }

    public void StopMusic()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        SoundManager.Instance.StopMusic(0.5f);
    }
}
