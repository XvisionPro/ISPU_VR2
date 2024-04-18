using UnityEngine;

public class CapActivate : MonoBehaviour
{
    public new Animation animation;
    public string animName;
    [SerializeField] public bool isForward = true;

    private void Start()
    {
        if (animation == null)
            animation = GetComponent<Animation>();
    }

    private void OnMouseDown()
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