﻿using System;
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

        public static byte[] AppTelBlock0 = new byte[31];
        public static byte[] AppTelBlock1 = new byte[56];
        public static byte[] AppTelBlock2 = new byte[100];

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
                            Value = Get32BitValueAddress(b_ValueRX[0], Data);
                            sData = Value.ToString();
                            bPass = true;
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
                    switch (i)
                    {
                        case 0:
                            if (Data.Length != 31)
                            { 
                                bPass = false;
                            }
                            else
                            {
                                AppTelBlock0 = Data;
                                sData = BitConverter.ToString(Data);
                                bPass = true;
                            }
                            break;
                        case 1:
                            if (Data.Length != 56)
                            {
                                bPass = false;
                            }
                            else
                            {
                                AppTelBlock1 = Data;
                                sData = BitConverter.ToString(Data);
                                bPass = true;
                            }
                            break;
                        case 2:
                            if (Data.Length != 100)
                            {
                                bPass = false;
                            }
                            else
                            {
                                AppTelBlock2 = Data;
                                sData = BitConverter.ToString(Data);
                                bPass = true;
                            }
                            break;
                        
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
            int i = 0;
            if (AppTelBlock0 != null)
            {
                //Status Register
                byte[] bStatusRegister = AppTelBlock0.Skip(0).Take(4).ToArray();
                BitArray bits_StatusReg = new BitArray(bStatusRegister);
                addSpecCase.Add(new List<string>());
                addSpecCase[i].Add("");//TX
                addSpecCase[i].Add("");//RX


                byte[] bMotorRegister = AppTelBlock0.Skip(4).Take(1).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] bControlSetpoint = AppTelBlock0.Skip(5).Take(2).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] bPWMdutyCycle = AppTelBlock0.Skip(7).Take(2).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] bCurrentControlTarget = AppTelBlock0.Skip(9).Take(2).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] bCurrentMeasurement = AppTelBlock0.Skip(11).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] bSpeedMeasurement = AppTelBlock0.Skip(15).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] bMotorDriverPower = AppTelBlock0.Skip(19).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] bDcDcTemperature = AppTelBlock0.Skip(23).Take(2).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] bFPGATemperature = AppTelBlock0.Skip(25).Take(2).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] bMotorTemperature = AppTelBlock0.Skip(27).Take(2).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] bMotorDriverTemperature = AppTelBlock0.Skip(29).Take(2).ToArray();
                addSpecCase.Add(new List<string>());
            }

            if (AppTelBlock1 != null)
            {
                byte[] bStatusRegister = AppTelBlock1.Skip(0).Take(4).ToArray();
                BitArray bits_StatusReg = new BitArray(bStatusRegister);
                addSpecCase.Add(new List<string>());

                //DC-DC Temperature
                byte[] bDcDcTemperature = AppTelBlock1.Skip(4).Take(2).ToArray();
                addSpecCase.Add(new List<string>());
                addSpecCase[1].Add("APP-TEL-02");
                addSpecCase[1].Add("DC-DC-Temperature");
                Array.Reverse(bDcDcTemperature);
                double temp = intToQ((int)BitConverter.ToUInt32(bDcDcTemperature, 0), 4 );
                addSpecCase[1].Add("Expected: 18-30 Received: " + temp.ToString());
                if (temp <= 30 && temp >= 18)
                { addSpecCase[1].Add("TRUE"); }
                else { addSpecCase[1].Add("FALSE"); }

                //FPGA Temperature
                byte[] bFPGATemperature = AppTelBlock1.Skip(6).Take(2).ToArray();
                addSpecCase.Add(new List<string>());
                addSpecCase[2].Add("APP-TEL-02");
                addSpecCase[2].Add("FPGA-Temperature");
                Array.Reverse(bDcDcTemperature);
                temp = intToQ((int)BitConverter.ToUInt32(bDcDcTemperature, 0), 4);
                addSpecCase[2].Add("Expected: * Received: " + temp.ToString());
                addSpecCase[2].Add("TRUE");

                //Motor Temperature
                byte[] bMotorTemperature = AppTelBlock1.Skip(8).Take(2).ToArray(); 
                addSpecCase.Add(new List<string>());
                addSpecCase[2].Add("APP-TEL-02");
                addSpecCase[2].Add("Motor-Temperature");
                Array.Reverse(bDcDcTemperature);
                temp = intToQ((int)BitConverter.ToUInt32(bDcDcTemperature, 0), 4);
                addSpecCase[2].Add("Expected: * Received: " + temp.ToString());
                addSpecCase[2].Add("TRUE");

                //Motor Driver Temperature
                byte[] bMotorDriverTemperature = AppTelBlock1.Skip(10).Take(2).ToArray();
                addSpecCase.Add(new List<string>());
                addSpecCase[3].Add("APP-TEL-02");
                addSpecCase[3].Add("Motor-Driver-Temperature");
                Array.Reverse(bDcDcTemperature);
                temp = intToQ((int)BitConverter.ToUInt32(bDcDcTemperature, 0), 4);
                addSpecCase[3].Add("Expected: * Received: " + temp.ToString());
                addSpecCase[3].Add("TRUE");

                //1V5 Power supply
                byte[] b1V5 = AppTelBlock1.Skip(12).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                addSpecCase[4].Add("APP-TEL-02");
                addSpecCase[4].Add("1V5 Power Supply");
                Array.Reverse(bDcDcTemperature);
                temp = intToQ((int)BitConverter.ToUInt32(bDcDcTemperature, 0), 4);
                addSpecCase[4].Add("Expected: 1.425-1.575 Received: " + temp.ToString());
                if (temp <= 1.425 && temp >= 1.575)
                { addSpecCase[4].Add("TRUE"); }
                else { addSpecCase[4].Add("FALSE"); }

                //3V3 Power supply
                byte[] b3V3Voltage = AppTelBlock1.Skip(16).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                addSpecCase[5].Add("APP-TEL-02");
                addSpecCase[5].Add("3V3 Power Supply");
                Array.Reverse(bDcDcTemperature);
                temp = intToQ((int)BitConverter.ToUInt32(bDcDcTemperature, 0), 4);
                addSpecCase[5].Add("Expected: 3.135-3.465 Received: " + temp.ToString());
                if (temp <= 3.135 && temp >= 3.465)
                { addSpecCase[5].Add("TRUE"); }
                else { addSpecCase[5].Add("FALSE"); }

                //5V Power supply
                byte[] b5VVoltage = AppTelBlock1.Skip(20).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                addSpecCase[6].Add("APP-TEL-02");
                addSpecCase[6].Add("5V Power Supply");
                Array.Reverse(bDcDcTemperature);
                temp = intToQ((int)BitConverter.ToUInt32(bDcDcTemperature, 0), 4);
                addSpecCase[6].Add("Expected: 4.75-5.25 Received: " + temp.ToString());
                if (temp <= 4.75 && temp >= 5.25)
                { addSpecCase[6].Add("TRUE"); }
                else { addSpecCase[6].Add("FALSE"); }

                //12V Power supply
                byte[] b12VVoltage = AppTelBlock1.Skip(24).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                addSpecCase[7].Add("APP-TEL-02");
                addSpecCase[7].Add("12V Power Supply");
                Array.Reverse(bDcDcTemperature);
                temp = intToQ((int)BitConverter.ToUInt32(bDcDcTemperature, 0), 4);
                addSpecCase[7].Add("Expected: 11.4-12.6 Received: " + temp.ToString());
                if (temp <= 11.4 && temp >= 12.6)
                { addSpecCase[7].Add("TRUE"); }
                else { addSpecCase[7].Add("FALSE"); }

                //24V Power supply
                byte[] b24VVoltage = AppTelBlock1.Skip(28).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                addSpecCase[8].Add("APP-TEL-02");
                addSpecCase[8].Add("24V Power Supply");
                Array.Reverse(bDcDcTemperature);
                temp = intToQ((int)BitConverter.ToUInt32(bDcDcTemperature, 0), 4);
                addSpecCase[8].Add("Expected: 22.8-25.2 Received: " + temp.ToString());
                if (temp <= 22.8 && temp >= 25.2)
                { addSpecCase[8].Add("TRUE"); }
                else { addSpecCase[8].Add("FALSE"); }

                //1V5 Current supply
                byte[] b1V5Current = AppTelBlock1.Skip(32).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                addSpecCase[9].Add("APP-TEL-02");
                addSpecCase[9].Add("1V5 Current Supply");
                Array.Reverse(bDcDcTemperature);
                temp = intToQ((int)BitConverter.ToUInt32(bDcDcTemperature, 0), 4);
                addSpecCase[9].Add("Expected: * Received: " + temp.ToString());
                addSpecCase[9].Add("TRUE");

                byte[] b3V3Current = AppTelBlock1.Skip(36).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] b5VCurrent = AppTelBlock1.Skip(40).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] b12VCurrent = AppTelBlock1.Skip(44).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] b24VCurrent = AppTelBlock1.Skip(48).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] b24VSensCurrent = AppTelBlock1.Skip(52).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
            }

            if (AppTelBlock2 != null)
            {
                byte[] bStatusRegister = AppTelBlock2.Skip(0).Take(4).ToArray();
                BitArray bits_StatusReg = new BitArray(bStatusRegister);
                addSpecCase.Add(new List<string>());

                byte[] bUpTime = AppTelBlock2.Skip(4).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] bCorrSUEcnt = AppTelBlock2.Skip(8).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] bErrSUEcnt = AppTelBlock2.Skip(12).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] bWriteUse = AppTelBlock2.Skip(16).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] bReadUse = AppTelBlock2.Skip(20).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] bScrub = AppTelBlock2.Skip(24).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] bRdxReceived = AppTelBlock2.Skip(28).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] brRdxReceived = AppTelBlock2.Skip(32).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] bTdxTransmitted = AppTelBlock2.Skip(36).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] brTdxTransmitted = AppTelBlock2.Skip(40).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] bRxdUartErr = AppTelBlock2.Skip(44).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] brRxdUartErr = AppTelBlock2.Skip(48).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] bRxdNspAckCnt = AppTelBlock2.Skip(52).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] brRxdNspAckCnt = AppTelBlock2.Skip(56).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] bRxdSlipErrCnt = AppTelBlock2.Skip(60).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] brRxdSlipErrCnt = AppTelBlock2.Skip(64).Take(4).ToArray();
                addSpecCase.Add(new List<string>()); 
                byte[] bRxdCrcErrCnt = AppTelBlock2.Skip(68).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] brRxdCrcErrCnt = AppTelBlock2.Skip(72).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] bRxdNspDiscCnt = AppTelBlock2.Skip(76).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] brRxdNspDiscCnt = AppTelBlock2.Skip(80).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] bRevCnt = AppTelBlock2.Skip(84).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] bHallSkipCnt = AppTelBlock2.Skip(88).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] bDrvFaultCnt = AppTelBlock2.Skip(92).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
                byte[] bDrvOverCnt = AppTelBlock2.Skip(96).Take(4).ToArray();
                addSpecCase.Add(new List<string>());
            }

            return addSpecCase;
        }

        //--------------------------------------------------------------------------------
        public static double Get32BitValueAddress(byte address, byte[] data)
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

        public static double Get32BitValueSpecified(int iBefore, int iAfter, byte[] data)
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
