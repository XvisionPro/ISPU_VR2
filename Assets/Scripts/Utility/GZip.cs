using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Utility
{
    public class GZip
    {
        /// <summary>
        /// After the incoming string is compressed by the Gzip algorithm, return Base64 encoding character.
        /// </summary>
        /// <param name = "rawstring"> String </ param> required to be compressed
        /// <returns> Compressed Base64 encoded string </ returns>
        public static string GZipCompressString(string rawString)
        {
            if (string.IsNullOrEmpty(rawString) || rawString.Length == 0)
            {
                return "";
            }
            else
            {
                byte[] rawData = System.Text.Encoding.UTF8.GetBytes(rawString.ToString());
                byte[] zippedData = Compress(rawData);
                return (string)(Convert.ToBase64String(zippedData));
            }
        }

        /// <summary>
        /// gzip compression
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        static byte[] Compress(byte[] rawData)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.IO.Compression.GZipStream compressedzipStream = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Compress, true);
            compressedzipStream.Write(rawData, 0, rawData.Length);
            compressedzipStream.Close();
            return ms.ToArray();
        }

        /// <summary>
        /// Decompression SRING 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string GetStringByString(string Value)
        {
            //DataSet ds = new DataSet();
            string CC = GZipDecompressString(Value);
            //System.IO.StringReader Sr = new System.IO.StringReader(CC);
            //ds.ReadXml(Sr);
            return CC;
        }

        /// <summary>
        /// Unzip
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static DataSet GetDatasetByString(string Value)
        {
            DataSet ds = new DataSet();
            string CC = GZipDecompressString(Value);
            System.IO.StringReader Sr = new System.IO.StringReader(CC);
            ds.ReadXml(Sr);
            return ds;
        }


        /// <summary>
        /// Dispatch the incoming binary string data with Gzip algorithm
        /// </summary>
        /// <param name = "zippedstring"> Binary string after Gzip compression </ param>
        /// <returns> Original uncompressed string </ returns>
        public static string GZipDecompressString(string zippedString)
        {
            if (string.IsNullOrEmpty(zippedString) || zippedString.Length == 0)
            {
                return "";
            }
            else
            {
                byte[] zippedData = Convert.FromBase64String(zippedString.ToString());
                return (string)(System.Text.Encoding.UTF8.GetString(Decompress(zippedData)));
            }
        }

        /// <summary>
        /// zip decompression
        /// </summary>
        /// <param name="zippedData"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] zippedData)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(zippedData);
            System.IO.Compression.GZipStream compressedzipStream = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Decompress);
            System.IO.MemoryStream outBuffer = new System.IO.MemoryStream();
            byte[] block = new byte[1024];
            while (true)
            {
                int bytesRead = compressedzipStream.Read(block, 0, block.Length);
                if (bytesRead <= 0)
                    break;
                else
                    outBuffer.Write(block, 0, bytesRead);
            }
            compressedzipStream.Close();
            return outBuffer.ToArray();

        }
    }
}
