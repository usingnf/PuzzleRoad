using RayFire;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;


public class GroundFall : MonoBehaviour
{
    [SerializeField] private RayfireShatter shatter;
    [SerializeField] private MeshRenderer meshRender;

    public void Broken()
    {
        meshRender.enabled = false;
        CameraManager.Instance.StopShake();
        CameraManager.Instance.Shake(0.2f, 0.5f, 60);
        shatter.scalePreview = true;
        shatter.previewScale = 0.1f;
        shatter.voronoi.amount = 60;
        shatter.DeleteFragmentsLast();
        shatter.Fragment();
        foreach (GameObject fragment in shatter.fragmentsLast)
            if (fragment != null)
                fragment.transform.localScale = Vector3.one * (1 - shatter.previewScale);

        if (!this.gameObject.activeInHierarchy)
            return;

        foreach (GameObject obj in shatter.fragmentsLast)
        {
            RayfireRigid rigid = obj.AddComponent<RayfireRigid>();
            rigid.Initialize();
            rigid.physics.rigidBody.AddForce(new Vector3(Random.Range(-1, 1), Random.Range(0.1f, 1), Random.Range(-1, 1)) * 15.0f, ForceMode.Impulse);
        }
        StartCoroutine(CoDestroy());
    }


    private IEnumerator CoDestroy()
    {
        yield return new WaitForSeconds(10.0f);
        foreach (GameObject obj in shatter.fragmentsLast)
        {
            Destroy(obj);
        }
    }
}
