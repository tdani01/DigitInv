using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.Server;

namespace VS_DigitInv.Utils
{
    internal class PosAttributeUtil
    {
        // Note : Only works for 3 dimensions, if sometime 4th dimension is needed, add a 4th integer per entry ( i += 4) so cross-dim coords stay accurate
        private const string Key = "linkedPositions";

        public static List<BlockPos> GetPositions(ITreeAttribute tree)
        {
            var result = new List<BlockPos>();
            int[] raw = (tree?[Key] as IntArrayAttribute)?.value;
            if (raw == null) return result;
            for (int i = 0; i + 2 < raw.Length; i+=3)
            {
                result.Add(new BlockPos(raw[i], raw[i + 1], raw[i + 2]));
            }
            return result;
        }

        public static void SetPositions(ITreeAttribute tree, List<BlockPos> positions)
        {
            int[] raw = new int[positions.Count * 3];
            for (int i = 0; i < positions.Count; i++)
            {
                raw[i * 3] = positions[i].X;
                raw[i * 3 + 1] = positions[i].Y;
                raw[i * 3 + 2] = positions[i].Z;
            }
            tree[Key] = new IntArrayAttribute(raw);
        }

        public static bool TogglePosition(ITreeAttribute tree, BlockPos pos)
        {
            var list = GetPositions(tree);
            int idx = list.FindIndex(p => p.Equals(pos));
            bool added;

            if (idx >= 0)
            {
                list.RemoveAt(idx);
                added = false;
            }
            else
            {
                list.Add(pos.Copy());
                added = true;
            }

            SetPositions(tree, list);
            return added;
        }
    }
}
