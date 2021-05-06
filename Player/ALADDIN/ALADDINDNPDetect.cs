using System.Collections;
using UnityEngine;

public class ALADDINDNPDetect : MonoBehaviour
{

    ALADDINSpecialAction playerCtrl;

    private void Awake()
    {
        playerCtrl = GetComponentInParent<ALADDINSpecialAction>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerDMG") || other.CompareTag("NPCReceiveDMG"))
        {
            playerCtrl.StopDash();
            playerCtrl.animator.SetTrigger("DNPPierce");
        }

    }
}
