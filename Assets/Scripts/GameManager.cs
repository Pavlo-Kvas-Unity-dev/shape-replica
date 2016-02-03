﻿using UnityEngine;
using UnityEngine.UI;

namespace ShapeReplica
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private float RoundLength = 10f;
        private float curRoundLenth;
        private float curRoundTimeLeft;

        private int score;

        private bool isPlaying;

        private RecognitionBoard recognitionBoard;

        //UI
        [SerializeField] private Slider RoundTimeIndicator;
        [SerializeField] private Button restartButton;
        [SerializeField] private Text gameOverText;
        [SerializeField] private Image gameOverPanel;

        private Image indicatorImage;

        void Awake()
        {
            ToggleGameOverPanel(false);
            recognitionBoard = GetComponent<RecognitionBoard>();
            RecognitionBoard.GestureRecognized += OnGestureRecognized;
            indicatorImage = RoundTimeIndicator.GetComponentInChildren<Image>();
        }

        void Start ()
        {
            RestartGame();
        }

        void Update ()
        {
            if (!isPlaying) return;

            UpdateTimeIndicator();
        }

        private void UpdateTimeIndicator()
        {
            curRoundTimeLeft -= Time.deltaTime;
            RoundTimeIndicator.value = curRoundTimeLeft;

            float timeLeftNorm = curRoundTimeLeft / curRoundLenth;
            if (timeLeftNorm > 2 / 3f) indicatorImage.color = Color.blue;
            else if (timeLeftNorm > 1 / 3f) indicatorImage.color = Color.yellow;
            else if (timeLeftNorm > 0) indicatorImage.color = Color.red;
            else GameOver();
        }

        private void GameOver()
        {
            gameOverText.text = string.Format("Time elapsed, you scored {0} points", score);
            isPlaying = false;
            ToggleGameOverPanel(true);
        }

        private void ToggleGameOverPanel(bool isVisible)
        {
            gameOverPanel.gameObject.SetActive(isVisible);
        }

        void OnGestureRecognized()
        {
            score++;
            ResetRound();
            recognitionBoard.NextGesture();
        }

        private void ResetRound()
        {
            RoundTimeIndicator.maxValue = curRoundLenth;
            curRoundTimeLeft = curRoundLenth;

            curRoundLenth *= .85f;
        }

        public void RestartGame()
        {
            ToggleGameOverPanel(false);
            isPlaying = true;
            score = 0;
            curRoundLenth = RoundLength;
            ResetRound();
            recognitionBoard.NextGesture();
        }
    }
}
