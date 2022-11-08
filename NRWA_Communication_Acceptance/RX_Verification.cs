using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NRWA_Communication_Acceptance
{
    internal class RX_Verification
    {
        public static bool ver_PollBA(byte[] a_TX, byte[] a_RX)
        {
            bool[] a_bTControl = new bool[3]; // {Poll Bit, B Bit, A Bit}
            bool[] a_bRControl = new bool[3]; // {Poll Bit, B Bit, A Bit}

            //TX Control Field
            byte[] a_MessContrF = new byte[1];
            a_MessContrF[0] = a_TX[3];
            BitArray bits_rxCtrl = new BitArray(a_MessContrF);
            a_bTControl[0] = bits_rxCtrl[7];
            a_bTControl[1] = bits_rxCtrl[6];
            a_bTControl[2] = bits_rxCtrl[5];

            // Poll bit vs RX
            if (a_bTControl[0])
            {
                if (a_RX == null) return false;

                //RX Control Field
                a_MessContrF[0] = a_RX[3];
                bits_rxCtrl = new BitArray(a_MessContrF);
                a_bRControl[0] = bits_rxCtrl[7];
                a_bRControl[1] = bits_rxCtrl[6];
                a_bRControl[2] = bits_rxCtrl[5];

                // B bit vs B bit
                if (a_bTControl[1] != a_bRControl[1]) return false;
            }
            else
            {
                if (a_RX != null) return false;
            }
            return true;
        }

        public static bool ver_CRC(byte[] a_recCRC, byte[] a_expCRC)
        {
            if (a_recCRC[0] != a_expCRC[0] || a_recCRC[1] != a_expCRC[1])
            {
                return false;
            }
            return true;
        }

        public static bool ver_Encapsulation(byte[] a_Buffer)
        {
            byte[] a_RX = removeEndingZeros(a_Buffer);
            try
            {
                if (a_RX[0] == 0xC0 && a_RX[a_RX.Length - 1] == 0xC0)
                {
                    for (int i = 1; i < a_RX.Length - 2; i++)
                    {
                        if (a_RX[i] == 0xC0)
                        {
                            throw new Exception("Contains FEND");
                        }

                        if (a_RX[i] == 0xDB)
                        {
                            if (a_RX[i + 1] != 0xDC || a_RX[i + 1] != 0xDD)
                            {
                                throw new Exception("Missing TFEND or TFESC");
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("Missing FEND");
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public static bool ver_DataPING(byte[] a_Data)
        {
            try
            {
                if (a_Data.Length == 4)
                {
                    uint uiDevType = a_Data[0];
                    uint uiDevSN = a_Data[1];
                    uint uiDevVersionMaj = a_Data[2];
                    uint uiDevVersionMin = a_Data[3];
                    uint uiDevVersionPatch = a_Data[4];
                }
                else
                {
                    throw new Exception("Data Lenght");
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }

        public static bool ver_Addresses(byte[] a_TX, byte[] a_RX)
        {
            if ((a_TX[1] == a_RX[2]) && (a_TX[2] == a_RX[1])) return true;
            else return false;
        }

        public static byte[] removeEndingZeros(byte[] a_RX)
        {
            List<byte> b = new List<byte>();
            b = a_RX.ToList();

            for (int i = b.Count() - 1; i > 4; i--)
            {
                if (b[i] == 0x00) b.RemoveAt(i);
                else if (b[i] == 0xC0) i = 4;
            }
            return b.ToArray();
        }

        public static bool verPeekVsPoke(byte[] a_PeekRX, byte[] a_PokeTX)
        {
            (bool peek_bFindSLIP, byte peek_bSrcAddr,  byte peek_bDstAddr,  byte peek_bCmnd, byte[] peek_Data, byte[] peek_bRecCRC, byte[] peek_bChkCRC) = SLIP_Frame.FindSLIPframe(a_PeekRX);
            (bool poke_bFindSLIP, byte poke_bSrcAddr, byte poke_bDstAddr, byte poke_bCmnd, byte[] poke_Data, byte[] poke_bRecCRC, byte[] poke_bChkCRC) = SLIP_Frame.FindSLIPframe(a_PokeTX);

            byte[] a_poke = new byte[4];
            a_poke[0] = poke_Data[1];
            a_poke[1] = poke_Data[2];
            a_poke[2] = poke_Data[3];
            a_poke[3] = poke_Data[4];
            if (peek_Data == a_poke)
            {
                return true;
            }
            else
            {
                return false;
            }
            

        }
 
    }
}
