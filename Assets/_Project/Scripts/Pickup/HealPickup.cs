using UnityEngine;

public class HealPickup : MonoBehaviour, IPickup
{
    [SerializeField] private int healAmount = 10;

    public void OnPickup(CoinCollector collector)
    {
        collector.AddHP(healAmount);
    }
}
