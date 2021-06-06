using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugStuff : MonoBehaviour
{
    
    public void Time1x()
    {
        Time.timeScale = 1;
    }

    public void Time03x()
    {
        Time.timeScale = 0.3f;
    }

    public void Time01x()
    {
        Time.timeScale = 0.1f;
    }

}
