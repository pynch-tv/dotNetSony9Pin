namespace dotNetSony9Pin.Sony9Pin.CommandBlocks.Return;

public class DeviceDescription
{
    public required string Manufacturer;
    public required string ManufacturerShort;
    public required string Model;
}

/// <summary>
/// 
/// </summary>
public class Device
{
    /// <summary>
    ///     The device names.
    /// </summary>
    public static readonly IDictionary<ushort, DeviceDescription> Names = new Dictionary<ushort, DeviceDescription> {
    {
      0x2000,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-10" }
    },
    {
      0x2100,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-10" }
    },
    {
      0x2002,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-11" }
    },
    {
      0x2102,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-11" }
    },
    {
      0x2003,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-15" }
    },
    {
      0x2103,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-15" }
    },
    {
      0x2010,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-35" }
    },
    {
      0x2110,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-35" }
    },
    {
      0x2001,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-40" }
    },
    {
      0x2101,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-40" }
    },
    {
      0x2030,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-50" }
    },
    {
      0x2130,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-50" }
    },
    {
      0x2020,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-60" }
    },
    {
      0x2120,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-60" }
    },
    {
      0x2021,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-65" }
    },
    {
      0x2121,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-65" }
    },
    {
      0x2022,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-95" }
    },
    {
      0x2122,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-95" }
    },
    {
      0x2023,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-96" }
    },
    {
      0x2123,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-96" }
    },
    {
      0x2024,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-70" }
    },
    {
      0x2124,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-70" }
    },
    {
      0x2025,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-75" }
    },
    {
      0x2125,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-75" }
    },
    {
      0x2046,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-D75" }
    },
    {
      0x2146,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-D75" }
    },
    {
      0x2047,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-9000" }
    },
    {
      0x2147,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-9000" }
    },
    {
      0x2040,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "PVW-2600" }
    },
    {
      0x2140,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "PVW-2600" }
    },
    {
      0x2041,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "PVW-2800" }
    },
    {
      0x2141,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "PVW-2800" }
    },
    {
      0x2018,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-35PM" }
    },
    {
      0x2029,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-65PM" }
    },
    {
      0x202A,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-95PM" }
    },
    {
      0x202D,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-75PM" }
    },
    {
      0x2126,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-85P" }
    },
    {
      0x212C,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-70S" }
    },
    {
      0x212D,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "BVW-75S" }
    },
    {
      0x3010,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "DVR-2000" }
    },
    {
      0x3110,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "DVR-2000" }
    },
    {
      0x3011,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "DVR-2100" }
    },
    {
      0x3111,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "DVR-2100" }
    },
    {
      0x20E4,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "J-H3" }
    },
    {
      0x21E4,
      new DeviceDescription{ Manufacturer = "Sony", ManufacturerShort = "Sony", Model = "J-H3" }
    },
    {
      0xD910,
       new DeviceDescription{ Manufacturer ="EVS", ManufacturerShort = "EVS", Model ="EVS XT" }
    },
    {
      0xD810,
       new DeviceDescription{ Manufacturer ="EVS",ManufacturerShort = "EVS", Model = "EVS XT" }
    },
    {
      0xF0E0,
       new DeviceDescription{ Manufacturer ="Blackmagic Design", ManufacturerShort = "BMD", Model = "HyperDeck NTSC" }
    },
    {
      0xF1E0,
       new DeviceDescription{ Manufacturer ="Blackmagic Design", ManufacturerShort = "BMD", Model = "HyperDeck PAL" }
    },
    {
      0xF2E0,
       new DeviceDescription{ Manufacturer ="Blackmagic Design", ManufacturerShort = "BMD", Model = "HyperDeck 24P" }
    },
    {
      0xA000,
       new DeviceDescription{ Manufacturer ="Generic", ManufacturerShort = "Genric",  Model = "DiskPlayer" }
    },
  };
}