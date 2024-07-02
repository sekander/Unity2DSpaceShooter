Shader "Custom/SpriteColorToggle"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _ShowWhite ("Show White", Range(0, 1)) = 0
    }
    
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        ZWrite Off
        Fog { Mode Off }
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            sampler2D _MainTex;
            float _ShowWhite;
            
            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the texture color with full alpha
                fixed4 texColor = tex2D(_MainTex, i.uv);
                
                // Conditionally return either texColor or pure white based on _ShowWhite
                fixed4 outputColor;
                if (_ShowWhite > 0.5)
                {
                    outputColor = fixed4(1, 1, 1, 1); // Pure white
                }
                else
                {
                    outputColor = texColor; // Original texture color
                }
                
                return outputColor;
            }
            ENDCG
        }
    }
}
