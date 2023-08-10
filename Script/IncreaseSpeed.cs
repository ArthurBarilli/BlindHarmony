using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseSpeed : MonoBehaviour
{
    public bool start;
    public float speed;
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            if (start)
            {
                GameManager.Instance.curseMove = true;
            }
            else
            {
                GameManager.Instance.curseSpeed += speed;
            }
        }
    }
}
