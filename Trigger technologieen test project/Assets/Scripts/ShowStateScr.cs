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
    public Text stability;
    private TrackableBehaviour vuforiaTrackable;
    private TrackingState arFoundationTrackable;

    public GameObject vuforiaGameObject;
    public GameObject arFoundationGameObject;

    // Start is called before the first frame update
    void Start()
    {
        arPlatform = autoDetectARPlatform();
        changeARPlatform(arPlatform);

        if (arPlatform == ARplatform.Vuforia)
        {
            vuforiaTrackable = FindObjectOfType<TrackableBehaviour>().GetComponent<TrackableBehaviour>();  
        }

        SetPlatformToText(arPlatform);
    }

    // Update is called once per frame
    void Update()
    {   
        if (arPlatform == ARplatform.ARFoundation)
        {
            SetARFoundationStateToText(arFoundationTrackable);

            if (arFoundationTrackable == TrackingState.Tracking)
            {
                StartCoroutine("CheckStability");
            }
        }
        else
        {
            SetVuforiaStateToText(vuforiaTrackable.CurrentStatus);

            if (vuforiaTrackable.CurrentStatus == TrackableBehaviour.Status.TRACKED)
            {
                StartCoroutine("CheckStability");
            }
            else
            {
                SetStabilityToText("Stable tracking not achieved");
                stability.color = Color.red;
            }
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
    private void SetStabilityToText(string stabilityStatus)
    {
        stability.text = "Stability: " + stabilityStatus;
        stability.color = Color.green;
    }

    private ARplatform autoDetectARPlatform()
    {
        return ARplatform.Vuforia;
        //return ARplatform.ARFoundation;
    }

    public void changeARPlatform(ARplatform newARPlatform)
    {
        arPlatform = newARPlatform;

        if (newARPlatform == ARplatform.Vuforia)
        {
            disableARFaundation();
            enableVuforia();
        }
        else if (newARPlatform == ARplatform.ARFoundation)
        {
            disableVuforia();
            enableARFoundation();
        }
    }

    private void enableVuforia()
    {
        VuforiaRuntime.Instance.InitVuforia();
        vuforiaGameObject.SetActive(true);
    }

    private void disableVuforia()
    {
        vuforiaGameObject.SetActive(false);
        VuforiaRuntime.Instance.Deinit();
    }

    private void enableARFoundation()
    {
        arFoundationGameObject.SetActive(true);
    }

    private void disableARFaundation()
    {
        arFoundationGameObject.SetActive(false);
    }

    private IEnumerator CheckStability()
    {
        yield return new WaitForSeconds(2f);
        SetStabilityToText("Stable tracking achieved");
    }

}
