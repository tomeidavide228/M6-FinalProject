using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WinZone : MonoBehaviour
{
    [Header("Win Event Settings")]
    [SerializeField] private UnityEvent _onEnterWinZone;

    private void OnTriggerEnter(Collider collided)
    {
        if (collided.CompareTag(Tags.Player))
        {
            _onEnterWinZone.Invoke();
        }
    }
}
