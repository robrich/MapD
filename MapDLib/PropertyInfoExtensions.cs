namespace MapDLib {

	#region using
	using System;
	using System.Collections.Generic;
	using System.Data.Linq.Mapping;
	using System.Data.Objects.DataClasses;
	using System.Linq;
	using System.Reflection;
	#endregion

	internal static class PropertyInfoExtensions {

		public static PropertyInfo GetPropertyByName( this List<PropertyInfo> List, string PropertyName ) {
			return (
				from p in List
				where string.Equals( p.Name, PropertyName, StringComparison.InvariantCultureIgnoreCase )
				select p
			).FirstOrDefault();
		}

		public static List<Attribute> GetAttributes( this PropertyInfo Property, Type AttributeType ) {
			Dictionary<PropertyInfo, List<Attribute>> attributeList = ReflectionHelper.GetPropertyAttributes( Property.DeclaringType );
			return (
				from a in attributeList
				where a.Key.Name == Property.Name
				from v in a.Value
				where AttributeType.IsAssignableFrom( v.GetType() )
				select v
			).ToList();
		}

		public static Attribute GetFirstAttribute( this PropertyInfo Property, Type AttributeType ) {
			return GetAttributes( Property, AttributeType ).FirstOrDefault();
		}

		public static bool IsListOfT( this PropertyInfo Property ) {
			return Property.PropertyType.IsListOfT();
		}

		public static bool IsLinqProperty( this PropertyInfo Property ) {
			bool results = false;
			if ( Property.PropertyType.IsLinqProperty() ) {
				results = true;
			}
			// LinqToSql
			AssociationAttribute association = (AssociationAttribute)GetFirstAttribute( Property, typeof( AssociationAttribute ) );
			if ( association != null ) {
				results = true;
			}
			// LinqToEntities
			EdmEntityTypeAttribute entity = (EdmEntityTypeAttribute)GetFirstAttribute( Property, typeof( EdmEntityTypeAttribute ) );
			if ( entity != null ) {
				results = true;
			}
			EdmRelationshipNavigationPropertyAttribute dataMember = (EdmRelationshipNavigationPropertyAttribute)GetFirstAttribute( Property, typeof( EdmRelationshipNavigationPropertyAttribute ) );
			if ( dataMember != null ) {
				results = true;
			}
			return results;
		}

		public static IgnoreDirection GetIgnoreStatus( this PropertyInfo Property ) {
			IgnoreDirection results = IgnoreDirection.None;
			IgnoreMapAttribute ignore = (IgnoreMapAttribute)GetFirstAttribute( Property, typeof(IgnoreMapAttribute) );
			if ( ignore != null ) {
				results |= ignore.IgnoreDirection;
			}
			// Is the type ignored?
			results |= Property.PropertyType.GetIgnoreStatus();
			return results;
		}

		public static PropertyIs GetIgnorePropertiesIf( this PropertyInfo Property ) {
			PropertyIs propertyIs = PropertyIs.NotSet;
			IgnorePropertiesIfAttribute ignoreIf = (IgnorePropertiesIfAttribute)GetFirstAttribute( Property, typeof( IgnorePropertiesIfAttribute ) );
			if ( ignoreIf != null ) {
				propertyIs |= ignoreIf.PropertyIs;
			}
			// Is the type ignored?
			propertyIs |= Property.PropertyType.GetIgnorePropertiesIf();
			return propertyIs;
		}

		public static bool IsPrimaryKeyPropertyViaMapAttribute( this PropertyInfo Property ) {
			bool results = false;
			PrimaryKeyAttribute key = (PrimaryKeyAttribute)GetFirstAttribute( Property, typeof( PrimaryKeyAttribute ) );
			if ( key != null ) {
				results = true;
			}
			return results;
		}

		public static bool IsPrimaryKeyPropertyViaDataAnnotation( this PropertyInfo Property ) {
			bool results = false;
#if NET_4
			// KeyAttribute primary key
			System.ComponentModel.DataAnnotations.KeyAttribute key = (System.ComponentModel.DataAnnotations.KeyAttribute)GetFirstAttribute( Property, typeof( System.ComponentModel.DataAnnotations.KeyAttribute ) );
			if ( key != null ) {
				results = true;
			}
#endif
			return results;
		}

		public static bool IsPrimaryKeyPropertyViaLinq( this PropertyInfo Property ) {
			bool results = false;
			
			// ColumnAttribute primary key
			ColumnAttribute linqToSql = (ColumnAttribute)GetFirstAttribute( Property, typeof( ColumnAttribute ) );
			if ( linqToSql != null && linqToSql.IsPrimaryKey ) {
				results = true;
			}

			// EdmScalarPropertyAttribute primary key
			EdmScalarPropertyAttribute linqToEntities = (EdmScalarPropertyAttribute)GetFirstAttribute( Property, typeof( EdmScalarPropertyAttribute ) );
			if ( linqToEntities != null && linqToEntities.EntityKeyProperty ) {
				results = true;
			}

			return results;
		}

		public static List<PropertyInfo> GetPrimaryKeys( this List<PropertyInfo> Properties ) {

			List<PropertyInfo> results = new List<PropertyInfo>();

			if ( Properties == null || Properties.Count == 0 ) {
				return results; // Nothing yields nothing
			}

			if ( results.Count == 0 ) {
				results = (
					from p in Properties
					where p.IsPrimaryKeyPropertyViaMapAttribute()
					select p
					).ToList();
			}

			if ( results.Count == 0 ) {
				results = (
					from p in Properties
					where p.IsPrimaryKeyPropertyViaDataAnnotation()
					select p
					).ToList();
			}

			if ( results.Count == 0 ) {
				results = (
					from p in Properties
					where p.IsPrimaryKeyPropertyViaLinq()
					select p
					).ToList();
			}

			return results;
		}

	}

}
