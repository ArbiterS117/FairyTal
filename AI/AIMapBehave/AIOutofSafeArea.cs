using UnityEngine;

public class AIOutofSafeArea : MonoBehaviour
{

    public enum MapSide
    {
        Left,
        Right
    }
    public MapSide mapSide;

    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.GetComponentInParent<AIPlayerBase>() != null && !other.CompareTag("Attack"))
        {
            AIPlayerBase aiBase = other.transform.GetComponentInParent<AIPlayerBase>();
            aiBase.aiState = AIPlayerBase.AIstate.back;
            
            if(aiBase.playerCtrl.isKnockOuted) aiBase.BacktoMapMidCDuration = -0.5f;
            else aiBase.BacktoMapMidCDuration = 0.0f;
            if (mapSide == MapSide.Left) aiBase.IsOutMapLeft = true;
            if (mapSide == MapSide.Right) aiBase.IsOutMapLeft = false;
        }

    }
}