using UnityEngine;
using UnityEngine.UI;

public class TowerBuyButtonBehaviour : MonoBehaviour
{
    public int towerIndex = 0;
    public TMPro.TMP_Text priceText;
    public Image image;

    public void UpdateButton()
    {
        priceText.SetText($"${TowerController.instance.AllTowers[towerIndex].Cost}");
        image.sprite = TowerController.instance.AllTowers[towerIndex].sprite;
    }

    public void StartPlacement()
    {
        TowerController.instance.StartPlacement(this);
    }
}
