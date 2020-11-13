using UnityEngine;

public class Fader : MonoBehaviour
{
    public float FadeAmount;
    Material mat;
    Transform target;
    Transform cam;
    SphereCollider sphere;
    bool doFade;

    void Awake()
    {
        target = transform.Find("cameraTarget");
        mat = GetComponentInChildren<Renderer>().material;
        sphere = GetComponent<SphereCollider>();
        cam = GameObject.Find("Main Camera").transform;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            doFade = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            mat.SetFloat("_FadeAmount", 0);
            doFade = false;
        }
    }

    void Update()
    {
        if (!doFade) { return; }
        //float distance = Vector3.Distance(transform.position, target.position);
        Vector3 sphereCenterInWorldSpace = sphere.transform.TransformPoint(sphere.center);
        Vector3 offsetPos = sphereCenterInWorldSpace - (cam.position - sphereCenterInWorldSpace).normalized * 0.1f;
        float distance = Vector3.Distance(cam.position, offsetPos);
        //float distance = Vector3.Distance(cam.position, transform.TransformPoint(sphere.center));
        //Debug.Log(distance);
        distance = distance / sphere.radius;
        //Debug.Log($"normalized distance: {distance}");
        mat.SetFloat("_FadeAmount", 2f - distance);
    }
}
