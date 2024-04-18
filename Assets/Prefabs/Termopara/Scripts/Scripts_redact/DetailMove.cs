using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailMove : MonoBehaviour
{
    public CapActivate Cup;
    public DetailMove parent;
    public bool Allow = false;

    public new Animation animation;
    public string animName;
    [SerializeField] private bool isForward = true;


    private void CheckAllowed()
    {
        if (Cup == null)
        {
            if (parent != null)
            {
                Allow = !parent.isForward;
                Debug.Log("Крышка нет. Есть родитель");
                Debug.Log(Allow);
            }
        }
        else
        {
            Allow = !Cup.isForward;
            Debug.Log("Крышка есть");
            Debug.Log(Allow);
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("Проверка");
        CheckAllowed();
        if (Allow)
        {

            if (isForward)
            {
                Debug.Log("Проигрываеться на прямую");
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
