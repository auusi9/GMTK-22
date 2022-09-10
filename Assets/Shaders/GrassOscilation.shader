Shader "Hidden/GrassOscilation"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _DispTex ("Displacement Texture", 2D) = "white" {}
        [Header(Displacement Settings)][Space]
        _DispSize ("Texture Size", Range(0, 10)) = 1
        _DispInt ("Intensity", Range(0, 1)) = 0.5
        _HeightInt ("Height Intensity", Range(0,10)) = 1
        _DispX ("Displacement X", Range(-1, 1)) = 0.5
        _DispY ("Displacement Y", Range(-1, 1)) = 0.5
        _DispSpeed ("Speed", Range(0, 10)) = 1
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
                half4 color : COLOR;
            };

            struct v2f
            {
                half2 uv : TEXCOORD0;
                half3 worldPos : TEXCOORD1;
                half4 vertex : SV_POSITION;
                half4 color : COLOR;
            };

            half _DispSize, _DispX, _DispY, _DispSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz * (10.1f - _DispSize);
                
                float t = _Time.y;
                half tx = _DispX * t * _DispSpeed;
                half ty = _DispY * t * _DispSpeed;
                o.worldPos.x -= tx;
                o.worldPos.y -= ty;

                o.uv = v.uv;
                o.color = v.color;

                return o;
            }

            sampler2D _DispTex, _MainTex;
            half4 _Color;
            half _DispInt, _HeightInt;

            half4 frag (v2f i) : SV_Target
            {
                half4 dispColor = tex2D(_DispTex, i.worldPos);
                half disp = (dispColor.r * 2) - 1;
                disp *= _DispInt;
                disp *= i.uv.y * _HeightInt;
                half4 mainColor = tex2D(_MainTex, half2(i.uv.x + disp, i.uv.y - (disp * 0.5)));
                
                half4 c = mainColor * i.color;

                return c;
            }
            ENDCG
        }
    }
}
