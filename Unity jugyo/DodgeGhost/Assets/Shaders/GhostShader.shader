Shader "PQ/GhostShader" {
    Properties {
        _Face_texture ("Face_texture", 2D) = "white" {}
        _Face_Color ("Face_Color", Color) = (0.3882353,0.0627451,0.0627451,1)
        _out_Color ("out_Color", Color) = (0.8897059,0.8897059,0.8897059,1)
        _in_Color ("in_Color", Color) = (0.572549,0.3568628,0.5764706,1)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "Outline"
            Tags {
            }
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma exclude_renderers flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _in_Color;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.pos = UnityObjectToClipPos(float4(v.vertex.xyz + v.normal*0.0,1));
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                return fixed4(_in_Color.rgb,0);
            }
            ENDCG
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            //#define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float4 _out_Color;
            uniform sampler2D _Face_texture; uniform float4 _Face_texture_ST;
            uniform float4 _Face_Color;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(float4(v.normal,0), unity_WorldToObject).xyz;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float3 normalDirection =  i.normalDir;
////// Lighting:
////// Emissive:
                float2 node_98 = i.uv0;
                float4 node_4 = tex2D(_Face_texture,TRANSFORM_TEX(node_98.rg, _Face_texture));
                float3 emissive = lerp(_out_Color.rgb,_Face_Color.rgb,node_4.rgb);
                float3 finalColor = emissive;
                float4 node_87 = _Time + _TimeEditor;
/// Final Color:
                return fixed4(finalColor,(node_4.r+(((1.0-max(0,dot(normalDirection, viewDirection)))*3.5)*((sin(node_87.a)/8.0)+0.6))));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
