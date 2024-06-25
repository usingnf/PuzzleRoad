using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Senario011 : Stage
{
    [SerializeField] private Obj_KeyWay[] defaultWays;
    private Obj_KeyWay[,] ways;
    [SerializeField] private int maxX;
    [SerializeField] private int maxY;

    [SerializeField] private Door door;
    [SerializeField] private GameObject detector_end;
    private bool isFirstOpen = false;
    protected void Start()
    {
        base.Start();
        SetQuestion();
    }
    public override void ReStage()
    {
        base.ReStage();
        //PlayerUnit.player.SetPos(startPos.position);
        SetQuestion();
    }
    public void SetQuestion()
    {
        ways = new Obj_KeyWay[maxX, maxY];
        for(int x = 0; x < maxX; x++)
        {
            for(int y = 0; y < maxY;  y++)
            {
                ways[x, y] = defaultWays[y * maxX + x];
                ways[x, y].x = x;
                ways[x, y].y = y;
            }
        }
    }

    public Obj_KeyWay GetWay(int x, int y, Obj_KeyWay.WayAngle playerAngle, int distance)
    {
        int playerX = x;
        int playerY = y;
        if(playerAngle == Obj_KeyWay.WayAngle.Left)
        {
            if (x > 0)
                playerX -= 1;
        }
        else if(playerAngle == Obj_KeyWay.WayAngle.Right)
        {
            if (x + 1 < maxX)
                playerX += 1;
        }
        else if(playerAngle == Obj_KeyWay.WayAngle.Up)
        {
            if (y + 1 < maxY)
                playerY += 1;
        }
        else if(playerAngle == Obj_KeyWay.WayAngle.Down)
        {
            if (y > 0)
                playerY -= 1;
        }
        Obj_KeyWay playerWay = ways[playerX,playerY];
        Vector3 playerPos = playerWay.transform.position;
        List<Obj_KeyWay> inWays = new List<Obj_KeyWay>();
        List<Obj_KeyWay> nextWays = new List<Obj_KeyWay>();
        List<Obj_KeyWay> tempWays;
        nextWays.Add(ways[x,y]);
        inWays.Add(ways[x, y]);
        for (int i = 0; i < distance; i++)
        {
            tempWays = new List<Obj_KeyWay>();
            foreach(Obj_KeyWay way in nextWays)
            {
                if(way == playerWay)
                    continue;
                //Debug.Log(way.x + "/" + way.y);
                if (way.x + 1 < maxX && 
                    ways[way.x, way.y].isOpen[(int)Obj_KeyWay.WayAngle.Right] && 
                    ways[way.x + 1, way.y].isOpen[(int)Obj_KeyWay.WayAngle.Left] && 
                    !inWays.Contains(ways[way.x + 1, way.y]) && 
                    playerWay != ways[way.x + 1, way.y])
                {
                    inWays.Add(ways[way.x + 1, way.y]);
                    tempWays.Add(ways[way.x + 1, way.y]);
                }
                if (way.x - 1 >= 0 && 
                    ways[way.x, way.y].isOpen[(int)Obj_KeyWay.WayAngle.Left] && 
                    ways[way.x - 1, way.y].isOpen[(int)Obj_KeyWay.WayAngle.Right] && 
                    !inWays.Contains(ways[way.x - 1, way.y]) &&
                    playerWay != ways[way.x - 1, way.y])
                {
                    inWays.Add(ways[way.x - 1, way.y]);
                    tempWays.Add(ways[way.x - 1, way.y]);
                }
                if (way.y + 1 < maxY &&
                    ways[way.x, way.y].isOpen[(int)Obj_KeyWay.WayAngle.Up] &&
                    ways[way.x, way.y + 1].isOpen[(int)Obj_KeyWay.WayAngle.Down] &&
                    !inWays.Contains(ways[way.x, way.y + 1]) &&
                    playerWay != ways[way.x, way.y + 1])
                {
                    inWays.Add(ways[way.x, way.y + 1]);
                    tempWays.Add(ways[way.x, way.y + 1]);
                }
                if (way.y - 1 >= 0 &&
                    ways[way.x, way.y].isOpen[(int)Obj_KeyWay.WayAngle.Down] &&
                    ways[way.x, way.y - 1].isOpen[(int)Obj_KeyWay.WayAngle.Up] &&
                    !inWays.Contains(ways[way.x, way.y - 1]) &&
                    playerWay != ways[way.x, way.y - 1])
                {
                    inWays.Add(ways[way.x, way.y - 1]);
                    tempWays.Add(ways[way.x, way.y - 1]);
                }
            }
            //nextWays.Clear();
            nextWays = new List<Obj_KeyWay>(tempWays);
        }

        float max = -1;
        float dis = 0;
        Obj_KeyWay target = null;
        foreach(Obj_KeyWay way in inWays)
        {
            dis = Vector3.Distance(way.transform.position, playerPos);
            if(dis > max)
            {
                max = dis;
                target = way;
            }
        }
        return target;
    }

    
    public void OpenDoor()
    {
        door.Lock(false);
        if(!isFirstOpen)
        {
            isFirstOpen = true;
            DialogManager.Instance.ShowDialog("Dialog_Senario011_Catch");
        }
    }

    public void Event_FirstEnter()
    {
        StartCoroutine(CoEvent_FirstEnter());
    }

    private IEnumerator CoEvent_FirstEnter()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        coMusic = CoMusic(0.1f);
        StartCoroutine(coMusic);
        yield return new WaitForSeconds(1.0f);
        if(DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Senario011_FirstEnter");
    }

    public void Event_End()
    {
        if (!isFirstOpen)
            return;
        detector_end.SetActive(false);
        StopMusic();
    }

    public override void Hint()
    {
        if (hintIndex == 0)
            UIManager.Toast(Language.Instance.Get("Hint_1011_1", MyKey.Interact.ToString()));
        else
            base.Hint();
    }

    private IEnumerator coMusic = null;
    private IEnumerator CoMusic(float t)
    {
        yield return new WaitForSeconds(t);
        SoundManager.Instance.StartMusic("Music_Just_Smile!", 0.2f, true);
    }

    public void StopMusic()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        SoundManager.Instance.StopMusic(0.5f);
    }
}
