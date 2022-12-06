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
            string[] Columns = new string[6] { "Command", "Test", "TX", "RX", "Data", "Pass" };
            Log(Columns, m_exePath, filename);

        }

        public static void AppendLog(string sSelectedPath, string sFilename, string sCommand, string sTest, string sTx, string sRx, string sData, string sPassFail)
        {
            string[] sLog = { "", "", "", "", "", ""};
            sLog[0] = sCommand;
            sLog[1] = sTest;
            sLog[2] = sTx;
            sLog[3] = sRx;
            sLog[4] = sData;
            sLog[5] = sPassFail;

            LogWriter.Log(sLog, sSelectedPath, sFilename);

        }
    }
}
