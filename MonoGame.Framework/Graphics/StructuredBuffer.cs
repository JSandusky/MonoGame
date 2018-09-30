using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Utilities;
using SharpDX.Direct3D11;

namespace Microsoft.Xna.Framework.Graphics
{
    public partial class StructuredBuffer : Texture
    {
        internal int dataSize;
        internal int dataStride;

        /// <summary>
        /// Get the total size of the structured buffer.
        /// </summary>
        public int DataSize { get { return dataSize; } }
        /// <summary>
        /// Get the total size (with padding) of the contained structure element type
        /// </summary>
        public int StructureSize { get { return dataStride; } }

        /// <summary>
        /// note structure size should include padding
        /// </summary>
        /// <param name="dataSize">Total size of the buffer, should be a multiple of the structure size</param>
        /// <param name="structureSize">Size of the element structure of each record in the structured buffer</param>
        public StructuredBuffer(GraphicsDevice graphicsDevice, int dataSize, int structureSize)
        {
            this.GraphicsDevice = graphicsDevice;
            this.dataSize = dataSize;
            this.dataStride = structureSize;
        }

        /// <summary>
        /// Helper for constructing a structured buffer for a specific record type with a count
        /// </summary>
        /// <typeparam name="T">Type of structure contained</typeparam>
        /// <param name="graphicsDevice">device instance</param>
        /// <param name="recordCt">number of records the structured buffer should hold</param>
        /// <returns>A new structured buffer with the appropriate size and stride</returns>
        public static StructuredBuffer Create<T>(GraphicsDevice graphicsDevice, int recordCt)
        {
            var elementSizeInByte = ReflectionHelpers.SizeOf<T>.Get();
            return new StructuredBuffer(graphicsDevice, elementSizeInByte * recordCt, elementSizeInByte);
        }
    }
}
