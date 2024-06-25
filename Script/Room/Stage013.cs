using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Stage013 : Stage
{
    [SerializeField] private Obj_Way[] defaultWays;
    [SerializeField] private UI_Page page;
    [SerializeField] private Door door;
    //[SerializeField] private Material[] mats;
    //[SerializeField] private Texture2D[] textures;

    private Obj_Way[,] ways;
    private Transform[,] pictures;

    private bool isRotate = false;

    private int count = 0;
    private List<Obj_Way> listWay = new List<Obj_Way>();

    private bool isFirstAnswer = true;
    protected void Start()
    {
        base.Start();

        //mats = new Material[renders.Length];
        pictures = new Transform[6, 6];
        ways = new Obj_Way[6, 6];
        for(int x = 0; x < 6; x++)
        {
            for(int y = 0; y < 6; y++)
            {
                pictures[x, y] = defaultWays[x + y * 6].transform;
                ways[x, y] = defaultWays[x + y * 6];
            }
        }
        //int x = -1;
        //int y = -1;
        //for(int i =0; i < renders.Length; i++)
        //{
        //    x += 1;
        //    if (x >= 6)
        //        x = 0;
        //    if (i % 6 == 0)
        //        y += 1;
        //    mats[i] = renders[i].material;
        //    pictures[x, y] = renders[i].transform;
        //}
        SetQuestion();
    }

    public override void ReStage()
    {
        base.ReStage();
        SetQuestion();
    }

    public void SetQuestion()
    {
        ways[0, 0].SetWayType(Obj_Way.WayType.Start);
        ways[0, 0].SetAngle(0);
        ways[0, 1].SetWayType(Obj_Way.WayType.Straight);
        ways[0, 1].SetAngle(90);
        ways[0, 2].SetWayType(Obj_Way.WayType.Curve);
        ways[0, 2].SetAngle(180);
        ways[0, 3].SetWayType(Obj_Way.WayType.Straight);
        ways[0, 3].SetAngle(90);
        ways[0, 4].SetWayType(Obj_Way.WayType.Three);
        ways[0, 4].SetAngle(270);
        ways[0, 5].SetWayType(Obj_Way.WayType.Straight);
        ways[0, 5].SetAngle(90);

        ways[1, 0].SetWayType(Obj_Way.WayType.Straight);
        ways[1, 0].SetAngle(0);
        ways[1, 1].SetWayType(Obj_Way.WayType.Straight);
        ways[1, 1].SetAngle(90);
        ways[1, 2].SetWayType(Obj_Way.WayType.Curve);
        ways[1, 2].SetAngle(270);
        ways[1, 3].SetWayType(Obj_Way.WayType.Straight);
        ways[1, 3].SetAngle(90);
        ways[1, 4].SetWayType(Obj_Way.WayType.Straight);
        ways[1, 4].SetAngle(90);
        ways[1, 5].SetWayType(Obj_Way.WayType.Curve);
        ways[1, 5].SetAngle(270);

        ways[2, 0].SetWayType(Obj_Way.WayType.Curve);
        ways[2, 0].SetAngle(0);
        ways[2, 1].SetWayType(Obj_Way.WayType.Straight);
        ways[2, 1].SetAngle(90);
        ways[2, 2].SetWayType(Obj_Way.WayType.Three);
        ways[2, 2].SetAngle(90);
        ways[2, 3].SetWayType(Obj_Way.WayType.Straight);
        ways[2, 3].SetAngle(90);
        ways[2, 4].SetWayType(Obj_Way.WayType.Curve);
        ways[2, 4].SetAngle(180);
        ways[2, 5].SetWayType(Obj_Way.WayType.Straight);
        ways[2, 5].SetAngle(90);

        ways[3, 0].SetWayType(Obj_Way.WayType.Curve);
        ways[3, 0].SetAngle(270);
        ways[3, 1].SetWayType(Obj_Way.WayType.Straight);
        ways[3, 1].SetAngle(90);
        ways[3, 2].SetWayType(Obj_Way.WayType.Straight);
        ways[3, 2].SetAngle(90);
        ways[3, 3].SetWayType(Obj_Way.WayType.Curve);
        ways[3, 3].SetAngle(270);
        ways[3, 4].SetWayType(Obj_Way.WayType.Three);
        ways[3, 4].SetAngle(0);
        ways[3, 5].SetWayType(Obj_Way.WayType.Curve);
        ways[3, 5].SetAngle(180);

        ways[4, 0].SetWayType(Obj_Way.WayType.Straight);
        ways[4, 0].SetAngle(90);
        ways[4, 1].SetWayType(Obj_Way.WayType.Straight);
        ways[4, 1].SetAngle(0);
        ways[4, 2].SetWayType(Obj_Way.WayType.Straight);
        ways[4, 2].SetAngle(90);
        ways[4, 3].SetWayType(Obj_Way.WayType.Straight);
        ways[4, 3].SetAngle(0);
        ways[4, 4].SetWayType(Obj_Way.WayType.Three);
        ways[4, 4].SetAngle(270);
        ways[4, 5].SetWayType(Obj_Way.WayType.Straight);
        ways[4, 5].SetAngle(0);

        ways[5, 0].SetWayType(Obj_Way.WayType.Straight);
        ways[5, 0].SetAngle(90);
        ways[5, 1].SetWayType(Obj_Way.WayType.Three);
        ways[5, 1].SetAngle(270);
        ways[5, 2].SetWayType(Obj_Way.WayType.Curve);
        ways[5, 2].SetAngle(270);
        ways[5, 3].SetWayType(Obj_Way.WayType.Straight);
        ways[5, 3].SetAngle(0);
        ways[5, 4].SetWayType(Obj_Way.WayType.Straight);
        ways[5, 4].SetAngle(90);
        ways[5, 5].SetWayType(Obj_Way.WayType.End);
        ways[5, 5].SetAngle(0);

        //for (int i = 0; i < 1; i++)
        //{
        //    for (int j = 0; j < 12; j++)
        //    {
        //        if (Random.Range(0, 2) == 0)
        //            TurnPictureForce(j);
        //    }
        //}
        int rand = Random.Range(0, 3);
        if (rand == 0)
        {
            TurnPictureForce(0);
            TurnPictureForce(2);
            TurnPictureForce(5);
            TurnPictureForce(6);
            TurnPictureForce(8);
            TurnPictureForce(9);
            TurnPictureForce(11);
        }
        else if (rand == 1)
        {
            TurnPictureForce(1);
            TurnPictureForce(3);
            TurnPictureForce(5);
            TurnPictureForce(6);
            TurnPictureForce(6);
            TurnPictureForce(8);
            TurnPictureForce(8);
            TurnPictureForce(9);
            TurnPictureForce(9);
            TurnPictureForce(9);
            TurnPictureForce(10);
        }
        else if (rand == 2)
        {
            TurnPictureForce(1);
            TurnPictureForce(3);
            TurnPictureForce(3);
            TurnPictureForce(4);
            TurnPictureForce(5);
            TurnPictureForce(7);
            TurnPictureForce(10);
            TurnPictureForce(10);
        }

        foreach (Obj_Way way in ways)
        {
            way.transform.rotation = Quaternion.Euler(0, way.angle, 0);
            //way.SetAngle(way.transform.rotation.eulerAngles.y);
            //way.transform.DORotate(new Vector3(0, way.angle, 0), 0.5f, RotateMode.Fast);
        }

        page.Clear();
        page.SetTitle("Stage013_Route", true);
        page.AddContent("Stage013_Content_1", true);
        page.AddContent("Stage013_Content_2", true);

        if (!CheckAnswer(true))
        {
            SetQuestion();
        }

        //List<int> list = new List<int>();
        //for (int i = 0; i < textures.Length; i++)
        //{
        //    list.Add(i);
        //}
        //int temp = 0;
        //for (int i = 0; i < textures.Length; i++)
        //{
        //    int rand = Random.Range(0, textures.Length);
        //    temp = list[rand];
        //    list[rand] = list[i];
        //    list[i] = temp;
        //}

        //for (int x = 0; x < 6; x++)
        //{
        //    for (int y = 0; y < 6; y++)
        //    {
        //        Texture2D answer = textures[list[0]];
        //        int width = (int)(answer.width / 6);
        //        int height = (int)(answer.height / 6);
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
        //        mats[y * 6 + x].SetTexture("_MainTex", puzzleTexture);
        //    }
        //}
    }

    public bool CheckAnswer(bool isStart)
    {
        if(StartFindRoute(0, 0))
        {
            door.Lock(false);
            door.Open();
            if(isFirstAnswer)
            {
                isFirstAnswer = false;
                StartCoroutine(CoFirst_Answer());
                StopMusic();
            }
            if(!isStart)
                SoundManager.Instance.StartSound("UI_Success3", 1.0f);
            return false;
        }
        else
        {
            door.Close();
            door.Lock(true);
        }

        return true;
    }

    private IEnumerator CoFirst_Answer()
    {
        yield return new WaitForSeconds(1);
        if (DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Stage013_End");
    }

    public void TurnPicture(int index)
    {
        if (isRotate)
            return;
        isRotate = true;
        if(index < 6)
        {
            for(int i = 0; i < 6; i++)
            {
                //pictures[index, i].Rotate(new Vector3(0, 90, 0));
                ways[index, i].SetAngle(pictures[index, i].rotation.eulerAngles.y + 90);
                pictures[index, i].DORotate(new Vector3(0, 90, 0), 0.5f, RotateMode.LocalAxisAdd).OnComplete(() =>
                {
                    isRotate = false;
                });
            }
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                //pictures[index, i].Rotate(new Vector3(0, 90, 0));
                ways[i, index - 6].SetAngle(pictures[i, index-6].rotation.eulerAngles.y + 90);
                pictures[i, index-6].DORotate(new Vector3(0, 90, 0), 0.5f, RotateMode.LocalAxisAdd).OnComplete(() =>
                {
                    isRotate = false;
                });
            }
        }
        CheckAnswer(false);
    }

    public void TurnPictureForce(int index)
    {
        if (index < 6)
        {
            for (int i = 0; i < 6; i++)
            {
                //pictures[index, i].Rotate(new Vector3(0, 90, 0));
                ways[index, i].SetAngle(ways[index, i].angle + 90);
                //pictures[index, i].DORotate(new Vector3(0, 90, 0), 0.5f, RotateMode.LocalAxisAdd).OnComplete(() =>
                //{
                //    isRotate = false;
                //});
            }
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                //pictures[i, index - 6].Rotate(new Vector3(0, 90, 0));
                //ways[i, index - 6].SetAngle(pictures[i, index - 6].rotation.eulerAngles.y + 90);
                ways[i, index - 6].SetAngle(ways[i, index - 6].angle + 90);
                //pictures[i, index - 6].DORotate(new Vector3(0, 90, 0), 0.5f, RotateMode.LocalAxisAdd).OnComplete(() =>
                //{
                //    isRotate = false;
                //});
            }
        }
    }

    private bool StartFindRoute(int x, int y)
    {
        listWay = new List<Obj_Way>();
        count = 0;
        return FindRoute(x, y);
    }
    
    private bool FindRoute(int x, int y)
    {
        count += 1;
        if (count >= 1000)
            return false;
        //if (x < 0 || y < 0)
        //    return false;
        //if (x > 5 || y > 5)
        //    return false;
        //Debug.Log(x + "/" + y);
        if (listWay.Contains(ways[x, y]))
            return false;

        listWay.Add(ways[x,y]);
        if (ways[x, y].wayType == Obj_Way.WayType.End)
            return true;
        if (x < 5 && ways[x, y].right && ways[x + 1,y].left)
            if (FindRoute(x + 1, y))
                return true;
        if (y < 5 && ways[x, y].up && ways[x, y + 1].down)
            if (FindRoute(x, y + 1))
                return true;
        if (x > 0 && ways[x, y].left && ways[x - 1, y].right)
            if (FindRoute(x - 1, y))
                return true;
        if (y > 0 && ways[x, y].down && ways[x, y - 1].up)
            if (FindRoute(x, y - 1))
                return true;
        return false;
    }

    public void Event_FirstEnter()
    {
        StartCoroutine(CoEvent_FirstEnter());
    }

    private IEnumerator CoEvent_FirstEnter()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        coMusic = CoMusic(1.0f);
        StartCoroutine(coMusic);
        yield return new WaitForSeconds(1.0f);
        if (DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Stage013_FirstEnter");
    }

    private IEnumerator coMusic = null;
    private IEnumerator CoMusic(float t)
    {
        yield return new WaitForSeconds(t);
        SoundManager.Instance.StartMusic("Music_Happiness_In_Your_Life_Loop_1", 0.2f, true);
    }

    public void StopMusic()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        SoundManager.Instance.StopMusic(0.5f);
    }
}
