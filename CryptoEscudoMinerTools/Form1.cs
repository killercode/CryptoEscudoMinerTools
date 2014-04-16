using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Configuration;


namespace CryptoEscudoMinerTools
{
    public partial class Form1 : Form
    {
        private ProcessCaller processCaller;
        private String strPool = "";
        public Form1()
        {
            InitializeComponent();
            try
            {
                
                txtUser.Text = Properties.Settings.Default.user;
                txtWorker.Text = Properties.Settings.Default.worker;
                txtPassword.Text = Properties.Settings.Default.password;
                if ((int)Properties.Settings.Default.pool == 1)
                {
                    rb1.Checked = true;
                }
                if ((int)Properties.Settings.Default.pool == 2)
                {
                    rb2.Checked = true;
                }
                if ((int)Properties.Settings.Default.pool == 3)
                {
                    rb3.Checked = true;
                }
                if ((int)Properties.Settings.Default.pool == 4)
                {
                    rb4.Checked = true;
                }
                else
                {
                    rb1.Checked = true;
                }
            }
            catch
            {
            }
        }

        private void SaveSettings()
        {

                Properties.Settings.Default.user = txtUser.Text;
                Properties.Settings.Default.worker = txtWorker.Text;
                Properties.Settings.Default.password = txtPassword.Text;

            int opt = 1;
            if (this.rb1.Checked)
                opt = 1;
            else if (rb2.Checked)
                opt = 2;
            else if (rb3.Checked)
                opt = 3;
            else if (rb4.Checked)
                opt = 4;
            Properties.Settings.Default.pool = opt;
            Properties.Settings.Default.Save();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveSettings();           
            button1.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            processCaller = new ProcessCaller(this);
            //processCaller.FileName = @"..\..\hello.bat";
            processCaller.FileName = @"Miners/GPUNVIDIA/cudaminer.exe";
            processCaller.Arguments = "-i 0 -C 2 -l 32x4 -m 1 -o stratum+tcp://" + strPool + "  -O " +txtUser.Text+ "." + txtWorker.Text+ ":" + txtPassword.Text;
            processCaller.StdErrReceived += new DataReceivedHandler(writeStreamInfo);
            processCaller.StdOutReceived += new DataReceivedHandler(writeStreamInfo);
            processCaller.Completed += new EventHandler(processCompletedOrCanceled);
            processCaller.Cancelled += new EventHandler(processCompletedOrCanceled);
            // processCaller.Failed += no event handler for this one, yet.

            this.textBox1.Text = "A começar a minar..." + Environment.NewLine;

            // the following function starts a process and returns immediately,
            // thus allowing the form to stay responsive.
            processCaller.Start();  
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (processCaller != null)
            {
                processCaller.Cancel();
                textBox1.Text += "\r\n Processo Parado!";
            }
        }

        /// <summary>
        /// Handles the events of StdErrReceived and StdOutReceived.
        /// </summary>
        /// <remarks>
        /// If stderr were handled in a separate function, it could possibly
        /// be displayed in red in the richText box, but that is beyond 
        /// the scope of this demo.
        /// </remarks>
        private void writeStreamInfo(object sender, DataReceivedEventArgs e)
        {
            this.textBox1.AppendText(e.Text + Environment.NewLine);
        }

        /// <summary>
        /// Handles the events of processCompleted & processCanceled
        /// </summary>
        private void processCompletedOrCanceled(object sender, EventArgs e)
        {
            this.button1.Enabled = true;
            this.button3.Enabled = true;
            this.button4.Enabled = true;
        }

        private void rb1_CheckedChanged(object sender, EventArgs e)
        {
            strPool = "cesc.hashing.at:3028";
        }

        private void rb2_CheckedChanged(object sender, EventArgs e)
        {
            strPool = "cesc.pool.mn:7208";
        }

        private void rb3_CheckedChanged(object sender, EventArgs e)
        {
            strPool = "cesc.hashfever.com:3281";
        }

        private void rb4_CheckedChanged(object sender, EventArgs e)
        {
            strPool = "cesc.okaypool.com:3398";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveSettings();
            button1.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            processCaller = new ProcessCaller(this);
            //processCaller.FileName = @"..\..\hello.bat";
            processCaller.FileName = @"Miners/CPU/minerd.exe";
            processCaller.Arguments = " --url stratum+tcp://" + strPool + " -u " + txtUser.Text + "." + txtWorker.Text + " -p " + txtPassword.Text;
            processCaller.StdErrReceived += new DataReceivedHandler(writeStreamInfo);
            processCaller.StdOutReceived += new DataReceivedHandler(writeStreamInfo);
            processCaller.Completed += new EventHandler(processCompletedOrCanceled);
            processCaller.Cancelled += new EventHandler(processCompletedOrCanceled);
            // processCaller.Failed += no event handler for this one, yet.

            this.textBox1.Text = "A começar a minar..." + Environment.NewLine;

            // the following function starts a process and returns immediately,
            // thus allowing the form to stay responsive.
            processCaller.Start();  
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveSettings();
            button1.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            processCaller = new ProcessCaller(this);
            //processCaller.FileName = @"..\..\hello.bat";
            processCaller.FileName = @"Miners/CPU/minerd.exe";
            processCaller.Arguments = " --scrypt --url stratum+tcp://" + strPool + " -u " + txtUser.Text + "." + txtWorker.Text + " -p " + txtPassword.Text + "  --thread-concurrency 4096 -I 10 -g 1 -w 256";
            processCaller.StdErrReceived += new DataReceivedHandler(writeStreamInfo);
            processCaller.StdOutReceived += new DataReceivedHandler(writeStreamInfo);
            processCaller.Completed += new EventHandler(processCompletedOrCanceled);
            processCaller.Cancelled += new EventHandler(processCompletedOrCanceled);
            // processCaller.Failed += no event handler for this one, yet.

            this.textBox1.Text = "A começar a minar..." + Environment.NewLine;

            // the following function starts a process and returns immediately,
            // thus allowing the form to stay responsive.
            processCaller.Start();  
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon1.Visible = true;
                notifyIcon1.BalloonTipText = "A aplicação continua a minar...";
                notifyIcon1.BalloonTipTitle = "CryptoEscudo Miner Tools";
                notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                notifyIcon1.ShowBalloonTip(500);
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible = false;
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("cryptoescudo:CP9GYqQHaKzwkih9cgTzWMFZ8xR2Ne6h5z?label=Donations");
            Process.Start(sInfo);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("http://www.cryptoescudo.org/");
            Process.Start(sInfo);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("mailto:alves_diogo@hotmail.com");
            Process.Start(sInfo);
        }

    }
}
