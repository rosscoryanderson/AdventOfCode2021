using src.Base;
using src.Features;

namespace src.Puzzles;

public class Day16 : Day, IPuzzle
{
    public string Puzzle1()
    {
        var packetDecoder = new PacketDecoder(Input[0]);

        var decodedPacket = packetDecoder.GetVersionNumbers();

        return decodedPacket.ToString();
    }

    public string Puzzle2()
    {
        var packetDecoder = new PacketDecoder(Input[0]);

        var value = packetDecoder.CalculateValue();

        return value.ToString();
    }
}