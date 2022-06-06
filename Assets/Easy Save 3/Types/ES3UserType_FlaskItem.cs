using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("Position")]
	public class ES3UserType_FlaskItem : ES3ScriptableObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_FlaskItem() : base(typeof(Inventory.Item.FlaskItem)){ Instance = this; priority = 1; }


		protected override void WriteScriptableObject(object obj, ES3Writer writer)
		{
			var instance = (Inventory.Item.FlaskItem)obj;
			
			writer.WriteProperty("Position", instance.Position, ES3Type_int.Instance);
		}

		protected override void ReadScriptableObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Inventory.Item.FlaskItem)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
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


	public class ES3UserType_FlaskItemArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_FlaskItemArray() : base(typeof(Inventory.Item.FlaskItem[]), ES3UserType_FlaskItem.Instance)
		{
			Instance = this;
		}
	}
}