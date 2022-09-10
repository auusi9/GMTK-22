Shader "Hidden/GrassOscilationVertex"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Speed ("Grass Speed", Range(0, 50)) = 1
        _Frequency ("Grass Frequency", Range(0, 10)) = 0.1
        _Amplitude ("Grass Amplitude", Range(0, 10)) = 0.1
        _2WaveMul ("Second Wave Multiplier", Range(0, 10)) = 1
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Opaque"
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
                half4 color : COLOR;
            };

            struct v2f
            {
                half2 uv : TEXCOORD0;
                half4 vertex : SV_POSITION;
                half3 worldPos : TEXCOORD1;
                half4 color : COLOR;
            };

            sampler2D _MainTex;
            half4 _MainTex_ST;
            half _Speed, _Frequency, _Amplitude, _2WaveMul;

            half4 Wind(half4 vertexPos, half2 uv, half3 worldPosition)
            {
                half t = _Time.y * _Speed;
                vertexPos.x += (sin((uv.x - t) * _Frequency + worldPosition.x) * (uv.y * _Amplitude) + (uv.y * _Amplitude)) + 
                               (sin((uv.x - t * _2WaveMul) * _Frequency * _2WaveMul + worldPosition.x) * (uv.y * _Amplitude) + (uv.y * _Amplitude));
                half4 vertex = UnityObjectToClipPos(vertexPos);
                return vertex;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.vertex = Wind(v.vertex, v.uv, o.worldPos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 mainColor = tex2D(_MainTex, i.uv);                
                half4 c = mainColor * i.color;
                return c;
            }
            ENDCG
        }
    }
}
