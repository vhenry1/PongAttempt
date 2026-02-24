using UnityEngine;

public class StartGameUI : MonoBehaviour
{
    [SerializeField] private GameObject startButtonRoot;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        if (startButtonRoot == null)
            startButtonRoot = gameObject;
    }

    void Update()
    {
        if (gameManager != null && gameManager.GameStarted)
        {
            HideButton();
        }
    }

    public void OnStartButtonClicked()
    {
        if (gameManager == null) return;

        gameManager.StartGame();
    }

    private void HideButton()
    {
        if (startButtonRoot != null)
            startButtonRoot.SetActive(false);
    }
}