using System;
using System.Linq;
using System.Collections.Generic;

namespace GRG.LeisureCards.PDF.DocumentTemplates
{
	public abstract class ContentTemplate
	{
		public abstract string[] Fields {
			get;
		}
	}

	public class ContentTemplate<T> : ContentTemplate where T : Content {

		public override string[] Fields {
			get {
				return typeof(T).GetProperties ().Where (
					prop => Attribute.IsDefined (prop, typeof(ContentPropertyAttribute))).Select (p => p.Name).ToArray ();
			}
		}
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class ContentPropertyAttribute : Attribute {	}

	public abstract class Content {

		internal Dictionary<string, string> GetContent(){

			return this.GetType().GetProperties ().Where (
				prop => Attribute.IsDefined (prop, typeof(ContentPropertyAttribute))).Select (p => new Tuple<string, string>( p.Name, p.GetValue(this, null).ToString()))
				.ToDictionary(t=>t.Item1, t=>t.Item2.ToString());
		}
	}
}

