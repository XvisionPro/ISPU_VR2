using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;
using Valve.VR.InteractionSystem;

public class CapActivate : MonoBehaviour
{
    public new Animation animation;
    [SerializeField] public List<DetailMove> child = new List<DetailMove>();
    public bool Allow = true;
    public string animName;
    [SerializeField] public bool isForward = true;

    private void Start()
    {
        if (animation == null)
            animation = GetComponent<Animation>();
    }

    private void CheckAllowed()
    {
        foreach (DetailMove detail in child)
        {
            if (detail.isForward == false)
            {
                Allow = false;
                break;
            }
            else
            {
                Allow = true;
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