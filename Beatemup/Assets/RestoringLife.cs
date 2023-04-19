using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoringLife : MonoBehaviour {
    PlayerMovementScript player;

    private void Start() {
        player = FindObjectOfType<PlayerMovementScript>();
    }
    private void OnTriggerStay(Collider other) {
        if (player.currentHealth < player.maxHealth)
            player.currentHealth++;
            }
}
