using NoUIAnimator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubController = NoUIAnimator.SubtitleController;

public class SubtitleObject : MonoBehaviour
{
    
    public SubtitleView subtitleView;
    public SubtitleModel subtitleModel;   

    private void Awake()
    {
        subtitleModel = GetComponent<SubtitleModel>();
        subtitleView = GetComponent<SubtitleView>();
    }    

    private void Start()
    {
        StartCoroutine(LoadMediaIDSettings());
    }

    private IEnumerator LoadMediaIDSettings()
    {
        while(!MediaIDController.MEDIAIDLOADED || !LanguageController.LANGUAGESREADY || !SoundsController.SOUNDSREADY)
        {           
            yield return null;
        }
        yield return subtitleModel.ApplyMediaIDSettings();
        yield return subtitleView.SetupView(subtitleModel.thisEvent.ToString());

    }

    public void UpdateSubtitleText(string infoToDisplay)
    {
        subtitleView.UpdateSubtitleText(infoToDisplay);
    }

    public void UpdateTitleText(string infoToDisplay)
    {
        subtitleView.UpdateTitleText(infoToDisplay);
    }

    public void ShowSubtitle()
    {
        subtitleView.Show(subtitleModel.timeOut);
    }

    public void PlayAudio()
    {
        subtitleView.PlayAudio(subtitleModel.playAudio);
    }

    public void StopAudio()
    {
        subtitleView.StopAudio();
    }

    public void HideSubtitle()
    {
        subtitleView.Hide();
        subtitleView.StopAudio();
    }

    internal void UpdateSubtitlePosition(float tempSubHeight)
    {
        subtitleView.UpdateShowPosition(tempSubHeight);
        subtitleView.TweenElements();
    }

    internal void ClearDetailInformation()
    {
        subtitleView.ResetSubtitleText(subtitleModel.thisEvent.ToString());
    }

    internal void UpdateTitleText(string message, bool v)
    {
        throw new NotImplementedException();
    }
}
