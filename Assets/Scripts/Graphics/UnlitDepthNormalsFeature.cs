// daniel todo: see https://forum.unity.com/threads/2020-2-urp-ssao-not-working-with-unlit-shaders.1026334/
/*
Render Unlit Opaque objects into CameraNormalsTexture for SSAO Depth Normals mode. 
If you need Alpha clipping or vertex displacement, you'll need to use Depth mode,
or edit the generated shader code to add in the DepthNormals pass instead.
This is meant as an alternative if you don't need those.
For depthNormalsMat, I'm using a blank PBR/Lit SG with passIndex set to 4 (corresponds to it's DepthNormals pass).
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class UnlitDepthNormalsFeature : ScriptableRendererFeature {
    class UnlitDepthNormalsPass : ScriptableRenderPass {
        
        private Material depthNormalsMat;
        private int passIndex;

        private ProfilingSampler m_ProfilingSampler;
        private FilteringSettings m_FilteringSettings;
        private List<ShaderTagId> m_ShaderTagIdList = new List<ShaderTagId>();

        private RenderTargetHandle depthTex;
        private RenderTargetHandle normalsTex;
        
        public UnlitDepthNormalsPass(Material depthNormalsMat, int passIndex, LayerMask layerMask) {
            this.depthNormalsMat = depthNormalsMat;
            this.passIndex = passIndex;
            m_ProfilingSampler = new ProfilingSampler("UnlitDepthNormals");
            m_FilteringSettings = new FilteringSettings(RenderQueueRange.opaque, layerMask);

            m_ShaderTagIdList.Add(new ShaderTagId("SRPDefaultUnlit"));

            depthTex.Init("_CameraDepthTexture");
            normalsTex.Init("_CameraNormalsTexture");
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData) {
            ConfigureTarget(normalsTex.Identifier(), depthTex.Identifier());
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {
            SortingCriteria sortingCriteria = renderingData.cameraData.defaultOpaqueSortFlags;

            DrawingSettings drawingSettings = CreateDrawingSettings(m_ShaderTagIdList, ref renderingData, sortingCriteria);
            drawingSettings.overrideMaterial = depthNormalsMat;
            drawingSettings.overrideMaterialPassIndex = passIndex;

            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, m_ProfilingSampler)) {
                context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref m_FilteringSettings);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
        
        public override void OnCameraCleanup(CommandBuffer cmd) {

        }
    }

    public Material depthNormalsMat;
    public int passIndex;
    public LayerMask layerMask;

    UnlitDepthNormalsPass m_ScriptablePass;
    
    public override void Create() {
        m_ScriptablePass = new UnlitDepthNormalsPass(depthNormalsMat, passIndex, layerMask);
        
        m_ScriptablePass.renderPassEvent = RenderPassEvent.AfterRenderingPrePasses;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {
        if (depthNormalsMat == null) return;
        renderer.EnqueuePass(m_ScriptablePass);
    }
}

/*
In Unlit Shader Graph, can then use a Custom Function (String mode) to sample the SSAO texture :

#ifdef SHADERGRAPH_PREVIEW
   DirectAO = 1;
#else
   AmbientOcclusionFactor aoFactor = GetScreenSpaceAmbientOcclusion(ScreenPos);
   DirectAO = aoFactor.directAmbientOcclusion;
#endif

Unsure exactly how to combine it with the shader, but a Multiply might do.
There's also aoFactor.indirectAmbientOcclusion if you want to support that,
See the UniversalFragmentPBR method in URP's Shader Library Lighting.hlsl.
*/