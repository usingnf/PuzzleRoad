using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrapGroup : MonoBehaviour
{
    [SerializeField] private Stage stage;
    [SerializeField] private Trap[] traps;
    [SerializeField] private SoundData[] soundData;
    [SerializeField] private bool inRange = false;

    private void OnEnable()
    {
        StartCoroutine(CoWhile());
    }

    private void Awake()
    {
        foreach(Trap trap in traps)
        {
            trap.group = this;
        }
    }

    public void DetectPlayer()
    {
        if (coDelayRestage != null)
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

    private IEnumerator CoWhile()
    {
        float time = 0.1f;
        WaitForSeconds wait = new WaitForSeconds(time);
        foreach (Trap trap in traps)
        {
            trap.ResetTime();
        }
        while (true)
        {
            bool swt = false;
            foreach (Trap trap in traps)
            {
                trap.currentIndex += 1;
                if (trap.CheckShoot())
                    swt = true;
            }
            if(inRange && swt)
            {
                foreach(SoundData s in soundData)
                {
                    SoundManager.Instance.StartSound(s.soundName, s.volume);
                }
            }
            yield return wait;
        }
    }

    public void SetInRange(bool b)
    {
        inRange = b;
    }
}
