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
public class BgManager : STMonoBehaviour {

    //[SerializeField] private float offset = 0f;
    [SerializeField] private TexSet[] texSets;
    //[SerializeField] private BgManager MasterValSource;
    [SerializeField] int texSet = 0;
    [SerializeField] int texID = 0;
    [SerializeField] int loopsBeforeChangingID = 3;
    [SerializeField] bool randomize = true;

    private Texture tex1;
    private Texture tex2;
    private Texture tex3;

    private Image img = null;
	private Material mat = null;
    
    [Range(-1f, 1f)] private float currVal = 0;
    private float w = 0;
    private float h = 0;
    private float ratio = 1;
    private bool swappedPrev = false;
    private bool swappedNext = false;
    private int loopCount = 0;

    public enum StatusUpdate { Idle, Loop_Positive, Loop_Negative, SwapNext, SwapPrevious }
    
    private float speed => GameConfigManager.Instance.gameSettings.startingSpeed;
    private Texture Tex1 { get => texSets[texSet].tex[texID + 0]; set { texSets[texSet].tex[texID + 0] = value; } }
    private Texture Tex2 { get => texSets[texSet].tex[texID + 1]; set { texSets[texSet].tex[texID + 1] = value; } }
    private Texture Tex3 { get => texSets[texSet].tex[texID + 2]; set { texSets[texSet].tex[texID + 2] = value; } }

    public override void Init()
    {
        base.Init(); // allows aspect ratio changed to be called automatically
        // make a material instance so two different backgrounds can be independent, but in sync
        img = GetComponent<Image>();
        mat = Instantiate(img.materialForRendering);
        img.material = mat;
        w = texSets[0].tex[0].width;
        h = texSets[0].tex[0].height;
        
        swappedPrev = false;
        swappedNext = false;
        loopCount = 0;
        currVal = -1;// shader texture sampling offset from -1 to 1;

        mat.SetFloat("_CurrVal", currVal);
        tex1 = mat.GetTexture("_PrevTex");
        tex2 = mat.GetTexture("_CurrTex");
        tex3 = mat.GetTexture("_NextTex");
    }

    void UpdateTextures()
    {
        switch (GetStatusUpdate())
        {
            case StatusUpdate.Loop_Positive:
                // Example if it's 1.1, it spits out -0.9
                currVal = -1f + (currVal - 1f);
                Loop();

                // swap tex (prev/CURR/next)
                if (verbose) Debug.Log("[BgManager]: Loop CurrVal");
                mat.SetTexture("_CurrTex", Tex2);
                break;
            case StatusUpdate.Loop_Negative:
                // Example if it's -1.1, it spits out 0.9
                currVal = 1f - currVal;
                Loop();

                // swap tex (prev/CURR/next)
                if (verbose) Debug.Log("[BgManager]: Loop negative CurrVal");
                mat.SetTexture("_CurrTex", Tex2);
                break;
            case StatusUpdate.SwapNext:
                // swap tex (prev/curr/NEXT)

                mat.SetTexture("_NextTex", tex1);

                if (verbose) Debug.Log("[BgManager]: Swap Next");
                swappedNext = true;
                swappedPrev = false;
                break;
            case StatusUpdate.SwapPrevious:
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
                    if (tex1 == Tex3)
                        tex1 = Tex1;
                    else
                        tex1 = Tex3;
                    if (verbose) Debug.Log("[BgManager]: TEX3 fallback");
                }

                mat.SetTexture("_PrevTex", tex3);

                // update old tex while it's not being displayed
                if (Tex2 == mat.GetTexture("_CurrTex"))
                {
                    if (verbose) Debug.Log("[BgManager]: Replaced old tex");
                    mat.SetTexture("_CurrTexOld", Tex2);
                }

                if (verbose) Debug.Log("[BgManager]: Swap Prev");
                swappedPrev = true;
                swappedNext = false;
                break;
            case StatusUpdate.Idle:
            default:
                break;
        }
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
            if (!randomize)
            {
                // this predictable debug mode allows testing the background changing procedure in editor
                if (texID == 0)
                {
                    texID = 3;
                }
                else
                {
                    texID = 0;
                }
                return;
            }
            // minus 3 because this is the index of the first texture out of three (prev/curr/next)
            texID = UnityEngine.Random.Range(0, texSets[texSet].tex.Length - 3);
            loopCount = 0;
        }
    }

    StatusUpdate GetStatusUpdate()
    {
        if (currVal > 1f) // loop it to -1 when it reaches the end range 1
        {
            return StatusUpdate.Loop_Positive;
        }
        else if (currVal < -1f) // loop it to 1 when it reaches the end range -1
        {
            return StatusUpdate.Loop_Negative;
        }
        else if (currVal < 0 && currVal > -ratio && !swappedNext) // swap next before it gets to display it
        {
            return StatusUpdate.SwapNext;
        }
        else if (currVal > ratio && !swappedPrev) // swap previous while it displaying next
        {
            return StatusUpdate.SwapPrevious;
        }
        return StatusUpdate.Idle;
    }

    public override void AspectRatioChanged()
    {
        float ratioX = Screen.width / w;
        float ratioY = Screen.height / h;

        // use whichever multiplier is smaller
        //float ratio = ratioX < ratioY ? ratioX : ratioY;
        // use horizontal ratio
        ratio = ratioX;
        Debug.Log("[AspectRatioChanged]: ratio = " + ratio);

        // set texture tiling to compensate for aspect ratio of the screen (it's a full-screen image)
        var texScale = mat.GetTextureScale("_MainTex");
        float invertedRatio = texScale.x / ratio;
        texScale.y = invertedRatio;
        mat.SetTextureScale("_MainTex", texScale);
    }

    void Update () {
        // set value from previous update loop
        mat.SetFloat("_CurrVal", currVal);

        currVal = GetCurrVal();

        UpdateTextures();
	}
}
