using UnityEngine;

public class TimePickup : MonoBehaviour, IPickup
{
    [SerializeField] private float timeToAdd = 5f;

    public void OnPickup(CoinCollector collector)
    {
        collector.AddTime(timeToAdd);
    }
}
