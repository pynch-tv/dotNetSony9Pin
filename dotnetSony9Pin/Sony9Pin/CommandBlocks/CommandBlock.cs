using System.Collections;
using System.Diagnostics;
using dotNetSony9Pin.Sony9Pin.CommandBlocks.Return;

namespace dotNetSony9Pin.Sony9Pin.CommandBlocks;

/// <summary>
/// </summary>
[DebuggerDisplay("Cmd1DataCount = {Cmd1DataCount}, Cmd2 = {Cmd2}, Data = {Data}, CheckSum = {CheckSum}")]
public class CommandBlock : IComparable, IEquatable<CommandBlock>
{
    #region Constructors and Destructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="CommandBlock" /> class.
    /// </summary>
    public CommandBlock()
        : this(0x00, 0x00, [])
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="CommandBlock" /> class.
    /// </summary>
    /// <param name="cmd1DataCount">
    /// </param>
    /// <param name="cmd2">
    /// </param>
    /// <param name="data">
    /// </param>
    public CommandBlock(byte cmd1DataCount, byte cmd2, byte[] data)
    {
        Cmd1DataCount = cmd1DataCount;
        Cmd2 = cmd2;
        Data = data;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="CommandBlock" /> class.
    /// </summary>
    /// <param name="cmd1">
    /// </param>
    /// <param name="dataCount">
    /// </param>
    /// <param name="cmd2">
    /// </param>
    /// <param name="data">
    /// </param>
    public CommandBlock(CommandFunction cmd1, int dataCount, byte cmd2, byte[] data)
        : this((byte)(((byte)cmd1 << 4) + dataCount), cmd2, data)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="CommandBlock" /> class.
    /// </summary>
    /// <param name="buffer">
    ///     The buffer.
    /// </param>
    public CommandBlock(byte[] buffer)
    {
        Debug.Assert(buffer.Length >= 3, "Buffer is not long enough");

        Cmd1DataCount = buffer[0];
        Cmd2 = buffer[1];

        var dataCount = Cmd1DataCount & 0x0F;
        Data = new byte[dataCount];
        Buffer.BlockCopy(buffer, 2, Data, 0, dataCount);
    }

    #endregion

    #region Public Properties

    /// <summary>
    ///     Gets or sets the Cmd1 (Leaving DataCount untouched)
    /// </summary>
    public CommandFunction Cmd1
    {
        get => GetCmd1(Cmd1DataCount);

        set => Cmd1DataCount = (byte)(Cmd1DataCount & 0x0F | (int)value << 4);
    }

    /// <summary>
    ///     Gets or sets the cmd 1 data count.
    /// </summary>
    public byte Cmd1DataCount { get; set; }

    /// <summary>
    ///     Gets or sets the cmd 2.
    /// </summary>
    public byte Cmd2 { get; set; }

    /// <summary>
    ///     Gets or sets the data.
    /// </summary>
    public byte[] Data { get; set; }

    /// <summary>
    ///     Gets or sets the DataCount (leaving Cmd1 untouched).
    /// </summary>
    public int DataCount
    {
        get => Cmd1DataCount & 0x0F;

        set => Cmd1DataCount = (byte)(Cmd1DataCount & 0xF0 | value);
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// </summary>
    /// <param name="cmd1DataCount"></param>
    /// <returns></returns>
    public static CommandFunction GetCmd1(byte cmd1DataCount)
    {
        return (CommandFunction)((cmd1DataCount & 0xF0) >> 4);
    }

    /// <summary>
    /// Converts the byte array representation of a commandblock. A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">A byte array containing a CommandBlock to convert.</param>
    /// <param name="result">When this method returns, contains the CommandBlock value equivalent of 
    /// the buffer contained in s, if the conversion succeeded, or zero if the conversion failed.
    ///  The conversion fails if the s parameter is null, is not of the correct format. This 
    /// parameter is passed uninitialized.</param>
    /// <returns>true if s was converted successfully; otherwise, false</returns>
    public static bool TryParse(List<byte> s, out CommandBlock? result)
    {
        result = null;

        try
        {
            // We need a minimum 3 bytes to work with (Cmd1Datacount, Cmd2 and Checksum)
            // See Cmd1 Block Format of the Sony 9-Pin remote protocol.
            if (s.Count < 3)
            {
                return false; // No, wait for more Data to come in.
            }

            //
            //Protocol.Cmd1 cmd1;
            //if (!Protocol.Cmd1.TryParse(s[0].ToString(), out cmd1))
            //    throw new ArgumentException();

            var cmd1 = GetCmd1(s[0]);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            // Parse content of the incoming Buffer
            // CMD-1 is the upper 4 bits, DATA COUNT is the lower 4.

            var dataCount = GetDataCount(s[0]);
            var commandBlockLength = 2 + dataCount + 1;
            // 2 = sizeof(Cmd1DataCount) + sizeof(Cmd2) + dataCount + sizeof(checksum)

            // Do we have enough bytes to work with?
            // (3 = length of cmd1datacount(1) + length of Cmd2(1) + length of checksum(1))
            if (s.Count < commandBlockLength)
            {
                return false; // No, wait for more Data to come in.
            }

            var cmd2 = s[1];

            var data = new byte[dataCount];
            for (var i = 0; i < dataCount; i++) data[i] = s[2 + i];

            // Check if the checksum is correct
            var checkSum = s[commandBlockLength - 1];

            // Checksum = lower eight bits of the sum of the bytes in the Cmd1 block
            // (exclusing the checksum itself)
            var calculatedCheckSum = 0;
            for (var i = 0; i < commandBlockLength - 1; i++)
            {
                calculatedCheckSum += s[i];
            }
            calculatedCheckSum &= 0xFF;

            if (calculatedCheckSum != checkSum)
            {
                result = new NakCommandBlock(NakCommandBlock.Nak.ChecksumError);
                return true;
            }

            result = new CommandBlock(cmd1, dataCount, cmd2, data);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="cmd1DataCount"></param>
    /// <returns></returns>
    public static int GetDataCount(byte cmd1DataCount)
    {
        return cmd1DataCount & 0x0F;
    }

    /// <summary>
    /// </summary>
    /// <param name="cmd1"></param>
    /// <param name="dataCount"></param>
    /// <returns></returns>
    public static byte ToCmd1DataCount(CommandFunction cmd1, int dataCount)
    {
        var cmd1DataCount = (byte)0x00;

        if (dataCount > 0x0F)
            throw new ArgumentOutOfRangeException(nameof(dataCount), $"Maximum length of dataCount is 15, supplied dataCount is {dataCount}");

        cmd1DataCount = (byte)(cmd1DataCount & 0x0F | (int)cmd1 << 4);
        cmd1DataCount = (byte)(cmd1DataCount & 0xF0 | dataCount);

        return cmd1DataCount;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public int CompareTo(object? obj)
    {
        var other = obj as CommandBlock;
        Debug.Assert(other != null);

        if (other.Equals(this))
            return 0;

        return 1;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(CommandBlock? other)
    {
        if (null == other) return false;

        if (Cmd1DataCount != other.Cmd1DataCount) return false;
        if (Cmd2 != other.Cmd2) return false;
        if (Data.Length != other.Data.Length) return false;

        return true;
    }

    /// <summary>
    /// </summary>
    /// <returns>
    ///     The
    ///     <see>
    ///         <cref>byte[]</cref>
    ///     </see>
    ///     .
    /// </returns>
    public byte[] ToBytes()
    {
        var buffer = new byte[3 + Data.Length];
        buffer[0] = Cmd1DataCount;
        buffer[1] = Cmd2;
        Buffer.BlockCopy(Data, 0, buffer, 2, Data.Length);

        buffer[2 + Data.Length] = CheckSum;

        return buffer;
    }

    /// <summary>
    ///     Assemble a friendly string
    /// </summary>
    /// <returns>
    ///     The <see cref="string" />.
    /// </returns>
    public override string ToString()
    {
        switch (Cmd1)
        {
            case CommandFunction.Return:
                switch ((Return.Return)Cmd2)
                {
                    case Return.Return.Ack:
                        return "Ack";
                    case Return.Return.DeviceType:
                        var deviceId = (ushort)(Data[0] << 8 | Data[1]);
                        var device = !Device.Names.TryGetValue(deviceId, out var deviceDescription) ? BitConverter.ToString(Data).Replace("-", string.Empty) : deviceDescription.Model;
                        return device ?? "Unknown";
                    case Return.Return.Nak:
                        var bits = new BitArray(new int[] { Data[0] });
                        if (bits.Get((int)NakCommandBlock.Nak.ChecksumError))
                            return "Nak ChecksumError";
                        if (bits.Get((int)NakCommandBlock.Nak.FrameError))
                            return "Nak FrameError";
                        if (bits.Get((int)NakCommandBlock.Nak.OverrunError))
                            return "Nak OverrunError";
                        if (bits.Get((int)NakCommandBlock.Nak.ParityError))
                            return "Nak ParityError";
                        if (bits.Get((int)NakCommandBlock.Nak.TimeOut))
                            return "Nak TimeOut";
                        if (bits.Get((int)NakCommandBlock.Nak.UndefinedError))
                            return "Nak UndefinedError";
                        break;
                }
                break;

            case CommandFunction.SystemControl:
                switch ((SystemControl.SystemControl)Cmd2)
                {
                    case SystemControl.SystemControl.LocalDisable:
                        return "LocalDisable";
                    case SystemControl.SystemControl.DeviceTypeRequest:
                        return "DeviceTypeRequest";
                    case SystemControl.SystemControl.LocalEnable:
                        return "LocalEnable";
                }
                break;

            case CommandFunction.TransportControl:
                switch ((TransportControl.TransportControl)Cmd2)
                {
                    case TransportControl.TransportControl.Stop:
                        return "Stop";
                    case TransportControl.TransportControl.Play:
                        return "Play";
                    case TransportControl.TransportControl.Record:
                        return "Record";
                    case TransportControl.TransportControl.StandbyOff:
                        return "StandbyOff";
                    case TransportControl.TransportControl.StandbyOn:
                        return "StandbyOn";
                    case TransportControl.TransportControl.Eject:
                        return "Eject";
                    case TransportControl.TransportControl.FastFwd:
                        return "FastFwd";
                    case TransportControl.TransportControl.JogFwd:
                        return "JogFwd";
                    case TransportControl.TransportControl.VarFwd:
                        return "VarFwd";
                    case TransportControl.TransportControl.ShuttleFwd:
                        return "ShuttleFwd";
                    case TransportControl.TransportControl.Rewind:
                        return "Rewind";
                    case TransportControl.TransportControl.JogRev:
                        return "JogRev";
                    case TransportControl.TransportControl.VarRev:
                        return "VarRev";
                    case TransportControl.TransportControl.ShuttleRev:
                        return "ShuttleRev";
                    case TransportControl.TransportControl.Preroll:
                        return "Preroll";
                    case TransportControl.TransportControl.CueUpWithData:
                        return "CueUpWithData";
                }
                break;

            case CommandFunction.PresetSelectControl:
                break;

            case CommandFunction.SenseRequest:
                //                var timeCode = new TimeCode(this.Data);
                switch ((SenseRequest.SenseRequest)Cmd2)
                {
                    case SenseRequest.SenseRequest.TcGenSense:
                        return $"TcGenSense";
                    case SenseRequest.SenseRequest.CurrentTimeSense:
                        return $"CurrentTimeSense";
                    case SenseRequest.SenseRequest.StatusSense:
                        return $"StatusSense";
                }
                break;

            case CommandFunction.SenseReturn:
                switch ((SenseReturn.SenseReturn)Cmd2)
                {
                    case SenseReturn.SenseReturn.Timer1Data:
                    case SenseReturn.SenseReturn.Timer2Data:
                    case SenseReturn.SenseReturn.LtcTimeData:
                    case SenseReturn.SenseReturn.UserBitsLtcData:
                    case SenseReturn.SenseReturn.VitcTimeData:
                    case SenseReturn.SenseReturn.UserBitsVitcData:
                    case SenseReturn.SenseReturn.GenTimeData:
                    case SenseReturn.SenseReturn.GenUserBitsData:
                    case SenseReturn.SenseReturn.CorrectedLtcTimeData:
                    case SenseReturn.SenseReturn.HoldUbLtcData:
                    case SenseReturn.SenseReturn.HoldVitcTimeData:
                    case SenseReturn.SenseReturn.HoldUbVitcData:
                        var timeCode = new TimeCode(Data);
                        return $"SenseReturn {timeCode}";

                    case SenseReturn.SenseReturn.StatusData:
                        break;
                }
                break;

            case CommandFunction.rrrReturn:
                return "rrrReturn";

            default:
                return "Unknown";
        }

        return "Unknown";
    }

    #endregion

    #region Methods

    /// <summary>
    ///     Calculate the checksum based on the current fields
    /// </summary>
    /// <returns>
    ///     The <see cref="byte" />.
    /// </returns>
    public byte CheckSum
    {
        get
        {
            var checksum = Cmd1DataCount;
            checksum += Cmd2;
            foreach (var b in Data)
            {
                checksum += b;
            }

            //        checksum &= 0xFF;

            return checksum;
        }
    }

    #endregion
}