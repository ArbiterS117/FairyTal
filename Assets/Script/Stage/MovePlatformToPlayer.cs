using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MovePlatformToPlayer : MonoBehaviour {

    public List<Transform> targets = new List<Transform>();

    //float speedX = 0.0f;
    //float speedY = 0.0f;

    public bool isSlope = false;
    public float SlopeDownGrav = 2.0f; // 2.0能應付ZRotation = 15左右 

	//Vector2 prePos;

    XXXCtrl playerCtrl = null;
    //DirectionEffectCtrl itemCtrl = null;

    void Start () {
		//prePos = transform.position;
        if (this.transform.localRotation.z != 0) isSlope = true;

    }

    void FixedUpdate()
    {
        
        //更新現在速度
        //speedX = (transform.position.x - prePos.x) / Time.deltaTime;
        //speedY = (transform.position.y - prePos.y) / Time.deltaTime;
        //prePos = transform.position;
        
        //移動在上方的目標
        for(int i = targets.Count - 1; i >= 0; i--)
        //foreach (Transform target in targets)
        {

            if (targets[i] == null)
            {
                targets.Remove(targets[i]);
            }

            else
            {
                if (targets[i].transform.GetComponent<XXXCtrl>())
                {
                    playerCtrl = targets[i].transform.GetComponent<XXXCtrl>();
                    if (playerCtrl.isOnMovingPlatform == false)
                    {
                        targets[i].transform.GetComponent<Rigidbody2D>().gravityScale = playerCtrl.initGravity;
                        targets.Remove(targets[i]);
                        playerCtrl.transform.SetParent(null); //離開子物件
                        break;
                    }
                    
                    if (playerCtrl.grounded)
                    {
                        if (targets[i].transform.GetComponent<Rigidbody2D>().gravityScale != 0.0f) targets[i].transform.GetComponent<Rigidbody2D>().gravityScale = 0.0f; // 防止滑落
                        if (isSlope && playerCtrl.speedX != 0)
                        {
                            playerCtrl.toSetMPVelocityY = true;
                            playerCtrl.setMPVelocityY = 0.0f - SlopeDownGrav; // 2.0f 不能應付超斜的坡度 未來須按情況調整
                        }
                    }
                    
                }            

            }

            /*=================舊版==================================
            else
            {
                if (targets[i].transform.GetComponent<XXXCtrl>())
                {
                    playerCtrl = targets[i].transform.GetComponent<XXXCtrl>();
                    if (playerCtrl.isOnMovingPlatform == false)
                    {
                        targets[i].transform.GetComponent<Rigidbody2D>().gravityScale = playerCtrl.initGravity;
                        targets.Remove(targets[i]);
                        break;
                    }
                    if (playerCtrl.grounded)
                    {
                        playerCtrl.toSetVelocityX = true;
                        playerCtrl.setVelocityX = playerCtrl.speedX + speedX;
                        playerCtrl.toSetMPVelocityY = true;
                        playerCtrl.setMPVelocityY = speedY;
                        targets[i].transform.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
                        if(isSlope && playerCtrl.speedX != 0) playerCtrl.setMPVelocityY = speedY - SlopeDownGrav; // 2.0f 不能應付超斜的坡度 未來須按情況調整
                    }
                }

                if (targets[i].transform.GetComponent<DirectionEffectCtrl>())
                {
                    itemCtrl = targets[i].transform.GetComponent<DirectionEffectCtrl>();
                    itemCtrl.toSetVelocityX = true;
                    itemCtrl.setVelocityX = itemCtrl.speedX + speedX;
                    itemCtrl.toSetMPVelocityY = true;
                    itemCtrl.setMPVelocityY = speedY;
                }
            }
            ==========================================================================================*/
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BottomPenetrateDetect"))
        {

            bool bTargetOnMove = false;
            for(int i = targets.Count - 1; i >= 0; i--)
            {
                if (targets[i] == other.transform.parent.transform) bTargetOnMove = true;
            }

            if (!bTargetOnMove)
            {
                if (other.transform.parent.CompareTag("Player"))
                {
                    if (other.GetComponentInParent<Rigidbody2D>().velocity.y <= 0.0f)
                    {
                        other.GetComponentInParent<Rigidbody2D>().gravityScale = 0.0f;
                        targets.Add(other.transform.parent.parent.transform);
                        other.transform.parent.parent.transform.SetParent(this.transform); //加進子物件
                        other.GetComponentInParent<XXXCtrl>().isOnMovingPlatform = true;
                    }
                }

                if (other.transform.parent.CompareTag("Item"))
                {
                    if (other.GetComponentInParent<DirectionEffectCtrl>().hasRigidBody)
                    {
                        if (other.GetComponentInParent<Rigidbody2D>().velocity.y <= 0.0f)
                        {
                            targets.Add(other.transform.parent.parent.transform);
                            other.transform.parent.parent.transform.SetParent(this.transform); //加進子物件
                        }
                    }
                }

            }

        }

     }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("BottomPenetrateDetect"))
        {

            bool bTargetOnMove = false;
            for(int i = targets.Count - 1; i >= 0; i--)
            {
                if (targets[i] == other.transform.parent.parent.transform) bTargetOnMove = true;
            }
            if (bTargetOnMove)
            {
                if (other.transform.parent.CompareTag("Player"))
                {
                    other.GetComponentInParent<XXXCtrl>().isOnMovingPlatform = false;
                    other.GetComponentInParent<Rigidbody2D>().gravityScale = playerCtrl.initGravity;
                    other.transform.parent.parent.transform.SetParent(null); //離開子物件
                }

                if (other.transform.parent.CompareTag("Item"))
                {
                    other.transform.parent.parent.transform.SetParent(null); //離開子物件
                }

                targets.Remove(other.transform.parent.parent.transform);
                
            }
        }



    }

}
