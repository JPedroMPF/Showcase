using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenSettings : MonoBehaviour
{
    public static Ease EASE_IN_TYPE = Ease.OutBack;
    public Ease easeInType;

    public static Ease EASE_ÕUT_TYPE = Ease.OutBack;
    public Ease easeOutType;

    public static float WF_BG_SPEED = 0.5f;
    public float wfBgSpeed;

    public static float WF_SUBTITLE_SPEED = 0.5f;
    public float wfSubtitleSpeed;

    public static float ACTUATOR_SHOW_SPEED = 1f;
    public float actuatorShowSpeed;

    public static float ACTUATOR_HIDE_SPEED = 1f;
    public float actuatorHideSpeed;

    private void Update()
    {
        EASE_IN_TYPE = easeInType;
        EASE_ÕUT_TYPE = easeOutType;
        WF_BG_SPEED = wfBgSpeed;
        WF_SUBTITLE_SPEED = wfSubtitleSpeed;
        ACTUATOR_HIDE_SPEED = actuatorHideSpeed;
        ACTUATOR_SHOW_SPEED = actuatorShowSpeed;
    }


}
