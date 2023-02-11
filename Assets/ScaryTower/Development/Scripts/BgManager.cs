using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

[Serializable]
public class TexSet
{
    public Texture[] tex;
}

[RequireComponent(typeof(Renderer))]
public class BgManager : MonoBehaviour {

    [SerializeField] private float offset = 0f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private TexSet[] texSets;
    [SerializeField] private BgManager MasterValSource;
    
    private Texture[] tex;
    private Texture tex1 { get => tex[0]; set { tex[0] = value; } }
    private Texture tex2 { get => tex[1]; set { tex[1] = value; } }
    private Texture tex3 { get => tex[2]; set { tex[2] = value; } }

    //private Texture tex1;
    //private Texture tex2;
    //private Texture tex3;

	//Renderer rend;
	Image img = null;
	Material mat = null;
    //MaterialPropertyBlock mpb;
    private float currVal = 0;
    private bool swappedPrev = false;
    private bool swappedNext = false;
    
    private float MasterVal => MasterValSource.currVal;

    void Start()
    {
        //rend = GetComponent<Renderer>();
        // make a material instance so two different backgrounds can be independent, but in sync
        img = GetComponent<Image>();
        mat = Instantiate(img.materialForRendering);
        img.material = mat;
        
        //mpb = new MaterialPropertyBlock();
        currVal = 0;// offset;
        swappedPrev = false;
        swappedNext = false;
        
        tex = new Texture[3] {
            mat.GetTexture("_PrevTex"),
            mat.GetTexture("_CurrTex"),
            mat.GetTexture("_NextTex")
        };
    }

    float GetCurrVal()
    {
        // value that comes from another bg manager
        if (MasterValSource)
        {
            return MasterVal + offset;
        }
        
        // value starts at zero and goes up
        return currVal + Time.deltaTime * speed;
    }

    void Update () {
        float ratioX = (float)Screen.width / (float)tex[0].width;
        float ratioY =  (float)Screen.height / (float)tex[0].height;
        // use whichever multiplier is smaller
        float ratio = ratioX < ratioY ? ratioX : ratioY;

        // now we can get the new height and width
        //float newHeight = tex[0].height * ratio;
        //float newWidth = tex[0].width * ratio;
        var texScale = mat.GetTextureScale("_MainTex");
        float invertedRatio = texScale.x / ratio;
        texScale.y = invertedRatio;
        mat.SetTextureScale("_MainTex", texScale);

        //mpb.SetFloat("_CurrVal", currVal);
        //rend.SetPropertyBlock(mpb);
        mat.SetFloat("_CurrVal", currVal);

        currVal = GetCurrVal();
        
        if(currVal >= 1f) // loop it to -1 when it reaches the end range 1
        {
            // Example if it's 1.1, it spits out -0.9
            currVal = -1f + (currVal - 1f);
            Debug.Log("[BgManager]: Loop CurrVal");
        }
        else if (currVal < -1f)
        {
            // Example if it's -1.1, it spits out 0.9
            currVal = 2f + currVal;
            Debug.Log("[BgManager]: Loop negative CurrVal");
        }
        else if (currVal < 0 && currVal > -ratio && !swappedNext) // swap next before it gets to display it
        {
            // swap tex (prev/curr/next)
            if (tex1 == null) return;

            mat.SetTexture("_NextTex", tex1);

            Debug.Log("[BgManager]: Swap Next");
            swappedNext = true;
            swappedPrev = false;
        }
        else if (currVal > ratio && !swappedPrev) // swap previous while it displaying next
        {
            // swap tex (prev/curr/next)
            tex3 = mat.GetTexture("_NextTex");
            //if (tex3 == null) return;
            tex1 = mat.GetTexture("_PrevTex");
            //if(tex3.GetHashCode() != tex1.GetHashCode())
            mat.SetTexture("_PrevTex", tex3);

            Debug.Log("[BgManager]: Swap Prev");
            swappedPrev = true;
            swappedNext = false;
        }
	}
}
