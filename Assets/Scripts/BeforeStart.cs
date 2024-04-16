﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;  // For TextMeshProUGUI
using UnityEngine.SceneManagement;
using System.Collections;

public class BeforeStart : MonoBehaviour
{
    public TextMeshProUGUI hintText;
    private bool calibration = false;

    // UI
    public Button startButton;
    public Button InstructionButton;
    public Button endButton;

    /// <summary>
    ///  For ViveGazeDataRecorder
    /// </summary>
    private ViveGazeDataRecorder recorder;
    public GameObject gazePointPrefab; // 拖入一个预制体作为注视点的视觉表示
    public Canvas canvas;

    public Image fillImage;

    void Start()
    {
        RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();
        RectTransform rectTransform1 = startButton.GetComponent<RectTransform>();
        RectTransform rectTransform2 = InstructionButton.GetComponent<RectTransform>();
        RectTransform rectTransform3 = endButton.GetComponent<RectTransform>();

        recorder = new ViveGazeDataRecorder(gazePointPrefab,fillImage, canvasRectTransform, canvasRectTransform.sizeDelta.x, canvasRectTransform.sizeDelta.y, rectTransform1, rectTransform2, rectTransform3);
    }

    private void Update()
    {

        if (!calibration)
        {
            hintText.text = "Eye Gaze calibration\n Please gazing the moving cube";

            startButton.gameObject.SetActive(false);
            InstructionButton.gameObject.SetActive(false);
            endButton.gameObject.SetActive(false);

            if (recorder.EyeCalibration())
            {
                recorder.ResetTimer();
                calibration = true;      
            }

        }
        else
        {
            recorder.EyeTracking();          
            startButton.gameObject.SetActive(true);
            InstructionButton.gameObject.SetActive(true);
            endButton.gameObject.SetActive(true);
            hintText.text = "Please focus your eye gaze to the any button";

            if (recorder.CheckGazeWithoutTimer("1"))
            {
                if (recorder.CheckGaze("1"))
                    SceneManager.LoadScene("Experiment");

            }

            if (recorder.CheckGazeWithoutTimer("2"))
            {
                if (recorder.CheckGaze("2"))
                    SceneManager.LoadScene("Instruction");

            }
            
            if (recorder.CheckGazeWithoutTimer("3"))
            {
                if (recorder.CheckGaze("3"))
                    SceneManager.LoadScene("End");
            }
        }

    }

    private void OnDestroy()
    {
        recorder.StopRecording();
    }
}