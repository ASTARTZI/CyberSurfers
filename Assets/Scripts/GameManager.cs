using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Πίνακες που εμφανίζονται στο τέλος του παιχνιδιού
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject congratulationsPanel;

    // Κείμενο εμφάνισης των rewards
    [SerializeField] private TextMeshProUGUI scoreText;

    // Μουσική υπόκρουσης του παιχνιδιού
    [SerializeField] private AudioSource backgroundMusic;

    private int rewardsCollected = 0;

    private void Start()
    {
        // Επαναφορά του χρόνου σε κανονική ροή
        Time.timeScale = 1f;

        gameOverPanel.SetActive(false);
        congratulationsPanel.SetActive(false);

        // Εκκίνηση της μουσικής
        if (backgroundMusic != null)
        {
            backgroundMusic.Play();
        }

        UpdateScoreText();
    }

    public void AddReward()
    {
        // Αύξηση του μετρητή rewards
        rewardsCollected++;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        // Ενημέρωση του σκορ στην οθόνη
        scoreText.text = "Rewards: " + rewardsCollected;
    }

    public void GameOver()
    {
        // Εμφάνιση της οθόνης Game Over
        gameOverPanel.SetActive(true);

        if (backgroundMusic != null)
        {
            backgroundMusic.Stop();
        }

        // Πάγωμα του παιχνιδιού
        Time.timeScale = 0f;
    }

    public void Congratulations()
    {
        // Εμφάνιση της οθόνης επιτυχούς ολοκλήρωσης
        congratulationsPanel.SetActive(true);

        if (backgroundMusic != null)
        {
            backgroundMusic.Stop();
        }

        // Πάγωμα του παιχνιδιού
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        // Επανεκκίνηση της τρέχουσας σκηνής
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}