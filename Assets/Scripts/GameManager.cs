using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject congratulationsPanel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private AudioSource backgroundMusic;

    private int rewardsCollected = 0;

    private void Start()
    {
        Time.timeScale = 1f;

        gameOverPanel.SetActive(false);
        congratulationsPanel.SetActive(false);

        if (backgroundMusic != null)
        {
            backgroundMusic.Play();
        }

        UpdateScoreText();
    }

    public void AddReward()
    {
        rewardsCollected++;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Rewards: " + rewardsCollected;
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);

        if (backgroundMusic != null)
        {
            backgroundMusic.Stop();
        }

        Time.timeScale = 0f;
    }

    public void Congratulations()
    {
        congratulationsPanel.SetActive(true);

        if (backgroundMusic != null)
        {
            backgroundMusic.Stop();
        }

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}