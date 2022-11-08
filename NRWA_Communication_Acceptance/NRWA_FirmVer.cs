using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using NRWA_Communication_Acceptance;

namespace NRWA_Communication_Acceptance
{
    public partial class NRWA_FirmVer : Form
    {
        bool bLogging = false;
        string sSelectedPath;
        string sFilename;

        public static  List<AutoCommands> l_NRWACommands = new List<AutoCommands>();
        public static SerialPort _serialPort;

        public NRWA_FirmVer()
        {
            InitializeComponent();
           // SLIP_Frame.crcTest();
            string[] ports = PortCommunication.FindPorts();
            cbPorts.Items.Clear();
            cbPorts.Items.AddRange(ports);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ComPort = cbPorts.Text;

            _serialPort = new SerialPort();

            _serialPort.PortName = ComPort;
            _serialPort.BaudRate = 460800;
            _serialPort.Parity = Parity.None;
            _serialPort.StopBits = StopBits.One;
            _serialPort.ReadTimeout = 10000;

            try
            {
                _serialPort.Open();
                _serialPort.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            if (!bLogging)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "Custom Description";

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    sSelectedPath = fbd.SelectedPath;
                    bLogging = true;

                    sFilename = "_NRWA_Log_" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                    lblLog.Text = sFilename;
                    LogWriter.Create(sSelectedPath, sFilename);
                }
            }
            else
            {
                bLogging = false;
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            int idec = 10 / 8;
            byte[] est = new byte[3] { 0x45, 0xA0, 0xFF };
            string test = est[1].ToString("x");




            //CreateCommandObjects();
            byte[] test1 = AutoCommands.GetRandom32BitValue(8, 0);
            byte[] test2 = AutoCommands.GetRandom32BitValue(32, 0);
            byte[] test3 = AutoCommands.GetRandom32BitValue(20, 12);
            byte[] test4 = AutoCommands.GetRandom32BitValue(16, 0);
            byte[] test5 = AutoCommands.GetRandom32BitValue(24, 0);
            List<List<bool>> VerificationReply = new List<List<bool>>();
            try
            {
                VerificationReply.Add(new List<bool>());
                (bool bSlip, bool bAck, string sFeedback, byte[] a_TX, byte[] a_RX, byte[] a_Data, byte[] bRecCRC, byte[] bChkCRC) = NRWA_Cmnds.cmnd_Ping(_serialPort, 0x07, 0x11);
                VerificationReply.Add(Verify(bSlip, bAck, sFeedback, a_TX, a_RX, a_Data, bRecCRC, bChkCRC));

                _serialPort.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        private List<bool> Verify(bool bSlip, bool bAck, string sFeedback, byte[] a_TX, byte[] a_RX, byte[] a_Data, byte[] bRecCRC, byte[] bChkCRC)
        {
            List<bool> VerificationReply = new List<bool>();

            byte bCommand = FindTXCommand(a_TX);
            // 1 Verify CRC
            VerificationReply.Add(RX_Verification.ver_CRC(bRecCRC, bChkCRC));
            // 2 Verify SLIP Encapsulation
            VerificationReply.Add(RX_Verification.ver_Encapsulation(a_RX));
            // 3 Verify Poll B and A bits 
            VerificationReply.Add(RX_Verification.ver_PollBA(a_TX, a_RX));
            // 4 Verify adressing
            VerificationReply.Add(RX_Verification.ver_Addresses(a_TX, a_RX));








            return VerificationReply;
        }

        private byte FindTXCommand(byte[] a_TX)
        {
            byte bCommand = 0;
            byte[] a_MessContrF = new byte[1];
            a_MessContrF[0] = a_TX[3];
            BitArray bits_rxCtrl = new BitArray(a_MessContrF);

            List<bool> bools = new List<bool>();
            for (int i = 0; i < 5; i++)
            {
                bools.Add(bits_rxCtrl[i]);
            }
            bools.Add(false);
            bools.Add(false);
            bools.Add(false);

            for (int i = 0; i < 8; i++)
            {
                if (bools[i])
                {
                    bCommand |= (byte)(1 << i);
                }
            }
            return bCommand;
        }

        private void CreateCommandObjects()
        {
            _serialPort.Open();
            _serialPort.Close();

            int[] a_iFormat = new int[2];
            //PING
            AutoCommands ping = new AutoCommands();
            ping.sCommand = "PING";
            rtbLog.Text = ping.sCommand + "\n";
            ping.TX_correct = SLIP_Frame.CreateSLIPframe(0x07, 0x11, 0x80, null);
            rtbLog.Text += BitConverter.ToString(ping.TX_correct) + "\n";
            ping.RX_correct = NRWA_Cmnds.cmnd_Gen(_serialPort, ping.TX_correct);
            rtbLog.Text += BitConverter.ToString(ping.RX_correct) + "\n";
            ping.RX_correct = RX_Verification.removeEndingZeros(ping.RX_correct);
            rtbLog.Text += BitConverter.ToString(ping.RX_correct) + "\n";
            l_NRWACommands.Add(ping);

            //INIT
            AutoCommands initiate = new AutoCommands();
            initiate.sCommand = "INIT";
            rtbLog.Text += initiate.sCommand;
            initiate.TX_correct = SLIP_Frame.CreateSLIPframe(0x07, 0x11, 0x81, null);
            rtbLog.Text += BitConverter.ToString(initiate.TX_correct) + "\n";
            initiate.RX_correct = NRWA_Cmnds.cmnd_Gen(_serialPort, initiate.TX_correct);
            rtbLog.Text += BitConverter.ToString(initiate.RX_correct) + "\n";
            initiate.RX_correct = RX_Verification.removeEndingZeros(initiate.RX_correct);
            rtbLog.Text += BitConverter.ToString(initiate.RX_correct) + "\n";
            l_NRWACommands.Add(initiate);

            //PEEK // Still needs 1E-2D
            for (int i = 0; i < 30; i++)
            {
                AutoCommands peek = new AutoCommands();
                peek.sCommand = "PEEK" + "-" + i.ToString("X");
                peek.bAddress = (byte)i;
                rtbLog.Text += peek.sCommand;
                byte[] data = new byte[1];
                data[0] = (byte)i;
                peek.TX_correct = SLIP_Frame.CreateSLIPframe(0x07, 0x11, 0x82, data);
                rtbLog.Text += BitConverter.ToString(peek.TX_correct) + "\n";
                peek.RX_correct = NRWA_Cmnds.cmnd_Gen(_serialPort, peek.TX_correct);
                rtbLog.Text += BitConverter.ToString(peek.RX_correct) + "\n";
                peek.RX_correct = RX_Verification.removeEndingZeros(peek.RX_correct);
                rtbLog.Text += BitConverter.ToString(peek.RX_correct) + "\n";
                a_iFormat[0] = 32;
                a_iFormat[1] = 0;

                if (i == 7 || i == 8)
                {
                    a_iFormat[0] = 20;
                    a_iFormat[1] = 12;
                } else if (i == 9 || i == 29)
                {
                    a_iFormat[0] = 16;
                    a_iFormat[1] = 16;
                } else if (i == 16)
                {
                    a_iFormat[0] = 14;
                    a_iFormat[1] = 10;
                } else if (i == 17)
                {
                    a_iFormat[0] = 0;
                    a_iFormat[1] = 16;
                }

                peek.l_iFormat.Add(a_iFormat);
                l_NRWACommands.Add(peek);
            }

            //POKE // Still needs 1E-2D
            for (int i = 0; i < 30; i++)
            {
                AutoCommands poke = new AutoCommands();
                poke.sCommand = "POKE" + "-" + i.ToString("X");
                poke.bAddress = (byte)i;
                rtbLog.Text += poke.sCommand;
                byte[] data = new byte[5] { 0x00, 0x00, 0x00, 0x00, 0x00 };
                data[0] = (byte)i;
                poke.TX_correct = SLIP_Frame.CreateSLIPframe(0x07, 0x11, 0x83, data);
                rtbLog.Text += BitConverter.ToString(poke.TX_correct) + "\n";
                a_iFormat[0] = 32;
                a_iFormat[1] = 0;

                if (i == 7 || i == 8)
                {
                    a_iFormat[0] = 20;
                    a_iFormat[1] = 12;
                }
                else if (i == 9 || i == 29)
                {
                    a_iFormat[0] = 16;
                    a_iFormat[1] = 16;
                }
                else if (i == 16)
                {
                    a_iFormat[0] = 14;
                    a_iFormat[1] = 10;
                }
                else if (i == 17)
                {
                    a_iFormat[0] = 0;
                    a_iFormat[1] = 16;
                }

                poke.l_iFormat.Add(a_iFormat);
                l_NRWACommands.Add(poke);
            }

            //SYSTEM-TELEMETRY
            for (int i = 160; i < 182; i++)
            {
                AutoCommands sysTel = new AutoCommands();
                sysTel.sCommand = "SYS_TEL" + "-" + i.ToString("X");
                rtbLog.Text += sysTel.sCommand;
                byte[] data = new byte[1];
                data[0] = (byte)i;
                sysTel.TX_correct = SLIP_Frame.CreateSLIPframe(0x07, 0x11, 0x84, data);
                rtbLog.Text += BitConverter.ToString(sysTel.TX_correct) + "\n";
                sysTel.RX_correct = NRWA_Cmnds.cmnd_Gen(_serialPort, sysTel.TX_correct);
                rtbLog.Text += BitConverter.ToString(sysTel.RX_correct) + "\n";
                sysTel.RX_correct = RX_Verification.removeEndingZeros(sysTel.RX_correct);
                rtbLog.Text += BitConverter.ToString(sysTel.RX_correct) + "\n";
                a_iFormat[0] = 32;
                a_iFormat[1] = 0;

                if (i == 161)
                {
                    a_iFormat[0] = 30;
                    a_iFormat[1] = 2;
                }

                sysTel.l_iFormat.Add(a_iFormat);
                l_NRWACommands.Add(sysTel);
            }

            //APPLICATION-TELEMETRY
            for (int i = 0; i < 3; i++)
            {
                AutoCommands sysApp = new AutoCommands();
                sysApp.sCommand = "APP_TEL" + "-" + i.ToString("X");
                rtbLog.Text += sysApp.sCommand;
                byte[] data = new byte[1];
                data[0] = (byte)i;
                sysApp.TX_correct = SLIP_Frame.CreateSLIPframe(0x07, 0x11, 0x87, data);
                rtbLog.Text += BitConverter.ToString(sysApp.TX_correct) + "\n";
                sysApp.RX_correct = NRWA_Cmnds.cmnd_Gen(_serialPort, sysApp.TX_correct);
                rtbLog.Text += BitConverter.ToString(sysApp.RX_correct) + "\n";
                sysApp.RX_correct = RX_Verification.removeEndingZeros(sysApp.RX_correct);
                rtbLog.Text += BitConverter.ToString(sysApp.RX_correct) + "\n";

                //switch (i)
                //{
                //    case 0:
                //        {
                //            a_iFormat[0] = 32; a_iFormat[1] = 0; sysApp.l_iFormat.Add(a_iFormat);
                //            a_iFormat[0] = 8; a_iFormat[1] = 0; sysApp.l_iFormat.Add(a_iFormat);
                //            a_iFormat[0] = 14; a_iFormat[1] = 0; sysApp.l_iFormat.Add(a_iFormat);
                //            break;
                //        }
                //    case 1:
                //        {

                //            break;
                //        }
                //    case 2:
                //        {

                //            break;
                //        }
                //}

                l_NRWACommands.Add(sysApp);
            }
        }

    }
}
