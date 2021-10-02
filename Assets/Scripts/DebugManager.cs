using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            BreakAllLines();
        }

        if(Input.GetKeyDown(KeyCode.F2))
        {
            TimeControl(false);
        }
        else if(Input.GetKeyDown(KeyCode.F3))
        {
            TimeControl(false);
        }
    }

    private void BreakAllLines()
    {
        foreach (var item in FindObjectsOfType<RopeConnection>())
        {
            item.Break();
        }
    }
    private void TimeControl(bool forward)
    {
        Time.timeScale = (Time.timeScale + 3 * (forward ? 1 : -1)) % 15;
    }
}
