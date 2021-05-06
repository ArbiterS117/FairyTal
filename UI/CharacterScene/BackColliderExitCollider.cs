using UnityEngine;

public class BackColliderExitCollider : MonoBehaviour {

    BackToMainMenuCtrl backToMainMenuCtrl;

	void Start () {
        backToMainMenuCtrl = GetComponentInParent<BackToMainMenuCtrl>();

    }

    private void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //if (other.GetComponentInParent<PointerCtrl>().isPressed) backToMainMenuCtrl.SetIsBacking(false);

        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponentInParent<PointerCtrl>().isPressed && !other.GetComponentInParent<PointerCtrl>().isPressedInbtn)
            {
                backToMainMenuCtrl.SetIsBacking(false);
            }
        }
    }

}
