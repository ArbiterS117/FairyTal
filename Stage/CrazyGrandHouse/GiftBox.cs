using UnityEngine;
using System.Collections;

public class GiftBox : MonoBehaviour {

	[System.NonSerialized] public StageObjectCtrlCrazyGrandHouse stageCtrl;
	[System.NonSerialized] public GameCtrl gameCtrl;

	public Transform playerTransform;

	Vector2 pos;
	Vector2 prePos;


	public bool isCatched = false;
	public bool canCatch  = false;

	float canCatchColdTime  = 0.5f;
	float canCatchColdCTime = 0.0f;

    bool bNoteSprite = true;

	void Awake(){
		gameCtrl = GameObject.FindGameObjectWithTag("GameCtrl").GetComponent<GameCtrl>();
		stageCtrl = GameObject.FindGameObjectWithTag("StageCtrl").GetComponent<StageObjectCtrlCrazyGrandHouse>();

	}

	void Start () {
		
	}
	

	void Update () {

		if (isCatched) {

			if (playerTransform != null) {
				if(!playerTransform.GetComponent<XXXCtrl>().isCatchingGift)playerTransform.GetComponent<XXXCtrl> ().isCatchingGift = true;
				pos = new Vector2 (playerTransform.position.x, playerTransform.position.y + 1.5f);
				this.transform.position = new Vector2 (pos.x, pos.y);

			} 
			else {
				drop ();
			}

			if (playerTransform != null && playerTransform.GetComponent<XXXCtrl> ().isKnockOuted) {
				drop ();
			}

			prePos = pos;

            //關閉提示符號
            if (bNoteSprite)
            {
                bNoteSprite = false;
                transform.Find("NoteSprite").GetComponent<SpriteRenderer>().enabled = false;
                transform.Find("NoteSprite").GetComponent<ItemNoteSprite>().enabled = false;
                canCatchColdCTime = 0.0f;
            }


		}
		else {
			if(playerTransform != null){
				if(playerTransform.GetComponent<XXXCtrl>().isCatchingGift)playerTransform.GetComponent<XXXCtrl> ().isCatchingGift = false;

			}

			if (!canCatch) {
				canCatchColdCTime += Time.deltaTime;
				if (canCatchColdCTime >= canCatchColdTime) {
					canCatch = true;
					canCatchColdCTime = 0.0f;

                    if (!bNoteSprite)
                    {
                        bNoteSprite = true;
                        transform.Find("NoteSprite").GetComponent<SpriteRenderer>().enabled = true;
                        transform.Find("NoteSprite").GetComponent<ItemNoteSprite>().enabled = true;
                    }

				}
			}
		}

		//收到禮物
		if (stageCtrl.CrazyOldMan != null) {
			if(stageCtrl.CrazyOldMan.GetComponent<CrazyOldMan>().isGetGift){
				price();
				destroy();
			}
		}



	}

	//=======自訂=================================

	void drop(){
		if(playerTransform != null){
			if(playerTransform.GetComponent<XXXCtrl>().isCatchingGift)playerTransform.GetComponent<XXXCtrl> ().isCatchingGift = false;
		}
		canCatch = false;
		isCatched = false;
		playerTransform = null;
		this.transform.position = new Vector2 (prePos.x, prePos.y);
		GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
	}

	public void price(){
		if (playerTransform != null) {
			playerTransform.GetComponent<XXXCtrl>().HP = playerTransform.GetComponent<XXXCtrl>().MaxHP; 
			playerTransform.GetComponent<XXXCtrl>().MP = playerTransform.GetComponent<XXXCtrl>().MaxMP;
			playerTransform.GetComponent<XXXCtrl>().actionInvincible(10.0f);
			playerTransform.GetComponent<XXXCtrl>().isUntouchable = true;
		}
	}

	public void destroy(){
		if (playerTransform != null) {
			if (playerTransform.GetComponent<XXXCtrl> ().isCatchingGift)
				playerTransform.GetComponent<XXXCtrl> ().isCatchingGift = false;
		}
		stageCtrl.GiftIsLive = false;
		Destroy (this.gameObject);
	}


}
