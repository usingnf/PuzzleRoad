using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// 캐릭터 이동, 조작 기능.
/// 본래 RTS용으로 작성한 것을 사용.
/// </summary>

public class PlayerSystem : Singleton<PlayerSystem>
{
    public enum ClickType
    {
        Normal,
    }

    [System.Serializable]
    public struct ClickStruct
    {
        public Transform trans;
        public VisualEffect[] effects;
    }


    [Header("Inspector")]
    [SerializeField] private ClickStruct[] clickStruct;
    [SerializeField] private ParticleSystem clickParticle;
    [SerializeField] private RectTransform dragRect;
    [SerializeField] private Transform maskTrans;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask unitLayer;

    [Header("Status")]
    private Camera cam;
    [SerializeField] private List<Unit> selectedUnits = new List<Unit>();


    private Vector3 startMouse;
    private Vector3 currentMouse;

    private void Awake()
    {
        isDontDestroyed = false;
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    public static bool IsOwner(Unit u)
    {
        if (u.IsEnemy)
            return false;
        return true;
    }


    // Update is called once per frame
    void Update()
    {
        currentMouse = Input.mousePosition;
        //Update_UnitSelect();
        Update_UnitOrder();
        //Update_Mask();
    }

    #region Unit

    //유닛을 선택하는 기능. 유닛이 유일하므로 사용하지 않음.
    private void Update_UnitSelect()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startMouse = Input.mousePosition;
            Ray ray;
            RaycastHit hit;
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, unitLayer))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (hit.transform.TryGetComponent<PlayerUnit>(out PlayerUnit u))
                    {
                        if (IsOwner(u))
                        {
                            if (selectedUnits.Contains(u))
                                DeselectUnit(u);
                            else
                                SelectUnit(u);
                        }
                        else
                        {
                            DeselectAllUnit();
                            SelectUnit(u);
                        }
                    }
                }
                else
                {
                    DeselectAllUnit();
                    if (hit.transform.TryGetComponent<PlayerUnit>(out PlayerUnit u))
                    {
                        SelectUnit(u);
                    }
                }
            }
            else
            {
                DeselectAllUnit();
            }
        }
        if (Input.GetMouseButton(0))
        {
            DrawDragRect();
        }
        if (Input.GetMouseButtonUp(0))
        {
            currentMouse = Input.mousePosition;
            EndDrawDragRect();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray;
            RaycastHit hit;
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, groundLayer))
            {
                Click(ClickType.Normal, hit.point);

                foreach (PlayerUnit selectUnit in selectedUnits)
                {
                    selectUnit.TrySetOrder(PlayerUnit.PlayerOrderState.Move, hit.point);
                }
            }
        }
    }


    private void Update_UnitOrder()
    {

        if (Input.GetKeyDown(MyKey.Stop))
        {
            foreach (PlayerUnit myUnit in selectedUnits)
            {
                myUnit.TrySetOrder(PlayerUnit.PlayerOrderState.Stop, Vector3.zero);
            }
        }

        //if (Input.GetMouseButtonDown(1))
        if(Input.GetKeyDown(MyKey.Move))
        {
            Ray ray;
            RaycastHit hit;
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, groundLayer))
            {
                Click(ClickType.Normal, hit.point);

                if(hit.transform.CompareTag("MoveTile"))
                {
                    if(hit.transform.TryGetComponent<Obj_MoveTileTarget>(out Obj_MoveTileTarget tile))
                    {
                        Transform target = tile.target;
                        foreach (PlayerUnit selectUnit in selectedUnits)
                        {
                            selectUnit.SetMoveTile(true, target, hit.point);
                        }
                    }
                    else
                    {
                        foreach (PlayerUnit selectUnit in selectedUnits)
                        {
                            selectUnit.SetOffMoveTile();
                            selectUnit.TrySetOrder(PlayerUnit.PlayerOrderState.Move, hit.point);
                        }
                    }
                }
                else
                {
                    foreach (PlayerUnit selectUnit in selectedUnits)
                    {
                        selectUnit.SetOffMoveTile();
                        selectUnit.TrySetOrder(PlayerUnit.PlayerOrderState.Move, hit.point);
                    }
                }
                
            }
        }
    }

    private void Click(ClickType type, Vector3 pos)
    {
        clickParticle.transform.position = pos+ new Vector3(0,0.05f,0);
        clickParticle.Play();
        clickStruct[(int)type].trans.position = pos;
        foreach (VisualEffect effect in clickStruct[(int)type].effects)
        {
            effect.enabled = false;
            effect.enabled = true;
            effect.SendEvent("Click");
        }
    }


    private void EndDrawDragRect()
    {
        dragRect.gameObject.SetActive(false);
        if ((currentMouse - startMouse).magnitude < 0.02f)
            return;
        Rect rect = new Rect();
        if (currentMouse.x < startMouse.x)
        {
            rect.xMin = currentMouse.x;
            rect.xMax = startMouse.x;
        }
        else
        {
            rect.xMin = startMouse.x;
            rect.xMax = currentMouse.x;
        }
        if (currentMouse.y < startMouse.y)
        {
            rect.yMin = currentMouse.y;
            rect.yMax = startMouse.y;
        }
        else
        {
            rect.yMin = startMouse.y;
            rect.yMax = currentMouse.y;
        }
        foreach (Unit u in Unit.AllUnits)
        {
            if (rect.Contains(cam.WorldToScreenPoint(u.transform.position)))
            {
                SelectUnit(u);
            }
            CheckEnemyInSelect();
        }
    }

    private void DrawDragRect()
    {
        dragRect.gameObject.SetActive(true);
        dragRect.position = (startMouse + currentMouse) * 0.5f;
        dragRect.sizeDelta = new Vector2(Mathf.Abs(startMouse.x - currentMouse.x), Mathf.Abs(startMouse.y - currentMouse.y));
    }

    private void SelectUnit(Unit u)
    {
        if (selectedUnits.Contains(u))
            return;
        if (!u.IsVisible)
            return;
        u.SwtSelectCircle(true);
        selectedUnits.Add(u);
    }

    private void CheckEnemyInSelect()
    {
        List<Unit> enemy = new List<Unit>();
        if (selectedUnits.Count == 1)
            return;
        foreach (Unit u in selectedUnits)
        {
            if (!IsOwner(u))
            {
                enemy.Add(u);
            }
        }

        foreach (Unit u in enemy)
        {
            DeselectUnit(u);
        }
    }

    public void DeselectUnit(Unit u)
    {
        u.SwtSelectCircle(false);
        selectedUnits.Remove(u);
    }

    private void DeselectAllUnit()
    {
        foreach (Unit u in selectedUnits)
        {
            u.SwtSelectCircle(false);
        }
        selectedUnits.Clear();
    }

    public bool IsUnitSelected(Unit u)
    {
        if (selectedUnits.Contains(u))
            return true;
        return false;
    }


    #endregion

    //See Through 기능. 투시 기능을 만들었지만 사용하지 않음.
    private void Update_Mask()
    {
        maskTrans.position = cam.ScreenToWorldPoint(currentMouse) + cam.transform.forward * 1.0f;
        maskTrans.rotation = cam.transform.rotation;
    }
}
