using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRWA_Communication_Acceptance
{
    internal class LogWriter
    {
        public static void Log(string[] logMessage, string m_exePath, string filename)
        {
            StreamWriter w = File.AppendText(m_exePath + "\\" + filename);
            TextWriter txtWriter = w;

            try
            {
                txtWriter.WriteLine(string.Join(",", logMessage));
            }
            catch (Exception ex)
            {
            }
            txtWriter.Close();
        }

        public static void Create(string m_exePath, string filename)
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            File.Create(m_exePath + "\\" + filename).Close();
            string[] Columns = new string[10] { "Data", "TX", "RX", "Data", "Message Control", "Field 1", "Field 2", "Field 3", "Field 4", "Field 5" };
            Log(Columns, m_exePath, filename);

        }

        public static void AppendLog(string sSelectedPath, string sFilename, string sTX, string sRx, string sData, string sCommand, string sField1, string sField2, string sField3, string sField4, string sField5)
        {
            string[] sLog = { "", "", "", "", "", "", "", "", "", "" };
            sLog[0] = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            sLog[1] = sTX;
            sLog[2] = sRx;
            sLog[3] = sData;
            sLog[4] = sCommand;
            sLog[5] = sField1;
            sLog[6] = sField2;
            sLog[7] = sField3;
            sLog[8] = sField4;
            sLog[9] = sField5;

            LogWriter.Log(sLog, sSelectedPath, sFilename);

        }
    }
}
