using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputNavigator : MonoBehaviour
{
    EventSystem EventSystem;

    /// <summary>
    /// List of GameObjects that are selectable
    /// </summary>
    public List<GameObject> Selectables;

    /// <summary>
    /// The index of the current selected GameObject
    /// </summary>
    public int CurrentSelectableIndex = 0;

    void Start()
    {
        EventSystem = EventSystem.current;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)
            || Input.GetKeyDown(KeyCode.RightArrow))
        {
            CurrentSelectableIndex += 1;
            EventSystem.SetSelectedGameObject(Selectables[CurrentSelectableIndex % Selectables.Count()], new BaseEventData(EventSystem));
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CurrentSelectableIndex -= 1;
            EventSystem.SetSelectedGameObject(Selectables[CurrentSelectableIndex % Selectables.Count()], new BaseEventData(EventSystem));
        }
    }
}
