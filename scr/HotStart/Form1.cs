using System;
using System.Windows.Forms;
//System Directives used
using System.Diagnostics;
using System.Security.Principal;

namespace HotStart
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;                                                  //Disables the 'Stop' button, It must be disabled until the app starts the hotspot
            button3.Enabled = false;                                                  //Disables the 'Restart' button, It must be disabled until the app starts the hotspot
            label6.ForeColor = System.Drawing.Color.Red;                              //Change the color of the Status label to Red color Meaning An Error, in this case is that the hotspot is Stopped 
            label6.Text = "Stopped!";                                                 //Change the value in the Status label to 'Stopped'
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        //Starting button >> Button #1
        private void button1_Click(object sender, EventArgs e)
        {
            label6.Text = "Starting...";                                               //Changes the status label to 'Starting'     
            string ssid = textBox1.Text; string key = textBox2.Text;                   //Declares the variables and storage the values from TextBox1 & TextBox2 in them

            if (String.IsNullOrEmpty(ssid))                                            //Check if TextBox #1 is empty
            {
                MessageBox.Show("Network Name (SSID) cannot be left blank...","Invalid SSID",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                label6.Text = "Stopped!";
                goto endhotspot;
            }
            else if (String.IsNullOrEmpty(key))                                        //Cheack if TextBox #2 is empty
            {
                MessageBox.Show("Network Key cannot be left blank !", "Invalid Key", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                label6.Text = "Stopped!";
                goto endhotspot;
            }
            else if(textBox2.Text.Length < 8)                                          //Check if Entered Key is valid (8 characters or more)
            {
                MessageBox.Show("Network Key must have at least 8 characters !", "Invalid Key", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                label6.Text = "Stopped!";
                goto endhotspot;
            }
            else
            {
                textBox1.Enabled = false;                                               //Disables the SSID input TextBox
                textBox2.Enabled = false;                                               //Disables the KEY input TextBox
            }
            //Start the hotspot
            button1.Enabled = false;                                                    //Disables Start Button
            HotspotRun(ssid, key);                                                      //Start the Hostpot Using the Inputted SSid & KEY
            button2.Enabled = true;                                                     //Enables the Stop button
            button3.Enabled = true;                                                     //Enables the Restart button
            label6.ForeColor = System.Drawing.Color.Green;
            label6.Text = "Started!";                                                    //Changes the value of the Status Label to 'Running'
            endhotspot:;
        }
        #region Methods
        //Hotspot Run Method >> Starts the Hotspot creating a hidden system process and calling the console...
        public static void HotspotRun(string ssid, string key)                         //Declares the input variables                               
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe");       //Creates a new system process structure and calls 'CMD'
            processStartInfo.RedirectStandardInput = true;                             //Redirects all code of the 'StandardInput' form to the process
            processStartInfo.RedirectStandardOutput = true;                            //Redirects all output of the process using the 'StandarOutput' form
            processStartInfo.CreateNoWindow = true;                                    //Creates the process as hidden in the background
            processStartInfo.UseShellExecute = false;                                  //Tells the process to use no Shell
            Process process = Process.Start(processStartInfo);                         //Finally we create the actual process using the 'processStartInfo' form

            if (process != null)                                                       //If the process creation was succesfully
            {
                process.StandardInput.WriteLine("netsh wlan set hostednetwork mode=allow ssid=" + ssid + " key=" + key);    //Run the 'CMD' commands for configuring the hosted network using the SSID & KEY variables
                process.StandardInput.WriteLine("netsh wlan start hosted network");    //Run the 'CMD' commands for starting the hosted network
                process.StandardInput.Close();                                         //Closes the process
            }
           
        }
        //Stop Hotspot Method >> Stops the Hotspot creating a hidden system process and calling the console...
        public static void HotspotStop()
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe");        //Creates a new system process structure and calls 'CMD'
            processStartInfo.RedirectStandardInput = true;                              //Redirects all code of the 'StandardInput' form to the process
            processStartInfo.RedirectStandardOutput = true;                             //Redirects all output of the process using the 'StandarOutput' form
            processStartInfo.CreateNoWindow = true;                                     //Creates the process as hidden in the background
            processStartInfo.UseShellExecute = false;                                   //Tells the process to use no Shell
            Process process = Process.Start(processStartInfo);                          //Finally we create the actual process using the 'processStartInfo' form

            if (process != null)                                                        //If the process creation was succesfully
            {
                process.StandardInput.WriteLine("netsh wlan stop hostednetwork");       //Run the 'CMD' commands for stopping the hosted network
                process.StandardInput.Close();                                          //Closes the process
            }
        }
        //Reset Method >> Calls the other two methods in reverse order...
        public static void HotspotReset(string ssid,string key)
        {
            HotspotStop();                                       //Calls the HotspotStop method                        
            HotspotRun(ssid, key);                               //Calls the HotspotRun method                         >> SSID & KEY
        }
        #endregion
        //Resetting Hotspot >> Button #3
        private void button3_Click(object sender, EventArgs e)
        {
            string ssid = textBox1.Text, key = textBox2.Text;    //Declares the variables                               >> SSID & KEY
            button1.Enabled = false;                             //Disables Start Hotspot buttton                       >> Button #1
            button2.Enabled = false;                             //Disables Stop Hotspot button                         >> Button #2
            button3.Enabled = false;                             //Disables Restart Hotspot button                      >> Button #3
            textBox1.Enabled = false;                            //Disables First textbox, where user inputs SSID       >> TextBox #1
            textBox2.Enabled = false;                            //Disables Second textbox, where user inputs KEY       >> TextBox #2
            label6.ForeColor = System.Drawing.Color.Yellow;      //Changes the Color of the Status label to Yellow      >> Label #6
            label6.Text = "Restarting..";                        //Changes the Status label to 'Restarting'             >> Label #6
            HotspotReset(ssid, key);                             //Calls the HotspotReset method, using the variables:  >> SSID & KEY
            label6.ForeColor = System.Drawing.Color.Green;       //Changes the Color of the Status label to Green       >> Label #6
            label6.Text = "Started!";                            //Changes the Status label to 'Hotspot Started!'       >> Label #6
            button2.Enabled = true;                              //Enables the Stop Hotspot button                      >> Button #2
            button3.Enabled = true;                              //Enables the Restart Hotspot button                   >> Button #3
        }
        //Stop Hotspot button >> Button #2
        private void button2_Click(object sender, EventArgs e)
        {
            label6.Text = "Stopping...";
            button2.Enabled = false;
            button3.Enabled = false;
            HotspotStop();
            button1.Enabled = true;
            label6.ForeColor = System.Drawing.Color.Red;
            label6.Text = "Stopped!";
            textBox1.Enabled = true;
            textBox2.Enabled = true;
        }
        //Help button >> Button #4
        private void button4_Click(object sender, EventArgs e)
        {
            HelpForm helpform = new HelpForm();                  //Create an instance of the Help form class            >> HelpForm
            helpform.ShowDialog();                               //We Show the Form Dialog
        }
    }
}
