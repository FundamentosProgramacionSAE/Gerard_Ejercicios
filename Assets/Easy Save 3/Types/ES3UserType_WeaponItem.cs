using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("IsUsed", "Position")]
	public class ES3UserType_WeaponItem : ES3ScriptableObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_WeaponItem() : base(typeof(Inventory.Item.WeaponItem)){ Instance = this; priority = 1; }


		protected override void WriteScriptableObject(object obj, ES3Writer writer)
		{
			var instance = (Inventory.Item.WeaponItem)obj;
			
			writer.WriteProperty("IsUsed", instance.IsUsed, ES3Type_bool.Instance);
			writer.WriteProperty("Position", instance.Position, ES3Type_int.Instance);
		}

		protected override void ReadScriptableObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Inventory.Item.WeaponItem)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "IsUsed":
						instance.IsUsed = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "Position":
						instance.Position = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_WeaponItemArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_WeaponItemArray() : base(typeof(Inventory.Item.WeaponItem[]), ES3UserType_WeaponItem.Instance)
		{
			Instance = this;
		}
	}
}