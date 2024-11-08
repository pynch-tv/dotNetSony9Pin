using System.Collections.Specialized;

namespace lathoub;

public enum SlotStatus
{
    Empty,
    Mounting,
    Mounted,
    Error
}


public class SlotInfo
{
    public int Id;

    public string Name;

    public string Device;

    public SlotStatus Status;

    public string VolumeName;

    public ulong RecordingTime; // in seconds

    public string VideoFormat;

    public bool Blocked;

    public ulong RemainingSize; // in bytes

    public ulong TotalSize; // in bytes

    /// <summary>
    /// 
    /// </summary>
    /// <param name="slotInfo"></param>
    /// <returns></returns>
    public static SlotInfo FromSlotInfo(NameValueCollection parameters)
    {
        var slot = new SlotInfo();

        var s = parameters["slot id"];
        if (null != s)
            slot.Id = Convert.ToInt16(s);

        s = parameters["slot name"];
        if (null != s)
            slot.Name = s ?? "";

        s = parameters["device"];
        if (null != s)
            slot.Device = s ?? "";

        s = parameters["status"];
        if (null != s)
           slot.Status = (SlotStatus)Enum.Parse(typeof(SlotStatus), s, true);

        s = parameters["volume name"];
        if (null != s)
            slot.VolumeName = s ?? "";

        s = parameters["recording time"];
        if (null != s)
            slot.RecordingTime = Convert.ToUInt64(s);

        s = parameters["video format"];
        if (null != s)
            slot.VideoFormat = s ?? "";

        s = parameters["blocked"];
        if (null != s)
            slot.Blocked = s.Equals("true");

        s = parameters["remaining size"];
        if (null != s)
            slot.RemainingSize = Convert.ToUInt64(s);

        s = parameters["total size"];
        if (null != s)
            slot.TotalSize = Convert.ToUInt64(s);

        return slot;
    }
}
