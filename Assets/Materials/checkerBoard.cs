using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkerBoard : MonoBehaviour
{

    MeshRenderer meshRenderer;
    Material material;
    Texture2D texture;

    [SerializeField] float width = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.material;
        texture = new Texture2D(256,256, TextureFormat.RGBA32, true, true);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Bilinear;
        material.SetTexture("_MainTex", texture);    
        CreateCheckerBoard();
    }

    void CreateCheckerBoard() {
        for(int y = 0; y < texture.height; y++) {
            for (int x = 0; x < texture.width; x++)  {
                Color temp = EvaluateCheckerboardPixel(x,y);
                texture.SetPixel(x,y,temp);
            }
        }
        texture.Apply();
        
    }
    Color EvaluateCheckerboardPixel(int x, int y) {
        float valueX = x % (width*2.0f) / (width * 2.0f);
        int vX = 0;;
        if(valueX < 0.5f) {
            vX = 1;
        }
        float valueY= y % (width*2.0f) / (width * 2.0f);
        int vY = 0;
        if(valueY < 0.5f) {
            vY = 1;
        }
        float value = 1;
        if(vX == vY) {
            value = 0;
        }
        return new Color(value, value, value, 1.0f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
