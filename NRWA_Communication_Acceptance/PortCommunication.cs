using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRWA_Communication_Acceptance
{
    internal class PortCommunication
    {
        public static string[] FindPorts()
        {
            // Get a list of serial port names.
            string[] ports = SerialPort.GetPortNames();

            return ports;
        }

        public static (bool, string, byte[]) PortWrite(SerialPort _Port, byte bAddress, byte bSource, byte bCmnd, byte[] Data)
        {
            string sMessage = "#PortWrite";

            if (_Port.IsOpen == false)
            {
                sMessage = "ERROR: COM PORT CLOSED";
                return (false, sMessage, null);
            }
            byte[] DataFrame = SLIP_Frame.CreateSLIPframe(bAddress, bSource, bCmnd, Data);
            Console.WriteLine("TX: " + BitConverter.ToString(DataFrame));
            try
            {
                _Port.Write(DataFrame, 0, DataFrame.Length);
            }
            catch (Exception ex)
            {
                sMessage = "ERROR: " + ex.Message;
                return (false, sMessage, null);
            }

            sMessage = "SUCCESS";
            return (true, sMessage, DataFrame);
        }

        public static (bool, bool, byte[], string, byte, byte, byte, byte[], byte[], byte[]) PortRead(SerialPort _Port)
        {
            byte[] Buffer = new byte[260];
            int iReadbytes = 0;
            string sFeedback = "";
            bool bAck = false;

            if (!_Port.IsOpen)
            {
                sFeedback = "ERROR: COM PORT CLOSED";
                return (false, bAck, null, sFeedback, 0x00, 0x00, 0x00, null, null, null);
            }
            else
            {
                byte[] bRecCRC = new byte[2];
                byte[] bChkCRC = new byte[2];

                byte bSrcAddr = 0x00;
                byte bDstAddr = 0x00;
                byte bCmnd = 0x00;
                byte[] Data = null;
                bool bFindSLIP = false; ;

                _Port.ReadTimeout = 1000;

                try
                {
                    while (iReadbytes == 0)
                    {
                        iReadbytes = _Port.Read(Buffer, 0, 260);
                    }
                    Console.WriteLine("RX: " + BitConverter.ToString(Buffer));

                    (bFindSLIP,bSrcAddr, bDstAddr, bCmnd,  Data, bRecCRC, bChkCRC) = SLIP_Frame.FindSLIPframe(Buffer);

                    if (Data.Length != 0)
                    {
                        if (Data[0] == 0 && sFeedback != "")
                        {
                            Data = Encoding.ASCII.GetBytes(sFeedback);
                            sFeedback = "UNSUCCESSFUL";
                            return (false, bAck, Buffer, sFeedback, 0x00, 0x00, 0x00, Data, null, null);
                        }
                    }
                    bAck = CheckACK(bCmnd);
                    if (CheckACK(bCmnd))
                    sFeedback = "SUCCESS";

                }
                catch (Exception ex)
                {
                    bAck = false;
                    sFeedback = "ERROR: " + ex.Message;

                }
                Console.WriteLine("DATA: " + BitConverter.ToString(Data));
                return (bFindSLIP, bAck, Buffer, sFeedback, bSrcAddr, bDstAddr, bCmnd, Data, bRecCRC, bChkCRC);
            }
        }

        public static bool CheckACK(byte b_ctrl)
        {
            byte[] arr_rxCtrl = new byte[1];
            arr_rxCtrl[0] = b_ctrl;
            BitArray bits_rxCtrl = new BitArray(arr_rxCtrl);
            return bits_rxCtrl[5];
        }

    }
}
