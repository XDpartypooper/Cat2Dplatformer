using UnityEngine;
using System.Runtime.InteropServices;

public class LandScape : MonoBehaviour
{
    // This links to the .jslib file
    [DllImport("__Internal")]
    private static extern void RequestLandscapeFullscreen();

    public void TriggerLandscape()
    {
       
        #if UNITY_WEBGL && !UNITY_EDITOR
            RequestLandscapeFullscreen();
        #else
        Debug.Log("Fullscreen landscape requested (Only works in WebGL build).");
        #endif
    }
}