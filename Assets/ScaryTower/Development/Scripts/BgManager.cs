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
public class BgManager : STMonoBehaviour {

    //[SerializeField] private float offset = 0f;
    [SerializeField] private TexSet[] texSets;
    //[SerializeField] private BgManager MasterValSource;
    [SerializeField] int texSet = 0;
    [SerializeField] int texID = 0;
    [SerializeField] int loopsBeforeChangingID = 3;
    [SerializeField] bool randomize = true;

    private Image img = null;
	private Material mat = null;

    private const float currValMin = -1f;
    private const float currValMax = 2f;
    [Range(currValMin, currValMax)] private float currVal = 0;
    private float w = 0;
    private float h = 0;
    private float ratio = 1;
    private float invertedRatio = 1;
    private bool swappedPrev = false;
    private bool swappedNext = false;
    private int loopCount = 0;

    public enum StatusUpdate { Idle, Loop_Positive, Loop_Negative, SwapNext, SwapPrevious }

    private float speed => Runtime.gameSpeed;// Settings.startingSpeed;
    private Texture Tex1 { get => texSets[texSet].tex[texID + 0]; set { texSets[texSet].tex[texID + 0] = value; } }
    private Texture Tex2 { get => texSets[texSet].tex[texID + 1]; set { texSets[texSet].tex[texID + 1] = value; } }
    private Texture Tex3 { get => texSets[texSet].tex[texID + 2]; set { texSets[texSet].tex[texID + 2] = value; } }
    protected new bool verbose => false; // comment this to fallback to default value from STMonobehaviour which comes from GameConfigManager

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
        currVal = currValMin;// shader texture sampling offset from -1 to 2;

        AspectRatioChanged();
        mat.SetFloat("_CurrVal", currVal);
    }

    void UpdateTextures()
    {
        switch (GetStatusUpdate())
        {
            case StatusUpdate.Loop_Positive:
                // Example if it's 2.1, it spits out -0.9,
                // because it was 0.1 beyond the max value
                currVal = currValMin + (currVal - currValMax);

                if (verbose) Debug.Log("[BgManager]: Loop CurrVal");
                Loop();

                // swap tex (prev/CURR/next)
                mat.SetTexture("_CurrTex", Tex2);
                swappedPrev = false;
                swappedNext = false;
                break;
            case StatusUpdate.Loop_Negative:
                // Example if it's -1.1, it spits out 0.9,
                // because it was 0.1 below the min value
                currVal = currValMax - currVal;

                if (verbose) Debug.Log("[BgManager]: Loop negative CurrVal");
                Loop();

                // swap tex (prev/CURR/next)
                mat.SetTexture("_CurrTex", Tex2);
                swappedPrev = false;
                swappedNext = false;
                break;
            case StatusUpdate.SwapNext:
                if (verbose) Debug.Log("[BgManager]: Swap Next");

                // swap tex (prev/curr/NEXT)
                mat.SetTexture("_NextTex", Tex3);
                swappedNext = true;
                break;
            case StatusUpdate.SwapPrevious:
                if (verbose) Debug.Log("[BgManager]: Swap Prev");

                // swap tex (PREV/curr/next)
                mat.SetTexture("_PrevTex", Tex1);
                swappedPrev = true;
                break;
            case StatusUpdate.Idle:
            default:
                break;
        }
    }

    /// <summary>
    /// value looping between -1 and 1 that starts at zero and goes up (or down, if speed is negative!)
    /// </summary>
    void UpdateCurrVal()
    {
        currVal += Time.deltaTime * speed;
        // temporary while we test without updating textures
        //if (currVal > currValMax)
        //{
        //    currVal = currValMin;
        //}
        //else if (currVal < currValMin)
        //{
        //    currVal = currValMax;
        //}
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
        // loop it to -1 when it reaches the end range 1
        if (currVal > currValMax)
        {
            return StatusUpdate.Loop_Positive;
        }
        // loop it to 1 when it reaches the end range -1
        else if (currVal < currValMin)
        {
            return StatusUpdate.Loop_Negative;
        }
        // swap next before it gets to display it
        else if (currVal > currValMin + ((invertedRatio - 1f) * 1f) && currVal < 0 && !swappedNext)
        {
            return StatusUpdate.SwapNext;
        }
        // swap previous while it displaying next
        else if (currVal > ((invertedRatio - 1f) * 1f) && currVal < 1f && !swappedPrev)
        {
            return StatusUpdate.SwapPrevious;
        }
        return StatusUpdate.Idle;
    }

    public override void AspectRatioChanged()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        float ratioX = Screen.width / w;
        float ratioY = Screen.height / h;
        
        // difference between ratio of the screen and ratio of the background image
        // this is multiplied by a factor that approximates the best visual outcome
        // perhaps could be improved to remove this magical number
        ratio = ((float)Screen.width / Screen.height) / (w / h) * 0.88375f;

        Debug.Log("[AspectRatioChanged]: ratio = " + ratio + ", width = " + Screen.width + ", height = " + Screen.height);

        // set texture tiling to compensate for aspect ratio of the screen (it's a full-screen image)
        var texScale = mat.GetTextureScale("_MainTex");
        invertedRatio = texScale.x / ratio;
        texScale.y = invertedRatio;
        mat.SetTextureScale("_MainTex", texScale);
    }

    void Update () {
        if (Runtime.state != STState.Playing) return;

        // set value from previous update loop
        mat.SetFloat("_CurrVal", currVal);
        UpdateCurrVal();

        UpdateTextures();
    }
}
