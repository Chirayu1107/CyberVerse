using UnityEngine;
using UnityEngine.Video;

public class IntroController : MonoBehaviour
{
    [Header("Intro Settings")]
    [SerializeField] private VideoPlayer videoPlayer;

    [Header("Scene Settings")]
    [SerializeField] private string mainMenuScene = "MainMenu";

    private bool introFinished = false;

    private void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
        }
    }

    private void Update()
    {
        // Press SPACE to skip the intro
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SkipIntro();
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        GoToMainMenu();
    }

    private void SkipIntro()
    {
        if (introFinished)
            return;

        if (videoPlayer != null)
        {
            videoPlayer.Stop();
        }

        GoToMainMenu();
    }

    private void GoToMainMenu()
    {
        if (introFinished)
            return;

        introFinished = true;

        if (SceneTransition.Instance != null)
        {
            SceneTransition.Instance.LoadScene(mainMenuScene);
        }
        else
        {
            Debug.LogError("SceneTransition instance not found!");
        }
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }
}