Shader "Hidden/PolkadotPattern"
{
    Properties
    {
        _PolkaTex ("Polkadot Texture", 2D) = "white" {}
        _PolkaSize ("Polkadot Size", Range(0, 10)) = 1
        _PolkaRange ("Polkadot Range", Range(0, 1)) = 1
        _IntensityTex ("Intensity Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _AlphaStep ("Alpha Step", Range(0.01, 2)) = 0
        _SmoothStep ("Smooth Step", Range(0, 0.05)) = 0
        [Header(Displacement)][Space]
        _DispX ("Displacement X", Range(-1, 1)) = 0.5
        _DispY ("Displacement Y", Range(-1, 1)) = 0.5
        _DispSpeed ("Displacement Speed", Range(0, 10)) = 1
        [Toggle(USE_UNSCALED_TIME)]_UseUnscaledTime ("Use Unscaled Time", Float) = 0
        _UnscaledTime ("Unscaled Time", Float) = 1
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
            #pragma shader_feature USE_UNSCALED_TIME
            #include "UnityCG.cginc"

            struct appdata
            {
                half4 vertex : POSITION;
                half2 slicedUV : TEXCOORD0;
                half4 color : COLOR;
            };

            struct v2f
            {
                half2 slicedUV : TEXCOORD0;
                half2 regularUV : TEXCOORD1;
                half3 worldPos : TEXCOORD2;
                half4 vertex : SV_POSITION;
                half4 color : COLOR;
            };

            half _PolkaSize, _DispX, _DispY, _DispSpeed, _UnscaledTime;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz * (10.1f - _PolkaSize);
                
                #ifdef USE_UNSCALED_TIME
                    float t = _UnscaledTime;
                #else
                    float t = _Time.y;
                #endif

                half tx = _DispX * t *_DispSpeed;
                half ty = _DispY * t * _DispSpeed;
                o.worldPos.x -= tx;
                o.worldPos.y -= ty;

                o.slicedUV = v.slicedUV;
                o.color = v.color;

                //No sliced UVs
                o.regularUV.x = v.slicedUV.x > 0.5 ? 1 : 0;
                o.regularUV.y = v.slicedUV.y > 0.5 ? 1 : 0;

                return o;
            }

            sampler2D _MainTex, _PolkaTex, _IntensityTex;
            half4 _Color;
            half _AlphaStep, _SmoothStep, _PolkaRange;

            half4 frag (v2f i) : SV_Target
            {
                half4 polkaColor = tex2D(_PolkaTex, i.worldPos);
                half4 intColor = tex2D(_IntensityTex, i.regularUV);

                intColor.a = (intColor.a - (1 - _PolkaRange)) * (1/_PolkaRange);

                half stepMax = saturate(_AlphaStep-intColor.a) + 0.01;
                half smoothStepMin = stepMax - _SmoothStep;
                half newAlpha = smoothstep(smoothStepMin, stepMax, polkaColor.a); 
                polkaColor.a = newAlpha;
                polkaColor.rgb = _Color.rgb;

                half4 mainColor = tex2D(_MainTex, i.slicedUV);
                
                half4 c = mainColor * i.color;
                c.rgb = (polkaColor.rgb * polkaColor.a) + (c.rgb * (1 - polkaColor.a));

                return c;
            }
            ENDCG
        }
    }
}
