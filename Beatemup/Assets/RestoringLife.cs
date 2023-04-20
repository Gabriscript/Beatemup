using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoringLife : MonoBehaviour {
    PlayerMovementScript player;
    public GameObject Healingsymbol;
   

    private void Awake() {
        player = FindObjectOfType<PlayerMovementScript>();
        Healingsymbol.SetActive(false);
    }
    private void OnTriggerStay(Collider other) {
        if (player.currentHealth < player.maxHealth)
            player.currentHealth++;
        Healingsymbol.SetActive(true);
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player"))
            Healingsymbol.SetActive(false);
    }





}
