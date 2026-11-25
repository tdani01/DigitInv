using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace VS_DigitInv.Models
{
	public class LedgerItemEntry
	{
		public ItemStack TemplateStack { get; set; }
		public int TotalCount { get; set; }
		public HashSet<BlockPos> LinkedPositions { get; set; } = new();
	}
}

