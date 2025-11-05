Shader "Custom/RefillPrint"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.0
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Shape ("PrintShape", 2D) = "white" {}
        [ShowAsVector2] _HitPoint("hit", Vector) = (0,0,0,0)
        _RadiusX("RadiusX", Range(0,0.1)) = 0.015
        _RadiusY("RadiusY", Range(0,0.1)) = 0.015
        _Alpha("Alpha", Range(0,1)) = 1
        _PrintColor("Print Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex, _Shape;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        float _RadiusX, _RadiusY, _Alpha;
        fixed4 _Color, _HitPoint, _PrintColor;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        bool printCoord(float2 uv, out float2 printUV) 
        {
            if (_HitPoint.z == 1) {

                if (uv.x < _HitPoint.x + _RadiusX && uv.x > _HitPoint.x - _RadiusX && uv.y < _HitPoint.y + _RadiusY && uv.y > _HitPoint.y - _RadiusY) {
                    printUV = float2((uv.x - _HitPoint.x + _RadiusX) / _RadiusX / 2, (uv.y - _HitPoint.y + _RadiusY) / _RadiusY / 2) ;
                    return true;
                }

            } 

            return false;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;

            float2 printUV;

            if (printCoord(IN.uv_MainTex, printUV)) {
                fixed4 p = tex2D(_Shape, printUV) * _Color;

                if (p.a != 0) {
                    o.Albedo = c.rgb * (1 - _Alpha * 0.4) + _PrintColor.rgb * _Alpha * 0.4;
                    o.Alpha = _PrintColor.a;
                }
            }

            
            
        }
        ENDCG
    }
    FallBack "Diffuse"
}
