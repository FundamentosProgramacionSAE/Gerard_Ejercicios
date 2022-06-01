using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("Data", "StackSize")]
	public class ES3UserType_InventoryItem : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_InventoryItem() : base(typeof(InventoryItem)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (InventoryItem)obj;
			
			writer.WritePropertyByRef("Data", instance.Data);
			writer.WriteProperty("StackSize", instance.StackSize, ES3Type_int.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (InventoryItem)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "Data":
						instance.Data = reader.Read<ItemData>(ES3UserType_ItemData.Instance);
						break;
					case "StackSize":
						instance.StackSize = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new InventoryItem(ScriptableObject.CreateInstance<ItemData>());
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_InventoryItemArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_InventoryItemArray() : base(typeof(InventoryItem[]), ES3UserType_InventoryItem.Instance)
		{
			Instance = this;
		}
	}
}