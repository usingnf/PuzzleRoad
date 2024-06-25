using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 1번 스테이지에서 사용되는 물체 감지에 사용 됨.
/// </summary>

public class Detector_Weight : MonoBehaviour
{
    [SerializeField] private Stage001 stage;
    [SerializeField] private List<Inter_Weight> weights = new List<Inter_Weight>();
    [SerializeField] private bool isRight = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Obj_Weight_Dummy>(out Obj_Weight_Dummy weight))
        {
            if (weights.Contains(weight.weight))
                return;
            weights.Add(weight.weight);
            stage.AddWeight(weight.weight, isRight);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Obj_Weight_Dummy>(out Obj_Weight_Dummy weight))
        {
            if (!weights.Contains(weight.weight))
                return;
            weights.Remove(weight.weight);
            stage.SubWeight(weight.weight, isRight);
        }
    }

    public void WeightReset()
    {
        weights.Clear();
    }
}
