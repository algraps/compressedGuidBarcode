/*
 * Generate a compressed BarCode with C#
 * Author: Alessandro Graps
 * Year: 2013
 */

using System;
using System.Numerics;

namespace WindowsFormsApplication1
{
    public static class BarCode
    {
        public static string GUIDToCompressedString128(Guid lguid)
        {
            byte[] lguidb = lguid.ToByteArray();
            int offset = 0;
            int spareOff = 0;

            UInt32 spare = 0x0;

            foreach (byte b in lguidb)
            {
                spare = (UInt32)(spare | (UInt32)((b & 0x01) << (spareOff)));
                lguidb[offset] = (byte)(b >> 1);
                offset++;
                spareOff++;
                if (spareOff % 8 == 7)
                    spareOff++;
            }

            byte[] spareBits = new byte[3];

            spareBits[0] = (byte)(spare >> 0x10);
            spareBits[1] = (byte)(spare >> 0x08);
            spareBits[2] = (byte)spare;

            return System.Text.ASCIIEncoding.ASCII.GetString(lguidb) + System.Text.ASCIIEncoding.ASCII.GetString(spareBits);
        }

        public static Guid CompressedStringToGUID128(string cs)
        {
            byte[] guidBa = System.Text.UnicodeEncoding.ASCII.GetBytes(cs);
            UInt32 spare = 0x0;

            spare = (UInt32)(spare | (UInt32)(guidBa[16] << 0x10));
            spare = (UInt32)(spare | (UInt32)(guidBa[17] << 0x08));
            spare = (UInt32)(spare | guidBa[18]);

            int offset = 0;
            int spareOff = 0;

            foreach (byte b in guidBa)
            {
                UInt32 mask = (UInt32)(0x1 << (spareOff));

                guidBa[offset] = (byte)((b << 1) | (byte)((spare & mask) >> spareOff));
                offset++;
                spareOff++;
                if (spareOff % 8 == 7)
                    spareOff++;
            }

            byte[] ret = new byte[16];

            Array.Copy(guidBa, ret, 16);

            return new Guid(ret);
        }

        const int Code128Base = 110;

        public static string GUIDToCompressedString(Guid lguid)
        {
            byte[] positiveba = new byte[17];

            Array.Copy(lguid.ToByteArray(), positiveba, 16);
            positiveba[16] = 0x00;

            BigInteger n = new BigInteger(positiveba);
            string ret = "";

            while (n != 0)
            {
                ret = ((char)(n % Code128Base)) + ret;
                n /= Code128Base;
            }
            return ret;
        }

        public static Guid CompressedStringToGUID(string cs)
        {
            byte[] csb = System.Text.UnicodeEncoding.ASCII.GetBytes(cs);

            BigInteger n = 0;
            int i = 0;

            while (i < csb.Length)
            {
                n += csb[csb.Length - i - 1] * new BigInteger(System.Math.Pow(Code128Base, i));
                i++;
            }

            byte[] guidba = new byte[16];

            Array.Copy(n.ToByteArray(), guidba, 16);
            return (new Guid(guidba));
        }
        
    }
}
