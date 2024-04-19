using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailMove : MonoBehaviour
{
    public DetailMove parent;
    public DetailMove child;
    [SerializeField] public List<DetailMove> childMore = new List<DetailMove>();
    public bool Allow = false;

    public new Animation animation;
    public string animName;
    [SerializeField] public bool isForward = true;


    private void CheckAllowed()
    {
        if (childMore.Count == 0)
        {
            if (parent != null)
            {
                if (child != null)
                {
                    if ((parent.isForward == false) && (isForward == true))
                    {
                        Allow = true;
                    }
                    else if ((child.isForward == true) && (isForward == false))
                    {
                        Allow = true;
                    }
                    else
                    {
                        Allow = false;
                    }
                }
                else
                {
                    if (parent.isForward == false)
                    {
                        Allow = true;
                    }
                    else { Allow = false; }
                }
            }
            else
            {
                if (child != null)
                {
                    if (child.isForward == true)
                    {
                        Allow = true;
                    }
                    else
                    {
                        Allow = false;
                    }
                }
                else
                {
                    Allow = true;
                }
            }
        }
        else
        {
            foreach (DetailMove detail in childMore)
            {
                if (detail.isForward == false)
                {
                    Allow = false;
                    return;
                }
                else
                {
                    Allow = true;
                }
            }
        }
    }

    private void OnMouseDown()
    {
        CheckAllowed();
        if (Allow)
        {

            if (isForward)
            {
                animation[animName].speed = 1;
                animation.Play(animName);
                isForward = false;
            }
            else
            {
                animation[animName].time = animation[animName].length;
                animation[animName].speed = -1;
                animation.Play(animName);
                isForward = true;
            }
        }
    }
}
