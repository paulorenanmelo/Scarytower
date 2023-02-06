using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Renderer))]
public class BgManager : MonoBehaviour {

    [SerializeField] private float speed = 1f;
    [SerializeField] private Texture[] tex;

    private Texture tex1;
    private Texture tex2;
    private Texture tex3;

	//Renderer rend;
	Image img = null;
	Material mat = null;
    //MaterialPropertyBlock mpb;
    private float currVal = 0;
    private bool swappedPrev = false;
    private bool swappedNext = false;
    

    void Start()
    {
        //rend = GetComponent<Renderer>();
        img = GetComponent<Image>();
        mat = img.materialForRendering;
        //mpb = new MaterialPropertyBlock();
        currVal = 0;
        swappedPrev = false;
        swappedNext = false;
        tex = new Texture[3] {
            mat.GetTexture("_PrevTex"),
            mat.GetTexture("_CurrTex"),
            mat.GetTexture("_NextTex")
        };
    }

    void Update () {
        //mpb.SetFloat("_CurrVal", currVal);
        //rend.SetPropertyBlock(mpb);
        mat.SetFloat("_CurrVal", currVal);
        
        // value starts at zero and goes up
        currVal += Time.deltaTime * speed;
        if(currVal >= 1f)
        {
            currVal = -1f;
        }
        else if (currVal < 0 && !swappedNext)
        {
            // swap tex (prev/curr/next)
            mat.SetTexture("_NextTex", tex1);

            //Debug.Log("Swap Next");
            swappedNext = true;
            swappedPrev = false;
        }
        else if (currVal >= 0f && !swappedPrev)
        {
            // swap tex (prev/curr/next)
            tex3 = mat.GetTexture("_NextTex");
            tex1 = mat.GetTexture("_PrevTex");
            mat.SetTexture("_PrevTex", tex3);

            //Debug.Log("Swap Prev");
            swappedPrev = true;
            swappedNext = false;
        }
	}
}
