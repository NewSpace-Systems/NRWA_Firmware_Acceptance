﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Newtonsoft.Json;
using NRWA_Communication_Acceptance;
using static NRWA_Communication_Acceptance.AutoCommands;

namespace NRWA_Communication_Acceptance
{
    public partial class NRWA_FirmVer : Form
    {
        bool bLogging = false;
        static string url = "";
        static HttpClient httpClient = new HttpClient();

        public static string sSelectedPath;
        public static string sFilename;

        public static  List<AutoCommands> l_NRWACommands = new List<AutoCommands>();
        public static SerialPort _serialPort;

        BackgroundWorker bgWorker;

        public NRWA_FirmVer()
        {
            InitializeComponent();
           // SLIP_Frame.crcTest();
            string[] ports = PortCommunication.FindPorts();
            cbPorts.Items.Clear();
            cbPorts.Items.AddRange(ports);
            cbPorts.SelectedIndex = 0;

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
                this.btnPortCon.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
                this.btnPortCon.ForeColor = System.Drawing.Color.Black;
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

                    sFilename = "_NRWA_Log_" + DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss");
                    lblLog.Text = sFilename;
                    LogWriter.Create(sSelectedPath, sFilename);
                }
            }
            else
            {
                bLogging = false;
            }
        }

        private void Verify(int iStep)
        {
            int iProgress = 0;

            if (cbInitial.Checked)
            {
                iProgress += iStep;
                ShowSerialData("Initial Values");
                List<List<string>> autoInitPeek = AutoCommands.PeekInitialValues();
                for (int i = 0; i < autoInitPeek.Count; i++)
                {
                    LogWriter.AppendLog(sSelectedPath, sFilename, "INITIAL-" + autoInitPeek[i][4], "Initial Value Compair", autoInitPeek[i][0], autoInitPeek[i][1], autoInitPeek[i][2], autoInitPeek[i][3]);
                }
            }


            if (cbPeekPoke.Checked)
            {
                iProgress += iStep;
                ShowSerialData("Auto Peek-Poke");
                List<List<string>> autoPeekPoke = AutoCommands.PokePeek();
                for (int i = 0; i < autoPeekPoke.Count; i++)
                {
                    LogWriter.AppendLog(sSelectedPath, sFilename, "PEEK-POKE-" + autoPeekPoke[i][4], "peek-poke", autoPeekPoke[i][0], autoPeekPoke[i][1], autoPeekPoke[i][2], autoPeekPoke[i][3]);
                }
            }

            if (cbSystemTelemetryDataCast.Checked)
            {
                iProgress += iStep;
                ShowSerialData("Auto System Telemetry Read");
                List<List<string>> autoSysTelRead = AutoCommands.SysTel();
                for (int i = 0; i < autoSysTelRead.Count; i++)
                {
                    LogWriter.AppendLog(sSelectedPath, sFilename, "SYSTEM-TELEMENTRY-" + autoSysTelRead[i][4] + "-DATA-CONVERSION", "Correct", autoSysTelRead[i][0], autoSysTelRead[i][1], autoSysTelRead[i][2], autoSysTelRead[i][3]);
                }
            }

            if (cbPeekOutsideAd.Checked)
            {
                iProgress += iStep;
                ShowSerialData("Auto Peek Outside Address Range");
                List<List<string>> autoPeekOutRange = AutoCommands.PeekOutsideAddressRanges();
                for (int i = 0; i < autoPeekOutRange.Count; i++)
                {
                    LogWriter.AppendLog(sSelectedPath, sFilename, "PEEK-" + autoPeekOutRange[i][4], "Outside Address Range", autoPeekOutRange[i][0], autoPeekOutRange[i][1], autoPeekOutRange[i][2], autoPeekOutRange[i][3]);
                }

            }

            if (cbPokeOutsideAd.Checked)
            {
                iProgress += iStep;
                ShowSerialData("Auto Poke Outside Address Range");
                List<List<string>> autoPokeOutRange = AutoCommands.PokeOutsideAddressRanges();
                for (int i = 0; i < autoPokeOutRange.Count; i++)
                {
                    LogWriter.AppendLog(sSelectedPath, sFilename, "POKE-" + autoPokeOutRange[i][4], "Outside Address Range", autoPokeOutRange[i][0], autoPokeOutRange[i][1], autoPokeOutRange[i][2], autoPokeOutRange[i][3]);
                }
            }

            if (cbPokeOutsideData.Checked)
            {
                iProgress += iStep;
                ShowSerialData("Auto Poke Outside Data Range");
                List<List<string>> autoPokeOutRangeData = AutoCommands.PokeOutsideDataRange();
                for (int i = 0; i < autoPokeOutRangeData.Count; i++)
                {
                    LogWriter.AppendLog(sSelectedPath, sFilename, "POKE-" + autoPokeOutRangeData[i][4], "Lack of Data", autoPokeOutRangeData[i][0], autoPokeOutRangeData[i][1], autoPokeOutRangeData[i][2], autoPokeOutRangeData[i][3]);
                    i++;
                    LogWriter.AppendLog(sSelectedPath, sFilename, "POKE-" + autoPokeOutRangeData[i][4], "Excess of Data", autoPokeOutRangeData[i][0], autoPokeOutRangeData[i][1], autoPokeOutRangeData[i][2], autoPokeOutRangeData[i][3]);
                }

            }

            if (cbFalseCRC.Checked)
            {
                iProgress += iStep;
                ShowSerialData("Automatic False CRC");
                List<List<string>> autoFalseCRC = AutoCommands.FalseCRC();
                for (int i = 0; i < autoFalseCRC.Count; i++)
                {
                    LogWriter.AppendLog(sSelectedPath, sFilename, autoFalseCRC[i][4], "False CRC", autoFalseCRC[i][0], autoFalseCRC[i][1], autoFalseCRC[i][2], autoFalseCRC[i][3]);
                }
            }

            if (cbAppTellDataCast.Checked)
            {
                iProgress += iStep;
                ShowSerialData("Application Telemetry Data Casting");
                List<List<string>> autoAppTelCom = AutoCommands.AppTelCom();
                for (int i = 0; i < autoAppTelCom.Count; i++)
                {
                    LogWriter.AppendLog(sSelectedPath, sFilename, "APP TEL-" + autoAppTelCom[i][4], "Application Telemetry Data Casting", autoAppTelCom[i][0], autoAppTelCom[i][1], autoAppTelCom[i][2], autoAppTelCom[i][3]);
                }
            }

            if (cbAppTelData.Checked)
            {
                iProgress += iStep;
                ShowSerialData("Application Telemetry Data Validation");
                List<List<string>> autoAppTelCom = AutoCommands.AddressSpecificCases();
                for (int i = 0; i < autoAppTelCom.Count; i++)
                {
                    LogWriter.AppendLog(sSelectedPath, sFilename, "APP TEL-" + autoAppTelCom[i][4], "Application Telemetry Data Casting", autoAppTelCom[i][0], autoAppTelCom[i][1], autoAppTelCom[i][2], autoAppTelCom[i][3]);
                }
            }

            if (cbPokeIncorrectData.Checked)
            {
                iProgress += iStep;
                ShowSerialData("Auto Poke WIth Corrupt Data");
                List<List<string>> autoPokeOutRangeData = AutoCommands.PokeWithCorruptData();
                for (int i = 0; i < autoPokeOutRangeData.Count; i++)
                {
                    LogWriter.AppendLog(sSelectedPath, sFilename, "POKE-" + autoPokeOutRangeData[i][4], "Corrupt Data", autoPokeOutRangeData[i][0], autoPokeOutRangeData[i][1], autoPokeOutRangeData[i][2], autoPokeOutRangeData[i][3]);
                }
            }

            ShowSerialData("DONE\n");

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
            //string sSelectedPath, string sFilename, string sCommand, string sTest, string sTx, string sRx, string sData, string sPassFail
            _serialPort.Open();
            _serialPort.Close();

            int[] a_iFormat = new int[2];
            //PING
            AutoCommands ping = new AutoCommands();
            ping.sCommand = "PING" ;
            ping.bCommand = 0x80;
            ping.TX_correct = SLIP_Frame.CreateSLIPframe(0x07, 0x11, 0x80, null);
            ping.RX_correct = NRWA_Cmnds.cmnd_Gen(_serialPort, ping.TX_correct);
            ping.RX_correct = RX_Verification.removeEndingZeros(ping.RX_correct);
            l_NRWACommands.Add(ping);
           // LogWriter.AppendLog(sSelectedPath, sFilename, ping.sCommand, "Correct", BitConverter.ToString(ping.TX_correct), BitConverter.ToString(ping.RX_correct), "", "");

            //INIT
            AutoCommands initiate = new AutoCommands();
            initiate.sCommand = "INIT";
            initiate.bCommand = 0x81;
            initiate.TX_correct = SLIP_Frame.CreateSLIPframe(0x07, 0x11, 0x81, null);
            initiate.RX_correct = NRWA_Cmnds.cmnd_Gen(_serialPort, initiate.TX_correct);
            initiate.RX_correct = RX_Verification.removeEndingZeros(initiate.RX_correct);
            l_NRWACommands.Add(initiate);
           // LogWriter.AppendLog(sSelectedPath, sFilename, initiate.sCommand, "Correct", BitConverter.ToString(initiate.TX_correct), BitConverter.ToString(initiate.RX_correct), "", "");


            //PEEK // Still needs 1E-2D
            for (int i = 0; i < 45; i++)
            {
                AutoCommands peek = new AutoCommands();
                peek.sCommand = "PEEK" + "-" + i.ToString("X2");
                peek.bAddress = (byte)i;
                peek.bCommand = 0x82;
                byte[] data = new byte[1];
                data[0] = (byte)i;
                peek.TX_correct = SLIP_Frame.CreateSLIPframe(0x07, 0x11, 0x82, data);
                peek.RX_correct = NRWA_Cmnds.cmnd_Gen(_serialPort, peek.TX_correct);
                peek.RX_correct = RX_Verification.removeEndingZeros(peek.RX_correct);
                l_NRWACommands.Add(peek);
               // LogWriter.AppendLog(sSelectedPath, sFilename, peek.sCommand, "Correct", BitConverter.ToString(peek.TX_correct), BitConverter.ToString(peek.RX_correct), "", "");

            }

            //POKE // 
            for (int i = 0; i < 45; i++)
            {
                AutoCommands poke = new AutoCommands();
                poke.sCommand = "POKE" + "-" + i.ToString("X2");
                poke.bAddress = (byte)i;
                poke.bCommand = 0x83;
                byte[] data = new byte[5] { 0x00, 0x00, 0x00, 0x00, 0x00 };
                data[0] = (byte)i;
                poke.TX_correct = SLIP_Frame.CreateSLIPframe(0x07, 0x11, 0x83, data);
                l_NRWACommands.Add(poke);
              //  LogWriter.AppendLog(sSelectedPath, sFilename, poke.sCommand, "Correct", BitConverter.ToString(poke.TX_correct), "", "", "");

            }

            //SYSTEM-TELEMETRY
            for (int i = 160; i < 182; i++)
            {
                AutoCommands sysTel = new AutoCommands();
                sysTel.sCommand = "SYS_TEL" + "-" + i.ToString("X2");
                sysTel.bAddress = (byte)i;
                sysTel.bCommand = 0x84;
                byte[] data = new byte[1];
                data[0] = (byte)i;
                sysTel.TX_correct = SLIP_Frame.CreateSLIPframe(0x07, 0x11, 0x84, data);
                sysTel.RX_correct = NRWA_Cmnds.cmnd_Gen(_serialPort, sysTel.TX_correct);
                sysTel.RX_correct = RX_Verification.removeEndingZeros(sysTel.RX_correct);
                l_NRWACommands.Add(sysTel);
               // LogWriter.AppendLog(sSelectedPath, sFilename, sysTel.sCommand, "Correct", BitConverter.ToString(sysTel.TX_correct), BitConverter.ToString(sysTel.RX_correct), "", "");

            }

            //APPLICATION - TELEMETRY
            for (int i = 0; i < 3; i++)
            {
                AutoCommands sysApp = new AutoCommands();
                sysApp.sCommand = "APP_TEL" + "-" + i.ToString("X2");
                sysApp.bAddress = (byte)i;
                byte[] data = new byte[1];
                data[0] = (byte)i;
                sysApp.TX_correct = SLIP_Frame.CreateSLIPframe(0x07, 0x11, 0x87, data);
                sysApp.RX_correct = NRWA_Cmnds.cmnd_Gen(_serialPort, sysApp.TX_correct);
                sysApp.RX_correct = RX_Verification.removeEndingZeros(sysApp.RX_correct);
                l_NRWACommands.Add(sysApp);
               // LogWriter.AppendLog(sSelectedPath, sFilename, sysApp.sCommand, "Correct", BitConverter.ToString(sysApp.TX_correct), BitConverter.ToString(sysApp.RX_correct), "", "");

            }
           

        }

        private async Task DeserializeJson()
        {
            try
            {
                var httpNRWAvariables = await httpClient.GetAsync(url);
                string jsonResponse = await httpNRWAvariables.Content.ReadAsStringAsync();
                Console.WriteLine(jsonResponse);
                var NRWAvarObjects = JsonConvert.DeserializeObject<AutoCommands.NRWAConfigDRV110>(jsonResponse);


            }
            catch (Exception)
            {

                throw;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void NRWA_FirmVer_Load(object sender, EventArgs e)
        {
            dgvTX.Rows[0].Cells[0].Value = "C0";
            dgvTX.Rows[0].Cells[1].Value = "07";
            dgvTX.Rows[0].Cells[2].Value = "11";
            dgvTX.Rows[0].Cells[3].Value = "80";
            dgvTX.Rows[0].Cells[5].Value = "C6 E8";
            dgvTX.Rows[0].Cells[6].Value = "C0";
        }

        private async void button1_Click_1(object sender, EventArgs e)
        {
            if (sSelectedPath == "" || sFilename == "")
            {
                btnLog.PerformClick();
            }
            else
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    ShowSerialData("Creating Command Objects");
                    CreateCommandObjects();
                    ShowSerialData("Deserialize NRWA variable Configuration");
                    await Task.Run(() => DeserializeJson());
                    ShowSerialData("Verifying");
                    (int iTotal, int iStep) = ProgressBarSetUp();
                    Verify(iStep);
                    this.Cursor = Cursors.Default;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    this.Cursor = Cursors.Default;
                    ShowSerialData("FAILED\n");

                }
            }
        }

        private void btnTXsend_Click(object sender, EventArgs e)
        {
            if (sSelectedPath == "" || sFilename == "")
            {
                btnLog.PerformClick();
            }
            else
            {
                try
                {
                    int j = 0;
                    byte[] a_FEND_start = null;
                    byte[] a_DestAdd = null;
                    byte[] a_SourceAdd = null;
                    byte[] a_Command = null;
                    byte[] a_Data = null;
                    byte[] a_CRC = null;
                    byte[] a_FEND_end = null;

                    this.Cursor = Cursors.WaitCursor;

                    //FEND Start
                    if (dgvTX.Rows[0].Cells[0].Value == null)
                    {
                        a_FEND_start = null;
                    }
                    else
                    {
                        try
                        {
                            string[] temp = dgvTX.Rows[0].Cells[0].Value.ToString().Split(' ');
                            a_FEND_start = new byte[temp.Length];
                            for (int i = 0; i < temp.Length; i++)
                            {
                                a_FEND_start[i] = Convert.ToByte(temp[i], 16);
                            }
                        }
                        catch
                        {
                            a_FEND_start = new byte[1];
                            a_FEND_start[0] = Convert.ToByte(dgvTX.Rows[0].Cells[0].Value.ToString(), 16);
                        }
                        j += a_FEND_start.Length;
                    }
                    

                    //Destination Address
                    if (dgvTX.Rows[0].Cells[1].Value == null)
                    {
                        a_DestAdd = null;
                    }
                    else
                    {
                        try
                        {
                            string[] temp = dgvTX.Rows[0].Cells[1].Value.ToString().Split(' ');
                            a_DestAdd = new byte[temp.Length];
                            for (int i = 0; i < temp.Length; i++)
                            {
                                a_DestAdd[i] = Convert.ToByte(temp[i], 16);
                            }
                        }
                        catch
                        {
                            a_DestAdd = new byte[1];
                            a_DestAdd[0] = Convert.ToByte(dgvTX.Rows[0].Cells[1].Value.ToString(), 16);
                        }
                        j += a_DestAdd.Length;
                    }
                    

                    //Source Address
                    if (dgvTX.Rows[0].Cells[2].Value == null)
                    {
                        a_SourceAdd = null;
                    }
                    else
                    {
                        try
                        {
                            string[] temp = dgvTX.Rows[0].Cells[2].Value.ToString().Split(' ');
                            a_SourceAdd = new byte[temp.Length];
                            for (int i = 0; i < temp.Length; i++)
                            {
                                a_SourceAdd[i] = Convert.ToByte(temp[i], 16);
                            }
                        }
                        catch
                        {
                            a_SourceAdd = new byte[1];
                            a_SourceAdd[0] = Convert.ToByte(dgvTX.Rows[0].Cells[2].Value.ToString(), 16);
                        }
                        j += a_SourceAdd.Length;
                    }


                    //Command
                    if (dgvTX.Rows[0].Cells[3].Value == null)
                    {
                        a_Command = null;
                    }
                    else
                    {
                        try
                        {
                            string[] temp = dgvTX.Rows[0].Cells[3].Value.ToString().Split(' ');
                            a_Command = new byte[temp.Length];
                            for (int i = 0; i < temp.Length; i++)
                            {
                                a_Command[i] = Convert.ToByte(temp[i], 16);
                            }
                        }
                        catch
                        {
                            a_Command = new byte[1];
                            a_Command[0] = Convert.ToByte(dgvTX.Rows[0].Cells[3].Value.ToString(), 16);
                        }
                        j += a_Command.Length;
                    }


                    //Data
                    if (dgvTX.Rows[0].Cells[4].Value == null)
                    {
                        a_Data = null;
                    }
                    else
                    {
                        try
                        {
                            string[] temp = dgvTX.Rows[0].Cells[4].Value.ToString().Split(' ');
                            a_Data = new byte[temp.Length];
                            for (int i = 0; i < temp.Length; i++)
                            {
                                a_Data[i] = Convert.ToByte(temp[i], 16);
                            }
                        }
                        catch
                        {
                            a_Data = new byte[1];
                            a_Data[0] = Convert.ToByte(dgvTX.Rows[0].Cells[4].Value.ToString(), 16);
                        }
                        j += a_Data.Length;
                    }


                    //CRC
                    if (dgvTX.Rows[0].Cells[5].Value == null)
                    {
                        if (cbCrc.Checked)
                        {
                            List<byte> l_btemp = new List<byte>();
                            if (a_DestAdd != null)
                            {
                                foreach (var item in a_DestAdd)
                                {
                                    l_btemp.Add(item);
                                }
                            }
                            if (a_SourceAdd != null)
                            {
                                foreach (var item in a_SourceAdd)
                                {
                                    l_btemp.Add(item);
                                }
                            }
                            if (a_Command != null)
                            {
                                foreach (var item in a_Command)
                                {
                                    l_btemp.Add(item);
                                }
                            }
                            if (a_Data != null)
                            {
                                foreach (var item in a_Data)
                                {
                                    l_btemp.Add(item);
                                }
                            }
                            SLIP_Frame.Crc16Base crc_CCITT_FALSE = new SLIP_Frame.Crc16Base(0x1021, 0xffff, 0x0000, false, false, 0x29B1);
                            byte[] bCRC = crc_CCITT_FALSE.ComputeHash(l_btemp.ToArray());

                            string sTemp = "";
                            foreach (var item in bCRC)
                            {
                                sTemp += item.ToString("X2") + " ";
                            }

                            sTemp = sTemp.TrimEnd(' ');
                            dgvTX.Rows[0].Cells[5].Value = sTemp;

                            try
                            {
                                string[] temp = dgvTX.Rows[0].Cells[5].Value.ToString().Split(' ');
                                a_CRC = new byte[temp.Length];
                                for (int i = 0; i < temp.Length; i++)
                                {
                                    a_CRC[i] = Convert.ToByte(temp[i], 16);
                                }
                            }
                            catch
                            {
                                a_CRC = new byte[1];
                                a_CRC[0] = Convert.ToByte(dgvTX.Rows[0].Cells[5].Value.ToString(), 16);
                            }
                        }
                        else
                        {
                            a_CRC = null;
                        }
                    }
                    else
                    {
                        if (cbCrc.Checked)
                        {
                            List<byte> l_btemp = new List<byte>();
                            if (a_DestAdd != null)
                            {
                                foreach (var item in a_DestAdd)
                                {
                                    l_btemp.Add(item);
                                }
                            }
                            if (a_SourceAdd != null)
                            {
                                foreach (var item in a_SourceAdd)
                                {
                                    l_btemp.Add(item);
                                }
                            }
                            if (a_Command != null)
                            {
                                foreach (var item in a_Command)
                                {
                                    l_btemp.Add(item);
                                }
                            }
                            if (a_Data != null)
                            {
                                foreach (var item in a_Data)
                                {
                                    l_btemp.Add(item);
                                }
                            }
                            SLIP_Frame.Crc16Base crc_CCITT_FALSE = new SLIP_Frame.Crc16Base(0x1021, 0xffff, 0x0000, false, false, 0x29B1);
                            byte[] bCRC = crc_CCITT_FALSE.ComputeHash(l_btemp.ToArray());

                            string sTemp = "";
                            foreach (var item in bCRC)
                            {
                                sTemp += item.ToString("X2") + " ";
                            }

                            sTemp = sTemp.TrimEnd(' ');
                            dgvTX.Rows[0].Cells[5].Value = sTemp;

                            try
                            {
                                string[] temp = dgvTX.Rows[0].Cells[5].Value.ToString().Split(' ');
                                a_CRC = new byte[temp.Length];
                                for (int i = 0; i < temp.Length; i++)
                                {
                                    a_CRC[i] = Convert.ToByte(temp[i], 16);
                                }
                            }
                            catch
                            {
                                a_CRC = new byte[1];
                                a_CRC[0] = Convert.ToByte(dgvTX.Rows[0].Cells[5].Value.ToString(), 16);
                            }
                        }
                        else
                        {
                            try
                            {
                                string[] temp = dgvTX.Rows[0].Cells[5].Value.ToString().Split(' ');
                                a_CRC = new byte[temp.Length];
                                for (int i = 0; i < temp.Length; i++)
                                {
                                    a_CRC[i] = Convert.ToByte(temp[i], 16);
                                }
                            }
                            catch
                            {
                                a_CRC = new byte[1];
                                a_CRC[0] = Convert.ToByte(dgvTX.Rows[0].Cells[5].Value.ToString(), 16);
                            }
                        }
                        
                        j += a_CRC.Length;
                    }


                    //FEND end
                    if (dgvTX.Rows[0].Cells[6].Value == null)
                    {
                        a_FEND_end = null;
                    }
                    else
                    {
                        try
                        {
                            string[] temp = dgvTX.Rows[0].Cells[6].Value.ToString().Split(' ');
                            a_FEND_end = new byte[temp.Length];
                            for (int i = 0; i < temp.Length; i++)
                            {
                                a_FEND_end[i] = Convert.ToByte(temp[i], 16);
                            }
                        }
                        catch
                        {
                            a_FEND_end = new byte[1];
                            a_FEND_end[0] = Convert.ToByte(dgvTX.Rows[0].Cells[6].Value.ToString(), 16);
                        }
                        j += a_FEND_end.Length;
                    }

                    byte[] a_TX = new byte[j];
                    j = 0;
                    if (a_FEND_start != null)
                    {
                        a_FEND_start.CopyTo(a_TX, j);
                        j += a_FEND_start.Length;
                    }
                    if (a_DestAdd != null)
                    {
                        a_DestAdd.CopyTo(a_TX, j);
                        j += a_DestAdd.Length;
                    }
                    if (a_SourceAdd != null)
                    {
                        a_SourceAdd.CopyTo(a_TX, j);
                        j += a_SourceAdd.Length;
                    }
                    if (a_Command != null)
                    {
                        a_Command.CopyTo(a_TX, j);
                        j += a_Command.Length;
                    }
                    if (a_Data != null)
                    {
                        a_Data.CopyTo(a_TX, j);
                        j += a_Data.Length;
                    }
                    if (a_CRC != null)
                    {
                        a_CRC.CopyTo(a_TX, j);
                        j += a_CRC.Length;
                    }
                    if (a_FEND_end != null)
                    {
                        a_FEND_end.CopyTo(a_TX, j);
                        j += a_FEND_end.Length;
                    }

                    rtbUser.Text += "TX: " + BitConverter.ToString(a_TX) + "\n";
                    byte[] a_RX = NRWA_Cmnds.cmnd_Gen(_serialPort, a_TX);

                    if (a_RX != null)
                    {
                        rtbUser.Text += "RX: " + BitConverter.ToString(RX_Verification.removeEndingZeros(a_RX)) + "\n\n";
                    }
                    else
                    {
                        rtbUser.Text += "RX: null\n\n";
                    }
                    this.Cursor = Cursors.Default;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void cbAll_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAll.Checked)
            {
                cbInitial.Checked = cbAll.Checked;
                cbPeekPoke.CheckState = CheckState.Checked;
                cbSystemTelemetryDataCast.CheckState = CheckState.Checked;
                cbPeekOutsideAd.CheckState = CheckState.Checked;
                cbPokeOutsideAd.CheckState = CheckState.Checked;
                cbPokeOutsideData.CheckState = CheckState.Checked;
                cbFalseCRC.CheckState = CheckState.Checked;
                cbAppTellDataCast.CheckState = CheckState.Checked;
                cbPokeIncorrectData.CheckState = CheckState.Checked;
            }
           
        }

        private void cbPeekPoke_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbPeekPoke.Checked && cbAll.Checked)
            { cbAll.CheckState = CheckState.Unchecked; }
        }


        public delegate void ShowSerialDatadelegate(string r);

        private void ShowSerialData(string s)
        {
            if (rtbLog.InvokeRequired)
            {
                ShowSerialDatadelegate SSDD = ShowSerialData;
                Invoke(SSDD, s);
            }
            else
            {
                rtbLog.AppendText(Environment.NewLine + s);
                rtbLog.ScrollToCaret();
            }
        }

        private void cbInitial_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbInitial.Checked && cbAll.Checked)
            { cbAll.CheckState = CheckState.Unchecked; }
        }

        private void cbAppTelData_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAppTelData.Checked)
            {
                cbAppTellDataCast.CheckState = CheckState.Checked;
            }
        }

        private void cbPorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.btnPortCon.ForeColor = System.Drawing.Color.Black;
        }


        delegate void ToolStripPrograssDelegate(int value);
        
        private (int, int) ProgressBarSetUp()
        {
            int iTotal = 0;
            int iStep = 0;

            if (cbAll.Checked)
            {
                iTotal = 10;
                iStep = Convert.ToInt32(Math.Floor(100 / (double)iTotal));
                return (iStep, iTotal);
            }
            else
            {
                if (cbInitial.Checked)
                { iTotal++; }
                if (cbSystemTelemetryDataCast.Checked)
                { iTotal++; }
                if (cbPeekOutsideAd.Checked)
                { iTotal++; }
                if (cbPokeOutsideAd.Checked)
                { iTotal++; }
                if (cbPokeOutsideData.Checked)
                { iTotal++; }
                if (cbFalseCRC.Checked)
                { iTotal++; }
                if (cbAppTellDataCast.Checked)
                { iTotal++; }
                if (cbAppTelData.Checked)
                { iTotal++; }
                if (cbPokeIncorrectData.Checked)
                { iTotal++; }

                iStep = Convert.ToInt32(Math.Floor(100 / (double)iTotal));

                return (iStep, iTotal);
            }

        }
    }
}
