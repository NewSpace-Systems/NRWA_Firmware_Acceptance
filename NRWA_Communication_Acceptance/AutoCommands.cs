using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
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



        public static List<byte[]> l_AppTelBlock = new List<byte[]>();

        public static Root NRWAvarObjects;

        // Dictionary for Datafield formatting of different addresses
        // returns [places before comma, decimal places] of a 32 bit format
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
            { 0xA0, new int[2] {32,0} },
            { 0xA1, new int[2] {30,2} },
            { 0xA2, new int[2] {32,0} },
            { 0xA3, new int[2] {32,0} },
            { 0xA4, new int[2] {32,0} },
            { 0xA5, new int[2] {32,0} },
            { 0xA6, new int[2] {32,0} },
            { 0xA7, new int[2] {32,0} },
            { 0xA8, new int[2] {32,0} },
            { 0xA9, new int[2] {32,0} },
            { 0xAA, new int[2] {32,0} },
            { 0xAB, new int[2] {32,0} },
            { 0xAC, new int[2] {32,0} },
            { 0xAD, new int[2] {32,0} },
            { 0xAE, new int[2] {32,0} },
            { 0xAF, new int[2] {32,0} },
            { 0xB0, new int[2] {32,0} },
            { 0xB1, new int[2] {32,0} },
            { 0xB2, new int[2] {32,0} },
            { 0xB3, new int[2] {32,0} },
            { 0xB4, new int[2] {32,0} },
            { 0xB5, new int[2] {32,0} },
            { 0xB6, new int[2] {32,0} },
            { 0xB7, new int[2] {32,0} },
            { 0xB8, new int[2] {32,0} },

        };

        // Dictionary for Datafield initial value of different addresses
        // returns 4 byte array
        private static readonly Dictionary<byte, byte[]> AddressInitial = new Dictionary<byte, byte[]>
        {
            { 0x04, new byte[4] {0x00, 0x00, 0x9C, 0x40 } },
            { 0x05, new byte[4] {0x00, 0x00, 0x5D, 0xC0 } },
            { 0x06, new byte[4] {0x01, 0x77, 0x00, 0x00 } },
            { 0x07, new byte[4] {0x00, 0x1E, 0x00, 0x00 } },
            { 0x08, new byte[4] {0x01, 0xDE, 0x00, 0x00 } },
            { 0x09, new byte[4] {0x00, 0x00, 0x07, 0x79 } },
            { 0x0A, new byte[4] {0x01, 0xDE, 0x00, 0x00 } },
            { 0x0B, new byte[4] {0x00, 0x00, 0x07, 0x79 } },
            { 0x0C, new byte[4] {0x01, 0xDE, 0x00, 0x00 } },
            { 0x0D, new byte[4] {0x00, 0x00, 0x07, 0x79 } },
            { 0x0E, new byte[4] {0x00, 0x00, 0x0F, 0x54 } },
            { 0x0F, new byte[4] {0x00, 0x00, 0x00, 0x67 } },
            { 0x10, new byte[4] {0x03, 0xFF, 0xFF, 0xFF } },
            { 0x11, new byte[4] {0x00, 0x00, 0x04, 0x00 } },
            { 0x12, new byte[4] {0x00, 0x00, 0x00, 0x01 } },
            { 0x13, new byte[4] {0x00, 0x5D, 0xC0, 0x00 } },
            { 0x14, new byte[4] {0x00, 0x03, 0x20, 0x00 } },
            { 0x15, new byte[4] {0x00, 0x06, 0x40, 0x00 } },
            { 0x16, new byte[4] {0x00, 0x0F, 0xA0, 0x00 } },
            { 0x17, new byte[4] {0x00, 0x1F, 0x40, 0x00 } },
            { 0x18, new byte[4] {0x00, 0x00, 0x01, 0xA4 } },
            { 0x19, new byte[4] {0x00, 0x00, 0x01, 0x2C } },
            { 0x1A, new byte[4] {0x00, 0x00, 0x02, 0x80 } },
            { 0x1B, new byte[4] {0x00, 0x00, 0x04, 0x00 } },
            { 0x1C, new byte[4] {0x00, 0x00, 0x00, 0x01 } },
            { 0x1D, new byte[4] {0x00, 0x00, 0x04, 0x00 } },
            { 0x1E, new byte[4] {0x00, 0x00, 0x04, 0x00 } },
            { 0x1F, new byte[4] {0x00, 0x00, 0x04, 0x00 } },
            { 0x20, new byte[4] {0x00, 0x00, 0x00, 0x08 } },
            { 0x21, new byte[4] {0x00, 0x00, 0xFF, 0xFF } },
            { 0x22, new byte[4] {0x00, 0x1F, 0x00, 0x00 } },
            { 0x23, new byte[4] {0x00, 0x00, 0x00, 0x01 } },
            { 0x24, new byte[4] {0x00, 0x00, 0x02, 0x98 } },
            { 0x25, new byte[4] {0x00, 0x00, 0x06, 0xFE } },
            { 0x26, new byte[4] {0x00, 0x00, 0x0A, 0x03 } },
            { 0x27, new byte[4] {0x00, 0x00, 0x06, 0xFE } },
            { 0x28, new byte[4] {0x00, 0x00, 0x01, 0xCC } },
            { 0x29, new byte[4] {0x00, 0x00, 0x0A, 0x50 } },
            { 0x2A, new byte[4] {0x00, 0x00, 0x00, 0x9C } },
            { 0x2B, new byte[4] {0x00, 0x00, 0x14, 0xA0 } },
            { 0x2C, new byte[4] {0x00, 0x00, 0x00, 0x35 } },
            { 0x2D, new byte[4] {0x00, 0x00, 0x00, 0x50 } },
            { 0x2E, new byte[4] {0x00, 0x00, 0x02, 0x45 } },
            { 0x2F, new byte[4] {0x00, 0x00, 0x00, 0x00 } },
            { 0x30, new byte[4] {0xFF, 0xF5, 0x1A, 0x42 } },
            { 0x31, new byte[4] {0x00, 0x00, 0x00, 0x00 } },
            { 0xA0, new byte[4] {0x00, 0x00, 0x00, 0x00 } },
            { 0xA1, new byte[4] {0x00, 0x00, 0x00, 0x00 } },
            { 0xA2, new byte[4] {0x00, 0x00, 0x00, 0x00 } },
            { 0xA3, new byte[4] {0x00, 0x00, 0x00, 0x00 } },
            { 0xA4, new byte[4] {0x00, 0x00, 0x00, 0x00 } },
            { 0xA5, new byte[4] {0x00, 0x00, 0x00, 0x00 } },
            { 0xA6, new byte[4] {0x00, 0x00, 0x00, 0x00 } },
            { 0xA7, new byte[4] {0x00, 0x00, 0x00, 0x00 } },
            { 0xA8, new byte[4] {0x00, 0x00, 0x00, 0x00 } },
            { 0xA9, new byte[4] {0x00, 0x00, 0x00, 0x00 } },
            { 0xAA, new byte[4] {0x00, 0x00, 0x00, 0x00 } },
            { 0xAB, new byte[4] {0x00, 0x00, 0x00, 0x00 } },
            { 0xAC, new byte[4] {0x00, 0x00, 0x00, 0x00 } },
            { 0xAD, new byte[4] {0x00, 0x00, 0x00, 0x00 } },
            { 0xAE, new byte[4] {0x00, 0x00, 0x00, 0x00 } },
            { 0xAF, new byte[4] {0x00, 0x00, 0x00, 0x00 } },
            { 0xB0, new byte[4] {0x00, 0x00, 0x00, 0x00 } },
            { 0xB1, new byte[4] {0x00, 0x00, 0x00, 0x00 } },
            { 0xB2, new byte[4] {0x00, 0x00, 0x00, 0x00 } },
            { 0xB3, new byte[4] {0x00, 0x00, 0x00, 0x00 } },
            { 0xB4, new byte[4] {0x00, 0x00, 0x00, 0x00 } },
            { 0xB5, new byte[4] {0x00, 0x00, 0x00, 0x00 } },
            { 0xB6, new byte[4] {0x00, 0x00, 0x00, 0x00 } },
            { 0xB7, new byte[4] {0x00, 0x00, 0x00, 0x00 } },
            { 0xB8, new byte[4] {0x00, 0x00, 0x00, 0x00 } },

        };

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        public class Datum
        {
            public string field { get; set; }
            public int lenght { get; set; }
            public bool sign { get; set; }
            public List<int> format { get; set; }
            public string initial { get; set; }
        }

        public class NRWAConfigDRV110
        {
            public List<NRWAMemory> NRWA_Memory { get; set; }
            public List<NRWATelemetry> NRWA_Telemetry { get; set; }
        }

        public class NRWAMemory
        {
            public string name { get; set; }
            public string address { get; set; }
            public List<int> format { get; set; }
            public bool sign { get; set; }
            public string initial { get; set; }
        }

        public class NRWATelemetry
        {
            public string Designation { get; set; }
            public int lenght { get; set; }
            public List<Datum> Data { get; set; }
        }

        public class Root
        {
            public List<NRWAConfigDRV110> NRWA_Config_DRV110 { get; set; }
        }


        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        // Returns randomized 32 bit value based on range sent 
        // iVal: number of bytes before comma
        // iDec: number of bytes after comma
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

        // This method reads the address value, increments it by 1%
        // Writes that new value to the address and reads to ensure it has been updated 
        // result true: updated correctly
        // reult false: not updated correctly 
        public static List<List<string>> PokePeek()
        {
            List<List<string>> pokevspeek = new List<List<string>>();
            byte[] bValue = new byte[4];
            double Value;
            bool bPass = false;
            string sTXpoke, sRXpeek, sCommand, sData;


            for (int i = 0; i < NRWA_FirmVer.l_NRWACommands.Count; i++)
            {
                if (NRWA_FirmVer.l_NRWACommands[i].bCommand == 0x82)
                {
                    pokevspeek.Add(new List<string>());
                    sTXpoke = "";
                    sRXpeek = "";
                    sData = "";
                    bPass = false;
                    sCommand = "";
                    try
                    {
                        byte[] b_ValueRX = new byte[1];
                        b_ValueRX[0] = NRWA_FirmVer.l_NRWACommands[i].bAddress;
                        sCommand = NRWA_FirmVer.l_NRWACommands[i].bAddress.ToString("X2");
                        (bool bFoundPeek, bool bAckPeek, string sRFeedbackPeek, byte[] a_TXPeek, byte[] a_RXPeek, byte[] DataPeek, byte[] bRecCRCPeek) = NRWA_Cmnds.cmnd_Peek(NRWA_FirmVer._serialPort, 0x07, 0x11, b_ValueRX);
                        Value = Get32BitValueAddress(b_ValueRX[0], DataPeek);
                        Value = Value * 1.1;
                        sData = Value.ToString();
                        bValue = BitConverter.GetBytes((int)Value);
                        Array.Reverse(bValue);
                        byte[] b_ValueTX = new byte[5];
                        b_ValueTX[0] = NRWA_FirmVer.l_NRWACommands[i].bAddress;
                        b_ValueTX[1] = bValue[0];
                        b_ValueTX[2] = bValue[1];
                        b_ValueTX[3] = bValue[2];
                        b_ValueTX[4] = bValue[3];


                        (bool bFoundPoke, bool bAckPoke, string sRFeedbackPoke, byte[] a_TXPoke, byte[] a_RXPoke, byte[] DataPoke, byte[] bRecCRCPoke) = NRWA_Cmnds.cmnd_Poke(NRWA_FirmVer._serialPort, 0x07, 0x11, b_ValueTX);
                        try { sTXpoke = BitConverter.ToString(a_TXPoke); }
                        catch { sTXpoke = "";  throw new Exception(); };
                        (bFoundPeek, bAckPeek, sRFeedbackPeek, a_TXPeek, a_RXPeek, DataPeek, bRecCRCPeek) = NRWA_Cmnds.cmnd_Peek(NRWA_FirmVer._serialPort, 0x07, 0x11, b_ValueRX);
                        try { sRXpeek = BitConverter.ToString(RX_Verification.removeEndingZeros(a_RXPeek)); }
                        catch { sRXpeek = ""; throw new Exception(); }
                        
                        if (bValue.SequenceEqual(DataPeek))
                        {
                            bPass = true;
                        }
                        else
                        {
                            bPass = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        bPass = false;
                    }

                    pokevspeek[pokevspeek.Count - 1].Add(sTXpoke);
                    pokevspeek[pokevspeek.Count - 1].Add(sRXpeek);
                    pokevspeek[pokevspeek.Count - 1].Add(sData.Replace(',', '.'));
                    pokevspeek[pokevspeek.Count - 1].Add(bPass.ToString());
                    pokevspeek[pokevspeek.Count - 1].Add(sCommand);
                }
            }

            return pokevspeek;
        }

        // This method reads the data field 
        // Converts it to the specified data format
        // result true: converted successfully
        // result false: not converted successfully 
        public static List<List<string>> SysTel()
        {
            List<List<string>> systel = new List<List<string>>();
            double Value;
            byte[] bValue = new byte[4];
            bool bPass = false;
            string sTXsystel, sRXsystel, sCommand, sData;
            try
            {
                for (int i = 0; i < NRWA_FirmVer.l_NRWACommands.Count; i++)
                {
                    if (NRWA_FirmVer.l_NRWACommands[i].bCommand == 0x84)
                    {
                        systel.Add(new List<string>());
                        sTXsystel = "";
                        sRXsystel = "";
                        sData = "";
                        bPass = false;
                        sCommand = "";

                        try
                        {
                            byte[] b_ValueRX = new byte[1];
                            b_ValueRX[0] = NRWA_FirmVer.l_NRWACommands[i].bAddress;
                            sCommand = NRWA_FirmVer.l_NRWACommands[i].bAddress.ToString("X2");
                            (bool bFound, bool bAck, string sRFeedback, byte[] a_TX, byte[] a_RX, byte[] Data, byte[] bRecCRC) = NRWA_Cmnds.cmnd_SysTel(NRWA_FirmVer._serialPort, 0x07, 0x11, b_ValueRX);
                            try { sTXsystel = BitConverter.ToString(a_TX); }
                            catch { sTXsystel = ""; throw new Exception(); }
                            try { sRXsystel = BitConverter.ToString(RX_Verification.removeEndingZeros(a_RX)); }
                            catch { sRXsystel = ""; throw new Exception(); }
                            
                            if (bAck)
                            {
                                bPass = false;
                            }
                            else
                            {
                                bPass = true;
                            }
                            
                        } catch (Exception ex)
                        {
                            bPass = false;
                        }
                        systel[systel.Count - 1].Add(sTXsystel);
                        systel[systel.Count - 1].Add(sRXsystel);
                        systel[systel.Count - 1].Add(sData.Replace(',', '.'));
                        systel[systel.Count - 1].Add(bPass.ToString());
                        systel[systel.Count - 1].Add(sCommand);
                    }

                }
            }
            catch (Exception ex)
            {

            }

            return systel;
        }

        // This method reads the data field 
        // Checkes that it contains the correct amount of data
        // result true: correct amount
        // result false: too little or too much
        public static List<List<string>> AppTelCom()
        {
            List<List<string>> systelcom = new List<List<string>>();
            bool bPass = false;
            string sTXsystel, sRXsystel, sCommand, sData;
            double Value;
            try
            {
                for (int i = 0; i < 3; i++)
                {
                   
                    systelcom.Add(new List<string>());
                    sTXsystel = "";
                    sRXsystel = "";
                    sData = "";
                    bPass = false;
                    sCommand = "";

                    byte[] b_ValueRX = new byte[1];
                    b_ValueRX[0] = (byte)i;
                    (bool bFound, bool bAck, string sRFeedback, byte[] a_TX, byte[] a_RX, byte[] Data, byte[] bRecCRC) = NRWA_Cmnds.cmnd_AppTel(NRWA_FirmVer._serialPort, 0x07, 0x11, b_ValueRX);
                    
                    if (bFound)
                    {
                        if (Data.Length != NRWAvarObjects.NRWA_Config_DRV110[1].NRWA_Telemetry[i].lenght)
                        {
                            bPass = false;
                        }
                        else
                        {
                            l_AppTelBlock.Add(Data);
                            sData = BitConverter.ToString(Data);
                            bPass = true;
                        }
                    }
                     
                    try { sTXsystel = BitConverter.ToString(a_TX); }
                    catch { sTXsystel = ""; throw new Exception(); }
                    try { sRXsystel = BitConverter.ToString(RX_Verification.removeEndingZeros(a_RX)); }
                    catch { sRXsystel = ""; throw new Exception(); }
                    try { sData = BitConverter.ToString(Data); }
                    catch { sData = ""; throw new Exception(); }

                    systelcom[systelcom.Count - 1].Add(sTXsystel);
                    systelcom[systelcom.Count - 1].Add(sRXsystel);
                    systelcom[systelcom.Count - 1].Add(sData.Replace(',', '.'));
                    systelcom[systelcom.Count - 1].Add(bPass.ToString());
                    systelcom[systelcom.Count - 1].Add(i.ToString("X2"));
                    
                }
            }
            catch (Exception ex)
            {

            }
            return systelcom;
        }

        // This method atempts to read address 0A0 - 0xFF
        // Returns true: received NACK
        // Returns false: received ACK
        public static List<List<string>> PeekOutsideAddressRanges()
        {
            List<List<string>> OutsideDataRanges = new List<List<string>>();
            byte[] bValue = new byte[4];
            double Value;
            bool bPass = false;
            string sTX, sRX, sCommand, sData;

            // Peek outside address range 
            // Expect NACK
            for (int i = 160; i < 256; i++)
            {
                OutsideDataRanges.Add(new List<string>());
                sTX = "";
                sRX = "";
                sCommand = i.ToString("X2") ;
                sData = "";
                byte[] b_ValueRX = new byte[1];
                b_ValueRX[0] = (byte)i;
                (bool bFoundPeek, bool bAckPeek, string sRFeedbackPeek, byte[] a_TXPeek, byte[] a_RXPeek, byte[] DataPeek, byte[] bRecCRCPeek) = NRWA_Cmnds.cmnd_Peek(NRWA_FirmVer._serialPort, 0x07, 0x11, b_ValueRX);
                try { sTX = BitConverter.ToString(a_TXPeek); }
                catch { sTX = "";}
                try { sRX = BitConverter.ToString(RX_Verification.removeEndingZeros(a_RXPeek)); }
                catch { sRX = "";}
                try { sData = BitConverter.ToString(DataPeek); }
                catch { sData = ""; }
                if (bAckPeek == false)
                {
                    bPass = false;
                }
                else
                {
                    bPass = true;
                }
                OutsideDataRanges[OutsideDataRanges.Count - 1].Add(sTX);
                OutsideDataRanges[OutsideDataRanges.Count - 1].Add(sRX);
                OutsideDataRanges[OutsideDataRanges.Count - 1].Add(sData.Replace(',', '.'));
                OutsideDataRanges[OutsideDataRanges.Count - 1].Add(bPass.ToString());
                OutsideDataRanges[OutsideDataRanges.Count - 1].Add(sCommand);
            }

            return OutsideDataRanges;
        }

        // This method atempts to write to address 0A0 - 0xFF
        // Returns true: received NACK
        // Returns false: received ACK
        public static List<List<string>> PokeOutsideAddressRanges()
        {
            List<List<string>> OutsideDataRanges = new List<List<string>>();
            bool bPass = false;
            string sTX, sRX, sCommand, sData;

            byte[] b_ValueTX = new byte[5] { 0x00, 0x00, 0x00, 0x00, 0x01 };

            // Poke outside address range 
            // Expect NACK
            for (int i = 160; i < 256; i++)
            {
                OutsideDataRanges.Add(new List<string>());
                sTX = "";
                sRX = "";
                sCommand = i.ToString("X2");
                sData = "";
                b_ValueTX[0] = (byte)i;

                (bool bFoundPoke, bool bAckPoke, string sRFeedbackPoke, byte[] a_TXPoke, byte[] a_RXPoke, byte[] DataPoke, byte[] bRecCRCPoke) = NRWA_Cmnds.cmnd_Poke(NRWA_FirmVer._serialPort, 0x07, 0x11, b_ValueTX);
                try { sTX = BitConverter.ToString(a_TXPoke); }
                catch { sTX = ""; }
                try { sRX = BitConverter.ToString(RX_Verification.removeEndingZeros(a_RXPoke)); }
                catch { sRX = ""; }
                try { sData = BitConverter.ToString(DataPoke); }
                catch { sData = ""; }
                if (bAckPoke == false)
                {
                    bPass = true;
                }
                else
                {
                    bPass = false;
                }
                OutsideDataRanges[OutsideDataRanges.Count - 1].Add(sTX);
                OutsideDataRanges[OutsideDataRanges.Count - 1].Add(sRX);
                OutsideDataRanges[OutsideDataRanges.Count - 1].Add(sData.Replace(',', '.'));
                OutsideDataRanges[OutsideDataRanges.Count - 1].Add(bPass.ToString());
                OutsideDataRanges[OutsideDataRanges.Count - 1].Add(sCommand);
            }
            return OutsideDataRanges;
        }

        // This method atempts to write to the address fields with both a datafield that is too large and too small
        // Returns true: received NACK
        // Returns false: received ACK
        public static List<List<string>> PokeOutsideDataRange()
        {
            List<List<string>> OutsideDataRanges = new List<List<string>>();
            bool bPass = false;
            string sTX, sRX, sCommand, sData;

            byte[] b_ValueTX = new byte[5] { 0x00, 0x00, 0x00, 0x00, 0x01 };

            // Poke outside address range 
            // Expect NACK
            for (int i = 0; i < 45; i++)
            {
                OutsideDataRanges.Add(new List<string>());
                sTX = "";
                sRX = "";
                sCommand = i.ToString("X2");
                sData = "";
                b_ValueTX = new byte[4] { 0x00, 0x00, 0x00, 0x01 };
                b_ValueTX[0] = (byte)i;

                (bool bFoundPoke, bool bAckPoke, string sRFeedbackPoke, byte[] a_TXPoke, byte[] a_RXPoke, byte[] DataPoke, byte[] bRecCRCPoke) = NRWA_Cmnds.cmnd_Poke(NRWA_FirmVer._serialPort, 0x07, 0x11, b_ValueTX);
                try { sTX = BitConverter.ToString(a_TXPoke); }
                catch { sTX = ""; }
                try { sRX = BitConverter.ToString(RX_Verification.removeEndingZeros(a_RXPoke)); }
                catch { sRX = ""; }
                try { sData = BitConverter.ToString(DataPoke); }
                catch { sData = ""; }
                if (bAckPoke == false)
                {
                    bPass = true;
                }
                else
                {
                    bPass = false;
                }
                OutsideDataRanges[OutsideDataRanges.Count - 1].Add(sTX);
                OutsideDataRanges[OutsideDataRanges.Count - 1].Add(sRX);
                OutsideDataRanges[OutsideDataRanges.Count - 1].Add(sData.Replace(',', '.'));
                OutsideDataRanges[OutsideDataRanges.Count - 1].Add(bPass.ToString());
                OutsideDataRanges[OutsideDataRanges.Count - 1].Add(sCommand);


                OutsideDataRanges.Add(new List<string>());
                sTX = "";
                sRX = "";
                sCommand = i.ToString("X2");
                sData = "";
                b_ValueTX = new byte[6] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
                b_ValueTX[0] = (byte)i;

                (bFoundPoke, bAckPoke, sRFeedbackPoke, a_TXPoke, a_RXPoke, DataPoke, bRecCRCPoke) = NRWA_Cmnds.cmnd_Poke(NRWA_FirmVer._serialPort, 0x07, 0x11, b_ValueTX);
                try { sTX = BitConverter.ToString(a_TXPoke); }
                catch { sTX = ""; }
                try { sRX = BitConverter.ToString(RX_Verification.removeEndingZeros(a_RXPoke)); }
                catch { sRX = ""; }
                try { sData = BitConverter.ToString(DataPoke); }
                catch { sData = ""; }
                if (bAckPoke == false)
                {
                    bPass = true;
                }
                else
                {
                    bPass = false;
                }
                OutsideDataRanges[OutsideDataRanges.Count - 1].Add(sTX);
                OutsideDataRanges[OutsideDataRanges.Count - 1].Add(sRX);
                OutsideDataRanges[OutsideDataRanges.Count - 1].Add(sData.Replace(',', '.'));
                OutsideDataRanges[OutsideDataRanges.Count - 1].Add(bPass.ToString());
                OutsideDataRanges[OutsideDataRanges.Count - 1].Add(sCommand);

            }
            return OutsideDataRanges;
        }

        public static List<List<string>> FalseCRC()
        {
            List<List<string>> FalseCRC = new List<List<string>>();
            bool bPass = false;
            string sTX, sRX, sCommand, sData;

            for (int i = 0; i < NRWA_FirmVer.l_NRWACommands.Count; i++)
            {
                FalseCRC.Add(new List<string>());
                sTX = "";
                sRX = "";
                sCommand = NRWA_FirmVer.l_NRWACommands[i].sCommand;
                sData = "";
                try
                {
                    byte[] b_TX = NRWA_FirmVer.l_NRWACommands[i].TX_correct;
                    byte[] temp = b_TX;
                    if (temp[temp.Length-2] != 0xAD)
                    { temp[temp.Length - 2] = 0xAD; }
                    else { temp[temp.Length - 2] = 0xAF; }
                    NRWA_FirmVer.l_NRWACommands[i].TX_wrongCRC = temp;
                    sTX = BitConverter.ToString(NRWA_FirmVer.l_NRWACommands[i].TX_wrongCRC);
                    byte[] b_RX = NRWA_Cmnds.cmnd_Gen(NRWA_FirmVer._serialPort, NRWA_FirmVer.l_NRWACommands[i].TX_wrongCRC);
                    try 
                    { 
                        sRX = BitConverter.ToString(RX_Verification.removeEndingZeros(b_RX));
                        (bool bFindSLIP, byte bSrcAddr, byte bDstAddr, byte bCmnd, byte[] Data, byte[] bRecCRC, byte[] bChkCRC) = SLIP_Frame.FindSLIPframe(b_RX);
                        if (PortCommunication.CheckACK(bCmnd))
                        {
                            bPass = false;
                        }
                        else
                        {
                            bPass = true;
                        }
                    }
                    catch (Exception)
                    { 
                        if (b_RX == null)
                        {
                            sRX = "";
                            bPass = true;
                        }                        
                    }
                    
                }
                catch (Exception ex)
                {
                    bPass = false;
                }

                FalseCRC[FalseCRC.Count - 1].Add(sTX);
                FalseCRC[FalseCRC.Count - 1].Add(sRX);
                FalseCRC[FalseCRC.Count - 1].Add(sData.Replace(',', '.'));
                FalseCRC[FalseCRC.Count - 1].Add(bPass.ToString());
                FalseCRC[FalseCRC.Count - 1].Add(sCommand);


            }

            return FalseCRC;
        }

        public static List<List<string>> PokeWithCorruptData()
        {
            List<List<string>> OutsideDataRanges = new List<List<string>>();
            bool bPass = false;
            string sTX, sRX, sCommand, sData;

            string author = "test";
            // Convert a C# string to a byte array  
            byte[] bytes = Encoding.ASCII.GetBytes(author);

            byte[] b_ValueTX = new byte[5] { 0x00, 0x00, 0x00, 0x00, 0x01 };

            // Poke outside address range 
            // Expect NACK
            for (int i = 0; i < 45; i++)
            {
                OutsideDataRanges.Add(new List<string>());
                sTX = "";
                sRX = "";
                sCommand = i.ToString("X2");
                sData = "";

                b_ValueTX = new byte[5];
                b_ValueTX[0] = (byte)i;
                b_ValueTX[1] = bytes[0];
                b_ValueTX[2] = bytes[1];
                b_ValueTX[3] = bytes[2];
                b_ValueTX[4] = bytes[3];

                (bool bFoundPoke, bool bAckPoke, string sRFeedbackPoke, byte[] a_TXPoke, byte[] a_RXPoke, byte[] DataPoke, byte[] bRecCRCPoke) = NRWA_Cmnds.cmnd_Poke(NRWA_FirmVer._serialPort, 0x07, 0x11, b_ValueTX);
                try { sTX = BitConverter.ToString(a_TXPoke); }
                catch { sTX = ""; }
                try { sRX = BitConverter.ToString(RX_Verification.removeEndingZeros(a_RXPoke)); }
                catch { sRX = ""; }
                try { sData = BitConverter.ToString(DataPoke); }
                catch { sData = ""; }
                if (bAckPoke == false)
                {
                    bPass = true;
                }
                else
                {
                    bPass = false;
                }
                OutsideDataRanges[OutsideDataRanges.Count - 1].Add(sTX);
                OutsideDataRanges[OutsideDataRanges.Count - 1].Add(sRX);
                OutsideDataRanges[OutsideDataRanges.Count - 1].Add(sData.Replace(',', '.'));
                OutsideDataRanges[OutsideDataRanges.Count - 1].Add(bPass.ToString());
                OutsideDataRanges[OutsideDataRanges.Count - 1].Add(sCommand);
            }
            return OutsideDataRanges;
        }

        public static List<List<string>> IncrementingVariables()
        {
            List<List<string>> variables = new List<List<string>>();











            return variables;
        }

        public static List<List<string>> PeekInitialValues()
        {
            List<List<string>> peekInitialValues = new List<List<string>>();
            bool bPass = false;
            string sTX, sRX, sCommand, sData;

            string author = "test";
            // Convert a C# string to a byte array  
            byte[] bytes = Encoding.ASCII.GetBytes(author);

            // Poke outside address range 
            // Expect NACK
            for (int i = 4; i < 49; i++)
            {
                byte[] expectedData = AddressInitial[(byte)i];
                peekInitialValues.Add(new List<string>());
                sTX = "";
                sRX = "";
                sCommand = i.ToString("X2");
                sData = "";

                byte[] b_ValueTX = new byte[1];
                b_ValueTX[0] = (byte)i;

                (bool bFoundPeek, bool bAckPeek, string sRFeedbackPeek, byte[] a_TXPeek, byte[] a_RXPeek, byte[] DataPeek, byte[] bRecCRCPeek) = NRWA_Cmnds.cmnd_Peek(NRWA_FirmVer._serialPort, 0x07, 0x11, b_ValueTX);
                try { sTX = BitConverter.ToString(a_TXPeek); }
                catch { sTX = ""; }
                try { sRX = BitConverter.ToString(RX_Verification.removeEndingZeros(a_RXPeek)); }
                catch { sRX = ""; }
                try { sData = BitConverter.ToString(DataPeek); }
                catch { sData = ""; }

                 if (bAckPeek == true && DataPeek.SequenceEqual(expectedData))
                {
                    bPass = true;
                }
                else
                {
                    bPass = false;
                }
                peekInitialValues[peekInitialValues.Count - 1].Add(sTX);
                peekInitialValues[peekInitialValues.Count - 1].Add(sRX);
                peekInitialValues[peekInitialValues.Count - 1].Add( "Expected: " + BitConverter.ToString(expectedData) + " Received: " + sData);
                peekInitialValues[peekInitialValues.Count - 1].Add(bPass.ToString());
                peekInitialValues[peekInitialValues.Count - 1].Add(sCommand);
            }

            return peekInitialValues;
        }

        public static List<List<string>> AddressSpecificCases()
        {
            List<List<string>> addSpecCase = new List<List<string>>();

            string j_field;
            int j_lenght;
            bool signed;
            List<int> j_format;
            string j_initial;
            double Initial;
            double Value;
            byte[] b_value;
            byte[] b_data;
            int k = 0;


            for (int i = 0; i < 3; i++ )
            {
                try
                {
                    if (l_AppTelBlock[i].Length == NRWAvarObjects.NRWA_Config_DRV110[1].NRWA_Telemetry[i].lenght)
                    {
                        int idata = NRWAvarObjects.NRWA_Config_DRV110[1].NRWA_Telemetry[i].Data.Count;
                        int iPos = 0;
                        for (int j = 0; j < idata; j++)
                        {
                            addSpecCase.Add(new List<string>());

                            j_field = NRWAvarObjects.NRWA_Config_DRV110[1].NRWA_Telemetry[i].Data[j].field;
                            j_lenght = NRWAvarObjects.NRWA_Config_DRV110[1].NRWA_Telemetry[i].Data[j].lenght;
                            j_format = NRWAvarObjects.NRWA_Config_DRV110[1].NRWA_Telemetry[i].Data[j].format;
                            j_initial = NRWAvarObjects.NRWA_Config_DRV110[1].NRWA_Telemetry[i].Data[j].initial;
                            signed = NRWAvarObjects.NRWA_Config_DRV110[1].NRWA_Telemetry[i].Data[j].sign;
                            b_data = l_AppTelBlock[i].Skip(iPos).Take(j_lenght).ToArray();
                            b_value = new byte[4] { 0x00, 0x00, 0x00, 0x00 };

                            if (j_lenght == 1)
                            { b_value[3] = b_data[0]; }
                            if (j_lenght == 2)
                            {
                                b_value[3] = b_data[1];
                                b_value[2] = b_data[0];
                            }
                            if (j_lenght == 3)
                            {
                                b_value[3] = b_data[2];
                                b_value[2] = b_data[1];
                                b_value[1] = b_data[0];
                            }
                            if (j_lenght == 4)
                            {
                                b_value[3] = b_data[3];
                                b_value[2] = b_data[2];
                                b_value[1] = b_data[1];
                                b_value[0] = b_data[0];
                            }

                            if (j_format[0] + j_format[1] == 16)
                            {
                                if (signed)
                                {
                                    Value = intToQ((int)(short)BitConverter.ToInt16(b_data, 0), j_format[1]);
                                }
                                else
                                {
                                    Value = intToQ((int)BitConverter.ToUInt16(b_data, 0), j_format[1]);
                                }

                            }
                            else if (j_format[0] + j_format[1] == 32)
                            {
                                if (signed)
                                {
                                    Value = intToQ((int)BitConverter.ToInt32(b_data, 0), j_format[1]);
                                }
                                else
                                {
                                    Value = intToQ((int)BitConverter.ToUInt32(b_data, 0), j_format[1]);
                                }

                            }
                            else if (j_format[0] + j_format[1] == 8)
                            {
                                if (signed)
                                {
                                    Value = (int)b_data[0];
                                }
                                else
                                {
                                    Value = (uint)b_data[0];
                                }

                            }
                            else
                            {
                                Value = -1;
                            }

                            if (j_initial != "")
                            {
                                Initial = Convert.ToDouble(j_initial.Replace('.',','));
                              //  Value = intToQ((int)BitConverter.ToUInt32(b_value, 0), j_format[1]);

                                if (0.95 * Initial <= Value && 1.05 * Initial >= Value)
                                {
                                    addSpecCase[k].Add("APP-TEL " + NRWAvarObjects.NRWA_Config_DRV110[1].NRWA_Telemetry[i].Designation);
                                    addSpecCase[k].Add(NRWAvarObjects.NRWA_Config_DRV110[1].NRWA_Telemetry[i].Data[j].field);
                                    addSpecCase[k].Add("");
                                    addSpecCase[k].Add("");
                                    addSpecCase[k].Add("Received: " + Value.ToString().Replace(',', '.') + " Expected: " + Initial.ToString().Replace(',', '.'));
                                    addSpecCase[k].Add("TRUE");
                                }
                                else
                                {
                                    addSpecCase[k].Add("APP-TEL " + NRWAvarObjects.NRWA_Config_DRV110[1].NRWA_Telemetry[i].Designation);
                                    addSpecCase[k].Add(NRWAvarObjects.NRWA_Config_DRV110[1].NRWA_Telemetry[i].Data[j].field);
                                    addSpecCase[k].Add("");
                                    addSpecCase[k].Add("");
                                    addSpecCase[k].Add("Received: " + Value.ToString().Replace(',', '.') + " Expected: " + Initial.ToString().Replace(',', '.'));
                                    addSpecCase[k].Add("FALSE");
                                }

                            }
                            else
                            {
                                

                               // Value = intToQ((int)BitConverter.ToUInt32(b_value, 0), j_format[1]);
                                addSpecCase[k].Add("APP-TEL " + NRWAvarObjects.NRWA_Config_DRV110[1].NRWA_Telemetry[i].Designation);
                                addSpecCase[k].Add(NRWAvarObjects.NRWA_Config_DRV110[1].NRWA_Telemetry[i].Data[j].field);
                                addSpecCase[k].Add("");
                                addSpecCase[k].Add("");
                                addSpecCase[k].Add("Received: " + Value.ToString().Replace(',', '.') + " Expected: -");
                                addSpecCase[k].Add("TRUE");
                            }
                            k++;
                            iPos = iPos + j_lenght;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }

                
            }

            return addSpecCase;
        }

        public static List<List<string>> EdgeCases()
        {
            List<List<string>> edgeCase = new List<List<string>>();
            byte[] bValue = new byte[4]  { 0x00, 0xdb, 0xdc, 0xc0 };
            byte[] rDataPeek = null;
            bool bPass = false;
            string sTXpoke, sRXpeek, sCommand, sData;

            for (int i = 0; i < NRWA_FirmVer.l_NRWACommands.Count; i++)
            {
                if (NRWA_FirmVer.l_NRWACommands[i].bCommand == 0x82)
                {
                    edgeCase.Add(new List<string>());
                    sTXpoke = "";
                    sRXpeek = "";
                    sData = "";
                    bPass = false;
                    sCommand = "";
                    try
                    {

                        byte[] b_ValueRX = new byte[1];
                        b_ValueRX[0] = NRWA_FirmVer.l_NRWACommands[i].bAddress;

                        byte[] b_ValueTX = new byte[5];
                        b_ValueTX[0] = NRWA_FirmVer.l_NRWACommands[i].bAddress;
                        b_ValueTX[1] = bValue[0];
                        b_ValueTX[2] = bValue[1];
                        b_ValueTX[3] = bValue[2];
                        b_ValueTX[4] = bValue[3];


                        (bool bFoundPoke, bool bAckPoke, string sRFeedbackPoke, byte[] a_TXPoke, byte[] a_RXPoke, byte[] DataPoke, byte[] bRecCRCPoke) = NRWA_Cmnds.cmnd_Poke(NRWA_FirmVer._serialPort, 0x07, 0x11, b_ValueTX);
                        try { sTXpoke = BitConverter.ToString(a_TXPoke); }
                        catch { sTXpoke = ""; throw new Exception(); };
                        (bool bFoundPeek, bool bAckPeek, string sRFeedbackPeek, byte[] a_TXPeek, byte[] a_RXPeek, byte[] DataPeek, byte[] bRecCRCPeek) = NRWA_Cmnds.cmnd_Peek(NRWA_FirmVer._serialPort, 0x07, 0x11, b_ValueRX);
                        try { sRXpeek = BitConverter.ToString(RX_Verification.removeEndingZeros(a_RXPeek)); }
                        catch { sRXpeek = ""; throw new Exception(); }

                        if (bValue.SequenceEqual(DataPeek))
                        {
                            bPass = true;
                            rDataPeek = DataPeek;
                        }
                        else
                        {
                            bPass = false;
                            rDataPeek = DataPeek;
                        }
                    }
                    catch (Exception ex)
                    {
                        bPass = false;
                    }

                    edgeCase[edgeCase.Count - 1].Add(sTXpoke);
                    edgeCase[edgeCase.Count - 1].Add(sRXpeek);
                    edgeCase[edgeCase.Count - 1].Add("Expected: " + BitConverter.ToString(bValue)  + " Received: " + BitConverter.ToString(rDataPeek));
                    edgeCase[edgeCase.Count - 1].Add(bPass.ToString());
                    edgeCase[edgeCase.Count - 1].Add(sCommand);
                }
            }

            return edgeCase;
        }

        public static List<List<string>> NackCrc()
        {
            List<List<string>> crclist = new List<List<string>>();

            double Value;
            byte[] bValue = new byte[4];
            bool bPass = false;
            string sTXcrc, sRXcrc, sCommand, sData;
            try
            {
                //INIT
                crclist.Add(new List<string>());
                sTXcrc = "";
                sRXcrc = "";
                sData = "";
                bPass = false;
                sCommand = "";

                try
                {
                    (bool bFound, bool bAck, string sRFeedback, byte[] a_TX, byte[] a_RX, byte[] Data, byte[] bRecCRC) = NRWA_Cmnds.cmnd_Init(NRWA_FirmVer._serialPort, 0x07, 0x11);
                    try { sTXcrc = BitConverter.ToString(a_TX); }
                    catch { sTXcrc = ""; throw new Exception(); }
                    try { sRXcrc = BitConverter.ToString(RX_Verification.removeEndingZeros(a_RX)); }
                    catch { sRXcrc = ""; throw new Exception(); }

                    if (bAck)
                    {
                        bPass = true;
                    }
                    else
                    {
                        bPass = false;
                    }

                }
                catch (Exception ex)
                {
                    bPass = false;
                }
                crclist[crclist.Count - 1].Add(sTXcrc);
                crclist[crclist.Count - 1].Add(sRXcrc);
                crclist[crclist.Count - 1].Add(sData.Replace(',', '.'));
                crclist[crclist.Count - 1].Add(bPass.ToString());
                crclist[crclist.Count - 1].Add("0x01");

                //05 Reserved
                crclist.Add(new List<string>());
                sTXcrc = "";
                sRXcrc = "";
                sData = "";
                bPass = false;
                sCommand = "";

                try
                {
                    (bool bFound, bool bAck, string sRFeedback, byte[] a_TX, byte[] a_RX, byte[] Data, byte[] bRecCRC) = NRWA_Cmnds.cmnd_Res(NRWA_FirmVer._serialPort, 0x07, 0x11, 0x85);
                    try { sTXcrc = BitConverter.ToString(a_TX); }
                    catch { sTXcrc = ""; throw new Exception(); }
                    try { sRXcrc = BitConverter.ToString(RX_Verification.removeEndingZeros(a_RX)); }
                    catch { sRXcrc = ""; throw new Exception(); }

                    if (bAck)
                    {
                        bPass = true;
                    }
                    else
                    {
                        bPass = false;
                    }

                }
                catch (Exception ex)
                {
                    bPass = false;
                }
                crclist[crclist.Count - 1].Add(sTXcrc);
                crclist[crclist.Count - 1].Add(sRXcrc);
                crclist[crclist.Count - 1].Add(sData.Replace(',', '.'));
                crclist[crclist.Count - 1].Add(bPass.ToString());
                crclist[crclist.Count - 1].Add("0x05");


                //06 Reserved
                crclist.Add(new List<string>());
                sTXcrc = "";
                sRXcrc = "";
                sData = "";
                bPass = false;
                sCommand = "";

                try
                {
                    (bool bFound, bool bAck, string sRFeedback, byte[] a_TX, byte[] a_RX, byte[] Data, byte[] bRecCRC) = NRWA_Cmnds.cmnd_Res(NRWA_FirmVer._serialPort, 0x07, 0x11, 0x86);
                    try { sTXcrc = BitConverter.ToString(a_TX); }
                    catch { sTXcrc = ""; throw new Exception(); }
                    try { sRXcrc = BitConverter.ToString(RX_Verification.removeEndingZeros(a_RX)); }
                    catch { sRXcrc = ""; throw new Exception(); }

                    if (bAck)
                    {
                        bPass = true;
                    }
                    else
                    {
                        bPass = false;
                    }

                }
                catch (Exception ex)
                {
                    bPass = false;
                }
                crclist[crclist.Count - 1].Add(sTXcrc);
                crclist[crclist.Count - 1].Add(sRXcrc);
                crclist[crclist.Count - 1].Add(sData.Replace(',', '.'));
                crclist[crclist.Count - 1].Add(bPass.ToString());
                crclist[crclist.Count - 1].Add("0x06");


            }
            catch (Exception ex)
            {

            }

            try
            {
                for (int i = 0; i < NRWA_FirmVer.l_NRWACommands.Count; i++)
                {
                    if (NRWA_FirmVer.l_NRWACommands[i].bCommand == 0x84)
                    {
                        crclist.Add(new List<string>());
                        sTXcrc = "";
                        sRXcrc = "";
                        sData = "";
                        bPass = false;
                        sCommand = "";

                        try
                        {
                            byte[] b_ValueRX = new byte[1];
                            b_ValueRX[0] = NRWA_FirmVer.l_NRWACommands[i].bAddress;
                            sCommand = NRWA_FirmVer.l_NRWACommands[i].bAddress.ToString("X2");
                            (bool bFound, bool bAck, string sRFeedback, byte[] a_TX, byte[] a_RX, byte[] Data, byte[] bRecCRC) = NRWA_Cmnds.cmnd_SysTel(NRWA_FirmVer._serialPort, 0x07, 0x11, b_ValueRX);
                            try { sTXcrc = BitConverter.ToString(a_TX); }
                            catch { sTXcrc = ""; throw new Exception(); }
                            try { sRXcrc = BitConverter.ToString(RX_Verification.removeEndingZeros(a_RX)); }
                            catch { sRXcrc = ""; throw new Exception(); }

                            if (bAck)
                            {
                                bPass = true;
                            }
                            else
                            {
                                bPass = false;
                            }

                        }
                        catch (Exception ex)
                        {
                            bPass = false;
                        }
                        crclist[crclist.Count - 1].Add(sTXcrc);
                        crclist[crclist.Count - 1].Add(sRXcrc);
                        crclist[crclist.Count - 1].Add(sData.Replace(',', '.'));
                        crclist[crclist.Count - 1].Add(bPass.ToString());
                        crclist[crclist.Count - 1].Add(sCommand);
                    }

                }
            }
            catch (Exception ex)
            {

            }

            return crclist;
        }

        public static List<List<string>> CommandCodeCases()
        {
            List<List<string>> ccc = new List<List<string>>();
            byte[] rDataccc = null;
            bool bPass = false;
            string sTXccc, sRXccc, sCommand, sData;

            for (int i = 0; i < 256; i++)
            {
                
                ccc.Add(new List<string>());
                sTXccc = "";
                sRXccc = "";
                sData = "";
                bPass = false;
                sCommand = i.ToString("X2");
                try
                {
                    (bool bFoundccc, bool bAckccc, string sRFeedbackccc, byte[] a_TXccc, byte[] a_RXccc, byte[] Dataccc, byte[] bRecCRCccc) = NRWA_Cmnds.cmnd_CCC(NRWA_FirmVer._serialPort, 0x07, 0x11, (byte)i);
                    try { sTXccc = BitConverter.ToString(a_TXccc); }
                    catch { sTXccc = ""; throw new Exception(); };
                    try { sRXccc = BitConverter.ToString(RX_Verification.removeEndingZeros(a_RXccc)); }
                    catch { sRXccc = ""; throw new Exception(); }

                    if (bAckccc)
                    {
                        bPass = true;
                    }
                    else
                    {
                        bPass = false;
                    }
                }
                catch (Exception ex)
                {
                    bPass = false;
                }

                ccc[ccc.Count - 1].Add(sTXccc);
                ccc[ccc.Count - 1].Add(sRXccc);
                ccc[ccc.Count - 1].Add("");
                ccc[ccc.Count - 1].Add(bPass.ToString());
                ccc[ccc.Count - 1].Add(sCommand);
                
            }

            return ccc;
        }

        //--------------------------------------------------------------------------------
        public static double Get32BitValueAddress(byte address, byte[] data)
        {
            try
            {
                if (data != null)
                {
                    byte[] Value = data;
                    int[] iBitPlaces = AddressFormatting[address];

                    Array.Reverse(Value);
                    uint uiValue = BitConverter.ToUInt32(Value, 0);
                    int returnValue = (int)uiValue;
                    return intToQ((int)returnValue, iBitPlaces[1]);
                }
                else
                {
                    throw new Exception("no data");
                }
            }
            catch (Exception)
            {

                throw new Exception("Get32BitValueAddress Error");
            }
                
        }

        public static double Get32BitValueSpecified(int iAfter, byte[] data)
        {
            if (data != null)
            {
                byte[] Value = data;

                Array.Reverse(Value);
                uint uiValue = BitConverter.ToUInt32(Value, 0);
                int returnValue = (int)uiValue;
                return intToQ((int)returnValue, iAfter);
            }
            else
            {
                throw new Exception("no data");
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

        public static double intToQ(int signedValue, int fraction_bits)
        {
            var scaling = Math.Pow(2, fraction_bits);
            return (signedValue / scaling);
        }

        public static int QtoInt(double dValue, int fraction_bits)
        {
            var scaling = Math.Pow(2, fraction_bits);
            return (int)Math.Round(dValue * scaling);
        }

    }

    
}
