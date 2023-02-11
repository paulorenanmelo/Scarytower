using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using static UnityEditor.MaterialProperty;

[Serializable]
public class TexSet
{
    public Texture[] tex;
}

[RequireComponent(typeof(Renderer))]
public class BgManager : MonoBehaviour {

    [SerializeField] private float offset = 0f;
    [SerializeField] private TexSet[] texSets;
    [SerializeField] private BgManager MasterValSource;
    [SerializeField] int texSet = 0;
    [SerializeField] int texID = 0;
    [SerializeField] int loopsBeforeChangingID = 3;

    private Texture tex1;
    private Texture tex2;
    private Texture tex3;

    private Image img = null;
	private Material mat = null;
    
    [Range(-1f, 1f)] private float currVal = 0;
    private float w = 0;
    private float h = 0;
    private bool swappedPrev = false;
    private bool swappedNext = false;
    private int loopCount = 0;

    private bool verbose => GameConfigManager.Instance.gameSettings.logVerbose;
    private float speed => GameConfigManager.Instance.gameSettings.startingSpeed;
    private Texture Tex1 { get => texSets[texSet].tex[texID + 0]; set { texSets[texSet].tex[texID + 0] = value; } }
    private Texture Tex2 { get => texSets[texSet].tex[texID + 1]; set { texSets[texSet].tex[texID + 1] = value; } }
    private Texture Tex3 { get => texSets[texSet].tex[texID + 2]; set { texSets[texSet].tex[texID + 2] = value; } }
    
    void Start()
    {
        // make a material instance so two different backgrounds can be independent, but in sync
        img = GetComponent<Image>();
        mat = Instantiate(img.materialForRendering);
        img.material = mat;
        w = texSets[0].tex[0].width;
        h = texSets[0].tex[0].height;
        
        currVal = 0;// shader offset from -1 to 1;
        swappedPrev = false;
        swappedNext = false;
        loopCount = 0;

        tex1 = mat.GetTexture("_PrevTex");
        tex2 = mat.GetTexture("_CurrTex");
        tex3 = mat.GetTexture("_NextTex");
    }

    /// <summary>
    /// value looping between -1 and 1 that starts at zero and goes up (or down, if speed is negative!)
    /// </summary>
    float GetCurrVal()
    {
        return currVal + Time.deltaTime * speed;
    }

    void Loop()
    {
        loopCount++;
        if(loopCount >= loopsBeforeChangingID)
        {
            // minus 3 because this is the index of the first texture out of three (prev/curr/next)
            texID = UnityEngine.Random.Range(0, texSets[texSet].tex.Length - 3);
            loopCount = 0;
        }
    }

    void Update () {
        // todo: get it out of the update loop
        float ratioX = Screen.width / w;
        float ratioY = Screen.height / h;

        // use whichever multiplier is smaller
        float ratio = ratioX < ratioY ? ratioX : ratioY;

        // now we can get the new height and width
        //float newHeight = tex[0].height * ratio;
        //float newWidth = tex[0].width * ratio;
        
        // set tiling to compensate for aspect ratio of the screen (it's a full-screen image)
        var texScale = mat.GetTextureScale("_MainTex");
        float invertedRatio = texScale.x / ratio;
        texScale.y = invertedRatio;
        mat.SetTextureScale("_MainTex", texScale);

        // set value from previous update loop
        mat.SetFloat("_CurrVal", currVal);

        currVal = GetCurrVal();
        
        if(currVal >= 1f) // loop it to -1 when it reaches the end range 1
        {
            // Example if it's 1.1, it spits out -0.9
            currVal = -1f + (currVal - 1f);

            // swap tex (prev/CURR/next)
            if (verbose) Debug.Log("[BgManager]: Loop CurrVal");
            mat.SetTexture("_CurrTex", Tex2);
            Loop();
        }
        else if (currVal < -1f)
        {
            // Example if it's -1.1, it spits out 0.9
            currVal = 2f + currVal;

            // swap tex (prev/CURR/next)
            if (verbose) Debug.Log("[BgManager]: Loop negative CurrVal");
            mat.SetTexture("_CurrTex", Tex2);
            Loop();
        }
        else if (currVal < 0 && currVal > -ratio && !swappedNext) // swap next before it gets to display it
        {
            // swap tex (prev/curr/NEXT)

            mat.SetTexture("_NextTex", tex1);

            if (verbose) Debug.Log("[BgManager]: Swap Next");
            swappedNext = true;
            swappedPrev = false;
        }
        else if (currVal > ratio && !swappedPrev) // swap previous while it displaying next
        {
            // swap tex (PREV/curr/next)
            tex3 = mat.GetTexture("_NextTex");
            tex1 = mat.GetTexture("_PrevTex");

            if (tex1 != Tex1 && tex1 != Tex3)
            {
                tex1 = Tex1;
                if (verbose) Debug.Log("[BgManager]: TEX1");
            }
            else if (tex3 != Tex1 && tex3 != Tex3)
            {
                tex1 = Tex3;
                if (verbose) Debug.Log("[BgManager]: TEX3");
            }
            else if (tex1 == tex3)
            {
                if(tex1 == Tex3)
                    tex1 = Tex1;
                else
                    tex1 = Tex3;
                if (verbose) Debug.Log("[BgManager]: TEX3 fallback");
            }

            mat.SetTexture("_PrevTex", tex3);

            if (verbose) Debug.Log("[BgManager]: Swap Prev");
            swappedPrev = true;
            swappedNext = false;
        }
	}
}
