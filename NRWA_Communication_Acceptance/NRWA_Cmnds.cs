﻿using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NRWA_Communication_Acceptance
{
    internal class NRWA_Cmnds
    {
        public const byte bPING = 0x80;
        public const byte bPEEK = 0x82;
        public const byte bPOKE = 0x83;
        public const byte bSYS_TELEMETRY = 0x84;
        public const byte bAPP_TELEMETRY = 0x87;
        public const byte bAPP_COMMAND = 0x88;

        public static (bool, bool, string, byte[], byte[], byte[], byte[], byte[]) cmnd_Ping(SerialPort _Port, byte ssAddress, byte ssSource)
        {
            _Port.DtrEnable = true;
            _Port.Open();
            _Port.ReadTimeout = 1500;
            Console.WriteLine("PING");

            try
            {
                (bool bSuccess, string sWFeedback, byte[] a_TX) = PortCommunication.PortWrite(_Port, ssAddress, ssSource, bPING, null);
                System.Threading.Thread.Sleep(500);
                if (bSuccess == true)
                {
                    (bool bFindSLIP, bool bACK, byte[] a_RX, string sRFeedback, byte bSrcAddr, byte bDstAddr, byte bCmnd, byte[] Data, byte[] bRecCRC, byte[] bChkCRC) = PortCommunication.PortRead(_Port);

                    _Port.Close();

                    if (Data != null)
                    {
                        Console.WriteLine("DATA: " + BitConverter.ToString(Data));
                        return (bFindSLIP, bACK, sRFeedback, a_TX, a_RX, Data, bRecCRC, bChkCRC);
                    }
                    else
                    {
                        return (bFindSLIP, bACK, sRFeedback, a_TX, a_RX, null, null, null);
                    }
                }
                else
                {
                    return (bSuccess, false, sWFeedback, a_TX, null, null, null, null);
                }
            }
            catch (Exception ex)
            {
                return (false, false, ex.Message, null, null, null, null, null);
            }

        }

        public static (bool, bool, string, byte[], byte[], byte[], byte[]) cmnd_Peek(SerialPort _Port, byte ssAddress, byte ssSource, byte[] bCommand)
        {
            _Port.DtrEnable = true;
            _Port.Open();
            _Port.ReadTimeout = 1500;
            Console.WriteLine("PEEK");

            try
            {
                (bool bSuccess, string sWFeedback, byte[] a_TX) = PortCommunication.PortWrite(_Port, ssAddress, ssSource, bPEEK, bCommand);
                System.Threading.Thread.Sleep(500);
                if (bSuccess == true)
                {
                    (bool bFindSLIP, bool bACK, byte[] a_RX, string sRFeedback, byte bSrcAddr, byte bDstAddr, byte bCmnd, byte[] Data, byte[] bRecCRC, byte[] bChkCRC) = PortCommunication.PortRead(_Port);

                    _Port.Close();

                    if (Data != null)
                    {
                        Console.WriteLine("DATA: " + BitConverter.ToString(Data));
                        return (bFindSLIP, bACK, sRFeedback, a_TX, a_RX, Data, bRecCRC);
                    }
                    else
                    {
                        return (bFindSLIP, bACK, sRFeedback, a_TX, a_RX, null, null);
                    }
                }
                else
                {
                    return (bSuccess, false, sWFeedback, a_TX, null, null, null);
                }
            }
            catch (Exception ex)
            {
                return (false, false, ex.Message, null, null, null, null);
            }

        }

        public static (bool, bool, string, byte[], byte[], byte[], byte[]) cmnd_Poke(SerialPort _Port, byte ssAddress, byte ssSource, byte[] bCommand)
        {
            _Port.DtrEnable = true;
            _Port.Open();
            _Port.ReadTimeout = 1500;
            Console.WriteLine("POKE");

            try
            {
                (bool bSuccess, string sWFeedback, byte[] a_TX) = PortCommunication.PortWrite(_Port, ssAddress, ssSource, bPOKE, bCommand);
                System.Threading.Thread.Sleep(500);
                if (bSuccess == true)
                {
                    (bool bFindSLIP, bool bACK, byte[] a_RX, string sRFeedback, byte bSrcAddr, byte bDstAddr, byte bCmnd, byte[] Data, byte[] bRecCRC, byte[] bChkCRC) = PortCommunication.PortRead(_Port);

                    _Port.Close();

                    if (Data != null)
                    {
                        Console.WriteLine("DATA: " + BitConverter.ToString(Data));
                        return (bFindSLIP, bACK, sRFeedback, a_TX, a_RX, Data, bRecCRC);
                    }
                    else
                    {
                        return (bFindSLIP, bACK, sRFeedback, a_TX, a_RX, null, null);
                    }
                }
                else
                {
                    return (bSuccess, false, sWFeedback, a_TX, null, null, null);
                }
            }
            catch (Exception ex)
            {
                return (false, false, ex.Message, null, null, null, null);
            }

        }

        public static (bool, bool, string, byte[], byte[], byte[], byte[]) cmnd_SysTel(SerialPort _Port, byte ssAddress, byte ssSource, byte[] bCommand)
        {
            _Port.DtrEnable = true;
            _Port.Open();
            _Port.ReadTimeout = 1500;
            Console.WriteLine("SYSTEM TELEMETRY");

            try
            {
                (bool bSuccess, string sWFeedback, byte[] a_TX) = PortCommunication.PortWrite(_Port, ssAddress, ssSource, bSYS_TELEMETRY, bCommand);
                System.Threading.Thread.Sleep(500);
                if (bSuccess == true)
                {
                    (bool bFindSLIP, bool bACK, byte[] a_RX, string sRFeedback, byte bSrcAddr, byte bDstAddr, byte bCmnd, byte[] Data, byte[] bRecCRC, byte[] bChkCRC) = PortCommunication.PortRead(_Port);

                    _Port.Close();

                    if (Data != null)
                    {
                        Console.WriteLine("DATA: " + BitConverter.ToString(Data));
                        return (bFindSLIP, bACK, sRFeedback, a_TX, a_RX, Data, bRecCRC);
                    }
                    else
                    {
                        return (bFindSLIP, bACK, sRFeedback, a_TX, a_RX, null, null);
                    }
                }
                else
                {
                    return (bSuccess, false, sWFeedback, a_TX, null, null, null);
                }
            }
            catch (Exception ex)
            {
                return (false, false, ex.Message, null, null, null, null);
            }

        }

        public static (bool, bool, string, byte[], byte[], byte[], byte[]) cmnd_AppTel(SerialPort _Port, byte ssAddress, byte ssSource, byte[] bCommand)
        {
            _Port.DtrEnable = true;
            _Port.Open();
            _Port.ReadTimeout = 1500;
            Console.WriteLine("APPLICATION TELEMETRY");

            try
            {
                (bool bSuccess, string sWFeedback, byte[] a_TX) = PortCommunication.PortWrite(_Port, ssAddress, ssSource, bAPP_TELEMETRY, bCommand);
                System.Threading.Thread.Sleep(500);
                if (bSuccess == true)
                {
                    (bool bFindSLIP, bool bACK, byte[] a_RX, string sRFeedback, byte bSrcAddr, byte bDstAddr, byte bCmnd, byte[] Data, byte[] bRecCRC, byte[] bChkCRC) = PortCommunication.PortRead(_Port);

                    _Port.Close();

                    if (Data != null)
                    {
                        Console.WriteLine("DATA: " + BitConverter.ToString(Data));
                        return (bFindSLIP, bACK, sRFeedback, a_TX, a_RX, Data, bRecCRC);
                    }
                    else
                    {
                        return (bFindSLIP, bACK, sRFeedback, a_TX, a_RX, null, null);
                    }
                }
                else
                {
                    return (bSuccess, false, sWFeedback, a_TX, null, null, null);
                }
            }
            catch (Exception ex)
            {
                return (false, false, ex.Message, null, null, null, null);
            }

        }

        public static (bool, bool, string, byte[], byte[], byte[], byte[]) cmnd_AppCom(SerialPort _Port, byte ssAddress, byte ssSource, byte[] bCommand)
        {
            _Port.DtrEnable = true;
            _Port.Open();
            _Port.ReadTimeout = 1500;
            Console.WriteLine("APPLICATION COMMAND");

            try
            {
                (bool bSuccess, string sWFeedback, byte[] a_TX) = PortCommunication.PortWrite(_Port, ssAddress, ssSource, bAPP_COMMAND, bCommand);
                System.Threading.Thread.Sleep(500);
                if (bSuccess == true)
                {
                    (bool bFindSLIP, bool bACK, byte[] a_RX, string sRFeedback, byte bSrcAddr, byte bDstAddr, byte bCmnd, byte[] Data, byte[] bRecCRC, byte[] bChkCRC) = PortCommunication.PortRead(_Port);

                    _Port.Close();

                    if (Data != null)
                    {
                        Console.WriteLine("DATA: " + BitConverter.ToString(Data));
                        return (bFindSLIP, bACK, sRFeedback, a_TX, a_RX, Data, bRecCRC);
                    }
                    else
                    {
                        return (bFindSLIP, bACK, sRFeedback, a_TX, a_RX, null, null);
                    }
                }
                else
                {
                    return (bSuccess, false, sWFeedback, a_TX, null, null, null);
                }
            }
            catch (Exception ex)
            {
                return (false, false, ex.Message, null, null, null, null);
            }

        }

        public static byte[] cmnd_Gen(SerialPort _Port, byte[] TX_Message)
        {
            int iReadbytes = 0;
            byte[] Buffer = new byte[260];
            _Port.DtrEnable = true;
            _Port.Open();
            _Port.ReadTimeout = 1500;
            Console.WriteLine("PING");

            try
            {
                _Port.Write(TX_Message, 0, TX_Message.Length);
                System.Threading.Thread.Sleep(500);
                _Port.ReadTimeout = 2000;
                while (iReadbytes == 0)
                {
                    iReadbytes = _Port.Read(Buffer, 0, 260);
                }
                }
            catch (Exception ex)
            {
                _Port.Close();
                return (null);
            }

            _Port.Close();
            return (Buffer);
        }

    }
}