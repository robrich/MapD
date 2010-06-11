namespace AutoMapper2Lib {

	#region using
	using System;
	using System.Collections.Generic;
	using System.Data.Linq.Mapping;
	using System.Data.Objects.DataClasses;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	#endregion

	internal static class PropertyInfoExtensions {

		public static MethodInfo GetMethodByName( this List<MethodInfo> List, string PropertyName, int ParameterCount ) {
			return (
				from m in List
				where m.Name == PropertyName
					&& m.GetParameters().Length == ParameterCount
				select m
				).FirstOrDefault();
		}

		public static PropertyInfo GetPropertyByName( this List<PropertyInfo> List, string PropertyName ) {
			return (
				from p in List
				where p.Name == PropertyName
				select p
				).FirstOrDefault();
		}

		public static bool IsGenericAndNotNullable( this PropertyInfo Property ) {
			bool results = false;
			if ( Property.PropertyType.IsGenericType ) {
				if ( Property.PropertyType.IsNullable() ) {
					// It's Nullable<T> or written better T? so it's fine
					results = false;
				} else {
					// This is a V<T> which may be a List<T> or an EntitySet<T>
					results = true;
				}
			}
			return results;
		}

		public static bool IsListOfT( this PropertyInfo Property ) {
			return Property.PropertyType.IsListOfT();
		}

		public static bool IsLinqProperty( this PropertyInfo Property ) {
			bool results = false;
			if ( Property.PropertyType.Name.StartsWith( "EntityRef" )
				|| Property.PropertyType.Name.StartsWith( "EntitySet" )
					|| Property.PropertyType.Name.StartsWith( "EntityObject" ) ) {
				results = true;
			}
			// LinqToSql
			AssociationAttribute association = (AssociationAttribute)Attribute.GetCustomAttribute( Property, typeof( AssociationAttribute ) );
			if ( association != null && !string.IsNullOrEmpty( association.ThisKey ) ) {
				results = true;
			}
			// LinqToEntities
			EdmEntityTypeAttribute entity = (EdmEntityTypeAttribute)Attribute.GetCustomAttribute( Property, typeof( EdmEntityTypeAttribute ) );
			if ( entity != null ) {
				results = true;
			}
			// TODO: Determine if this is a value type that's linked to an association and exclude that too
			return results;
		}

		public static bool IsPrimaryKey( this PropertyInfo Property ) {
			bool results = false;

			// LinqToSql primary key
			ColumnAttribute linqToSql = (ColumnAttribute)Attribute.GetCustomAttribute( Property, typeof(ColumnAttribute) );
			if ( linqToSql != null && linqToSql.IsPrimaryKey ) {
				results = true;
			}
			// LinqToEntities primary key
			EdmScalarPropertyAttribute linqToEntities = (EdmScalarPropertyAttribute)Attribute.GetCustomAttribute( Property, typeof(EdmScalarPropertyAttribute) );
			if ( linqToEntities != null && linqToEntities.EntityKeyProperty ) {
				results = true;
			}
			// AutoMapper2 PrimaryKey
			MapPrimaryKeyAttribute mapPrimaryKeyAttribute = (MapPrimaryKeyAttribute)Attribute.GetCustomAttribute( Property, typeof(MapPrimaryKeyAttribute) );
			if ( mapPrimaryKeyAttribute != null ) {
				results = true;
			}

			return results;
		}

		public static bool IsMapIgnored( this PropertyInfo Property ) {
			bool results = false;
			MapIgnoreAttribute ignore = (MapIgnoreAttribute)Attribute.GetCustomAttribute( Property, typeof(MapIgnoreAttribute) );
			if ( ignore != null ) {
				results = true;
			}
			return results;
		}

		// TODO: What if there's multiple primary key fields on the object?  What if the source and destination primary keys are different properties?
		public static PropertyInfo GetPrimaryKey( this List<PropertyInfo> Properties ) {
			return (
				from p in Properties ?? new List<PropertyInfo>()
				where p.IsPrimaryKey()
				select p
				).FirstOrDefault();
		}

	}

}
