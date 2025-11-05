Shader "Unlit/ECG"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _LineColor("LineColor", Color) = (0,0,0,1)
        _SubTex("Texture", 2D) = "white" {}
        _Percent("Percent", Range(0,1)) = 0
        _OffsetSpeed("OffsetSpeed", float) = 0
        _Tile("Tile", int) = 0
        _CurUv("Current_UvX", float) = 1
    }
        SubShader
        {
            Tags { "RenderType" = "Transparent" }
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
                float _Percent, _OffsetSpeed, _Tile;
                float4 _LineColor;
                float _CurUv;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    
                    float2 uv = i.uv;
                    float offset = _OffsetSpeed * _Time;
                    uv.x *= _Tile;
                    
                    /*if (uv.x > 2.0 - offset && uv.x < 3.0 - offset) {
                   
                        uv.x = uv.x % 1;
                        return tex2D(_SubTex, uv) * _LineColor;
                    }*/

                    uv.x += offset;


                    if (uv.x > 1) {
                        uv.x = uv.x % 1;
                  
                    } 
                    // sample the texture
                    fixed4 col = tex2D(_MainTex, uv) * _LineColor;

  

                return col;
            }
            ENDCG
        }
        }
}
