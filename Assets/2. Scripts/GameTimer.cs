using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public event Action<float> ActionOnTimer;
    private float timer;
   
    public float Timer
    {
        get
        {
            return timer;
        }
        private set
        {
            timer = value;
            ActionOnTimer?.Invoke(timer);
        }
    }

    private void Update()
    {
        Timer += Time.deltaTime;
    }
}
