// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/SpriteGradient" {
Properties {
	[MainTexture] _MainTex ("Sprite Texture", 2D) = "white" {}
    _Color2 ("Up Color", Color) = (1,1,1,1)
    _Color1 ("Down Color", Color) = (1,1,1,1)
    _Scale ("Scale", Float) = 1
    
// these six unused properties are required when a shader
// is used in the UI system, or you get a warning.
// look to UI-Default.shader to see these.
_StencilComp ("Stencil Comparison", Float) = 8
_Stencil ("Stencil ID", Float) = 0
_StencilOp ("Stencil Operation", Float) = 0
_StencilWriteMask ("Stencil Write Mask", Float) = 255
_StencilReadMask ("Stencil Read Mask", Float) = 255
_ColorMask ("Color Mask", Float) = 15
// see for example
// http://answers.unity3d.com/questions/980924/ui-mask-with-shader.html

}
 
SubShader {
    Tags {"Queue"="Background"  "IgnoreProjector"="True" "RenderType"="Transparent"}
    LOD 100
 
    ZWrite On
 
    Pass {
        CGPROGRAM
        #pragma vertex vert  
        #pragma fragment frag
        #include "UnityCG.cginc"

        sampler2D _MainTex;
        fixed4 _Color1;
        fixed4 _Color2;
        fixed  _Scale;

        struct appdata
        {
            float4 vertex : POSITION; // vertex position
            float2 uv : TEXCOORD0; // texture coordinate
        };
        
        struct v2f {
            float2 uv : TEXCOORD0;
            float4 pos : SV_POSITION;
            fixed4 col : COLOR;
        };
 
        v2f vert (appdata v)
        {
            v2f o;
            o.pos = UnityObjectToClipPos (v.vertex);
            o.col = lerp(_Color1,_Color2, v.uv.y );
            o.uv = v.uv;
            // o.col = half4( v.vertex.y, 0, 0, 1);
            return o;
        }
       
 
        float4 frag (v2f i) : COLOR {
            if(tex2D(_MainTex, i.uv).a < 0.1) discard;
            float4 c = i.col;
            c.a = 1;
            if(tex2D(_MainTex, i.uv).a < 0.1) c.a = 0; //wesh c pas transparent :(
            return c;
        }
            ENDCG
        }
    }
}