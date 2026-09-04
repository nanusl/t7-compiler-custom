using System;
using System.IO;
using System.Text;

namespace T89CompilerLib
{
    /// <summary>
    /// Utility Functions
    /// </summary>
    internal static class Utility
    {
        /// <summary>
        /// Computes the number of bytes require to pad this value
        /// </summary>
        public static int ComputePadding(int value, int alignment) => (((value) + ((alignment) - 1)) & ~((alignment) - 1)) - value;

        /// <summary>
        /// Aligns the value to the given alignment
        /// </summary>
        public static int AlignValue(this int value, int alignment) => (((value) + ((alignment) - 1)) & ~((alignment) - 1));

        public static uint AlignValue(this uint value, uint alignment) => (((value) + ((alignment) - 1)) & ~((alignment) - 1));

        /// <summary>
        /// Determines if a context value meets a desired value
        /// </summary>
        /// <param name="context"></param>
        /// <param name="desired"></param>
        /// <returns></returns>
        public static bool HasContext(this uint context, ScriptContext desired)
        {
            return (context & (uint)desired) > 0;
        }

        /// <summary>
        /// Reads a UTF-8 string terminated by a null byte and returns the reader to the original position
        /// </summary>
        /// <returns>Read String</returns>
        public static string PeekNullTerminatedString(this BinaryReader br, long offset, int maxSize = -1)
        {
            var temp = br.BaseStream.Position;
            br.BaseStream.Position = offset;

            MemoryStream buffer = new MemoryStream();
            int byteRead;
            int size = 0;
            while ((byteRead = br.BaseStream.ReadByte()) != 0x0 && size++ != maxSize)
                buffer.WriteByte((byte)byteRead);

            br.BaseStream.Position = temp;
            return Encoding.UTF8.GetString(buffer.ToArray());
        }

        /// <summary>
        /// Writes a null terminated UTF-8 string
        /// </summary>
        /// <param name="br"></param>
        /// <param name="str"></param>
        public static void WriteNullTerminatedString(this BinaryWriter br, string str)
        {
            foreach (byte c in Encoding.UTF8.GetBytes(str))
                br.Write(c);
            br.Write((byte)0);
        }

        /// <summary>
        /// Counts the number of lines in the given string
        /// </summary>
        public static int GetLineCount(string str)
        {
            int index = -1;
            int count = 0;
            while ((index = str.IndexOf(Environment.NewLine, index + 1)) != -1)
                count++;
            return count + 1;
        }

        public static string SanitiseString(string value) => value.Replace("/", "\\").Replace("\b", "\\b");
    }
}
