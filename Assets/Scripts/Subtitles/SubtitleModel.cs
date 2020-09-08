using Honeti;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoUIAnimator
{   

    public class SubtitleModel : MonoBehaviour
    {
        #region Public Variables

        public SubtitleType thisType;
        public MediaArchitecture thisEvent;

        #endregion

        #region Internal Variables

        public bool showUI;
        public bool playAudio;
        public float timeOut = 2.0f;
        public bool supported;
        public string tmPath;

        internal AudioSource audioSource;
        internal I18NAudioSource languagesAudioClips;    

        #endregion
        
        public IEnumerator ApplyMediaIDSettings()
        {
            foreach (var item in MediaIDController.Instance.GetSupportedMediaIDsList())
            {

                if (item.avatarEvent == thisEvent.ToString())
                {
                    showUI = item.subtitle;
                    playAudio = item.audio;
                    supported = item.active;
                    tmPath = item.path;
                    timeOut = item.displayTime;
                }
            }
            yield return true;
        }

    }
}