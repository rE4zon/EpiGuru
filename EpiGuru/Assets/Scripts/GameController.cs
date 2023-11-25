using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private TextMeshProUGUI coinCountTextGameOver;
    [SerializeField] private Button restartButton;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button menuButton; 

    public Vector3 coinCountTextPosition = Vector3.zero; 

    private bool isPaused = false;

    void Start()
    {
        if (gameOverMenu != null)
        {
            gameOverMenu.SetActive(false);
        }

        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }

        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(TogglePause);
        }

        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(TogglePause);
        }

        if (menuButton != null)
        {
            menuButton.onClick.AddListener(LoadMainMenu);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void GameOver(int collectedCoins)
    {
        Time.timeScale = 0f; 

        if (gameOverMenu != null)
        {
            if (coinCountTextGameOver != null)
            {
                coinCountTextGameOver.text = "Coins Collected: " + collectedCoins;
                coinCountTextGameOver.rectTransform.localPosition = coinCountTextPosition;
            }

            gameOverMenu.SetActive(true); 
            menuButton.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(false);

        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; 
            if (pauseMenu != null)
            {
                pauseMenu.SetActive(true); 
            }
        }
        else
        {
            Time.timeScale = 1f; 
            if (pauseMenu != null)
            {
                pauseMenu.SetActive(false); 
            }
        }
    }

    void LoadMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("Menu"); 
    }
}
