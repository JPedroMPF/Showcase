using com.VisionBox.AvatarUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NoUIAnimator
{
    public class SubtitleController : MonoBehaviour
    {       

        #region Private Variables

        Dictionary<SubtitleType, Dictionary<MediaArchitecture, SubtitleObject>> subtitlesDict;
        Dictionary<MediaArchitecture, SubtitleObject> workflowSubtitles;
        Dictionary<MediaArchitecture, SubtitleObject> alarmsSubtitles;
        Dictionary<MediaArchitecture, SubtitleObject> customSubtiles;        

        SubtitleObject currentWFSubtitle;
        SubtitleObject currentAlarmSubtitle;
        SubtitleObject currentCustomSubtitle;

        CostumTimer subtitlesTimer;

        #endregion
       
        #region Monobehaviour Methods

        private void Start()
        {
            SetSubtitlesReferences();

            if (gameObject.GetComponent<CostumTimer>() == null)
                subtitlesTimer = gameObject.AddComponent(typeof(CostumTimer)) as CostumTimer;

            subtitlesTimer.onTimerComplete += SubtitleTimeOut;
        }
        

        #endregion

        #region References Methods       

        private void SetSubtitlesReferences()
        {
            //FIND REFERENCES IN SCENE
            subtitlesDict = new Dictionary<SubtitleType, Dictionary<MediaArchitecture, SubtitleObject>>();
            workflowSubtitles = new Dictionary<MediaArchitecture, SubtitleObject>();
            alarmsSubtitles = new Dictionary<MediaArchitecture, SubtitleObject>();
            customSubtiles = new Dictionary<MediaArchitecture, SubtitleObject>();

            var allSubtitles = FindObjectsOfType<SubtitleObject>();

            string availableUI = "";

            //CONVERT TO DICTIONARY
            foreach (var item in allSubtitles)
            {
                if (item.subtitleModel.thisType == SubtitleType.Workflow)
                {
                    workflowSubtitles.Add(item.subtitleModel.thisEvent, item.GetComponent<SubtitleObject>());
                }
                if (item.subtitleModel.thisType == SubtitleType.Alarm)
                {
                    alarmsSubtitles.Add(item.subtitleModel.thisEvent, item.GetComponent<SubtitleObject>());
                }
                if(item.subtitleModel.thisType == SubtitleType.Costum)
                {
                    customSubtiles.Add(item.subtitleModel.thisEvent, item.GetComponent<SubtitleObject>());
                }
            }


            subtitlesDict.Add(SubtitleType.Workflow, workflowSubtitles);
            subtitlesDict.Add(SubtitleType.Alarm, alarmsSubtitles);
            subtitlesDict.Add(SubtitleType.Costum, customSubtiles);

            foreach (var item in subtitlesDict)
            {                
                foreach (var itemIN in item.Value)
                {
                    availableUI += string.Format("{0} \n", itemIN.Key);
                }
            }

            Debug.Log("UI Subtitles: \n" + availableUI);
        }

        #endregion

        #region Show Subtitles Methods

        internal void ShowSubtitle(MediaArchitecture newEvent, Data extraData = null)
        {
            if(!MediaIDController.MEDIAIDLOADED || !LanguageController.LANGUAGESREADY || !SoundsController.SOUNDSREADY)
            {
                Debug.Log("SUBS NOT READY");
                return;
            }

            //FETCH SUBTITLE
            SubtitleObject newSubtitle = GetSubtitleGOFromMediaID(newEvent);

            //HIDE ALL ON RESET
            if (newEvent == MediaArchitecture.Reset || newEvent == MediaArchitecture.WarningOutOfService)
            {
                HideSubtitleOfType(SubtitleType.Alarm);
                HideSubtitleOfType(SubtitleType.Costum);
                HideSubtitleOfType(SubtitleType.Workflow);
                return;
            }

            //CHECK FOR NULLS
            if (newSubtitle == null)
            {

                if (AvatarUtils.IsMediaIDAlarm(newEvent))
                {
                    HideSubtitleOfType(SubtitleType.Alarm);
#if UNITY_EDITOR
                    Debug.Log("Alarm UI null", DLogType.GUI);                    
#endif
                }
                if (!AvatarUtils.IsMediaIDAlarm(newEvent))
                {
                    HideSubtitleOfType(SubtitleType.Workflow);
#if UNITY_EDITOR
                    Debug.Log($"No UI for {newEvent}", DLogType.GUI);
#endif
                }
                return;
            }

            //ELSE HANDLE SUBTILE
            HandleSubtitleLogic(newSubtitle, extraData);
        }

        private void HandleSubtitleLogic(SubtitleObject tempSubtitle, Data extraData)
        {   
            
            ref SubtitleObject subtitleToCompare = ref currentCustomSubtitle;

            switch (tempSubtitle.subtitleModel.thisType)
            {
                case SubtitleType.Alarm:
                    subtitleToCompare = ref currentAlarmSubtitle;
                    if(extraData.Value == false)
                    {
                        HideSubtitleOfType(subtitleToCompare.subtitleModel.thisType);
                        HandleSubtitlesRaise();
                        return;
                    }

                    break;
                case SubtitleType.Workflow:
                    subtitleToCompare = ref currentWFSubtitle;

                    //HANDLE EXTRA DATA
                    if(extraData == null)
                    {
                        tempSubtitle.ClearDetailInformation();
                    }
                    else if (String.IsNullOrEmpty(extraData.Message) == false)
                    {
                        tempSubtitle.UpdateSubtitleText(extraData.Message);
                    }

                    break;
                case SubtitleType.Costum:
                    subtitleToCompare = ref currentCustomSubtitle;

                    if (extraData != null && String.IsNullOrEmpty(extraData.Message) == false)
                    {
                        tempSubtitle.UpdateTitleText(extraData.Message);
                    }

                    break;
            }
            
            if (subtitleToCompare != null)
                subtitleToCompare.HideSubtitle();

            //CHECK IF IS SET BE SHOWNED
            if (tempSubtitle.subtitleModel.showUI == false)
            {
                if (subtitleToCompare != null)
                {
                    HideSubtitleOfType(subtitleToCompare.subtitleModel.thisType);
                    HandleSubtitlesRaise();
                }                    
                return;
            }

            //IF NO SUBTITLE OF SAME TYPE IS A
            if (subtitleToCompare == null || subtitleToCompare.subtitleView.subtitleVisible == false)
            {
               ShowAndTimeSubtitle(ref subtitleToCompare, tempSubtitle);
            }
            else
            {
                //RESET TIMER IF IS SAME
                if (subtitleToCompare == tempSubtitle)
                {
                    subtitlesTimer.AddTimer(subtitleToCompare.subtitleModel.thisType,
                    subtitleToCompare.subtitleModel.timeOut);
                }
                else
                {
                    subtitleToCompare.HideSubtitle();
                    ShowAndTimeSubtitle(ref subtitleToCompare, tempSubtitle);
                }
            }            
        }


        public void ShowAndTimeSubtitle(ref SubtitleObject currentSubtitle, SubtitleObject newSubtitle)
        {
            currentSubtitle = newSubtitle;
            PrioritizeAudios(currentSubtitle);
            //SHOW SUBTITLE
            currentSubtitle.ShowSubtitle();         

            //SET SHOW TIMER
            subtitlesTimer.AddTimer(currentSubtitle.subtitleModel.thisType,
                currentSubtitle.subtitleModel.timeOut);

            HandleSubtitlesRaise();
           
            
        }

        private void PrioritizeAudios(SubtitleObject currentSubtitle)
        {
            bool wfAudioPlaying = (currentWFSubtitle?.subtitleView.audioPlaying == true) ? true : false;
            bool alarmAudioPlaying = (currentAlarmSubtitle?.subtitleView.audioPlaying == true) ? true : false;
            
            if(!wfAudioPlaying && !alarmAudioPlaying)
            {
                currentSubtitle.PlayAudio();
                return;
            }
            if (currentSubtitle.subtitleModel.thisType == SubtitleType.Alarm && wfAudioPlaying == true)
            {
                currentWFSubtitle.StopAudio();
                currentSubtitle.PlayAudio();
            }

            if (currentSubtitle.subtitleModel.thisType == SubtitleType.Workflow 
                && alarmAudioPlaying == false)
            {
                currentSubtitle.PlayAudio();
            }            

        }

        public void HandleSubtitlesRaise()
        {
            float alarmHeight = (currentAlarmSubtitle != null) ? currentAlarmSubtitle.subtitleView.subtitleHeight : 0;
            float customHeight = (currentCustomSubtitle != null) ? currentCustomSubtitle.subtitleView.subtitleHeight :0;
            float finalWFRaiseHeight = 0;
            float finalALRaiseHeight = 0;

            //CHECK IF RAISE IS NEEDED
            if (currentWFSubtitle?.subtitleView.setToShow == true)
            {
                finalWFRaiseHeight += (currentAlarmSubtitle?.subtitleView.setToShow == true) ? alarmHeight : 0;
                finalWFRaiseHeight += (currentCustomSubtitle?.subtitleView.setToShow == true) ? customHeight : 0;

                currentWFSubtitle.UpdateSubtitlePosition(finalWFRaiseHeight);
            }

            if(currentAlarmSubtitle?.subtitleView.setToShow == true)
            {
                finalALRaiseHeight += (currentCustomSubtitle?.subtitleView.setToShow == true) ? customHeight : 0;

                currentAlarmSubtitle.UpdateSubtitlePosition(finalALRaiseHeight);
            }            
        }

        #endregion

        #region Hide Subtitles Methods

        private void SubtitleTimeOut(SubtitleType timerType, float requestedTimeOut)
        {
            HideSubtitleOfType(timerType);
            HandleSubtitlesRaise();
        }

        private void HideSubtitleOfType(SubtitleType subtitleTypeIN)
        {
            switch (subtitleTypeIN)
            {
                case SubtitleType.Alarm:
                    currentAlarmSubtitle?.HideSubtitle();
                    break;
                case SubtitleType.Workflow:
                    currentWFSubtitle?.HideSubtitle();
                    break;
                case SubtitleType.Costum:
                    currentCustomSubtitle?.HideSubtitle();
                    break;
                default:
                    break;
            }
        }


        #endregion

        #region Utils Methods

        public SubtitleObject GetSubtitleGOFromMediaID(MediaArchitecture newEvent)
        {
            SubtitleObject tempSubtitle = null;
            if (workflowSubtitles.TryGetValue(newEvent, out tempSubtitle))
            {
                return tempSubtitle;
            }           
            if (alarmsSubtitles.TryGetValue(newEvent, out tempSubtitle))
            {
                return tempSubtitle;
            }
            if (customSubtiles.TryGetValue(newEvent, out tempSubtitle))
            {
                return tempSubtitle;
            }

            return tempSubtitle;
        }

        #endregion

    }
}
