using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OCElecrical : MonoBehaviour
{
    // Start is called before the first frame update

    public bool flag =false, flag1=true;
    public Animation anim;
    public void StartAnimation()
    {
        if (flag1==true)
        {
            if (flag == false)
            {
                flag1 = false;
                anim.Play("OpenElecric");
                flag = !flag;
                StartCoroutine(time());
            }
            else
            {
                flag1 = false;
                anim.Play("CloseElecric");
                flag = !flag;
                StartCoroutine(time());

            }
        }
    }
    IEnumerator time()
    {
        yield return new WaitForSeconds(1.5f);
        flag1 = !flag1;

    }
}
