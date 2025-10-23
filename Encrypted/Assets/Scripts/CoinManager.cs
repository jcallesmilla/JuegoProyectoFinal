using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CoinManager : MonoBehaviour
{
    public int coinCount;
    public TextMeshProUGUI coinText; 

    void Start()
    {

    } 

    void Update()
    {
        coinText.text = "Coin count: " + coinCount.ToString();
    }
    
    
}
