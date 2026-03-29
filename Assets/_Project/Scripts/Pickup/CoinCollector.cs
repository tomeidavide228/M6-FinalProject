using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CoinCollector : MonoBehaviour
{
    [Header("Coin Number Settings")]
    [SerializeField] private int _coinNumber = 0;
    [SerializeField] private int _coinsToOpenDoor = 150;

    [Header("Events Settings")]
    [SerializeField] private UnityEvent<int> _onCoinChanged;
    [SerializeField] private UnityEvent _onDoorOpen;

    [Header("Sound Settings")]
    [SerializeField] private AudioManager _sound;

    private bool _doorOpened = false;
    private LifeController _life;
    private Timer _timer;

    private void Start()
    {
        _sound = FindObjectOfType<AudioManager>();
        _timer = FindObjectOfType<Timer>();
        _life = GetComponent<LifeController>();
    }

    private void OnTriggerEnter(Collider coin)
    {
        IPickup pickup = coin.GetComponent<IPickup>();
        if (pickup != null)
        {
            pickup.OnPickup(this);
            _sound.Coin();
            Destroy(coin.gameObject);
        }
    }

    public void AddCoins(int points)
    {
        _coinNumber += points;
        _onCoinChanged.Invoke(_coinNumber);

        if (_coinNumber >= _coinsToOpenDoor && !_doorOpened)
        {
            _doorOpened = true;
            _onDoorOpen.Invoke();
        }
    }

    public void AddTime(float time)
    {
        _timer.AddTime(time);
    }

    public void AddHP(int hp)
    {
        _life.AddHP(hp);
    }
}
