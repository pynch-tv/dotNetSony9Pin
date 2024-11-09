using System.IO.Ports;
using Pynch.Sony9Pin.Core.Sony9Pin.CommandBlocks;

namespace lathoub;

public abstract class Sony9Pin : RequestResponsePump<CommandBlock, CommandBlock>
{
    protected readonly SerialPort SerialPort = new();

    public string Port => SerialPort.PortName;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="host"></param>
    /// <param name="port"></param>
    public virtual bool Connect(string port)
    {
        SerialPort.PortName = port;
        SerialPort.BaudRate = 38400;
        SerialPort.DataBits = 8;
        SerialPort.StopBits = StopBits.One;
        SerialPort.Parity = Parity.Odd;
        SerialPort.Handshake = Handshake.None;
        SerialPort.DtrEnable = true;
        SerialPort.RtsEnable = true;
        SerialPort.ReadTimeout = 250; // Sony9Pin specs indicate 9ms

        SerialPort.Open();

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public virtual void Disconnect()
    {
        SerialPort.Close();
    }
}
