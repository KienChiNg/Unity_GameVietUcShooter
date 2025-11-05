Shader "Unlit/BtnBloodTestShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SubTex("Texture", 2D) = "white" {}
        _Percent("Percent", Range(0,1)) = 0

    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex, _SubTex;
            float4 _MainTex_ST, _SubTex_ST;
            float _Percent;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_SubTex, i.uv);

                // apply wipe
                if (_Percent > 0 && _Percent < 0.5) {

                    if (i.uv.x > 0.5 && i.uv.y - 0.5 - sin((0.25 - _Percent) * 6.2831) / cos((0.25 - _Percent) * 6.2831) * (i.uv.x - 0.5) > 0) {
                        col = tex2D(_MainTex, i.uv);
                    }
                }
                else if (_Percent >= 0.5 && _Percent < 1) {
                    if (i.uv.x > 0.5 || i.uv.y - 0.5 - sin((0.25 - _Percent) * 6.2831) / cos((0.25 - _Percent) * 6.2831) * (i.uv.x - 0.5) < 0) {
                        col = tex2D(_MainTex, i.uv);
                    }
                }
                else if (_Percent == 1) {
                    col = tex2D(_MainTex, i.uv);
                }
  
                return col;
            }
            ENDCG
        }
    }
}
