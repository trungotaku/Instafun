using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public static class UtilityExtension
{
    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = UnityEngine.Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    public static List<T> Random<T>(this IEnumerable<T> sequence)
    {
        List<T> list = sequence.ToList();
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = UnityEngine.Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }
    public static void ForEach<T>(this IEnumerable<T> sequence, Action<int, T> action)
    {
        // argument null checking omitted
        int i = 0;
        foreach (T item in sequence)
        {
            action(i, item);
            i++;
        }
    }

    /// <summary>
    /// Creates and returns a clone of any given scriptable object.
    /// </summary>
    public static T Clone<T>(this T scriptableObject) where T : ScriptableObject
    {
        if (scriptableObject == null)
        {
            Debug.LogError($"ScriptableObject was null. Returning default {typeof(T)} object.");
            return (T) ScriptableObject.CreateInstance(typeof(T));
        }

        T instance = UnityEngine.Object.Instantiate(scriptableObject);
        instance.name = scriptableObject.name; // remove (Clone) from name
        return instance;
    }
    public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
    {
        return listToClone.Select(item => (T) item.Clone()).ToList();
    }

    public static string FirstCharToUpper(this string input)
    {
        switch (input)
        {
            case null:
                throw new ArgumentNullException(nameof(input));
            case "":
                throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
            default:
                return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }

    public static Vector3 GetCenterPosition(List<Vector3> list_)
    {
        Vector3 pos = Vector3.zero;

        foreach (var item in list_)
        {
            pos += item;
        }

        return pos / (float) list_.Count;
    }
    public static Vector2 GetCenterPosition(List<Vector2> list_)
    {
        Vector2 pos = Vector2.zero;

        foreach (var item in list_)
        {
            pos += item;
        }

        return pos / (float) list_.Count;
    }
    public static bool CheckHitSquare(Vector2 pos_, Vector2 posMin_, Vector2 posMax_)
    {
        if (pos_.x > posMin_.x && pos_.y > posMin_.y && pos_.x < posMax_.x && pos_.y < posMax_.y)
        {
            return true;
        }
        return false;
    }
    public static bool CheckHitSquareLUDR(Vector2 pos_, Vector2 posMin_, Vector2 posMax_)
    {
        Vector2 posMin = new Vector2(posMin_.x, posMax_.y);
        Vector2 posMax = new Vector2(posMax_.x, posMin_.y);
        if (pos_.x > posMin.x && pos_.y > posMin.y && pos_.x < posMax.x && pos_.y < posMax.y)
        {
            return true;
        }
        return false;
    }

    public static string ConvertToFirebaseAnalyticsParamValue(string value_)
    {
        int length = value_.Length > 99 ? 99 : value_.Length;
        return value_.Substring(0, length);
    }

    public static bool IOS_Platform
    {
        get
        {
            return Application.platform.Equals(RuntimePlatform.IPhonePlayer) ||
                Application.platform.Equals(RuntimePlatform.OSXPlayer) ||
                Application.platform.Equals(RuntimePlatform.tvOS) ||
                Application.platform.Equals(RuntimePlatform.OSXEditor);
        }
    }

    public static bool AndroidPlatform
    {
        get
        {
            return Application.platform.Equals(RuntimePlatform.Android);
        }
    }

    public static bool CheckInternet()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }

    private static System.Random _random = new System.Random();
    public static string RandomDisplayName()
    {
        int length = 5;
        const string chars = "01234567890123456789";
        return "User" + new string(Enumerable.Repeat(chars, length)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
    }

    public static string GetDeviceId()
    {
        string deviceId = "";
#if UNITY_EDITOR
        deviceId = SystemInfo.deviceModel; // + UnityEngine.Random.Range(0, 1000);
#elif UNITY_IOS
        deviceId = SystemInfo.deviceUniqueIdentifier;
#else
        //Get the device id from native android
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject>("getContentResolver");
        AndroidJavaClass secure = new AndroidJavaClass("android.provider.Settings$Secure");
        deviceId = secure.CallStatic<string>("getString", contentResolver, "android_id");
#endif
        return deviceId;
    }

    static string m_gameCennterId = "";
    public static string GetGameCenterId()
    {
        //if (!string.IsNullOrEmpty(m_gameCennterId))
        //{
        //    return m_gameCennterId;
        //}

        string id = "GAME_CENTER_ID";
#if UNITY_EDITOR || UNITY_ANDROID
        id = "GAME_CENTER_ID";
#else
        // id = KTGameCenter.SharedCenter().PlayerId;
        // id = GameCenterHelper.GetPlayerId();
#endif

        m_gameCennterId = id;
        return id;
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    public static string GetIDFA()
    {
        UnityEngine.AndroidJavaClass up = new UnityEngine.AndroidJavaClass("com.unity3d.player.UnityPlayer");
        UnityEngine.AndroidJavaObject currentActivity = up.GetStatic<UnityEngine.AndroidJavaObject>("currentActivity");
        UnityEngine.AndroidJavaObject contentResolver = currentActivity.Call<UnityEngine.AndroidJavaObject>("getContentResolver");
        UnityEngine.AndroidJavaObject secure = new UnityEngine.AndroidJavaObject("android.provider.Settings$Secure");
        string deviceID = secure.CallStatic<string>("getString", contentResolver, "android_id");
        return Md5Sum(deviceID).ToUpper();
    }
#endif

#if UNITY_IOS && !UNITY_EDITOR
    public static string GetIDFA()
    {
        return Md5Sum(UnityEngine.iOS.Device.advertisingIdentifier).ToUpper();
    }
#endif

#if UNITY_EDITOR
    public static string GetIDFA()
    {
        return "Can't get IDFA on Editor mode!!!";
    }
#endif

    public static string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        string hashString = "";
        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }
    public static string ToScore(this string str_)
    {
        return String.Format("{0:#,###0}", int.Parse(str_));
    }
    public static string ToScore(this int int_)
    {
        return String.Format("{0:#,###0}", int_);
    }
    public static string ToScore(this float float_)
    {
        return String.Format("{0:#,###0}", (int) float_);
    }
    public static bool CheckOnScreen(Vector3 pos_)
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(pos_);
        bool onScreen = screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        return onScreen;
    }
    public static int GetFibonacciValue(int index_, int f1_, int f2_)
    {
        if (index_ == 0) return 0;
        if (index_ == 1) return f1_;
        if (index_ == 2) return f2_;

        int[] fibArr = new int[index_ + 1];
        fibArr[0] = 0;
        fibArr[1] = f1_;
        fibArr[2] = f2_;
        for (int i = 3; i < fibArr.Length; i++)
        {
            fibArr[i] = fibArr[i - 1] + fibArr[i - 2];
        }
        return fibArr[index_];
    }
    public static void SetFixedDeltaTimeByDeviceHz()
    {
        float refreshRate = (float) Screen.currentResolution.refreshRate;
        refreshRate = refreshRate < 60 ? 60 : refreshRate;
        Time.fixedDeltaTime = 1 / refreshRate;
        Time.maximumDeltaTime = 1 / refreshRate * 6f;

        // Debug.Log("This device refreshRate: " + Screen.currentResolution.refreshRate);
        // Debug.Log("refreshRate: " + refreshRate);
    }
    public static void SetFixedDeltaTimeByDefault()
    {
        Time.fixedDeltaTime = 1 / 120f;
        Time.maximumDeltaTime = 0.05f;
    }
    public static Vector3 CalculateBezierPoint(float t, Vector3 from, Vector3 fromControl, Vector3 toControl, Vector3 to)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 position = uuu * from;
        position += 3 * uu * t * fromControl;
        position += 3 * u * tt * toControl;
        position += ttt * to;

        return position;
    }
    public static string AppVersion
    {
        get
        {
#if UNITY_EDITOR
            return Application.version;
#else
            return Application.version;
#endif
        }
    }
    public static string ConvertTicksToTimeStr(long tick_)
    {
        DateTime dateTime = new DateTime(tick_);

        string hour = dateTime.Hour.ToString();
        string minute = dateTime.Minute < 10 && dateTime.Hour > 0 ? "0" + dateTime.Minute : dateTime.Minute + "";
        string second = dateTime.Second < 10 && (dateTime.Minute > 0 || dateTime.Hour > 0) ? "0" + dateTime.Second : dateTime.Second + "";

        hour = hour == "0" ? "" : hour + "h ";
        minute = minute == "0" ? "" : minute + "m ";
        second = second + "s";
        return hour + minute + second;
    }
    public static string ConvertTimeSpanToTimeStr_HMS(TimeSpan timeSpan_)
    {
        string hour = timeSpan_.Hours.ToString();
        string minute = timeSpan_.Minutes < 10 && timeSpan_.Hours > 0 ? "0" + timeSpan_.Minutes : timeSpan_.Minutes + "";
        string second = timeSpan_.Seconds < 10 && (timeSpan_.Minutes > 0 || timeSpan_.Hours > 0) ? "0" + timeSpan_.Seconds : timeSpan_.Seconds + "";

        hour = hour == "0" ? "" : hour + "h ";
        minute = minute == "0" ? "" : minute + "m ";
        second = second + "s";
        return hour + minute + second;
    }
    public static string ConvertTimeSpanToTimeStr_DHM(TimeSpan timeSpan_)
    {
        string day = timeSpan_.Days.ToString();
        string hour = timeSpan_.Hours < 10 && timeSpan_.Days > 0 ? "0" + timeSpan_.Hours : timeSpan_.Hours + "";
        string min = timeSpan_.Minutes < 10 && (timeSpan_.Hours > 0 || timeSpan_.Hours > 0) ? "0" + timeSpan_.Minutes : timeSpan_.Minutes + "";

        day = day == "0" ? "" : day + "d ";
        hour = hour == "0" ? "" : hour + "h ";
        min = min + "m";
        return day + hour + min;
    }
    public static T SetAlpha<T>(this T g, float newAlpha)
    where T : Graphic
    {
        var color = g.color;
        color.a = newAlpha;
        g.color = color;
        return g;
    }

    public static T ParseEnum<T>(string value)
    {
        return (T) Enum.Parse(typeof(T), value, true);
    }

    public static T ToEnum<T>(this string value)
    {
        return (T) Enum.Parse(typeof(T), value, true);
    }
    public static string ValidatePIN(string ATMPIN)
    {
        //Insert your code here
        if (ATMPIN.Length != 4 || ATMPIN.Length != 6)
        {
            return "The ATMPIN is invalid";
        }

        if (ATMPIN.Any(c => !char.IsDigit(c)))
        {
            return "The ATMPIN is invalidzz";
        }
        return "The ATMPIN is valid";
    }
    // Check if a point is inside the quadrilateral
    public static bool IsPointInsideQuadrilateral(Vector2 point, Vector2[] vertices)
    {
        Vector2 A = vertices[0];
        Vector2 B = vertices[1];
        Vector2 C = vertices[2];
        Vector2 D = vertices[3];

        // Split the quadrilateral into two triangles: ABC and ACD
        bool isInTriangle1 = IsPointInTriangle(point, A, B, C);
        bool isInTriangle2 = IsPointInTriangle(point, A, C, D);

        // If the point is inside either of the triangles, it's inside the quadrilateral
        return isInTriangle1 || isInTriangle2;
    }

    // Check if a point is inside a triangle
    static bool IsPointInTriangle(Vector2 point, Vector2 A, Vector2 B, Vector2 C)
    {
        float denominator = (B.y - C.y) * (A.x - C.x) + (C.x - B.x) * (A.y - C.y);
        float a = ((B.y - C.y) * (point.x - C.x) + (C.x - B.x) * (point.y - C.y)) / denominator;
        float b = ((C.y - A.y) * (point.x - C.x) + (A.x - C.x) * (point.y - C.y)) / denominator;
        float c = 1 - a - b;

        // If all barycentric coordinates are between 0 and 1, the point is inside the triangle
        return a >= 0 && a <= 1 && b >= 0 && b <= 1 && c >= 0 && c <= 1;
    }
}