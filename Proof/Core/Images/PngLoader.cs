using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;

namespace Proof.Core.Images
{
    public class PngLoader
    {
        private const int IntLength = 4;
        private const int HeaderLength = 8;
        private const int LengthDefinitionLength = IntLength;
        private const int ChunkTypeLength = 4;
        private const int CrcLength = 4;

        public PngImage LoadImage(string filePath)
        {
            byte[] bytes = File.ReadAllBytes(filePath);

            if(!HasValidHeader(bytes))
            {
                throw new IOException($"Invalid header for PNG found for: {filePath}");
            }

            try
            {
                return ProcessChunks(bytes);
            }
            catch(IOException e)
            {
                throw new IOException($"An error occurred loading {filePath}", e);
            }
        }

        private bool HasValidHeader(byte[] bytes)
        {
            return bytes[0] == 0x89
                && bytes[1] == 0x50
                && bytes[2] == 0x4E
                && bytes[3] == 0x47
                && bytes[4] == 0x0D
                && bytes[5] == 0x0A
                && bytes[6] == 0x1A
                && bytes[7] == 0x0A;
        }

        private PngImage ProcessChunks(byte[] bytes)
        {
            int index = HeaderLength;
            CoreImageData coreImageData = ProcessIhdr(bytes, ref index);

            byte[]? result = null;

            while(index < bytes.Length)
            {
                int length = ToInt(bytes, ref index);

                if (IsPlte(bytes, index))
                {
                    ProcessPlte(bytes, ref index);
                }
                else if (IsIdat(bytes, index))
                {
                    result = ProcessIdat(bytes, ref index, length, coreImageData.Width, coreImageData.Height);
                }
                else if (IEnd(bytes, index))
                {
                    break;
                }
                else if(IsCritical(bytes, index))
                {
                    throw new IOException("Found a critical chunk that could not be processed.");
                }

                // chunk type + length + CRC
                index += ChunkTypeLength + length + CrcLength;
            }

            if(result == null)
            {
                throw new IOException("Unable to load PNG image.");
            }

            return new PngImage(coreImageData, result);
        }

        private CoreImageData ProcessIhdr(byte[] bytes, ref int index)
        {
            int length = ToInt(bytes, ref index);

            bool isFirstChunkIhdr = bytes[index++] == 'I'
                && bytes[index++] == 'H'
                && bytes[index++] == 'D'
                && bytes[index++] == 'R';

            if (!isFirstChunkIhdr || length != 13)
            {
                throw new IOException("PNG chunk did not meet the PNG specifications");
            }

            int width = ToInt(bytes, ref index);
            int height = ToInt(bytes, ref index);
            byte bitDepth = bytes[index++];
            byte colourType = bytes[index++];
            byte compressionMethod = bytes[index++]; // Only 0 allowed (deflate/inflate)
            byte filterMethod = bytes[index++];  // Only 0 allowed
            byte interlaceMethod = bytes[index++];   // 0 = no interlace; 1 = Adam7 interlace

            index += CrcLength;

            return new CoreImageData(
                width,
                height,
                bitDepth,
                (ColourType)colourType,
                (CompressionMethod)compressionMethod,
                (FilterMethod)filterMethod,
                (InterlaceMethod)interlaceMethod);
        }

        private void ProcessPlte(byte[] bytes, ref int index)
        {
            throw new NotImplementedException();
        }

        private byte[] ProcessIdat(byte[] bytes, ref int index, int length, int width, int height)
        {
            var buffer = new byte[length];
            Array.Copy(bytes, index + ChunkTypeLength, buffer, 0, length);

            byte[] decompressedData = new byte[width * height * 3 + height];
            using (var memory = new MemoryStream(buffer))
            {
                using (var inflater = new InflaterInputStream(memory))
                {
                    inflater.Read(decompressedData, 0, decompressedData.Length);
                }
            }

            byte[] extractedData = new byte[decompressedData.Length - height];

            int bytesPerLine = width * 3 + 1;
            for(int line = 0; line < height; line++)
            {
                byte filterByte = decompressedData[line * bytesPerLine];
                for(int i = 1; i < bytesPerLine; i++)
                {
                    extractedData[line * (bytesPerLine - 1) + i - 1] = decompressedData[line * bytesPerLine + i];
                }
            }

            return extractedData;
        }

        private bool IsPlte(byte[] bytes, int index)
        {
            return bytes[index + 0] == 'P'
                && bytes[index + 1] == 'L'
                && bytes[index + 2] == 'T'
                && bytes[index + 3] == 'E';
        }

        private bool IsIdat(byte[] bytes, int index)
        {
            return bytes[index + 0] == 'I'
                && bytes[index + 1] == 'D'
                && bytes[index + 2] == 'A'
                && bytes[index + 3] == 'T';
        }

        private bool IEnd(byte[] bytes, int index)
        {
            return bytes[index + 0] == 'I'
                && bytes[index + 1] == 'E'
                && bytes[index + 2] == 'N'
                && bytes[index + 3] == 'D';
        }

        private bool IsCritical(byte[] bytes, int index)
        {
            return char.IsUpper((char)bytes[index]);
        }

        private int ToInt(byte[] bytes, ref int index)
        {
            var buffer = new Span<byte>(bytes, index, IntLength);

            if (BitConverter.IsLittleEndian)
            {
                buffer.Reverse();
            }

            int result = BitConverter.ToInt32(buffer);
            index += IntLength;
            return result;
        }
    }
}
