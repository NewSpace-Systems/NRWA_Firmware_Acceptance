using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crc;

namespace NRWA_Communication_Acceptance
{
    internal class SLIP_Frame
    {
        public static byte[] AddFrame(byte[] a_Message)
        {
            List<byte> l_outMessage = new List<byte>();
            foreach (var item in a_Message)
            {
                switch (item)
                {
                    case 0xC0:
                        l_outMessage.Add(0xDB);
                        l_outMessage.Add(0xDC);
                        break;
                    case 0xDB:
                        l_outMessage.Add(0xDB);
                        l_outMessage.Add(0xDD);
                        break;
                    default:
                        l_outMessage.Add(item);
                        break;
                }
            }
            byte[] a_outMessage = l_outMessage.ToArray();
            return a_outMessage;
        }

        public static byte[] RemoveFrame(byte[] a_Message)
        {
            List<byte> l_outMessage = new List<byte>();
            int n = 0;
            while (n < a_Message.Length)
            {
                if ((a_Message[n] == 0xDB) && (a_Message[n + 1] == 0xDC))
                {
                    l_outMessage.Add(0xC0);
                    n += 2;
                }
                else if ((a_Message[n] == 0xDB) && (a_Message[n + 1] == 0xDD))
                {
                    l_outMessage.Add(0xDB);
                    n += 2;
                }
                else
                {
                    l_outMessage.Add(a_Message[n]);
                    n++;
                }
            }
            byte[] a_outmessage = l_outMessage.ToArray();
            return a_outmessage;
        }

        public static byte[] CreateSLIPframe(byte bDestAddr, byte bSrcAddr, byte bCmnd, byte[] a_Data)
        {
            List<byte> l_content = new List<byte>();
            l_content.Add(bDestAddr);
            l_content.Add(bSrcAddr);
            l_content.Add(bCmnd);
            if (a_Data != null)
            {
                foreach (var item in a_Data)
                {
                    l_content.Add(item);
                }
            }

            Crc16Base crc_CCITT_FALSE = new Crc16Base(0x1021, 0xffff, 0x0000, false, false, 0x29B1);
            byte[] bCRC = crc_CCITT_FALSE.ComputeHash(l_content.ToArray());

            l_content.Add(bCRC[0]);
            l_content.Add(bCRC[1]);
            byte[] a_FramedContent = AddFrame(l_content.ToArray());

            List<byte> l_outMessage = new List<byte>();
            l_outMessage.Add(0XC0);
            foreach (var item in a_FramedContent)
            {
                l_outMessage.Add(item);
            }
            l_outMessage.Add(0XC0);
            byte[] a_outMessage = l_outMessage.ToArray();

            return a_outMessage;
        }

        public static (bool, byte, byte, byte, byte[], byte[], byte[]) FindSLIPframe(byte[] a_Buffer)
        {
            int iFrameEnd = -1;
            int iFrameStart = -1;

            byte bSrcAddr = 0x00, bDestAddr = 0x00, bCmnd;
            byte[] a_Data;
            byte[] bCRCrec;
            byte[] bCRCexp;

            bool start = false;
            bool end = false;

            List<byte> l_FramedContent = new List<byte>();
            int i = 0;
            foreach (var item in a_Buffer)
            {
                if (item == 0xC0)
                {
                    if (start == false)
                    {
                        iFrameStart = i;
                        start = true;
                    }
                    else
                    {
                        iFrameEnd = i;
                        end = true;
                        break;
                    }
                }
                else if ((start == true) && (end == false))
                {
                    l_FramedContent.Add(item);
                }
                i++;
            }

            if (start == false)
            {
                return (false, 0x00, 0x00, 0x00, null, null, null);
            }
            else if (end == false)
            {
                return (false, 0x00, 0x00, 0x00, null, null, null);
            }
            else
            {
                byte[] a_DeframedMessage = RemoveFrame(l_FramedContent.ToArray());
                Crc16Base crc_CCITT_FALSE1 = new Crc16Base(0x1021, 0xffff, 0x0000, false, false, 0x29B1);
                List<byte> lst_content = new List<byte>();

                bSrcAddr = a_DeframedMessage[0];
                bDestAddr = a_DeframedMessage[1];
                bCmnd = a_DeframedMessage[2];
                a_Data = a_DeframedMessage.Skip(3).Take(a_DeframedMessage.Length - 5).ToArray();

                lst_content.Add(bSrcAddr);
                lst_content.Add(bDestAddr);
                lst_content.Add(bCmnd);
                if (a_Data != null)
                {
                    foreach (var item in a_Data)
                    {
                        lst_content.Add(item);
                    }
                }
                bCRCexp = crc_CCITT_FALSE1.ComputeHash(lst_content.ToArray());
                bCRCrec = new byte[2];
                bCRCrec[1] = a_DeframedMessage[a_DeframedMessage.Length - 1];
                bCRCrec[0] = a_DeframedMessage[a_DeframedMessage.Length - 2];

                return  (true, bSrcAddr, bDestAddr, bCmnd, a_Data, bCRCexp, bCRCrec); ;
            }
        }

        public class Crc16Base : CrcBase
        {
            public Crc16Base(ushort polynomial, ushort initialValue, ushort finalXorValue, bool reflectInput, bool reflectOutput, ushort? check = null)
                : base(16, polynomial, initialValue, finalXorValue, reflectInput, reflectOutput, check)
            {
            }
        }

        public static ushort crc16CCITT(byte[] bytes)
        {
            const ushort poly = 4129;
            ushort[] table = new ushort[256];
            ushort initialValue = 0xffff;
            ushort temp, a;
            ushort crc = initialValue;
            for (int i = 0; i < table.Length; ++i)
            {
                temp = 0;
                a = (ushort)(i << 8);
                for (int j = 0; j < 8; ++j)
                {
                    if (((temp ^ a) & 0x8000) != 0)
                        temp = (ushort)((temp << 1) ^ poly);
                    else
                        temp <<= 1;
                    a <<= 1;
                }
                table[i] = temp;
            }
            for (int i = 0; i < bytes.Length; ++i)
            {
                crc = (ushort)((crc << 8) ^ table[((crc >> 8) ^ (0xff & bytes[i]))]);
            }
            return crc;
        }

        public static void crcTest()
        {
            Crc16Base crc_CCITT_FALSE1 = new Crc16Base(0x1021, 0xffff, 0x0000, false, false, 0x29B1);
            byte[] test1 = new byte[] {  0x11, 0x07, 0xA0, 0x20, 0x01, 0x00, 0x09, 0x1B };
            Console.WriteLine("DATA: " + BitConverter.ToString(test1));
            byte[] crc1Lourens = BitConverter.GetBytes(crc16CCITT(test1));
            Console.WriteLine("Lourens crc: " + BitConverter.ToString(crc1Lourens));
            byte[] crc1Library = crc_CCITT_FALSE1.ComputeHash(test1);
            Console.WriteLine("Library crc: " + BitConverter.ToString(crc1Library));

            Crc16Base crc_CCITT_FALSE2 = new Crc16Base(0x1021, 0xffff, 0x0000, false, false, 0x29B1);
            byte[] test2 = new byte[] { 0x01, 0x11, 0xA2, 0x01 };
            Console.WriteLine("DATA: " + BitConverter.ToString(test2));
            byte[] crc2Lourens = BitConverter.GetBytes(crc16CCITT(test2));
            Console.WriteLine("Lourens crc: " + BitConverter.ToString(crc2Lourens));
            byte[] crc2Library = crc_CCITT_FALSE2.ComputeHash(test2);
            Console.WriteLine("Library crc: " + BitConverter.ToString(crc2Library));

            Crc16Base crc_CCITT_FALSE3 = new Crc16Base(0x1021, 0xffff, 0x0000, false, false, 0x29B1);
            byte[] test3 = new byte[] { 0x07, 0x11, 0xA0 };
            Console.WriteLine("DATA: " + BitConverter.ToString(test3));
            byte[] crc3Lourens = BitConverter.GetBytes(crc16CCITT(test3));
            Console.WriteLine("Lourens crc: " + BitConverter.ToString(crc3Lourens));
            byte[] crc3Library = crc_CCITT_FALSE3.ComputeHash(test3);
            Console.WriteLine("Library crc: " + BitConverter.ToString(crc3Library));

            Crc16Base crc_CCITT_FALSE4 = new Crc16Base(0x1021, 0xffff, 0x0000, false, false, 0x29B1);
            byte[] test4 = new byte[] { 0xA0, 0x11, 0x01 };
            Console.WriteLine("DATA: " + BitConverter.ToString(test4));
            byte[] crc4Lourens = BitConverter.GetBytes(crc16CCITT(test4));
            Console.WriteLine("Lourens crc: " + BitConverter.ToString(crc4Lourens));
            byte[] crc4Library = crc_CCITT_FALSE4.ComputeHash(test4);
            Console.WriteLine("Library crc: " + BitConverter.ToString(crc4Library));
        }
    }

}
