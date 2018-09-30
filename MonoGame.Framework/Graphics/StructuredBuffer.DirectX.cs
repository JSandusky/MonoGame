using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Utilities;
using SharpDX.Direct3D11;

namespace Microsoft.Xna.Framework.Graphics
{
    partial class StructuredBuffer
    {
        internal override Resource CreateTexture()
        {
            return new SharpDX.Direct3D11.Buffer(GraphicsDevice._d3dDevice, new BufferDescription
            {
                Usage = ResourceUsage.Default,
                CpuAccessFlags = CpuAccessFlags.Write,
                BindFlags = BindFlags.ShaderResource,
                OptionFlags = ResourceOptionFlags.BufferStructured,
                SizeInBytes = dataSize,
                StructureByteStride = dataStride
            });
        }

        public void SetData(byte[] data)
        {
            var d3dContext = GraphicsDevice._d3dContext;
            var texture = GetTexture();
            if (texture != null)
                d3dContext.UpdateSubresource(data, texture);
        }

        public void SetData<T>(T[] data)
        {
            var elementSizeInByte = ReflectionHelpers.SizeOf<T>.Get();
            var dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);

            try
            {
                var dataPtr = (IntPtr)(dataHandle.AddrOfPinnedObject().ToInt64());
                var d3dContext = GraphicsDevice._d3dContext;
                var texture = GetTexture();
                if (texture != null)
                    d3dContext.UpdateSubresource(texture, 0, null, dataPtr, 0, 0);
            }
            finally
            {
                dataHandle.Free();
            }
        }
    }
}
