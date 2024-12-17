using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PLC_SLMP
{
    public partial class Form1 : Form
    {
        private TcpClient tcpClient;
        private NetworkStream networkStream;
        public Form1()
        {
            InitializeComponent();
        }
        private async void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IPAddress.TryParse(txtIpAddress.Text, out _))
                {
                    lblStatus.Text = "Invalid IP Address.";
                    MessageBox.Show("Invalid IP Address.");
                    return;
                }

                int port = string.IsNullOrWhiteSpace(txtPort.Text) ? 12289 : int.Parse(txtPort.Text);
                if (port < 1 || port > 65535)
                {
                    lblStatus.Text = "Invalid Port Number.";
                    MessageBox.Show("Invalid Port Number.");
                    return;
                }

                tcpClient = new TcpClient();
                await tcpClient.ConnectAsync(txtIpAddress.Text, port);
                networkStream = tcpClient.GetStream();

                btnSend.Enabled = true;
                btnDisconnect.Enabled = true;
                btnConnect.Enabled = false;
                chkReadOperation.Enabled = true;
                chkWriteOperation.Enabled = true;
                lblStatus.Text = $"Connected to PLC successfully on port {port}!";
                //pictureBoxStatus.Image = Properties.Resources.green;
                UpdateConnectionStatus(true); 


            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error connecting to PLC: {ex.Message}";
                MessageBox.Show($"Error connecting to PLC: {ex.Message}");
                UpdateConnectionStatus(false);
            }
        }

        private void UpdateConnectionStatus(bool isConnected)
        {
            try
            {
                string imagePath = isConnected ? @"E:\PLC_SLMP\PLC_SLMP\Resources\green.png" : @"E:\PLC_SLMP\PLC_SLMP\Resources\red.jpg";
                               
                pictureBoxStatus.Image = Image.FromFile(imagePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}");
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            Disconnect();
        }

        private void Disconnect()
        {
            try
            {
                if (tcpClient != null && tcpClient.Connected)
                {
                    networkStream?.Close();
                    tcpClient?.Close();
                    tcpClient = null;
                    networkStream = null;
                }

                btnSend.Enabled = false;
                btnDisconnect.Enabled = false;
                btnConnect.Enabled = true;
                chkReadOperation.Enabled= false;
                chkWriteOperation.Enabled= false;
                UpdateConnectionStatus(false);
                lblStatus.Text = "Disconnected from PLC.";
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error disconnecting: {ex.Message}";
                MessageBox.Show($"Error disconnecting: {ex.Message}");
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (networkStream == null || !tcpClient.Connected)
                {
                    lblStatus.Text = "Please connect to the PLC first.";
                    MessageBox.Show("Please connect to the PLC first.");
                    return;
                }

                string startingAddressText = txtCommand.Text.Trim();
                if (string.IsNullOrEmpty(startingAddressText))
                {
                    MessageBox.Show("Please enter a valid starting address (e.g., D200).", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool isWriteOperation = chkWriteOperation.Checked;
                byte[] writeData = null;

                if (isWriteOperation)
                {
                    string writeDataText = txtWriteData.Text.Trim();
                    if (string.IsNullOrEmpty(writeDataText))
                    {
                        MessageBox.Show("Please enter valid data to write.");
                        return;
                    }

                    if ((writeDataText.Length / 2) % 2 != 0)
                    {
                        var result = MessageBox.Show("Write data is incomplete for a word. Do you want to pad with 0x00?",
                                                     "Incomplete Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            writeDataText += "00";
                            Console.WriteLine($"Padded Write Data: {writeDataText}");
                        }
                        else
                        {
                            MessageBox.Show("Please enter valid data to complete the word.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    writeData = new byte[writeDataText.Length / 2];
                    for (int i = 0; i < writeData.Length; i++)
                    {
                        writeData[i] = Convert.ToByte(writeDataText.Substring(i * 2, 2), 16);
                    }
                    Console.WriteLine($"Write Data (Bytes): {BitConverter.ToString(writeData)}");
                }

                byte[] slmpRequest = ConstructSLMPCommand(startingAddressText, isWriteOperation, writeData);
                Console.WriteLine($"SLMP Request: {BitConverter.ToString(slmpRequest)}");

                lblStatus.Text = isWriteOperation ? "Sending Write Command..." : "Sending Read Command...";

                string response = await SendSLMPMessageAsync(slmpRequest);

                Console.WriteLine($"SLMP Response: {response}");
                txtResponse.Text = response;

                lblStatus.Text = isWriteOperation ? "Write Command Sent Successfully!" : "Read Command Sent Successfully!";
                await MonitorPLCRegisterAsync();
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error sending command: {ex.Message}";
                MessageBox.Show($"Error sending command: {ex.Message}");
            }
        }

        private async Task<string> SendSLMPMessageAsync(byte[] message)
        {
            if (networkStream == null || !tcpClient.Connected)
            {
                throw new InvalidOperationException("Not connected to the PLC.");
            }

            await networkStream.WriteAsync(message, 0, message.Length);
            lblStatus.Text = "Command Sent!";

            byte[] response = new byte[1024];
            int bytesRead = await networkStream.ReadAsync(response, 0, response.Length);
            Console.WriteLine(bytesRead.ToString() + " Bytes read");

            if (bytesRead <= 0)
            {
                MessageBox.Show("No response received from PLC.");
                return "Error: No response";
            }

            string responseHex = BitConverter.ToString(response, 0, bytesRead);
            //MessageBox.Show($"Received Response (Hex): {responseHex}");

            string responseString = DecodeSLMPResponse(response, bytesRead);

            if (txtResponse.InvokeRequired)
            {
                txtResponse.Invoke(new Action(() => { txtResponse.Text = responseString; }));
            }
            else
            {
                txtResponse.Text = responseString;
            }

            return responseString;
        }

        private string DecodeSLMPResponse(byte[] response, int bytesRead)
        {
            try
            {
                if (bytesRead < 12)
                {
                    return $"Error: Incomplete response (expected at least 12 bytes, got {bytesRead} bytes).";
                }

                if (response[9] != 0x00 || response[10] != 0x00)
                {
                    return $"Error: Response status indicates failure. Status: {response[9]:X2}{response[10]:X2}";
                }

                int dataIndex = 11;
                if (bytesRead >= dataIndex + 2)
                {
                    ushort value = BitConverter.ToUInt16(response, dataIndex);
                    Console.WriteLine(value);
                    return $"PLC Data Value: {value}";
                }

                return "Error: Unexpected or incomplete response format.";
            }
            catch (Exception ex)
            {
                return $"Error decoding response: {ex.Message}";
            }
        }

        private byte[] ConstructSLMPCommand(string startingAddressText, bool isWriteOperation, byte[] writeData = null)
        {
            startingAddressText = startingAddressText.Trim();
            Console.WriteLine($"Starting Address: {startingAddressText}");

            if (string.IsNullOrEmpty(startingAddressText) || !startingAddressText.StartsWith("D", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Please add a valid starting address in the format 'Dxxx' (e.g., D200).");
                throw new FormatException("Invalid starting address format.");
            }

            string addressNumberText = startingAddressText.Substring(1);
            if (!int.TryParse(addressNumberText, out int startingAddress) || startingAddress < 0)
            {
                MessageBox.Show("Please enter a valid starting address (positive integer after 'D').");
                throw new FormatException("Invalid starting address number.");
            }

            Console.WriteLine($"Starting Address Number: {startingAddress}");

            byte deviceCode = 0xA8;
            byte lowByte = (byte)(startingAddress & 0xFF);
            byte midByte = (byte)((startingAddress >> 8) & 0xFF);
            byte highByte = 0x00;

            byte[] command;

            if (isWriteOperation)
            {
                if (writeData == null || writeData.Length == 0)
                {
                    Console.WriteLine("Write data must be provided for a write operation.");
                    throw new ArgumentException("Write data must be provided for a write operation.");
                }

                if (writeData.Length % 2 != 0)
                {
                    Console.WriteLine("Write data must have an even number of bytes (for words).");
                    throw new ArgumentException("Write data must have an even number of bytes (for words).");
                }

                command = new byte[22 + writeData.Length];
                command[0] = 0x50;
                command[1] = 0x00;
                command[2] = 0x00;
                command[3] = 0xFF;
                command[4] = 0xFF;
                command[5] = 0x03;
                command[6] = 0x00;
                command[7] = 0x0C;
                command[8] = 0x00;
                command[9] = 0x10;
                command[10] = 0x00;
                command[11] = 0x01;  // Command
                command[12] = 0x04;  // Command function (write)
                command[13] = 0x00;
                command[14] = 0x00;
                command[15] = lowByte;  // Address
                command[16] = midByte;
                command[17] = highByte;
                command[18] = deviceCode;
                command[19] = (byte)(writeData.Length / 2);  // Number of words to write

                Array.Copy(writeData, 0, command, 20, writeData.Length);
            }
            else
            {
                command = new byte[]
                {
            0x50, 0x00, 0x00, 0xFF, 0xFF, 0x03, 0x00, 0x0C, 0x00, 0x10, 0x00, 0x01,
            0x04, 0x00, 0x00, lowByte, midByte, highByte, deviceCode, 0x01, 0x00
                };
            }

            Console.WriteLine($"Constructed SLMP Command: {BitConverter.ToString(command)}");

            return command;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Disconnect();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnSend.Enabled = false;
            btnDisconnect.Enabled = false;
            txtWriteData.Enabled = false;            
            txtCommand.Enabled = false; 
            chkWriteOperation.Enabled = false;
            chkReadOperation.Enabled = false;
            bool isConnected = tcpClient != null && tcpClient.Connected;
            UpdateConnectionStatus(isConnected);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Disconnect();
        }

        private void txtPort_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void txtResponse_TextChanged(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(txtResponse.Text))
            {
                Console.WriteLine("Response: " + txtResponse.Text);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtCommand_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWriteOperation.Checked)
            {
                txtWriteData.Enabled = true;
                txtCommand.Enabled=true;
            }
        }

        private void txtWriteData_TextChanged(object sender, EventArgs e)
        {

        }

        private async Task MonitorPLCRegisterAsync()
        {
            string lastKnownValue = string.Empty;

            while (tcpClient != null && tcpClient.Connected)
            {
                try
                {
                    string currentValue = await GetPLCRegisterValueAsync();

                    if (currentValue != lastKnownValue)
                    {
                        await TriggerWriteCommandIfNeeded();
                        lastKnownValue = currentValue;
                    }
                    
                    UpdateConnectionStatus(true); 
                }
                catch (Exception ex)
                {                  
                    lblStatus.Text = $"Error during monitoring: {ex.Message}";
                    MessageBox.Show($"Error during monitoring: {ex.Message}");
                  
                    UpdateConnectionStatus(false);
                    break;
                }

                await Task.Delay(1000); 
            }

            
            UpdateConnectionStatus(false);
        }

        private async Task<string> GetPLCRegisterValueAsync()
        {
            try
            {
                string startingAddressText = txtCommand.Text.Trim();
                byte[] slmpRequest = ConstructSLMPCommand(startingAddressText, false);

                string response = await SendSLMPMessageAsync(slmpRequest);
                return response;
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error reading PLC register: {ex.Message}";
                MessageBox.Show($"Error reading PLC register: {ex.Message}");
                return string.Empty;
            }
        }
        private async Task TriggerWriteCommandIfNeeded()
        {
            if (chkWriteOperation.Checked)
            {
                string writeDataText = txtWriteData.Text.Trim();
                byte[] writeData = null;

                if (string.IsNullOrEmpty(writeDataText))
                {
                    MessageBox.Show("Please enter valid data to write.");
                    return;
                }

                if ((writeDataText.Length / 2) % 2 != 0)
                {
                    var result = MessageBox.Show("Write data is incomplete for a word. Do you want to pad with 0x00?",
                                                 "Incomplete Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        writeDataText += "00";
                    }
                    else
                    {
                        MessageBox.Show("Please enter valid data to complete the word.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                writeData = new byte[writeDataText.Length / 2];
                for (int i = 0; i < writeData.Length; i++)
                {
                    writeData[i] = Convert.ToByte(writeDataText.Substring(i * 2, 2), 16);
                }

                byte[] writeCommand = ConstructSLMPCommand(txtCommand.Text.Trim(), true, writeData);

                await SendSLMPMessageAsync(writeCommand);
            }
        }

        private void chkReadOperation_CheckedChanged(object sender, EventArgs e)
        {
            if (chkReadOperation.Checked)
            {
                txtWriteData.Enabled = false;
                txtCommand.Enabled = true;
            }
        }
    }
}
