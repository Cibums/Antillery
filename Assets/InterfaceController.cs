using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfaceController : MonoBehaviour
{
    public static InterfaceController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public GameObject TowerBuyButtonPrefab;

    public Transform BuyPanel;
    public Transform ButtonContentPanel;
    public Transform GameOverPanel;
    public TMPro.TMP_Text moneyText;

    private void Start()
    {
        int index = 0;
        foreach (Tower tower in TowerController.instance.AllTowers)
        {
            var button = Instantiate(TowerBuyButtonPrefab, ButtonContentPanel);
            button.GetComponent<TowerBuyButtonBehaviour>().towerIndex = index;
            button.GetComponent<TowerBuyButtonBehaviour>().UpdateButton();

            index++;
        }
    }

    private void Update()
    {
        moneyText.SetText($"Wallet: ${PlayerStats.Money}");
    }

    public void SetBuyPanelState(bool state)
    {
        BuyPanel.gameObject.SetActive(state);
    }

    public void GameOver()
    {
        GameOverPanel.gameObject.SetActive(true);
        BuyPanel.gameObject.SetActive(false);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
