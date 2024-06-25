using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem
{
    public InventoryItem(SO_Item item, GameObject obj, int count)
    {
        this.item = item;
        ui_item = obj;
        this.count = count;
    }

    public SO_Item item;
    public int count;
    public GameObject ui_item;
}

public class MemoItem
{
    public MemoItem(SO_Memo memo, GameObject ui_memo)
    {
        this.memo = memo;
        this.ui_memo = ui_memo;
    }
    public SO_Memo memo;
    public GameObject ui_memo;
}

public class Inventory : Singleton<Inventory>
{
    [Header("Item")]
    [SerializeField] private GameObject prefab_item;
    private List<InventoryItem> items = new List<InventoryItem>();
    [SerializeField] private RectTransform panel_Inventory;
    [SerializeField] private Transform panel_InventoryConent;
    [SerializeField] private RectTransform panel_info;
    [SerializeField] private TextMeshProUGUI text_infoItem;
    [SerializeField] private Button btn_swtInventory;

    private bool isInventoryOpen = false;


    [Header("Memo")]
    [SerializeField] private GameObject prefab_memoPage;
    [SerializeField] private GameObject prefab_memoText;
    private List<MemoItem> memoItems = new List<MemoItem>();
    [SerializeField] private Transform panel_memo;
    [SerializeField] private Transform panel_memoLeft;
    [SerializeField] private Transform panel_memoRight;
    [SerializeField] private TextMeshProUGUI text_leftMemoIndex;
    [SerializeField] private TextMeshProUGUI text_rightMemoIndex;
    [SerializeField] private List<RectTransform> memoPages = new List<RectTransform>();
    [SerializeField] private GameObject panel_addMemo;

    [SerializeField] private Button btn_memo;
    [SerializeField] private Button btn_BeforePage;
    [SerializeField] private Button btn_NextPage;
    [SerializeField] private Button btn_closeMemo;
    [SerializeField] private Button btn_openAddMemo;
    [SerializeField] private Button btn_AddMemo;
    [SerializeField] private Button btn_closeAddMemo;

    [SerializeField] private TMP_InputField input_title;
    [SerializeField] private TMP_InputField input_content;

    [SerializeField] private SO_Memo tempMemo;
    private int pageIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        btn_swtInventory.onClick.AddListener(() =>
        {
            SwtInventory();
        });
        btn_memo.onClick.AddListener(() =>
        {
            panel_memo.gameObject.SetActive(true);
        });
        btn_BeforePage.onClick.AddListener(() =>
        {
            BeforeMemoPage();
        });
        btn_NextPage.onClick.AddListener(() =>
        {
            NextMemoPage();
        });
        btn_closeMemo.onClick.AddListener(() =>
        {
            panel_addMemo.SetActive(false);
            panel_memo.gameObject.SetActive(false);
        });
        btn_openAddMemo.onClick.AddListener(() =>
        {
            panel_addMemo.SetActive(true);
        });
        btn_closeAddMemo.onClick.AddListener(() =>
        {
            panel_addMemo.SetActive(false);
        });
        btn_AddMemo.onClick.AddListener(() =>
        {
            AddCustomMemo();
        });
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)) 
        {
            AddMemo(tempMemo);
        }
    }

    public void AddItem(SO_Item item, int count = 1)
    {
        if (item == null)
            return;
        foreach (InventoryItem findItem in items)
        {
            if(findItem.item == item)
            {
                findItem.count += count;
                if(findItem.ui_item.TryGetComponent<UI_Item>(out UI_Item findui))
                {
                    findui.SetData(count);
                }
                return;
            }
        }
        GameObject obj = Instantiate(prefab_item, panel_InventoryConent);
        if(obj.TryGetComponent<UI_Item>(out UI_Item ui))
        {
            ui.SetData(item);
        }
        InventoryItem it = new InventoryItem(item, obj, count);
        items.Add(it);
    }

    public void AddCustomMemo()
    {
        if (string.IsNullOrEmpty(input_title.text))
            return;
        if (string.IsNullOrEmpty(input_content.text))
            return;

        SO_Memo memo = new SO_Memo();
        memo.memoName = input_title.text;
        memo.content = input_content.text;
        AddMemo(memo);

        input_title.text = string.Empty;
        input_content.text = string.Empty;
    }
    public void AddMemo(SO_Memo memo)
    {
        if(memo == null) 
            return;
        GameObject obj = Instantiate(prefab_memoText);
        if(obj.TryGetComponent<UI_Memo>(out UI_Memo ui))
        {
            ui.SetData(memo);
        }
        MemoItem it = new MemoItem(memo, obj);
        memoItems.Add(it);
        //AddMemoBook(obj);
        StartCoroutine(AddMemoBook(obj));
    }

    private IEnumerator AddMemoBook(GameObject memo)
    {
        if(memoPages.Count == 0)
        {
            GameObject obj = Instantiate(prefab_memoPage, panel_memoLeft);
            memoPages.Add(obj.GetComponent<RectTransform>());
        }
        RectTransform rect = memo.GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        yield return null;
        if(memoPages[^1].sizeDelta.y + rect.sizeDelta.y > 850)
        {
            GameObject obj = null;
            if(memoPages.Count % 2 == 0)
            {
                obj = Instantiate(prefab_memoPage, panel_memoLeft);
            }
            else
            {
                obj = Instantiate(prefab_memoPage, panel_memoRight);
            }
            memoPages.Add(obj.GetComponent<RectTransform>());
            memo.transform.SetParent(obj.transform);
        }
        else
        {
            memo.transform.SetParent(memoPages[^1]);
        }
        OpenMemoPage(pageIndex);
    }

    public void OpenMemoPage(int index)
    {
        if (index < 0)
            return;
        if (index >= memoPages.Count)
            return;
        pageIndex = index;
        foreach (RectTransform page in memoPages)
        {
            page.anchoredPosition = new Vector2(page.anchoredPosition.x, 2000);
        }
        if(index % 2 == 0)
        {
            memoPages[index].anchoredPosition = new Vector2(memoPages[index].anchoredPosition.x, -50);
            if (index + 1 < memoPages.Count)
                memoPages[index + 1].anchoredPosition = new Vector2(memoPages[index + 1].anchoredPosition.x, -50);
            text_leftMemoIndex.text = (index+1).ToString();
            text_rightMemoIndex.text = (index+2).ToString();
        }
        else
        {
            memoPages[index - 1].anchoredPosition = new Vector2(memoPages[index].anchoredPosition.x, -50);
            memoPages[index].anchoredPosition = new Vector2(memoPages[index].anchoredPosition.x, -50);
            text_leftMemoIndex.text = (index).ToString();
            text_rightMemoIndex.text = (index+1).ToString();
        }
    }

    public void NextMemoPage()
    {
        if(pageIndex + 2 >= memoPages.Count)
        {
            OpenMemoPage(pageIndex + 1);
        }
        else
        {
            OpenMemoPage(pageIndex + 2);
        }
        
    }

    public void BeforeMemoPage()
    {
        if (pageIndex - 2 < 0)
        {
            OpenMemoPage(pageIndex - 1);
        }
        else
        {
            OpenMemoPage(pageIndex - 2);
        }
    }

    public void OpenMemo(SO_Memo memo)
    {
        if(memo == null) 
            return;
        panel_memo.gameObject.SetActive(true);
    }

    public void CloseMemo()
    {
        panel_memo.gameObject.SetActive(false);
    }

    public void Swt_Info(bool b)
    {
        panel_info.gameObject.SetActive(b);
    }

    public void SetInfoPos(Vector2 pos)
    {
        panel_info.position = pos;
    }
    public void SetInfoItem(SO_Item item, int count)
    {
        if (item == null)
            return;
        text_infoItem.text = item.itemName;
    }

    public void SetInfoMemo(SO_Memo memo)
    {
        if (memo == null)
            return;
        text_infoItem.text = memo.memoName;
    }

    public void SwtInventory()
    {
        isInventoryOpen = !isInventoryOpen;

        if(isInventoryOpen)
        {
            panel_Inventory.DOAnchorPos(new Vector2(0, 0), 0.5f);
        }
        else
        {
            panel_Inventory.DOAnchorPos(new Vector2(0, -250), 0.5f);
        }
    }

}
