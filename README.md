**PLC Communication Windows App (SLMP Protocol)**
This Windows Forms application is developed in C# to enable communication with Mitsubishi PLCs using the SLMP (Seamless Message Protocol) over the default port 12289. The application facilitates seamless interaction between a PC and PLC for data exchange, making it ideal for applications that require real-time data reading and writing.

Key Features
Read Operations: The application allows you to retrieve data from various PLC registers. This feature is useful for monitoring real-time data such as sensor values or system status from the PLC.
Write Operations: It supports writing values to the PLC registers, allowing you to control or modify system parameters remotely from your PC.
Real-Time Communication Monitoring: The results of read and write operations are displayed instantly, providing real-time feedback on the status of the PLC communication. This ensures users can verify operations are successful and troubleshoot issues efficiently.
User-Friendly Interface: Designed for simplicity, the application offers an intuitive Windows Forms interface. This allows users to easily configure the PLC connection, perform operations, and view results with minimal setup required.
SLMP Protocol Support: The application uses SLMP, Mitsubishi’s specialized protocol, over port 12289, ensuring compatibility with Mitsubishi PLCs.
How to Use
Launch the Application: Open the application on your Windows PC.
Enter PLC IP Address and Port: In the interface, input the PLC’s IP address and the port number (default port is 12289).
Perform Read/Write Operations: Use the interface to select registers and perform read or write operations as required.
View Results: The real-time communication status and operation results will be displayed instantly on the screen.
Technologies Used
C# for application development.
Windows Forms for the graphical user interface (GUI).
SLMP Protocol for communication with Mitsubishi PLCs.
Setup Instructions
Clone the Repository:
bash
Copy code
git clone https://github.com/niteesh017/PLC-Communication-App-SLMP-Protocol-.git  
Open in Visual Studio: After cloning, open the solution in Visual Studio.
Build and Run: Build the project and run the application.
Requirements
Mitsubishi PLC with SLMP support enabled.
.NET Framework or compatible runtime installed on your machine.
Advantages
Simplified Integration: Connect and communicate with Mitsubishi PLCs with minimal configuration.
Real-Time Control: Instant feedback on PLC operations for better monitoring and troubleshooting.
Efficient Operation: Perform essential PLC tasks like reading sensor data and sending control commands with ease.
