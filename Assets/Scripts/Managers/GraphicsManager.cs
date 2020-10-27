using System.Linq;
using UI.Data;
using UnityEditor;
using UnityEngine;

//-------------------------------------------------------------------------
/// <summary>
/// This allows for anyone to set custom resolutions through the Inspector
/// </summary>
[System.Serializable]
public class Resolution
{
	public int myWidth;
	public int myHeight;
}

public class GraphicsManager
{
    public static GraphicsManager ourInstance
    {
        get
        {
            if (ourUniqueInstance == null)
            {
                ourUniqueInstance = new GraphicsManager();
            }

            return ourUniqueInstance;
        }
    }

    public Resolution[] mySupportedResolutions
    {
        get
        {
            if (myResolutions == null)
            {
                myResolutions = GetSupportedResolutions();
            }

            return myResolutions;
        }
    }

    private static GraphicsManager ourUniqueInstance;

    private Resolution[] myResolutions;

    private float[] mySupportedAspects = new float[] { 16.0f / 9.0f, 16.0f / 10.0f };

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    public static void OnGameStart()
    {
#if UNITY_EDITOR
        Debug.Log("Loading graphics settings on game start");
#endif

        ourInstance.ApplyGraphicSettings();
    }

    public void ApplyGraphicSettings()
    {
        OptionsDataManager optionsDataManager = OptionsDataManager.ourInstance;

        Resolution targetResolution = GetResolutionOrDefault();

        Screen.SetResolution(
            width: targetResolution.myWidth,
            height: targetResolution.myHeight,
            fullscreen: optionsDataManager.FullScreenMode);

        QualitySettings.vSyncCount = optionsDataManager.VSync ? 1 : 0;

        //  Simulates fullscreen toggling during development
#if UNITY_EDITOR
        EditorWindow window = EditorWindow.focusedWindow;
        window.maximized = optionsDataManager.FullScreenMode;
#endif
    }

    private Resolution[] GetSupportedResolutions()
    {
        return Screen.resolutions
            .Select(r => new Resolution { myWidth = r.width, myHeight = r.height })
            .Where(r => IsSupported(r.myWidth, r.myHeight))
            .OrderBy(r => r.myWidth * r.myHeight)
            .ToArray();
    }

    private bool IsSupported(int aWidth, int aHeight)
    {
        float aspect = aWidth / (float)aHeight;
        string aspectStr = $"{aspect:F2}";
        
        foreach(float supportedAspect in mySupportedAspects)
        {
            if ($"{supportedAspect:F2}" == aspectStr)
            {
                return true;
            }
        }

        return false;
    }

    private Resolution GetResolutionOrDefault()
    {
        if (mySupportedResolutions.Length == 0)
        {
            return new Resolution { myWidth = 1920, myHeight = 1080 };
        }

        Resolution savedResolution = OptionsDataManager.ourInstance.Resolution;

        foreach(Resolution supportedResolution in mySupportedResolutions)
        {
            if (supportedResolution.myWidth == savedResolution.myWidth && supportedResolution.myHeight == savedResolution.myHeight)
            {
                return savedResolution;
            }
        }

        return mySupportedResolutions[0];
    }
}
