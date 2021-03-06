// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
//
// Author: Kenneth James Pouncey

namespace Microsoft.Xna.Framework.Graphics
{
    public sealed partial class SamplerStateCollection
    {
        private int _d3dDirty;

        private void PlatformSetSamplerState(int index)
        {
            _d3dDirty |= 1 << index;
        }

        private void PlatformClear()
        {
            _d3dDirty = int.MaxValue;
        }

        private void PlatformDirty()
        {
            _d3dDirty = int.MaxValue;
        }

        internal void PlatformSetSamplers(GraphicsDevice device)
        {
            if (_applyToStage != ShaderStage.Pixel && !device.GraphicsCapabilities.SupportsVertexTextures)
                return;

            // Skip out if nothing has changed.
            if (_d3dDirty == 0)
                return;

            // NOTE: We make the assumption here that the caller has
            // locked the d3dContext for us to use.
            SharpDX.Direct3D11.CommonShaderStage shaderStage = null;
            switch (_applyToStage)
            {
            case ShaderStage.Vertex:
                shaderStage = device._d3dContext.VertexShader;
                break;
            case ShaderStage.Hull:
                shaderStage = device._d3dContext.HullShader;
                break;
            case ShaderStage.Domain:
                shaderStage = device._d3dContext.DomainShader;
                break;
            case ShaderStage.Geometry:
                shaderStage = device._d3dContext.GeometryShader;
                break;
            case ShaderStage.Pixel:
                shaderStage = device._d3dContext.PixelShader;
                break;
            }

            for (var i = 0; i < _actualSamplers.Length; i++)
            {
                var mask = 1 << i;
                if ((_d3dDirty & mask) == 0)
                    continue;

                var sampler = _actualSamplers[i];
                SharpDX.Direct3D11.SamplerState state = null;
                if (sampler != null)
                    state = sampler.GetState(device);

                shaderStage.SetSampler(i, state);

                _d3dDirty &= ~mask;
                if (_d3dDirty == 0)
                    break;
            }

            _d3dDirty = 0;
        }
    }
}
