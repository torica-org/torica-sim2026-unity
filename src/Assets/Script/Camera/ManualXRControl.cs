using System;
using System.Collections;
using UnityEngine;

using UnityEngine.XR.Management;

public class ManualXRControl
{
    private GameManager gm = GameManager.instance;

    public IEnumerator StartXRCoroutine()
    {
        gm.game.error = true;
        gm.game.errorText = "Initializing XR...";
        Debug.Log("Initializing XR...");

        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        gm.game.error = true;
        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            gm.game.errorText = "Initializing XR Failed. Check Editor or Player log for details.";
            Debug.LogWarning("Initializing XR Failed. Check Editor or Player log for details.");
            throw new ArgumentNullException(nameof(XRGeneralSettings.Instance.Manager.activeLoader));
        }
        else
        {
            Debug.Log("Starting XR...");
            XRGeneralSettings.Instance.Manager.StartSubsystems();
            // ▼追加：VRサブシステム起動直後に、Unityのオーディオエンジンをリセット（再認識）させる
            AudioSettings.Reset(AudioSettings.GetConfiguration());
            Debug.Log("Audio Engine Reset for VR.");
            gm.game.errorText = "VR started.";
        }
    }

    public void StopXR()
    {
        gm.game.error = true;
        gm.game.errorText = "Stopping XR...";
        Debug.Log("Stopping XR...");

        XRGeneralSettings.Instance.Manager.StopSubsystems();
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        Debug.Log("XR stopped completely.");
        gm.game.error = true;
        gm.game.errorText = "VR stopped.";
    }
}