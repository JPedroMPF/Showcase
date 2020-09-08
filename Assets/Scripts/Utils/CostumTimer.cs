using NoUIAnimator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionBox.AvatarUtils
{
    public class CostumTimer : MonoBehaviour
    {
        public delegate void TimerEvent(SubtitleType timerType, float requestedTimeOut);
        public event TimerEvent onTimerComplete;

        Dictionary<SubtitleType, Timer> timersDictionary = new Dictionary<SubtitleType, Timer>();

        public float currentTime = 0.0f;

        private void Update()
        {
            if (timersDictionary.Count > 0)
            {
                foreach (var item in timersDictionary)
                {
                    if (item.Value.canCount)
                    {
                        item.Value.currentTime += Time.deltaTime;
                        if (item.Value.currentTime >= item.Value.timeToTime)
                        {
                            onTimerComplete?.Invoke(item.Key, item.Value.timeToTime);
                            item.Value.Reset();
                        }
                        
                    }
                }
            }
        }

        public void AddTimer(SubtitleType key, float timeToTimeIN)
        {
            if(timeToTimeIN == 0)
            {
                //Debug.Log(key + " subtitle time is set to zero no timer added");
                return;
            }


            if (!timersDictionary.ContainsKey(key))
            {
                Timer timer = new Timer();
                timer.timeToTime = timeToTimeIN;
                timersDictionary.Add(key, timer);

            }
            else
            {
                timersDictionary[key].currentTime = 0;
                timersDictionary[key].timeToTime = timeToTimeIN;
                timersDictionary[key].canCount = true;

            }
        }

        public void StopTimer(SubtitleType key)
        {
            if (timersDictionary.ContainsKey(key))
            {
                timersDictionary[key].currentTime = 0;
                timersDictionary[key].canCount = false;
            }               
            
        }
    }

    public class Timer
    {
        public float timeToTime { get; set; }
        public float currentTime { get; set; } = 0;
        public bool canCount { get; set; } = true;

        public void Reset()
        {
            timeToTime = 0;
            currentTime = 0;
            canCount = false;
        }

    }
}
