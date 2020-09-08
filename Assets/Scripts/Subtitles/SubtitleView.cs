using com.VisionBox.AvatarUtils;
using DG.Tweening;
using Honeti;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NoUIAnimator
{
    public class SubtitleView : MonoBehaviour
    {

        //VIEW SETTINGS
        public bool subtitleVisible = false;
        public bool setToShow = false;
        internal float elapsedShownedTime = 0;
        public bool audioPlaying = false;
        public bool animateBG = true;

        //SUBTITLE RECTTRANSFORM INFO
        RectTransform backgroundRectTransform;
        internal float subtitleHeight;
        internal float tempShowYPos;
        float thisHiddenYPos;
        float thisShowYPos;

        //BACKGROUND RECTTRANSFORM INFO    
        RectTransform subtitleRectTransform;
        internal float backgroundHeight;
        internal float tempBGShowYPos;
        float thisBGHiddenYPos;
        float thisBGShowYPos;

        //TEXT
        public Text titleText;
        I18NText titleTextTranslator;
        Text subtitleText;
        I18NText subtitleTextTranslator;
        
        //AUDIO
        I18NAudioSource audioTranslator;
        AudioSource audioSource;
        Sequence audioTimer;

        
        internal IEnumerator SetupView(string thisMediaID)
        {
            GetReferences();
            SetupTranslations(thisMediaID);
            SetupAudioTranslation(thisMediaID);
            CalculateSubtitleSize();
            Hide();
            yield return true;
        }

        internal void PlayAudio(bool playAudio)
        {
            if (!playAudio || audioSource.clip == null)
            {
                return;
            }

            audioSource.Play();
            audioPlaying = true;

            audioTimer = DOTween.Sequence();
            audioTimer.PrependInterval(audioSource.clip.length);
            audioTimer.OnComplete(StopAudio);
            audioTimer.Play();
        }


        internal void StopAudio()
        {
            audioSource.Stop();
            audioPlaying = false;
        }

        private void GetReferences()
        {
            subtitleRectTransform = transform.GetComponent<RectTransform>();
            backgroundRectTransform = transform.parent.Find("BG").GetComponent<RectTransform>();
            try
            {
                titleText = AvatarUtils.GetChildWithName(transform, "Title").GetComponent<Text>();
                titleTextTranslator = titleText.GetComponent<I18NText>();
            }
            catch { }

            try
            {
                subtitleText = AvatarUtils.GetChildWithName(transform, "SubTitle").GetComponent<Text>();
                subtitleTextTranslator = subtitleText.GetComponent<I18NText>();
            }
            catch { }

            try
            {
                audioTranslator = transform.GetComponent<I18NAudioSource>();
                audioSource = transform.GetComponent<AudioSource>();
            }
            catch { }
            
        }


        private void SetupAudioTranslation(string thisMediaID)
        {
            audioTranslator.enabled = true;
          //  audioTranslator.SetAudios(SoundType.Subtitles, thisMediaID);
        }

        internal  void SetupTranslations(string thisMediaID)
        {
            if (titleText == null)
            {
                Debug.LogWarning("No title component");
                return;
            }                

            titleText.text = thisMediaID;
            if(titleTextTranslator!=null)
            {
                titleTextTranslator.enabled = true;
            }

            if (subtitleText!=null)
            {
                subtitleText.text = thisMediaID + "_SUB";
                subtitleTextTranslator.enabled = true;

                if (subtitleText.text.Contains(I18N.instance._noTranslationText))
                {
                    subtitleText.gameObject.SetActive(false);
                }
            }
            
        }

        internal void UpdateSubtitleText(string newText)
        {
            if(subtitleText == null)
            {
                Debug.LogWarning("No Subtitle component");
                return;
            }

            subtitleTextTranslator.enabled = false;

            subtitleText.text = newText;
            subtitleText.gameObject.SetActive(true);
            CalculateSubtitleSize();
            TweenElements();
        }

        internal void ResetSubtitleText(string thisMediaID)
        {
            if (subtitleText == null)
            {
                Debug.LogWarning("No Subtitle component");
                return;
            }

            //ROLLBACK SUBTITLE TEXT
            subtitleText.text = thisMediaID + "_SUB";
            subtitleTextTranslator.updateTranslation();

            if (subtitleText.text.Contains(I18N.instance._noTranslationText))
            {
                subtitleText.gameObject.SetActive(false);
            }
           
            CalculateSubtitleSize();
            TweenElements();
        }

        internal void UpdateTitleText(string newText)
        {
            if (titleText == null)
            {
                Debug.LogWarning("No title component");
                return;
            }

            titleText.text = newText;
            CalculateSubtitleSize();
            tempBGShowYPos = thisBGShowYPos;
            TweenElements();
        }

        internal virtual void CalculateSubtitleSize()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(subtitleRectTransform);

            subtitleHeight = subtitleRectTransform.rect.height;
            backgroundHeight = backgroundRectTransform.rect.height;           

            thisHiddenYPos = 0 - subtitleHeight;
            thisShowYPos = 0;

            thisBGHiddenYPos = 0 - backgroundHeight;            
            thisBGShowYPos = -backgroundHeight + subtitleHeight;
        }
        
        internal virtual void Hide()
        {
            tempShowYPos = thisHiddenYPos;
            tempBGShowYPos = thisBGHiddenYPos;
            setToShow = false;
            TweenElements(false);
          
        }
     
        internal virtual void Show(float timeOut = 0)
        {
            tempShowYPos = thisShowYPos;
            tempBGShowYPos = thisBGShowYPos;
            setToShow = true;
            TweenElements(true);      
            
        }

        internal void TweenElements(bool finalState = true)
        {
            subtitleRectTransform.DOLocalMoveY(tempShowYPos,
                TweenSettings.WF_SUBTITLE_SPEED)
                .SetEase(TweenSettings.EASE_ÕUT_TYPE)
                .OnComplete(() =>
                {
                    subtitleVisible = finalState;
                    if (finalState == true)
                        BroadcastMessage("StartSpriteAnimation",SendMessageOptions.DontRequireReceiver);
                    else
                        BroadcastMessage("StopSpriteAnimation", SendMessageOptions.DontRequireReceiver);
                });

            if (!animateBG)
                return;

            backgroundRectTransform.DOLocalMoveY(tempBGShowYPos,
                TweenSettings.WF_BG_SPEED)
                .SetEase(TweenSettings.EASE_ÕUT_TYPE);
        }

        internal void UpdateShowPosition(float heightIn)
        {
            tempShowYPos = thisShowYPos + heightIn;
            tempBGShowYPos = thisBGShowYPos + heightIn;
        }
    }
}
