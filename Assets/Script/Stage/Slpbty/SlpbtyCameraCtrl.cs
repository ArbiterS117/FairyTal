using System.Collections;
using UnityEngine;

public class SlpbtyCameraCtrl : TRyCamera
{
    public Transform DragonATKPos;
    public float DragonATKOrthographicSize = 10.8f;

    public bool isAttatk = false;

    public override void FixedUpdate()
    {
        if (!isAttatk) base.FixedUpdate();
        else
        {
            playerCameraTarget(); // 繼續計算目標有無被抓取

            float posX = Mathf.SmoothDamp(transform.position.x, DragonATKPos.position.x, ref velocity.x, smoothTimeX);
            float posY = Mathf.SmoothDamp(transform.position.y, DragonATKPos.position.y, ref velocity.y, smoothTimeY);

            cameraObj.orthographicSize = Mathf.Lerp(cameraObj.orthographicSize, DragonATKOrthographicSize, Time.deltaTime * zoomOutSpeed);
            if (!isOrthographic) transform.position = new Vector3(posX, posY, cameraObj.orthographicSize * -1.7f + 1.0f);
            


        }
    }

    void playerCameraTarget()
    {
        //玩家物件
        for (int i = 0; i < 4; i++)
        {
            if (gameCtrl.isDead[i] && !addPlayerSwitch[i])
            {
                targets.Remove(TempPlayerTransform[i]);
                addPlayerSwitch[i] = true;
            }

            if (!gameCtrl.isDead[i] && addPlayerSwitch[i])
            {
                if (gameCtrl.player[i] != null)
                {
                    targets.Add(gameCtrl.player[i].transform);
                    TempPlayerTransform[i] = gameCtrl.player[i].transform;
                    addPlayerSwitch[i] = false;
                }
            }

            //死亡特效物件
            if (PlayerDeathTransform[i] == null && !addPlayerDeathSwitch[i])
            {
                targets.Remove(TempPlayerDeathTransform[i]);
                addPlayerDeathSwitch[i] = true;
            }

            if (PlayerDeathTransform[i] != null && addPlayerDeathSwitch[i])
            {
                TempPlayerDeathTransform[i] = PlayerDeathTransform[i];
                targets.Add(TempPlayerDeathTransform[i]);
                addPlayerDeathSwitch[i] = false;
            }

        }
    }

}
