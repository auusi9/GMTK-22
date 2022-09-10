Shader "Hidden/CloudsShadow"
{
    Properties
    {
        [Hidden]_MainTex ("Main Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _CloudsTex ("Clouds Texture", 2D) = "white" {}
        [Header(Settings)][Space]
        _Size ("Texture Size", Range(0, 10)) = 1
        _Size2 ("Texture Size 2", Range(0, 10)) = 1
        _AlphaSmoothStep ("Alpha Smooth Step", Range(0, 0.5)) = 0.1
        [Header(Movement Speed)][Space]
        _Speed1 ("Speed 01", Range(0, 10)) = 1
        _Speed2 ("Speed 02", Range(0, 10)) = 1
        [Header(Alpha Animation)][Space]
        _AlphaSpeed ("Alpha Speed", Range(0, 10)) = 1
        _AlphaSinSum ("Alpha Sinus Summ", Range(1, 10)) = 1        
        _AlphaSinDiv ("Alpha Sinus Div", Range(2, 10)) = 2        
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Lighting Off
        ZWrite Off
        ZTest Always
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                half4 vertex : POSITION;
                half2 uv : TEXCOORD0;
            };

            struct v2f
            {
                half2 uv : TEXCOORD0;
                half3 worldPos1 : TEXCOORD1;
                half3 worldPos2 : TEXCOORD2;
                half4 vertex : SV_POSITION;
            };

            half _Size, _Size2, _Speed1, _Speed2;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                half t1 = _Time.x * _Speed1;
                half t2 = _Time.x * _Speed2;

                half3 vecT1 = half3(t1, t1 * 0.5, 0);
                half3 vecT2 = half3(t2, t2 * -0.5, 0);

                o.worldPos1 = mul(unity_ObjectToWorld, v.vertex).xyz * (10.01f - _Size) - vecT1;
                o.worldPos2 = mul(unity_ObjectToWorld, v.vertex).xyz * (10.01f - _Size2) - vecT2 - 1;
                o.uv = v.uv;

                return o;
            }

            sampler2D _CloudsTex, _MainTex;
            half4 _Color;
            half _AlphaSpeed, _AlphaSmoothStep, _Test, _AlphaSinSum, _AlphaSinDiv;

            half4 frag (v2f i) : SV_Target
            {
                half4 c = _Color;
                half cloudsValue = tex2D(_CloudsTex, i.worldPos1).r;
                half cloudsValue2 = tex2D(_CloudsTex, i.worldPos2).r;
                
                half alphaMax = (sin(_Time.x * _AlphaSpeed) + _AlphaSinSum) / _AlphaSinDiv;
                half minRange = alphaMax - (_AlphaSmoothStep * 0.5);
                half maxRange = alphaMax + (_AlphaSmoothStep * 0.5);
                
                //cloudsValue = (cloudsValue * -1) + 1;
                //cloudsValue2 = (cloudsValue2 * -1) + 1;
                half cloud1Step = smoothstep(minRange, maxRange, cloudsValue);
                half cloud2Step = smoothstep(minRange, maxRange, cloudsValue2);
                
                c.a *= max(cloud1Step, cloud2Step);

                return c;
            }
            ENDCG
        }
    }
}
