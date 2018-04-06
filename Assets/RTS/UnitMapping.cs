using System;
using System.Collections.Generic;
using System.Linq;

namespace RTS
{
	public class UnitMapping
	{
		public string[] hotkeyToUnitname;

		public UnitMapping (string[] hotkeyToUnitname)
		{
			this.hotkeyToUnitname = hotkeyToUnitname;
		}

		public Unit FindUnitByHotkey (List<Unit> units, int hotkey)
		{
			string unitName = hotkey < hotkeyToUnitname.Length ? hotkeyToUnitname[hotkey] : null;

			foreach (Unit unit in units) 
			{
				if (unit.objectName == unitName)
				{
					return unit;
				}
			}

			return null;
		}
	}
}
