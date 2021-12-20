using System.Text;
using src.Util;

namespace src.Features;

public class PacketDecoder
{
    private string _binaryData;
    private long _versionSum;
    private readonly Dictionary<char, string> _hexMapping;
    
    public PacketDecoder(string input)
    {
        _hexMapping = new Dictionary<char, string>();
        _binaryData = string.Empty;
        _versionSum = 0;

        PopulateHexMapping();
        ParseHexString(input);
    }

    public long GetVersionNumbers()
    {
        ParsePackets(_binaryData);
        
        return _versionSum;
    }

    public long CalculateValue()
    {
        var (value, _) = ParsePackets(_binaryData);
        
        return value;
    }

    private (long value, int endPointer) ParsePackets(string binaryData)
    {
        if (binaryData.All(c => c == '0'))
        {
            return (0, binaryData.Length);
        }
        
        GetVersion(binaryData);
        var typeId = GetTypeId(binaryData);

        return typeId == 4 
            ? ParseLiteralValue(binaryData) 
            : ParseOperator(binaryData, typeId);
    }

    private (long value, int endPointer) ParseLiteralValue(string binaryData)
    {
        var pointer = 6;
        var endPointer = 0;
        var continueFlag = true;
        var sb = new StringBuilder();

        while (continueFlag)
        {
            continueFlag = binaryData[pointer++] == '1';
            endPointer = pointer + 4;
            sb.Append(binaryData[pointer..endPointer]);
            pointer = endPointer;
        }

        return (sb.ToString().ToLong(), endPointer);
    }

    private (long value, int endPointer) ParseOperator(string binaryData, long typeId)
    {
        var pointer = 6;

        var calculator = new PacketOperatorCalculator((int)typeId);

        return binaryData[pointer++] == '0' 
            ? ParseTotalLengthOperator(binaryData, pointer, calculator) 
            : ParseSubPacketOperator(binaryData, pointer, calculator);
    }

    private (long value, int endPointer) ParseTotalLengthOperator(string binaryData, int startingPointer,
        PacketOperatorCalculator calculator)
    {
        if (binaryData.Length < startingPointer + 15)
        {
            return (0, binaryData.Length);
        }

        var pointer = startingPointer;
        var endPointer = pointer + 15;
        var totalLength = binaryData[pointer..endPointer].ToLong();
        pointer = endPointer;
        var totalSize = 0;
        var value = 0L;

        while (totalSize < totalLength)
        {
            var packet = binaryData[pointer..];
            (value, endPointer) = ParsePackets(packet);
            value = calculator.Calculate(value);
            totalSize += endPointer;
            pointer += endPointer;

            if (pointer >= binaryData.Length) break;
        }

        return (value, pointer);
    }

    private (long value, int endPointer) ParseSubPacketOperator(string binaryData, int startingPointer,
        PacketOperatorCalculator calculator)
    {
        var pointer = startingPointer;
        var endPointer = pointer + 11;
        var numPackets = binaryData[pointer..endPointer].ToLong();
        pointer = endPointer;
        var value = 0L;

        for (var i = 1; i <= numPackets; i++)
        {
            var packet = binaryData[pointer..];
            (value, endPointer) = ParsePackets(packet);
            value = calculator.Calculate(value);
            pointer += endPointer;
            
            if (string.IsNullOrEmpty(packet)) break;
        }

        return (value, pointer);
    }

    private long GetTypeId(string binaryData)
    {
        var binaryRepresentation = binaryData[3..6];

        return binaryRepresentation.ToLong();
    }

    private void GetVersion(string binaryData)
    {
        if (string.IsNullOrEmpty(binaryData) || binaryData.Length < 3) return;
        
        var binaryRepresentation = binaryData[..3];
        var versionNum = binaryRepresentation.ToLong();

        _versionSum += versionNum;
    }

    private void ParseHexString(string input)
    {
        var sb = new StringBuilder();

        foreach (var hexValue in input)
        {
            sb.Append(_hexMapping[hexValue]);
        }

        _binaryData = sb.ToString();
    }

    private void PopulateHexMapping()
    {
        _hexMapping.Add('0', "0000");
        _hexMapping.Add('1', "0001");
        _hexMapping.Add('2', "0010");
        _hexMapping.Add('3', "0011");
        _hexMapping.Add('4', "0100");
        _hexMapping.Add('5', "0101");
        _hexMapping.Add('6', "0110");
        _hexMapping.Add('7', "0111");
        _hexMapping.Add('8', "1000");
        _hexMapping.Add('9', "1001");
        _hexMapping.Add('A', "1010");
        _hexMapping.Add('B', "1011");
        _hexMapping.Add('C', "1100");
        _hexMapping.Add('D', "1101");
        _hexMapping.Add('E', "1110");
        _hexMapping.Add('F', "1111");
    }
}