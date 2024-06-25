using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private  static T instance;
    public static T Instance
    {
        get { return instance; }
        set 
        {
            if(instance != null)
            {
                Destroy(value.gameObject);
                return;
            }
            instance = value;
        }
    }

    [SerializeField]
    protected bool isDontDestroyed = false;

    public static bool Exist()
    {
        if(instance == null)
            return false;
        return true;
    }
}
