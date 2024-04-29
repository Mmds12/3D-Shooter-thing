using TMPro;
using UnityEngine;

public class moneyManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI moneyAddedSpawn;
    public float money = 0f;

    public GameObject moneyAdded;
    public GameObject moneyExtraxted;

    void Update()
    {
        moneyText.text = "$ " + money.ToString();
    }

    public void addMoney(float amount)
    {
        amount = Random.Range(amount - 2, amount + 4);
        moneyAdded.GetComponent<TextMeshProUGUI>().text = "+" + ((int)amount).ToString();

        money += (int)amount;

        var t = Instantiate(moneyAdded, moneyAddedSpawn.rectTransform);
        Destroy(t, 3f);
    }

    public void extractMoney(float amount)
    {
        moneyExtraxted.GetComponent<TextMeshProUGUI>().text = "-" + ((int)amount).ToString();
        money -= amount;

        var t = Instantiate(moneyExtraxted, moneyAddedSpawn.rectTransform);
        Destroy(t, 3f);
    }
}
