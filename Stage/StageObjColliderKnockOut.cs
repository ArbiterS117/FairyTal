using UnityEngine;
using System.Collections;

public class StageObjColliderKnockOut : MonoBehaviour {

    public bool takeDataFromEnemyBase = false; // 

	public GameObject effectObject;
	
	AudioSource audioCtrl;
	
	public AudioClip hittedSE;
	public float hittedSEPitch;

    NPCEnemyBase owner = null;
    //外部變數

    public bool  randomDir      = true;
	public bool  sideType       = false;
	public int   Damage         = 3;
	public float knockOutTime   = 1.0f;
	public float knockOutSpeedX = 7.0f;
	public float hitForceY      = 300.0f;
    public float knockOutGravity = 2.5f;
    public float knockOutDecressSpeed = 0.0f;

	
	//內部變數
	
	public float dir = 1;

    bool[] isHittedPlayer = new bool[4];
    float[] hitColdCTime = new float[4];

	void Awake(){
		audioCtrl = transform.GetComponent<AudioSource>();
	}
	
	void Start(){
        if (takeDataFromEnemyBase) owner = GetComponentInParent<NPCEnemyBase>();
        else owner = null;
    }

    private void Update()
    {
        if (!takeDataFromEnemyBase) // 防止多次觸發傷害器
        {
            for(int i = 0; i <= 3; i++)
            {
                if (isHittedPlayer[i])
                {
                    hitColdCTime[i] += Time.deltaTime;
                    if(hitColdCTime[i] >= 0.5f)
                    {
                        isHittedPlayer[i] = false;
                        hitColdCTime[i] = 0.0f;
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {

        if (other.tag == "PlayerDMG") {
            XXXCtrl enemyCtrl = other.GetComponentInParent<XXXCtrl>();
            if (GetComponentInParent<DirectionEffectCtrl>().isFront == enemyCtrl.isFront)
            {
                float ran = Random.Range(0.0f, 2.0f);
                if (randomDir)
                {
                    if (ran <= 1.0f) dir = -1;
                    else dir = 1;
                }
                else
                {
                    if (transform.parent.transform.localScale.x >= 0) dir = 1;
                    else dir = -1;
                }
                if (!takeDataFromEnemyBase)
                {
                    if (!isHittedPlayer[enemyCtrl.PlayerNUM - 1])
                    {
                        enemyCtrl.actionKnockOuted(sideType, Damage, knockOutTime, dir, knockOutSpeedX, hitForceY, 0, knockOutGravity, knockOutDecressSpeed);
                        GameObject effect = Instantiate(effectObject, new Vector3(other.transform.position.x + Random.Range(-1.0f, 1.0f), other.transform.position.y + Random.Range(-1.0f, 1.0f), other.transform.position.z), Quaternion.identity) as GameObject;
                        effect.GetComponent<DirectionEffectCtrl>().owner = this.transform.parent.transform;

                        audioCtrl.pitch = hittedSEPitch + Random.Range(-0.05f, 0.05f);
                        audioCtrl.PlayOneShot(hittedSE);
                        isHittedPlayer[enemyCtrl.PlayerNUM - 1] = true;
                    }
                }
                else
                {
                    if (!owner.hittedPlayer[enemyCtrl.PlayerNUM - 1])
                    {
                        enemyCtrl.actionKnockOuted(owner.getATKData().sideType, owner.getATKData().ATK, owner.getATKData().knockOutTime, dir, owner.getATKData().knockBackSpeedX, owner.getATKData().hitForceY, 0, owner.getATKData().knockOutGravity, owner.getATKData().knockOutDecressSpeed);
                        GameObject effect = Instantiate(owner.getATKData().effectObject, new Vector3(other.transform.position.x + Random.Range(-1.0f, 1.0f), other.transform.position.y + Random.Range(-1.0f, 1.0f), other.transform.position.z), Quaternion.identity) as GameObject;
                        effect.GetComponent<DirectionEffectCtrl>().owner = this.transform.parent.transform;

                        audioCtrl.pitch = owner.getATKData().hittedSEPitch + Random.Range(-0.05f, 0.05f); 
                        audioCtrl.PlayOneShot(owner.getATKData().hittedSE);
                        owner.hittedPlayer[enemyCtrl.PlayerNUM - 1] = true;
                    }
                }
            }
		}

	}
}