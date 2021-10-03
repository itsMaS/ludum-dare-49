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
            TimeControl(true);
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
        if (!forward)
        {
            Time.timeScale = 1;
            return;
        } 
        Time.timeScale += 3;
    }
}
