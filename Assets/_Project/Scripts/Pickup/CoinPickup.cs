using UnityEngine;

public class CoinPickup : MonoBehaviour, IPickup
{
    [SerializeField] private int value = 1;

    public void OnPickup(CoinCollector collector)
    {
        collector.AddCoins(value);
    }
}
