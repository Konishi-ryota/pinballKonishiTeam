// Shader created with Shader Forge v1.40 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.40;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,cpap:True,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:544,x:32948,y:32673,varname:node_544,prsc:2|emission-9393-OUT,alpha-5432-OUT;n:type:ShaderForge.SFN_Fresnel,id:3183,x:32255,y:33015,varname:node_3183,prsc:2|EXP-6048-OUT;n:type:ShaderForge.SFN_Slider,id:6048,x:31891,y:33087,ptovrint:False,ptlb:node_6048,ptin:_node_6048,varname:node_6048,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.7318651,max:3;n:type:ShaderForge.SFN_OneMinus,id:5432,x:32458,y:32972,varname:node_5432,prsc:2|IN-3183-OUT;n:type:ShaderForge.SFN_Color,id:5004,x:32344,y:32626,ptovrint:False,ptlb:Color_outer,ptin:_Color_outer,varname:node_5004,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Lerp,id:8088,x:32580,y:32768,varname:node_8088,prsc:2|A-5004-RGB,B-3659-RGB,T-5432-OUT;n:type:ShaderForge.SFN_Color,id:3659,x:32332,y:32811,ptovrint:False,ptlb:Color_Inner,ptin:_Color_Inner,varname:_node_5004_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.4245283,c2:0.4245283,c3:0.4245283,c4:1;n:type:ShaderForge.SFN_Multiply,id:9393,x:32733,y:32673,varname:node_9393,prsc:2|A-9678-RGB,B-8088-OUT;n:type:ShaderForge.SFN_VertexColor,id:9678,x:32535,y:32445,varname:node_9678,prsc:2;proporder:6048-5004-3659;pass:END;sub:END;*/

Shader "Custom/AlphaFresnelInvert" {
    Properties {
        _node_6048 ("node_6048", Range(0, 3)) = 0.7318651
        _Color_outer ("Color_outer", Color) = (0,0,0,1)
        _Color_Inner ("Color_Inner", Color) = (0.4245283,0.4245283,0.4245283,1)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma target 3.0
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float, _node_6048)
                UNITY_DEFINE_INSTANCED_PROP( float4, _Color_outer)
                UNITY_DEFINE_INSTANCED_PROP( float4, _Color_Inner)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID( v );
                UNITY_TRANSFER_INSTANCE_ID( v, o );
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float4 _Color_outer_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Color_outer );
                float4 _Color_Inner_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Color_Inner );
                float _node_6048_var = UNITY_ACCESS_INSTANCED_PROP( Props, _node_6048 );
                float node_5432 = (1.0 - pow(1.0-max(0,dot(normalDirection, viewDirection)),_node_6048_var));
                float3 emissive = (i.vertexColor.rgb*lerp(_Color_outer_var.rgb,_Color_Inner_var.rgb,node_5432));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,node_5432);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
