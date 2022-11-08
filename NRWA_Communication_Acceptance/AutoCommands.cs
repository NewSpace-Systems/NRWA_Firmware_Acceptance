using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace NRWA_Communication_Acceptance
{
    public class AutoCommands
    {
        public string sCommand;
        public byte bCommand;
        public byte bAddress;
        public byte[] TX_correct;
        public byte[] RX_correct;
        public byte[] TX_noFEND;
        public byte[] TX_additionalFEND;
        public byte[] TX_wrongCRC;
        public byte[] TX_addressSwitch;
        public byte[] TX_noPoll;
        public byte[] TX_noBbit;
        public byte[] TX_noAbit;
        public byte[] TX_noData;
        public byte[] TX_tooLittleData;
        public byte[] TX_tooMuchData;
        public List<int[]> l_iFormat = new List<int[]>();

        private static readonly Dictionary<byte, int[]> AddressFormatting = new Dictionary<byte, int[]>
        {
            { 0x00, new int[2] {32,0} },
            { 0x01, new int[2] {32,0} },
            { 0x02, new int[2] {32,0} },
            { 0x03, new int[2] {32,0} },
            { 0x04, new int[2] {32,0} },
            { 0x05, new int[2] {32,0} },
            { 0x06, new int[2] {32,0} },
            { 0x07, new int[2] {20,12} },
            { 0x08, new int[2] {20,12} },
            { 0x09, new int[2] {16,16} },
            { 0x0A, new int[2] {32,0} },
            { 0x0B, new int[2] {16,0} },
            { 0x0C, new int[2] {32,0} },
            { 0x0D, new int[2] {16,0} },
            { 0x0E, new int[2] {32,0} },
            { 0x0F, new int[2] {16,0} },
            { 0x10, new int[2] {16,0} },
            { 0x11, new int[2] {16,0} },
            { 0x12, new int[2] {32,0} },
            { 0x13, new int[2] {16,0} },
            { 0x14, new int[2] {16,0} },
            { 0x15, new int[2] {24,0} },
            { 0x16, new int[2] {14,10} },
            { 0x17, new int[2] {0,16} },
            { 0x18, new int[2] {16,0} },
            { 0x19, new int[2] {16,0} },
            { 0x1A, new int[2] {16,0} },
            { 0x1B, new int[2] {16,0} },
            { 0x1C, new int[2] {16,0} },
            { 0x1D, new int[2] {16,0} },
            { 0x1E, new int[2] {16,0} },
            { 0x1F, new int[2] {16,0} },
            { 0x20, new int[2] {16,0} },
            { 0x21, new int[2] {16,0} },
            { 0x22, new int[2] {16,0} },
            { 0x23, new int[2] {16,0} },
            { 0x24, new int[2] {16,0} },
            { 0x25, new int[2] {16,0} },
            { 0x26, new int[2] {16,0} },
            { 0x27, new int[2] {16,0} },
            { 0x28, new int[2] {16,0} },
            { 0x29, new int[2] {16,0} },
            { 0x2A, new int[2] {16,0} },
            { 0x2B, new int[2] {16,0} },
            { 0x2C, new int[2] {32,0} },
            { 0x2D, new int[2] {16,16} },

        };

        public static byte[] GetRandom32BitValue(int iVal, int iDec)
        {
            byte[] value = new byte[4];
            List<byte> l_Value = new List<byte>();

            try
            {
                //Get maximum values
                int iValMax = (int)Math.Pow((double)2, (double)iVal) - 1;
                int iDecMax = (int)Math.Pow((double)2, (double)iDec) - 1; ;

                //get number of byte[] places
                int iValBytes = iVal / 8;
                int iDecBytes = iDec / 8;
                int iPreZeros = 4 - iValBytes - iDecBytes;

                for (int i = 0; i < iPreZeros; i++)
                {
                    l_Value.Add(0x00);
                }

                if ((iValBytes + iDecBytes) >= 5)
                    throw new Exception("Too many data bytes");

                if (iDec == 0)
                {
                    Random r = new Random();
                    Int32 iValRan = r.Next(iValMax);
                    byte[] a_ValRan = BitConverter.GetBytes(iValRan);
                    Array.Reverse(a_ValRan);
                    return a_ValRan;
                }
                else
                {
                    if (iVal == 0)
                    {
                        Random r = new Random();
                        Int32 iDecRan = r.Next(iDecMax);
                        byte[] a_DecRan = BitConverter.GetBytes(iDecRan);
                        Array.Reverse(a_DecRan);
                        return a_DecRan;
                    }
                    else
                    {
                        Random r = new Random();
                        Int32 iValRan = r.Next(iValMax);
                        byte[] a_ValRan = BitConverter.GetBytes(iValRan);

                        for (int i = (3 - iValBytes); i > -1; i--)
                        {
                            l_Value.Add(a_ValRan[i]);
                        }

                        r = new Random();
                        Int32 iDecRan = r.Next(iDecMax);
                        byte[] a_DecRan = BitConverter.GetBytes(iDecRan);

                        for (int i = (3 - iDecBytes); i > -1; i--)
                        {
                            l_Value.Add(a_DecRan[i]);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            
            return l_Value.ToArray();
        }

        private List<bool> PokePeek()
        {
            List<bool> pokevspeek = new List<bool>();
            byte[] bValue = new byte[4];
            double Value;

            for (int i = 0; i < NRWA_FirmVer.l_NRWACommands.Count; i++)
            {
                if (NRWA_FirmVer.l_NRWACommands[i].bCommand == 0x82)
                {
                    byte[] b_ValueRX = new byte[1];
                    b_ValueRX[0] = NRWA_FirmVer.l_NRWACommands[i].bAddress;
                    (bool bFoundPeek, bool bAckPeek, string sRFeedbackPeek, byte[] a_TXPeek, byte[] a_RXPeek, byte[] DataPeek, byte[] bRecCRCPeek) = NRWA_Cmnds.cmnd_Peek(NRWA_FirmVer._serialPort, 0x07, 0x11, b_ValueRX);
                    bValue[0] = DataPeek[1];
                    bValue[1] = DataPeek[2];
                    bValue[2] = DataPeek[3];
                    bValue[3] = DataPeek[4];
                    Value = Convert.ToDouble(bValue);
                    Value = Value * 1.1;
                    bValue = BitConverter.GetBytes(Value);

                    byte[] b_ValueTX = new byte[5];
                    b_ValueTX[1] = bValue[0];
                    b_ValueTX[2] = bValue[1];
                    b_ValueTX[3] = bValue[2];
                    b_ValueTX[4] = bValue[3];
                    b_ValueTX[0] = NRWA_FirmVer.l_NRWACommands[i].bAddress;
                    
                    (bool bFoundPoke, bool bAckPoke, string sRFeedbackPoke, byte[] a_TXPoke, byte[] a_RXPoke, byte[] DataPoke, byte[] bRecCRCPoke) = NRWA_Cmnds.cmnd_Poke(NRWA_FirmVer._serialPort, 0x07, 0x11, b_ValueTX);
                    (bFoundPeek, bAckPeek, sRFeedbackPeek, a_TXPeek, a_RXPeek, DataPeek, bRecCRCPeek) = NRWA_Cmnds.cmnd_Peek(NRWA_FirmVer._serialPort, 0x07, 0x11, b_ValueRX);

                    if (b_ValueTX == DataPeek)
                    {
                        pokevspeek.Add(true);
                    }
                    else
                    {
                        pokevspeek.Add(false);
                    }


                }
            }







            return pokevspeek;
        }

        public static double Get32BitValue(byte[] data)
        {
            byte address = data[0];
            byte[] Value = new byte[4] { data[1], data[2], data[3], data[4] };
            int[] iBitPlaces = AddressFormatting[address];
            
            if (iBitPlaces[1] == 0)
            { return Convert.ToDouble(Value); }
            else if (iBitPlaces[0] == 0)
            {
                double temp_val = Convert.ToDouble(Value);
                string stemp_val = "0." + Convert.ToString(temp_val);
                return Convert.ToDouble(stemp_val);
                
            }else if (iBitPlaces[0] == 16 && iBitPlaces[1] == 16)
            {
                byte[] Value1 = new byte[2] { Value[0], Value[1] };
                byte[] Value2 = new byte[2] { Value[2], Value[3] };
                string stemp_val = Convert.ToString(Convert.ToDouble(Value1)) + "." + Convert.ToString(Convert.ToDouble(Value2));
                return Convert.ToDouble(stemp_val);
            }else if (iBitPlaces[0] == 14 && iBitPlaces[1] == 10)
            {
                byte[] bdec = new byte[2];
                bdec[1] = Value[3];
                string temp = Value[2].ToString("x");
                temp = temp.Substring(1, 1);
                temp = 0 + temp;
                bdec = StringToByteArrayFastest(temp + Value[3].ToString("x"));

                byte[] bval = new byte[3];
                bval[1] = Value[3];
                temp = Value[2].ToString("x");
                temp = temp.Substring(1, 1);
                temp = 0 + temp;
                bdec = StringToByteArrayFastest(temp + Value[3].ToString("x"));


                return 657;
            }
            
        }

        public static byte[] StringToByteArrayFastest(string hex)
        {
            if (hex.Length % 2 == 1)
                hex = "0" + hex;

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        public static int GetHexVal(char hex)
        {
            int val = (int)hex;
            //For uppercase A-F letters:
            //return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }
    }

    
}
