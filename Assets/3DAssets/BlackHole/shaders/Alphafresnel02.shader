// Shader created with Shader Forge v1.40 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.40;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,cpap:True,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:544,x:32948,y:32673,varname:node_544,prsc:2|emission-9393-OUT,alpha-3908-OUT;n:type:ShaderForge.SFN_Fresnel,id:3183,x:31866,y:32923,varname:node_3183,prsc:2|EXP-6048-OUT;n:type:ShaderForge.SFN_Slider,id:6048,x:31537,y:32972,ptovrint:False,ptlb:FresnelOuter,ptin:_FresnelOuter,varname:node_6048,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.6325958,max:3;n:type:ShaderForge.SFN_OneMinus,id:5432,x:32034,y:32923,varname:node_5432,prsc:2|IN-3183-OUT;n:type:ShaderForge.SFN_Multiply,id:9393,x:32720,y:32779,varname:node_9393,prsc:2|A-9678-RGB,B-3908-OUT,C-9678-A;n:type:ShaderForge.SFN_VertexColor,id:9678,x:32440,y:32750,varname:node_9678,prsc:2;n:type:ShaderForge.SFN_Fresnel,id:4999,x:32058,y:33227,varname:node_4999,prsc:2|EXP-4837-OUT;n:type:ShaderForge.SFN_Slider,id:4837,x:31727,y:33309,ptovrint:False,ptlb:FresnelInner,ptin:_FresnelInner,varname:_node_6048_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:2.435759,max:3;n:type:ShaderForge.SFN_Multiply,id:7664,x:32318,y:33262,varname:node_7664,prsc:2|A-4999-OUT,B-9114-OUT;n:type:ShaderForge.SFN_Slider,id:9114,x:31727,y:33445,ptovrint:False,ptlb:InnerPower,ptin:_InnerPower,varname:_FresnelInner_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:2.189369,max:4;n:type:ShaderForge.SFN_Min,id:3908,x:32440,y:32895,varname:node_3908,prsc:2|A-5738-OUT,B-7664-OUT;n:type:ShaderForge.SFN_Multiply,id:5738,x:32205,y:33017,varname:node_5738,prsc:2|A-5432-OUT,B-8551-OUT;n:type:ShaderForge.SFN_Slider,id:8551,x:31537,y:33082,ptovrint:False,ptlb:OuterPower,ptin:_OuterPower,varname:_InnerPower_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:2.731807,max:4;proporder:6048-8551-4837-9114;pass:END;sub:END;*/

Shader "Custom/AlphaFresnelRim" {
    Properties {
        _FresnelOuter ("FresnelOuter", Range(0, 3)) = 0.6325958
        _OuterPower ("OuterPower", Range(1, 4)) = 2.731807
        _FresnelInner ("FresnelInner", Range(0, 3)) = 2.435759
        _InnerPower ("InnerPower", Range(1, 4)) = 2.189369
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
                UNITY_DEFINE_INSTANCED_PROP( float, _FresnelOuter)
                UNITY_DEFINE_INSTANCED_PROP( float, _FresnelInner)
                UNITY_DEFINE_INSTANCED_PROP( float, _InnerPower)
                UNITY_DEFINE_INSTANCED_PROP( float, _OuterPower)
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
                float _FresnelOuter_var = UNITY_ACCESS_INSTANCED_PROP( Props, _FresnelOuter );
                float _OuterPower_var = UNITY_ACCESS_INSTANCED_PROP( Props, _OuterPower );
                float _FresnelInner_var = UNITY_ACCESS_INSTANCED_PROP( Props, _FresnelInner );
                float _InnerPower_var = UNITY_ACCESS_INSTANCED_PROP( Props, _InnerPower );
                float node_3908 = min(((1.0 - pow(1.0-max(0,dot(normalDirection, viewDirection)),_FresnelOuter_var))*_OuterPower_var),(pow(1.0-max(0,dot(normalDirection, viewDirection)),_FresnelInner_var)*_InnerPower_var));
                float3 emissive = (i.vertexColor.rgb*node_3908*i.vertexColor.a);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,node_3908);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
