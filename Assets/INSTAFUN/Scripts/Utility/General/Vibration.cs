using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vibration
{
#if UNITY_ANDROID && !UNITY_EDITOR
        public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
        public static AndroidJavaClass vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
        public static int defaultAmplitude = vibrationEffectClass.GetStatic<int>("DEFAULT_AMPLITUDE");
        public static AndroidJavaClass androidVersion = new AndroidJavaClass("android.os.Build$VERSION");
        public static int apiLevel = androidVersion.GetStatic<int>("SDK_INT");
#else
    public static AndroidJavaClass unityPlayer;
    public static AndroidJavaObject vibrator;
    public static AndroidJavaObject currentActivity;
    public static AndroidJavaClass vibrationEffectClass;
    public static int defaultAmplitude = 100;
#endif

    public static void Vibrate(long milliseconds)
    {
        CreateOneShot(milliseconds, defaultAmplitude);
    }

    public static void CreateOneShot(long milliseconds, int amplitude)
    {
        CreateVibrationEffect("createOneShot", new object[] { milliseconds, amplitude });
    }

    public static void CreateWaveForm(long[] timings, int repeat)
    {
        CreateVibrationEffect("createWaveForm", new object[] { timings, repeat });
    }

    public static void CreateWaveForm(long[] timings, int[] amplitudes, int repeat)
    {
        CreateVibrationEffect("createWaveForm", new object[] { timings, amplitudes, repeat });
    }

    public static void CreateVibrationEffect(string function, params object[] args)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (isAndroid() && HasAmplituideControl())
        {
            AndroidJavaObject vibrationEffect = vibrationEffectClass.CallStatic<AndroidJavaObject>(function, args);
            vibrator.Call("vibrate", vibrationEffect);
        }
        else
        {
            Handheld.Vibrate();
        }
#endif
    }

    public static bool HasVibrator()
    {
        return vibrator.Call<bool>("hasVibrator");
    }

    public static bool HasAmplituideControl()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
                if (apiLevel >= 26)
                    return vibrator.Call<bool>("hasAmplitudeControl"); // API 26+ specific
                else
                    return false; // no amplitude control below API level 26
#else
        return false;
#endif
    }

    public static void Cancel()
    {
        if (isAndroid())
            vibrator.Call("cancel");
    }

    public static bool isAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
	        return true;
#else
        return false;
#endif
    }
}
