using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Vuforia;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ShowStateScr : MonoBehaviour
{
    public enum ARplatform { Vuforia, ARFoundation, Unknown };
    public ARplatform arPlatform = ARplatform.Vuforia;

    public Text arState;
    public Text platform;
    private TrackableBehaviour vuforiaTrackable;
    private TrackingState arFoundationTrackable;

    // Start is called before the first frame update
    void Start()
    {
        autoDetectARPlatform();

        if (arPlatform == ARplatform.Vuforia)
        {
            vuforiaTrackable = FindObjectOfType<TrackableBehaviour>().GetComponent<TrackableBehaviour>();  
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetPlatformToText(arPlatform);
        if (arPlatform == ARplatform.ARFoundation)
        {
            SetARFoundationStateToText(arFoundationTrackable);
        }
        else
        {
            SetVuforiaStateToText(vuforiaTrackable.CurrentStatus);
        }
    }

    private void SetARFoundationStateToText(TrackingState arFoundaitonState)
    {
        arState.text = "Tracking State: " + arFoundaitonState;
    }

    private void SetVuforiaStateToText(TrackableBehaviour.Status vuforiaState)
    {
        arState.text = "Tracking State: " + vuforiaState;
    }

    private void SetPlatformToText(ARplatform arPlatform)
    {
        platform.text = "Platform: " + arPlatform;
    }

    private ARplatform autoDetectARPlatform()
    {

        // For testing.
        //return ARplatform.Vuforia;

        if (ARSession.state == ARSessionState.Unsupported)
        {
            return ARplatform.Vuforia;
        }
        else
        {
            return ARplatform.ARFoundation;
        }
    }


}
