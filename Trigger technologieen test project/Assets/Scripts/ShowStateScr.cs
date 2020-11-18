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
    public Text timer;
    private TrackableBehaviour vuforiaTrackable;
    private TrackingState arFoundationTrackable;

    public GameObject vuforiaGameObject;
    public GameObject arFoundationGameObject;

    public GameObject imageTarget;
    public GameObject objectTarget;

    public GameObject arFoundationUiElements;
    public GameObject vuforiaUiElements;

    // Start is called before the first frame update
    void Start()
    {
        arPlatform = autoDetectARPlatform();
        ChangeARPlatform(arPlatform);
    }

    // Update is called once per frame
    void Update()
    {
        SetPlatformToText(arPlatform);

        if (arPlatform == ARplatform.Vuforia)
        {
            SetVuforiaStateToText(vuforiaTrackable.CurrentStatus);//imageTarget.GetComponent<ImageTargetBehaviour>().CurrentStatus);

            if (vuforiaTrackable.CurrentStatus == TrackableBehaviour.Status.TRACKED || vuforiaTrackable.CurrentStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                StartCoroutine("CheckStability");
            }
            else if (vuforiaTrackable.CurrentStatus == TrackableBehaviour.Status.NO_POSE || vuforiaTrackable.CurrentStatus == TrackableBehaviour.Status.LIMITED)
            {
                StopCoroutine("CheckStability");
                SetStabilityToText("Stable tracking not achieved");
                stability.color = Color.red;
            }
        }
    }

    //private void SetARFoundationStateToText(TrackingState arFoundaitonState)
    //{
    //    arState.text = "Tracking State: " + arFoundaitonState;
    //}

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

    private void SetTimerValueToText(int timerValue)
    {
        timer.text = "Timer: " + timerValue + " seconds";
    }

    private ARplatform autoDetectARPlatform()
    {
        return ARplatform.Vuforia;
        //return ARplatform.ARFoundation;
    }

    public void ChangeARPlatform(ARplatform newARPlatform)
    {
        arPlatform = newARPlatform;

        if (newARPlatform == ARplatform.Vuforia)
        {
            DisableARFaundation();
            EnableVuforia();
        }
        else if (newARPlatform == ARplatform.ARFoundation)
        {
            DisableVuforia();
            EnableARFoundation();
        }
    }

    private void EnableVuforia()
    {
        VuforiaRuntime.Instance.InitVuforia();
        vuforiaGameObject.SetActive(true);
        vuforiaTrackable = FindObjectOfType<TrackableBehaviour>().GetComponent<TrackableBehaviour>();
        vuforiaUiElements.SetActive(true);
    }

    private void DisableVuforia()
    {
        vuforiaGameObject.SetActive(false);
        VuforiaRuntime.Instance.Deinit();
        vuforiaUiElements.SetActive(false);
    }

    private void EnableARFoundation()
    {
        arFoundationGameObject.SetActive(true);
        arFoundationUiElements.SetActive(true);
    }

    private void DisableARFaundation()
    {
        arFoundationGameObject.SetActive(false);
        arFoundationUiElements.SetActive(false);
    }

    private IEnumerator CheckStability()
    {
        yield return new WaitForSeconds(2f);
        SetStabilityToText("Stable tracking achieved");
    }

    private IEnumerator Timer(int timerDuration)
    {
        int timerValue = 0;
        bool timerActive = true;
        while (timerActive)
        {
            yield return new WaitForSeconds(1f);
            timerValue++;
            SetTimerValueToText(timerValue);
            if (timerValue == timerDuration)
            {
                timer.color = Color.green;
                timerActive = false;
            }
        }

        yield return new WaitForSeconds(5f);
        timerValue = 0;
        SetTimerValueToText(timerValue);
        timer.color = Color.red;
    }

    public void ChangePlatformOnButtonPressed()
    {
        if (arPlatform == ARplatform.Vuforia)
        {
            ChangeARPlatform(ARplatform.ARFoundation);
        }
        else if (arPlatform == ARplatform.ARFoundation)
        {
            ChangeARPlatform(ARplatform.Vuforia);
        }
    }

    public void ChangeTargetTypeOnButtonPressed()
    {
        if (imageTarget.activeSelf)
        {
            imageTarget.SetActive(false);
            objectTarget.SetActive(true);
            vuforiaTrackable = objectTarget.GetComponent<TrackableBehaviour>();
        }
        else if (objectTarget.activeSelf)
        {
            objectTarget.SetActive(false);
            imageTarget.SetActive(true);
            vuforiaTrackable = imageTarget.GetComponent<TrackableBehaviour>();
        }
    }

    public void StartTimer()
    {
        StartCoroutine("Timer", 10);
    }
}
