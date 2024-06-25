using System.Collections;
using UnityEngine;

public class Boss_GridTrapGroup : MonoBehaviour
{
    [SerializeField] private SenarioTrue stage;
    [SerializeField] private Boss_GridTrap[] traps;
    [SerializeField] private Boss_GridTrap prefab_trap;
    [SerializeField] private Transform gridCenter;

    private Boss_GridTrap[,] trap;
    private int sizeX;
    private int sizeY;
    public int playerX;
    public int playerY;
    void Start()
    {
        sizeX = (int)Mathf.Sqrt(traps.Length);
        sizeY = (int)Mathf.Sqrt(traps.Length);
        trap = new Boss_GridTrap[sizeX, sizeY];
        int index = 0;
        for (int y = 0; y < sizeY; y++)
        {
            for(int x = 0; x < sizeX; x++)
            {
                trap[x, y] = traps[index];
                trap[x, y].x = x;
                trap[x, y].y = y;
                index++;
            }
        }
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.T))
    //    {
    //        Debug.Log("T");
    //        //AttackGrid(Random.Range(0, sizeX), Random.Range(0, sizeY));
    //        AttackPos(PlayerUnit.player.transform.position + new Vector3(1, 0, 1));
    //    }
    //}

    public void DetectPlayer()
    {
        if(coDelayRestage != null)
            StopCoroutine(coDelayRestage);
        coDelayRestage = CoDelayRestage();
        StartCoroutine(coDelayRestage);
    }

    private IEnumerator coDelayRestage = null;
    private IEnumerator CoDelayRestage()
    {
        if (PlayerUnit.player.IsShield)
            yield break;
        yield return new WaitForSeconds(0.1f);
        if (PlayerUnit.player.IsShield)
            yield break;
        stage.ReStage();
    }

    public bool IsGridIn(int x, int y)
    {
        if (x < 0)
            return false;
        if (y < 0)
            return false;
        if (x >= sizeX)
            return false;
        if (y >= sizeY)
            return false;
        if (trap[x, y] == null)
            return false;
        return true;
    }
    public void AttackGrid(int x, int y, float t = 4.0f, bool isSound = true)
    {
        if (x < 0)
            x = 0;
        if (y < 0)
            y = 0;
        if (x >= sizeX)
            x = sizeX - 1;
        if (y >= sizeY)
            y = sizeY - 1;
        if (trap[x, y] == null)
            return;
        trap[x, y].StartShoot(t, isSound);
    }

    public void AttackPos(Vector3 vec, float t = 2.0f, bool isSound = true)
    {
        if (vec.x > gridCenter.position.x + 9)
            vec.x = gridCenter.position.x + 9;
        else if (vec.x < gridCenter.position.x - 9)
            vec.x = gridCenter.position.x - 9;
        if (vec.z > gridCenter.position.z + 9)
            vec.z = gridCenter.position.z + 9;
        else if (vec.z < gridCenter.position.z - 9)
            vec.z = gridCenter.position.z - 9;
        Boss_GridTrap trap = Instantiate(prefab_trap, vec, Quaternion.identity, this.transform);
        trap.group = this;
        trap.StartShoot(t, isSound);
        Destroy(trap.gameObject, t+1);
    }

    
}
