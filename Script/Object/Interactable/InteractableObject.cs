using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 플레이어가 키 입력으로 상호작용 가능한 오브젝트.
/// </summary>
public class InteractableObject : MonoBehaviour
{
    [Header("Inspector")]
    [SerializeField] protected MeshRenderer[] renders;
    [SerializeField] protected SpriteRenderer[] spriteRenders;
    [SerializeField] protected Material[] mats;
    [SerializeField] protected Collider coll;
    [SerializeField] protected Transform stage;
    [SerializeField] [Range(0, 10.0f)] private float speed = 5.0f;
    [SerializeField] [Range(0, 1.0f)] private float intencity = 1.0f;
    [SerializeField] [Range(0, 1.0f)] private float spaceTime = 0.2f;
    [SerializeField] protected Color color = Color.white;
    [SerializeField] private UnityEvent interFunc;
    [SerializeField] protected bool isCanInteract = true;

    private void Awake()
    {
        //mat = render.material;
        int index = 0;
        if (renders != null)
            index += renders.Length;
        if(spriteRenders != null)
            index += spriteRenders.Length;
        if(index > 0)
        {
            int temp = 0;
            mats = new Material[index];
            if(renders != null)
            {
                temp = renders.Length;
                for (int i = 0; i < renders.Length; i++)
                {
                    mats[i] = renders[i].material;
                }
            }
            if(spriteRenders != null)
            {
                for (int i = temp; i < temp + spriteRenders.Length; i++)
                {
                    mats[i] = spriteRenders[i].material;
                }
            }
        }
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Q))
    //    {
    //        Swt_Interact(true);
    //    }
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        Swt_Interact(false);
    //    }
    //}

    private void OnDisable()
    {
        if(PlayerUnit.player != null)
            PlayerUnit.player.RemoveInteract(this); 
    }

    private void OnDestroy()
    {
        if (PlayerUnit.player != null)
            PlayerUnit.player.RemoveInteract(this);
    }

    public virtual void Swt_Interact(bool b)
    {
        if(b)
        {
            foreach(Material mat in mats)
            {
                mat.SetInt("_IsActive", 1);
            }
            //mat.SetInt("_IsActive", 1);
            if(coActive != null)
                StopCoroutine(coActive);
            coActive = CoActive();
            StartCoroutine(coActive);
        }
        else
        {
            foreach (Material mat in mats)
            {
                mat.SetInt("_IsActive", 0);
                mat.SetFloat("_Intencity", 0);
            }
            //mat.SetInt("_IsActive", 0);
            //mat.SetFloat("_Intencity", 0);
            if(coActive != null)
                StopCoroutine(coActive);
        }
    }


    //상호작용 가능 표시 연출.
    private IEnumerator coActive = null;
    private IEnumerator CoActive()
    {
        bool b = true;
        float _intencity = 0;
        WaitForSeconds wait = new WaitForSeconds(spaceTime);
        foreach (Material mat in mats)
        {
            mat.SetColor("_ActiveColor", color);
        }
        //mat.SetColor("_ActiveColor", color);
        while (true)
        {
            if(b)
            {
                _intencity += Time.deltaTime * speed;
            }
            else
            {
                _intencity += -Time.deltaTime * speed;
            }
            if(_intencity >= intencity)
            {
                b = false;
                _intencity = intencity;
                foreach (Material mat in mats)
                {
                    mat.SetFloat("_Intencity", _intencity);
                }
                //mat.SetFloat("_Intencity", _intencity);
                yield return wait;
            }
            else if(_intencity <= 0)
            {
                b = true;
                _intencity = 0;
                foreach (Material mat in mats)
                {
                    mat.SetFloat("_Intencity", _intencity);
                }
                //mat.SetFloat("_Intencity", _intencity);
                yield return wait;
            }
            else
            {
                foreach (Material mat in mats)
                {
                    mat.SetFloat("_Intencity", _intencity);
                }
                //mat.SetFloat("_Intencity", _intencity);
            }
            yield return null;
        }
    }

    public virtual void Interact(Unit u)
    {
        if (!isCanInteract)
            return;
        if (interFunc != null)
            interFunc.Invoke();
    }

    public void SetColor(Color c)
    {
        mats[0].SetColor("_Color", c);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (!isCanInteract)
                return;
            Swt_Interact(true);
            if(other.TryGetComponent<PlayerUnit>(out PlayerUnit player))
            {
                player.AddInteract(this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Swt_Interact(false);
            if (other.TryGetComponent<PlayerUnit>(out PlayerUnit player))
            {
                player.RemoveInteract(this);
                TriggerExitAction();
            }
        }
    }

    protected virtual void TriggerExitAction()
    {

    }

    public void SetCanInteract(bool b)
    {
        isCanInteract = b;
    }

    public void AddInterFunc(UnityAction func)
    {
        interFunc.AddListener(func);
    }
}
