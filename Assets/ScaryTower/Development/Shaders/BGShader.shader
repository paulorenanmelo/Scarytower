Shader "Unlit/BGShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PrevTex ("Previous Texture", 2D) = "white" {}
        _CurrTex ("Current Texture", 2D) = "white" {}
        _NextTex ("Next Texture", 2D) = "white" {}
        // -1 displaying Previous Texture, 0 displaying Current Texture, 1 displaying Next Texture
        _CurrVal ("Current Value", Range(-1.0, 1.0)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            sampler2D _PrevTex;
            float4 _PrevTex_ST;
            sampler2D _CurrTex;
            float4 _CurrTex_ST;
            sampler2D _NextTex;
            float4 _NextTex_ST;
            
            float _CurrVal;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col;// = tex2D(_MainTex, i.uv);
                fixed4 colPrevTex = fixed4(0, 0, 0, 0);// = tex2D(_PrevTex, i.uv);
                fixed4 colCurrTex = fixed4(0, 0, 0, 0);// = tex2D(_CurrTex, i.uv);
                fixed4 colNextTex = fixed4(0, 0, 0, 0);// = tex2D(_NextTex, i.uv);

                fixed2 uv = i.uv;// - fixed2(0, _CurrVal * 0.5);
                fixed2 uv2 = i.uv;// + fixed2(0, _CurrVal * 0.5);

                //colPrevTex = tex2D(_PrevTex, i.uv);
                //colCurrTex = tex2D(_CurrTex, i.uv);
                //colNextTex = tex2D(_NextTex, i.uv);

                // assign the correct texture index or mix based on _CurrVal
                if(_CurrVal < 0)
                {
                    uv.y -= _CurrVal + 1.0;
                    uv2.y -= _CurrVal;
                    if(uv.y < 1 && uv.y > 0)   colPrevTex = tex2D(_PrevTex, uv);
                    if(uv2.y < 1 && uv2.y > 0) colCurrTex = tex2D(_CurrTex, uv2);
                    //col = lerp(colPrevTex, colCurrTex, clamp (_CurrVal + 1, 0, 1));
                    col = colPrevTex + colCurrTex;
                }
                if(_CurrVal > 0)
                {
                    uv.y -= _CurrVal;
                    uv2.y -= _CurrVal - 1.0;
                    if(uv.y < 1 && uv.y > 0)   colCurrTex = tex2D(_CurrTex, uv);
                    if(uv2.y < 1 && uv2.y > 0) colNextTex = tex2D(_NextTex, uv2);
                    //col = lerp(colCurrTex, colNextTex, clamp (_CurrVal, 0, 1));
                    col = colCurrTex + colNextTex;
                }
                if(_CurrVal == 0)
                {
                    colCurrTex = tex2D(_CurrTex, i.uv);
                    col = colCurrTex;
                }

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
