using UnityEngine;

public class BorderCollider : MonoBehaviour {

    TRyCamera CameraObj;
    GameCtrl gameCtrl;

    public GameObject effectObjectDeath = null;

    Vector3 playerDeathPos;

    public enum WallDirection
    {
        Down,
        Up,
        Left,
        Right,
        DL,
        DR,
        UL,
        UR
    }

    public WallDirection direction;

    float rx;
    float ry;
    float rz;

    private void Awake()
    {
        gameCtrl = GameObject.FindGameObjectWithTag("GameCtrl").GetComponent<GameCtrl>();
        CameraObj = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TRyCamera>();
    }

  
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
            playerDeathPos = other.transform.position;

            //產生死亡特效
            if (effectObjectDeath != null)
            {

                XXXCtrl otherCtrl = other.GetComponentInParent<XXXCtrl>();
                gameCtrl.PadVibration(otherCtrl.PlayerNUM - 1, 0.8f, 1.0f);
                otherCtrl.dead();

                GameObject effect = Instantiate(effectObjectDeath, playerDeathPos, Quaternion.identity) as GameObject;
                //if(Down)effect.transform.localRotation = new Quaternion(-0.9f, 0.0f, 0.0f, effect.transform.localRotation.w);
                if (direction == WallDirection.Down)
                {
                    rx = -0.9f;
                    ry = 0.0f;
                    rz = 0.0f;
                }
                if (direction == WallDirection.Up)
                {
                    rx = 0.9f;
                    ry = 0.0f;
                    rz = 0.0f;
                }
                if (direction == WallDirection.Left)
                {
                    rx = 0.0f;
                    ry = 0.9f;
                    rz = 0.0f;
                }
                if (direction == WallDirection.Right)
                {
                    rx = 0.0f;
                    ry = -0.9f;
                    rz = 0.0f;
                }
                if (direction == WallDirection.DL)
                {
                    rx = -0.9f;
                    ry = 0.9f;
                    rz = 0.0f;
                }
                if (direction == WallDirection.DR)
                {
                    rx = -0.9f;
                    ry = -0.9f;
                    rz = 0.0f;
                }
                if (direction == WallDirection.UL)
                {
                    rx = 0.9f;
                    ry = 0.9f;
                    rz = 0.0f;
                }
                if (direction == WallDirection.UR)
                {
                    rx = 0.9f;
                    ry = -0.9f;
                    rz = 0.0f;
                }
                effect.transform.localRotation = new Quaternion(rx, ry, rz, effect.transform.localRotation.w);
                CameraObj.PlayerDeathTransform[other.transform.GetComponentInParent<XXXCtrl>().PlayerNUM - 1] = effect.transform;
                CameraObj.ShakeCamera(0.2f, 0.3f);
            }

            

		} 

		else if (other.tag == "Item") {
			if(other.transform.parent != null)Destroy(other.transform.parent.gameObject);
			else Destroy(other.transform.gameObject);
		}
	}

}
