using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class AvatarUtils
{

    public delegate void ChildHandler(GameObject child);

    /// <summary>
    /// Iterates all children of a game object
    /// </summary>
    /// <param name="gameObject">A root game object</param>
    /// <param name="childHandler">A function to execute on each child</param>
    /// <param name="recursive">Do it on children? (in depth)</param>
    public static void IterateChildren(GameObject gameObject, ChildHandler childHandler, bool recursive)
    {
        DoIterate(gameObject, childHandler, recursive);
    }

    /// <summary>
    /// NOTE: Recursive!!!
    /// </summary>
    /// <param name="gameObject">Game object to iterate</param>
    /// <param name="childHandler">A handler function on node</param>
    /// <param name="recursive">Do it on children?</param>
    private static void DoIterate(GameObject gameObject, ChildHandler childHandler, bool recursive)
    {
        foreach (Transform child in gameObject.transform)
        {
            childHandler(child.gameObject);
            if (recursive)
                DoIterate(child.gameObject, childHandler, true);
        }
    }

    public static string GetArg(string name)
    {
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == name && args.Length > i + 1)
            {
                return args[i + 1];
            }
        }
        return null;
    }

    public static readonly string[] SizeSuffixes =
                      { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
    public static string SizeSuffix(int value, int decimalPlaces = 1)
    {
        if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }

        // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
        int mag = (int)System.Math.Log(value, 1024);

        // 1L << (mag * 10) == 2 ^ (10 * mag) 
        // [i.e. the number of bytes in the unit corresponding to mag]
        decimal adjustedSize = (decimal)value / (1L << (mag * 10));

        // make adjustment when the value is large enough that
        // it would round up to 1000 or more
        if (System.Math.Round(adjustedSize, decimalPlaces) >= 1000)
        {
            mag += 1;
            adjustedSize /= 1024;
        }

        return string.Format("{0:n" + decimalPlaces + "} {1}",
            adjustedSize,
            SizeSuffixes[mag]);
    }

    public static double ConvertToMegabytes(double bytes)
    {
        return (bytes / 1024d) / 1024d;
    }

    public static T GetEnumFromMediaIDString<T>(string description)
    {
        var type = typeof(T);
        if (!type.IsEnum) throw new InvalidOperationException();

        foreach (var field in type.GetFields())
        {
            var attribute = Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (attribute != null)
            {
                if (attribute.Description == description)
                    return (T)field.GetValue(null);
            }
            else
            {
                if (field.Name == description)
                    return (T)field.GetValue(null);
            }
        }        
        return default(T);
    }

    public static string GetEnumDescription(Enum mediaID)
    {
        Type type = mediaID.GetType();
        MemberInfo[] memInfo = type.GetMember(mediaID.ToString());

        if (memInfo != null && memInfo.Length > 0)
        {
            object[] attrs = (object[])memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attrs != null && attrs.Length > 0)
            {
                return ((DescriptionAttribute)attrs[0]).Description;
            }
        }

        return mediaID.ToString();
    }

    public static bool HasParameter(string paramName, Animator animator)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            
            if (param.name == paramName)
                return true;
        }
        return false;
    }

    public static bool IsMediaIDAlarm(MediaArchitecture mediaIDIn)
    {
        bool _isAlarm = false;
        
        if (mediaIDIn == MediaArchitecture.WarningTailGate ||
            mediaIDIn == MediaArchitecture.WarningTwoFaces ||
            mediaIDIn == MediaArchitecture.WarningPersonNotDetected ||
            mediaIDIn == MediaArchitecture.WarningDoorsBlocked ||
            mediaIDIn == MediaArchitecture.WarningDoorsForced ||
            mediaIDIn == MediaArchitecture.WarningJumpIn ||
            mediaIDIn == MediaArchitecture.WarningJumpOut ||
            mediaIDIn == MediaArchitecture.WarningAbandonedObject)
        {
            _isAlarm = true;
        }

        return _isAlarm;        
    }

    public static Transform GetChildWithName(Transform parent, string childName)
    {
        Transform[] children = parent.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {           
            if(child.name == childName)
            {
                return child;
            }            
        }

        return null;

    }
}
