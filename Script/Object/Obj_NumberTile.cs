using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class Obj_NumberTile : MonoBehaviour
{
    [SerializeField] private Stage010 stage010;
    [SerializeField] private MeshRenderer render;
    [SerializeField] private Material mat;
    [SerializeField] public int index;
    private Color defaultColor;

    void Start()
    {
        mat = render.material;
        defaultColor = mat.GetColor("_Color");
    }

    private TweenerCore<Color, Color, ColorOptions> tween = null;
    public void ActiveTile(Color c, float speed = 0.5f, bool isSound = false)
    {
        if(tween == null)
            tween.Kill();
        mat.SetColor("_Color", defaultColor);
        tween = mat.DOColor(c, "_Color", speed).OnComplete(()=> 
        {
            tween = mat.DOColor(defaultColor, "_Color", speed);
        });
        if(isSound)
            SoundManager.Instance.StartSound("UI_Button_Click3", 1.0f);
    }

    public void Test(Color c)
    {
        mat.SetColor("_Color", c);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            stage010.EnterTile(index);
        }
    }
}
