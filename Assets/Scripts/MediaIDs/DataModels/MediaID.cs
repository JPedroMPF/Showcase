using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MediaID
{
    public List<MediaIDObject> mediaIdList;
}

[Serializable]
public class MediaIDObject
{
    public string avatarEvent;
    public string path;
    public bool active;
    public bool subtitle;
    public bool audio;
    public int displayTime;

}

