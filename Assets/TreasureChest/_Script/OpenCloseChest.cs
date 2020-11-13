using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseChest : MonoBehaviour
{
    public Animation myAnimation;


    // Start is called before the first frame update
    void Start()
    {

    }

    IEnumerator OnMouseEnter()
    {
        myAnimation.Play("open");
        yield return new WaitForSeconds(1.75f);
        myAnimation.Play("close");
    }


    // Update is called once per frame
    void Update()
    {

    }
}
