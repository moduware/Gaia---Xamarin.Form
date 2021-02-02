using System.Text;

namespace GaiaDemo
{
    public static class Utils
    {
        /// <summary>
        /// Convert a byte array to a human readable String.
        /// </summary>
        /// <param name="byteDatas">The byte array.</param>
        /// <returns>String containing values in byte array formatted as hex.</returns>
        public static string GetHexStringFromBytes(byte[] byteDatas)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < byteDatas.Length; i++)
            {
                builder.Append(string.Format("{0:X2} ", byteDatas[i]));
            }
            return builder.ToString().Trim();
        }
    }
}
