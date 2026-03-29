using UnityEngine;

public class LargeCoinPickup : MonoBehaviour, IPickup
{
    [SerializeField] private int value = 10;

    public void OnPickup(CoinCollector collector)
    {
        collector.AddCoins(value);
    }
}
