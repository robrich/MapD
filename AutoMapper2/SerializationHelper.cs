namespace MapDLib {

	#region using
	using System;
	using System.IO;
	using System.Reflection;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Xml;
	using System.Xml.Serialization;
	#endregion

	internal static class SerializationHelper {

		public static string Serialize( object instance, bool stripNamespace ) {

			if ( instance == null ) {
				return String.Empty;
			}

			MemoryStream ms = new MemoryStream();
			XmlSerializer xs = new XmlSerializer( instance.GetType() );
			XmlTextWriter tw = new XmlTextWriter( ms, Encoding.UTF8 );

			xs.Serialize( tw, instance );
			ms = (MemoryStream)tw.BaseStream;

			string serialized = Encoding.UTF8.GetString( ms.ToArray() );
			if ( stripNamespace && !string.IsNullOrEmpty( serialized ) ) {
				serialized = Regex.Replace( serialized, "^﻿<\\?xml[^>]*\\?>", "" );
			}
			if ( stripNamespace && !string.IsNullOrEmpty( serialized ) ) {
				serialized = serialized.Replace(
					" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"",
					""
					);
			}
			return serialized;
		}

		public static T Deserialize<T>( string serialized ) {
			return (T)Deserialize( serialized, typeof( T ) );
		}

		public static object Deserialize( string serialized, Type T ) {
			string s = AddDefaultNamespaceToRootNode( serialized, T );
			XmlSerializer xs = new XmlSerializer( T );
			MemoryStream ms = new MemoryStream( Encoding.UTF8.GetBytes( s ) );
			XmlTextWriter tw = new XmlTextWriter( ms, Encoding.UTF8 );

			return xs.Deserialize( ms );
		}

		public static string GetDefaultNamespace( Type T ) {

			if ( T == null ) {
				return null;
			}

			string results = string.Empty;

			MemberInfo inf = T;
			object[] attributes = inf.GetCustomAttributes( typeof( XmlRootAttribute ), false );

			foreach ( Object attribute in attributes ) {
				XmlRootAttribute root = attribute as XmlRootAttribute;
				if ( root != null && !string.IsNullOrEmpty( root.Namespace ) ) {
					results = root.Namespace;
					break;
				}
			}

			return results;
		}

		public static string AddDefaultNamespaceToRootNode( string XML, Type T ) {

			if ( string.IsNullOrEmpty( XML ) ) {
				return XML; // Can't get water from a stone
			}

			string result = XML;

			if ( !result.StartsWith( "﻿<?xml", StringComparison.InvariantCultureIgnoreCase ) ) {
				result = "﻿<?xml version=\"1.0\" encoding=\"utf-8\"?>" + result;
			}

			string nsName = GetDefaultNamespace( T );
			if ( string.IsNullOrEmpty( nsName ) ) {
				return XML; // No point adding nothing to it
			}

			string nsAttr = string.Format( "xmlns=\"{0}\"", nsName );
			if ( result.IndexOf( nsAttr ) < 0 ) {
				result = Regex.Replace( result, @"(\?><[^ >]+? )", @"$1" + nsAttr + " " );
			}

			return result;
		}

	}
}
