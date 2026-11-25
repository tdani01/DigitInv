using System;
using System.Collections.Generic;
using ProtoBuf;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using VS_DigitInv.Models;

namespace VS_DigitInv.Network
{
    [ProtoContract]
    public class ScanContainersRequestPacket
    {
        // Flattened positions of the containers to scan
        [ProtoMember(1)]
        public int[] FlatPositions {get; set;}
    }

    [ProtoContract]
    public class SerializedLedgerEntry
    {
        [ProtoMember(1)]
        public byte[] ItemStackBytes { get; set; }

        [ProtoMember(2)]
        public int TotalCount { get; set; }

        [ProtoMember(3)]
        public int[] FlatPositions { get; set; }
    }

    [ProtoContract]
    public class ScanContainersResponsePacket
    {
        [ProtoMember(1)]
        public List<SerializedLedgerEntry> Entries { get; set; } = new();
    }

    public static class LedgerPacketMapper
    {
        public static int[] FlattenPositions(IEnumerable<BlockPos> positions)
        {
            var list = new List<int>();
            foreach (var pos in positions)
            {
                list.Add(pos.X);
                list.Add(pos.Y);
                list.Add(pos.Z);
            }
            return list.ToArray();
        }

        public static List<BlockPos> UnflattenPositions(int[] flat)
        {
            var list = new List<BlockPos>();
            for (int i = 0; i + 2 < flat.Length; i += 3)
            {
                list.Add(new BlockPos(flat[i], flat[i + 1], flat[i + 2]));
            }
            return list;
        }

        public static SerializedLedgerEntry ToDto(LedgerItemEntry entry)
        {
            return new SerializedLedgerEntry
            {
                ItemStackBytes = entry.TemplateStack.ToBytes(),
                TotalCount = entry.TotalCount,
                FlatPositions = FlattenPositions(entry.LinkedPositions)
            };

        }

        public static LedgerItemEntry FromDto(IWorldAccessor world, SerializedLedgerEntry entry)
        {
            var stack = new ItemStack(entry.ItemStackBytes);
            stack.ResolveBlockOrItem(world);

            return new LedgerItemEntry
            {
                TemplateStack = stack,
                TotalCount = entry.TotalCount,
                LinkedPositions = new HashSet<BlockPos>(UnflattenPositions(entry.FlatPositions))
            };
        }
    }

}

