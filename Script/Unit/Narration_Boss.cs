using DG.Tweening;
using RayFire;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

//1 : 계단
//2 : 무작위 + 십자가
//3 : 1 + 2
//4 : 레이저
//5 : 레이저2
//6 : 쿵! + grid
//7 : 레이저 + grid
//8 : 레이저2 + grid
//9 : 쿵 + grid + 레이저
//10 : 쿵 + grid + 레이저2
//11 : grid + 레이저

public class Narration_Boss : MonoBehaviour
{
    [SerializeField] private SenarioTrue stage;
    [SerializeField] private Transform boss;

    [SerializeField] private Transform centerPos;
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;
    [SerializeField] private Transform appearPos;
    [SerializeField] private Transform target;

    [SerializeField] private float angle;
    [SerializeField] private float distance;

    [SerializeField][ColorUsage(true, true)] private Color normalColor;
    [SerializeField][ColorUsage(true, true)] private Color attackColor;
    [SerializeField] private MeshRenderer render;

    [SerializeField] private Boss_GridTrapGroup gridTrap;
    [SerializeField] private Boss_LazerTrapGroup lazerTrap;
    [SerializeField] private Boss_LazerTrapGroup2 lazerTrap2;
    [SerializeField] private Boss_RockTrapGroup rockTrap;
    [SerializeField] private int progress = 0;

    private Material mat;

    private bool isFight = false;
    private bool isLookAt = false;
    private float speed = 1.0f;
    private int fightCount = 0;
    void Start()
    {
        mat = render.materials[5];
    }

    // Update is called once per frame
    void Update()
    {
        if(isLookAt)
            boss.LookAt(target.position + new Vector3(0, 2, 0));
        if (!isFight)
            return;
        boss.position = Vector3.Lerp(boss.position, centerPos.position + new Vector3(0, 3, 0) + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad),0, Mathf.Sin(angle * Mathf.Deg2Rad)) * distance, Time.deltaTime*speed);
    }

    public void Appear()
    {
        this.gameObject.SetActive(true);
        boss.gameObject.SetActive(true);
        boss.position = startPos.position;
        boss.rotation = Quaternion.Euler(0, 180, 0);
        boss.DOMove(endPos.position, 1.0f).OnComplete(() =>
        {
            boss.position = appearPos.position + new Vector3(0,10,0);
            boss.LookAt(PlayerUnit.player.transform.position + new Vector3(0, 2, 0));
            target = PlayerUnit.player.transform;
            isLookAt = true;
            
            StartCoroutine(CoAppear2());
        }).SetEase(Ease.Linear);
        StartCoroutine(CoAppear1());
    }

    private IEnumerator CoAppear1()
    {
        yield return new WaitForSeconds(0.2f);
        SoundManager.Instance.StartSound("SE_SonicBoom_1", 0.8f);
    }

    private IEnumerator CoAppear2()
    {
        yield return new WaitForSeconds(1.5f);
        boss.DOMove(appearPos.position, 2.0f).OnComplete(() =>
        {
            if (fightCount == 0)
                DialogManager.Instance.ShowDialog("Dialog_Senario016_BossStart1", null, stage.Start_Fight);
            else
                DialogManager.Instance.ShowDialog("Dialog_Senario016_BossStart2", null, stage.Start_Fight);
        }).SetEase(Ease.Linear);
    }

    public void StartFight()
    {
        if (fightCount == 0)
            UIManager.Toast(Language.Instance.Get("UI_Toast_BossTip"));
        angle = 90;
        isFight = true;
        fightCount += 1;
        progress = 0;
        if(coPattern != null)
            StopCoroutine(coPattern);
        coPattern = CoPattern();
        StartCoroutine(coPattern);
    }

    public void DisAppear()
    {
        isFight = false;
        isLookAt = false;
        boss.DOMove(boss.transform.position + new Vector3(0, 20, 0), 2.5f).OnComplete(()=>
        {
            boss.gameObject.SetActive(false);
        }).SetEase(Ease.Linear);
    }

    public void AttackEye(float t = 1.0f)
    {
        if(coEye != null)
            StopCoroutine(coEye);
        coEye = CoEye(t);
        StartCoroutine(coEye);
    }

    private IEnumerator coEye = null;
    private IEnumerator CoEye(float t)
    {
        mat.DOColor(attackColor, "_EmissionColor", 0.25f);
        yield return new WaitForSeconds(t + 0.25f);
        mat.DOColor(normalColor, "_EmissionColor", 0.25f);
    }

    public void PerfectEnd()
    {
        isFight = false;
        isLookAt = false;
        this.gameObject.SetActive(false);
        StopAllCoroutines();
    }

    public void SetProgress(int progress)
    {
        this.progress = progress;
    }

    private void RandomMove()
    {
        float ang = Random.Range(45, 315);
        AngleMove(ang);
    }
    private void AngleMove(float ang)
    {
        this.angle = ang;
        SoundManager.Instance.StartSound("SE_AirMove_2", 1.0f);
    }

    private IEnumerator coPattern = null;
    private IEnumerator CoPattern()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        int accTime = 0;
        int patternCount = 0;
        Transform p = PlayerUnit.player.transform;
        Vector3 vec;
        yield return new WaitForSeconds(1.0f);
        while(true)
        {
            accTime += 1;
            if(progress == 0)
            {
                if(accTime >= 10)
                {
                    accTime = 0;
                    RandomMove();
                    yield return Pattern01(Random.Range(0, 4));
                }
            }
            else if(progress == 1)
            {
                if(patternCount % 2 == 0)
                {
                    if(accTime >= 10)
                    {
                        accTime = 0;
                        patternCount += 1;
                        RandomMove();
                        yield return Pattern02(20);
                    }
                }
                else
                {
                    if (accTime >= 10)
                    {
                        accTime = 0;
                        patternCount += 1;
                        RandomMove();
                        yield return Pattern03();
                    }
                }
            }
            else if(progress == 2)
            {
                if (accTime >= 10)
                {
                    accTime = 0;
                    RandomMove();
                    StartCoroutine(Pattern02(20));
                    yield return Pattern01(Random.Range(0, 4));
                }
            }
            else if(progress == 3)
            {
                if(patternCount % 2 == 0)
                {
                    if (accTime >= 10)
                    {
                        accTime = 0;
                        patternCount += 1;
                        RandomMove();
                        yield return Pattern04();
                    }
                }
                else
                {
                    if (accTime >= 10)
                    {
                        accTime = 0;
                        patternCount += 1;
                        RandomMove();
                        yield return Pattern05();
                    }
                }
                
            }
            else if(progress == 4)
            {
                if (accTime >= 10)
                {
                    accTime = 0;
                    patternCount += 1;
                    RandomMove();
                    yield return Pattern06();
                }
            }
            else if(progress == 5)
            {
                if (accTime >= 10)
                {
                    accTime = 0;
                    patternCount += 1;
                    RandomMove();
                    yield return Pattern07();
                }
            }
            else if (progress == 6)
            {
                if (accTime >= 10)
                {
                    accTime = 0;
                    patternCount += 1;
                    RandomMove();
                    yield return Pattern08();
                }
            }
            else if (progress == 7)
            {
                if (accTime >= 10)
                {
                    accTime = 0;
                    patternCount += 1;
                    RandomMove();
                    yield return Pattern09();
                }
            }
            else if (progress == 8)
            {
                if (accTime >= 10)
                {
                    accTime = 0;
                    patternCount += 1;
                    RandomMove();
                    yield return Pattern10();
                }
            }
            else if (progress == 9)
            {
                if (accTime >= 10)
                {
                    accTime = 0;
                    patternCount += 1;
                    RandomMove();
                    yield return Pattern11();
                }
            }
            else if (progress >= 10)
            {
                if (accTime >= 10)
                {
                    accTime = 0;
                    patternCount += 1;
                    RandomMove();
                    yield return Pattern12();
                }
            }
            yield return wait;
        }
    }

    private IEnumerator Pattern01(int ang)
    {
        AttackEye(2.0f);
        WaitForSeconds wait = new WaitForSeconds(1.0f);
        if(ang == 0)
        {
            for(int x = 0; x < 10; x++)
            {
                for(int y = 0; y < 10; y++)
                {
                    if(y == 0)
                        GridAttack(x, y, 2, true);
                    else
                        GridAttack(x, y, 2, false);
                }
                yield return wait;
            }
        }
        else if(ang == 1)
        {
            for (int y = 9; y >= 0; y--)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (x == 0)
                        GridAttack(x, y, 2, true);
                    else
                        GridAttack(x, y, 2, false);
                }
                yield return wait;
            }
        }
        else if (ang == 2)
        {
            for (int x = 9; x >= 0; x--)
            {
                for (int y = 0; y < 10; y++)
                {
                    if (y == 0)
                        GridAttack(x, y, 2, true);
                    else
                        GridAttack(x, y, 2, false);
                }
                yield return wait;
            }
        }
        else
        {
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (x == 0)
                        GridAttack(x, y, 2, true);
                    else
                        GridAttack(x, y, 2, false);
                }
                yield return wait;
            }
        }
        yield return new WaitForSeconds(1.0f);
    }

    private IEnumerator Pattern02(int count)
    {
        AttackEye(2.0f);
        WaitForSeconds wait = new WaitForSeconds(0.25f);
        for(int i = 0; i < count; i++)
        {
            if (i % 5 == 4)
                GridAttackRandom(0, 0, 2.0f);
            else
            {
                GridAttackRandom(Random.Range(-2, 3), Random.Range(-2, 3), 2.0f);
                GridAttackRandom(Random.Range(-2, 3), Random.Range(-2, 3), 2.0f, false);
            }
            yield return wait;
        }
        yield return new WaitForSeconds(1.0f);
    }

    private IEnumerator Pattern03()
    {
        AttackEye(2.0f);
        int temp = 0;
        WaitForSeconds wait = new WaitForSeconds(0.75f);
        List<int> list = new List<int>();
        for (int i = 0;i < 4;i++)
            list.Add(i);
        for (int i = 0;i<4;i++)
        {
            int rand = Random.Range(0, 4);
            temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
        for(int i = 0; i<4; i++)
        {
            int playerX = gridTrap.playerX;
            int playerY = gridTrap.playerY;
            if (list[i] == 0)
            {
                for(int x = 0; x < 10; x++)
                {
                    if(x == 0)
                        gridTrap.AttackGrid(x, playerY, 2.0f, true);
                    else
                        gridTrap.AttackGrid(x, playerY, 2.0f, false);
                }
            }
            else if (list[i] == 1)
            {
                gridTrap.AttackGrid(playerX, playerY, 2.0f, true);
                
                int x = playerX;
                int y = playerY;
                for(int j = 0; j < 10; j++)
                {
                    x += -1;
                    y += 1;
                    if (gridTrap.IsGridIn(x, y))
                        gridTrap.AttackGrid(x, y, 2.0f, false);
                    else
                        break;
                }
                x = playerX;
                y = playerY;
                for (int j = 0; j < 10; j++)
                {
                    x += 1;
                    y += -1;
                    if (gridTrap.IsGridIn(x, y))
                        gridTrap.AttackGrid(x, y, 2.0f, false);
                    else
                        break;
                }
            }
            else if (list[i] == 2)
            {
                for (int y = 0; y < 10; y++)
                {
                    if (y == 0)
                        gridTrap.AttackGrid(playerX, y, 2.0f, true);
                    else
                        gridTrap.AttackGrid(playerX, y, 2.0f, false);
                }
            }
            else
            {
                gridTrap.AttackGrid(playerX, playerY, 2.0f, true);

                int x = playerX;
                int y = playerY;
                for (int j = 0; j < 10; j++)
                {
                    x += 1;
                    y += 1;
                    if (gridTrap.IsGridIn(x, y))
                        gridTrap.AttackGrid(x, y, 2.0f, false);
                    else
                        break;
                }
                x = playerX;
                y = playerY;
                for (int j = 0; j < 10; j++)
                {
                    x += -1;
                    y += -1;
                    if (gridTrap.IsGridIn(x, y))
                        gridTrap.AttackGrid(x, y, 2.0f, false);
                    else
                        break;
                }
            }

            yield return wait;
        }
        yield return new WaitForSeconds(1.0f);
    }

    private IEnumerator Pattern04()
    {
        AttackEye(2.0f);
        WaitForSeconds wait = new WaitForSeconds(1.0f);
        for(int i = 0; i < 5; i++)
        {
            LazerAngle ang = (LazerAngle)Random.Range(0, 4);
            Lazer1(Random.Range(-5.0f, 0.0f), ang, 2.0f, true);
            Lazer1(Random.Range(0.0f, 5.0f), ang, 2.0f, false);
            yield return wait;
        }
        yield return wait;
    }

    private IEnumerator Pattern05()
    {
        AttackEye(2.0f);
        WaitForSeconds wait = new WaitForSeconds(1.0f);
        for (int i = 0; i < 5; i++)
        {
            LazerAngle ang = (LazerAngle)Random.Range(0, 4);
            Lazer1(Random.Range(-5.0f, 0.0f), ang, 2.0f, true);
            ang = (LazerAngle)Random.Range(0, 4);
            Lazer1(Random.Range(0.0f, 5.0f), ang, 2.0f, false);
            yield return wait;
        }
        yield return wait;
    }

    private IEnumerator Pattern06()
    {
        AttackEye(2.0f);
        WaitForSeconds wait = new WaitForSeconds(5.0f);
        int ang = Random.Range(0, 4);
        if(ang == 1)
        {
            ang += Random.Range(1, 4);
            ang = ang % 4;
        }
        if(Random.Range(0, 2) == 0)
            Lazer2((LazerAngle)ang, 2.0f, true);
        else
            Lazer2((LazerAngle)ang, 2.0f, false);
        yield return wait;
    }

    private IEnumerator Pattern07()
    {
        AttackEye(2.0f);
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        Transform trans = PlayerUnit.player.transform;
        rockTrap.Attack(trans.position, 2.0f);
        yield return new WaitForSeconds(2.0f);
        for (int i = 0; i < 6; i++)
        {
            GridAttack(Random.Range(0, 10), Random.Range(0, 10), 4.0f, false);
            GridAttack(Random.Range(0, 10), Random.Range(0, 10), 4.0f, false);
            GridAttackRandom(0, 0);
            GridAttackRandom(Random.Range(-1, 2), Random.Range(-1, 2), 4.0f, false);
            yield return wait;
        }
        
        
    }

    private IEnumerator Pattern08()
    {
        AttackEye(2.0f);
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        for (int i = 0; i < 10; i++)
        {
            if(i % 3 == 0)
            {
                LazerAngle ang = (LazerAngle)Random.Range(0, 4);
                Lazer1(Random.Range(-5.0f, 0.0f), ang, 2.0f, true);
                ang = (LazerAngle)Random.Range(0, 4);
                Lazer1(Random.Range(0.0f, 5.0f), ang, 2.0f, false);
                GridAttackRandom(0, 0);
                GridAttack(Random.Range(0, 10), Random.Range(0, 10), 4.0f, false);
                GridAttack(Random.Range(0, 10), Random.Range(0, 10), 4.0f, false);
            }
            else if(i % 3 == 1)
            {
                GridAttackRandom(Random.Range(-2, 3), Random.Range(-2, 3));
                GridAttack(Random.Range(0, 10), Random.Range(0, 10), 4.0f, false);
                GridAttack(Random.Range(0, 10), Random.Range(0, 10), 4.0f, false);
            }
            else
            {
                GridAttackRandom(Random.Range(-2, 3), Random.Range(-2, 3));
                GridAttack(Random.Range(0, 10), Random.Range(0, 10), 4.0f, false);
                GridAttack(Random.Range(0, 10), Random.Range(0, 10), 4.0f, false);
            }
            
            yield return wait;
        }
        yield return new WaitForSeconds(1.0f);
    }

    private IEnumerator Pattern09()
    {
        AttackEye(2.0f);
        WaitForSeconds wait = new WaitForSeconds(5.0f);
        int ang = Random.Range(0, 4);
        if (ang == 1)
        {
            ang += Random.Range(1, 4);
            ang = ang % 4;
        }

        if (Random.Range(0, 2) == 0)
            Lazer2((LazerAngle)ang, 2.0f, true);
        else
            Lazer2((LazerAngle)ang, 2.0f, false);


        GridAttack(1, 1, 4.0f, true);
        GridAttack(2, 1, 4.0f, false);
        GridAttack(3, 1, 4.0f, false);
        GridAttack(1, 2, 4.0f, false);
        GridAttack(2, 2, 4.0f, false);
        GridAttack(3, 2, 4.0f, false);

        GridAttack(6, 1, 4.0f, false);
        GridAttack(7, 1, 4.0f, false);
        GridAttack(8, 1, 4.0f, false);
        GridAttack(6, 2, 4.0f, false);
        GridAttack(7, 2, 4.0f, false);
        GridAttack(8, 2, 4.0f, false);

        GridAttack(1, 7, 4.0f, false);
        GridAttack(2, 7, 4.0f, false);
        GridAttack(3, 7, 4.0f, false);
        GridAttack(1, 8, 4.0f, false);
        GridAttack(2, 8, 4.0f, false);
        GridAttack(3, 8, 4.0f, false);

        GridAttack(6, 7, 4.0f, false);
        GridAttack(7, 7, 4.0f, false);
        GridAttack(8, 7, 4.0f, false);
        GridAttack(6, 8, 4.0f, false);
        GridAttack(7, 8, 4.0f, false);
        GridAttack(8, 8, 4.0f, false);

        yield return wait;
    }

    private IEnumerator Pattern10()
    {
        AttackEye(2.0f);
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        Transform trans = PlayerUnit.player.transform;
        rockTrap.Attack(trans.position, 2.0f);
        yield return new WaitForSeconds(2.0f);
        for (int i = 0; i < 6; i++)
        {
            LazerAngle ang = (LazerAngle)Random.Range(0, 4);
            Lazer1(Random.Range(0.0f, 5.0f), ang, 2.0f, true);
            ang = (LazerAngle)Random.Range(0, 4);
            Lazer1(Random.Range(-5.0f, 0.0f), ang, 2.0f, false);
            yield return wait;
        }

        yield return wait;
    }

    private IEnumerator Pattern11()
    {
        AttackEye(2.0f);
        Transform trans = PlayerUnit.player.transform;
        rockTrap.Attack(trans.position, 2.0f);
        WaitForSeconds wait = new WaitForSeconds(1.0f);
        int ang = Random.Range(0, 4);
        if (ang == 1)
        {
            ang += Random.Range(1, 4);
            ang = ang % 4;
        }

        if (Random.Range(0, 2) == 0)
            Lazer2((LazerAngle)ang, 2.0f, true);
        else
            Lazer2((LazerAngle)ang, 2.0f, false);

        for (int i = 0; i < 3; i++)
        {
            LazerAngle ang2 = (LazerAngle)Random.Range(0, 4);
            Lazer1(Random.Range(0.0f, 5.0f), ang2, 2.0f, true);
            yield return wait;
        }

        yield return new WaitForSeconds(2.0f);
    }

    private IEnumerator Pattern12()
    {
        AttackEye(2.0f);
        WaitForSeconds wait = new WaitForSeconds(0.25f);

        for (int i = 0; i < 10; i++)
        {
            if (i % 5 == 0)
            {
                LazerAngle ang = (LazerAngle)Random.Range(0, 4);
                Lazer1(Random.Range(0.0f, 5.0f), ang, 2.0f, true);
                ang = (LazerAngle)Random.Range(0, 4);
                Lazer1(Random.Range(-5.0f, 0.0f), ang, 2.0f, false);
            }
            else
            {
                GridAttack(Random.Range(0, 10), Random.Range(0, 10), 4.0f, true);
                GridAttack(Random.Range(0, 10), Random.Range(0, 10), 4.0f, false);
                GridAttack(Random.Range(0, 10), Random.Range(0, 10), 4.0f, false);
            }
            yield return wait;
        }
        LazerAngle ang2 = (LazerAngle)Random.Range(0, 4);
        Lazer1(Random.Range(0.0f, 5.0f), ang2, 2.0f, true);
        ang2 = (LazerAngle)Random.Range(0, 4);
        Lazer1(Random.Range(-5.0f, 0.0f), ang2, 2.0f, false);
        yield return new WaitForSeconds(1.0f);
    }

    private void GridAttack(int x, int y, float t = 4.0f, bool isSound = true)
    {
        gridTrap.AttackGrid(x, y, t, isSound);
    }
    private void GridAttackRandom(int x, int y, float t = 4.0f, bool isSound = true)
    {
        x = x + gridTrap.playerX;
        y = y + gridTrap.playerY;
        gridTrap.AttackGrid(x, y, t, isSound);
    }

    private void GridPosAttack(Vector3 vec, float t = 2.0f)
    {
        gridTrap.AttackPos(vec, t);
    }

    private void Lazer1(float p, LazerAngle ang, float t, bool isSound)
    {
        lazerTrap.Attack(p, ang, t, isSound);
    }

    private void Lazer2(LazerAngle ang, float t, bool isLeft)
    {
        lazerTrap2.Attack(ang, t, isLeft);
    }
}
