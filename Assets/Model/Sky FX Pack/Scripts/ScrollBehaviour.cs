using UnityEngine;
using System.Collections;

public class ScrollBehaviour : MonoBehaviour
{
    public int materialIndex = 0;
    public Vector2 uvAnimationRate = new Vector2(1.0f, 0.0f);
    public string textureName = "_MainTex";

    Vector2 uvOffset = Vector2.zero;
    MeshRenderer mrenderer;

    private void Start()
    {
        mrenderer = GetComponent<MeshRenderer>();
    }
    void LateUpdate()
    {
        uvOffset += (uvAnimationRate * Time.deltaTime);
        if (mrenderer.enabled)
        {
            mrenderer.materials[materialIndex].SetTextureOffset(textureName, uvOffset);
        }
    }
}
