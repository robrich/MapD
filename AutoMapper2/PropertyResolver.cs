namespace AutoMapper2Lib {

	#region using
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	#endregion

	internal static class PropertyResolver {

		// The way into this class
		public static string Resolve<T>( Expression<Func<T, object>> expression ) {
			Expression body = expression.Body;
			return GetBodyText( body );
		}

		private static string GetBodyText( Expression body ) {

			Stack<string> source = new Stack<string>();
			while ( body != null ) {
				if ( body.NodeType == ExpressionType.Call ) {
					MethodCallExpression callBody = (MethodCallExpression)body;
					if ( !IsSingleArgumentIndexer( callBody ) ) {
						throw new ArgumentOutOfRangeException( "body", callBody, "Function call doesn't have a single argument, don't know how to resolve it" );
					}
					Expression callArgs = callBody.Arguments.Single<Expression>();
					source.Push( GetIndexerText( callArgs ) );
					body = callBody.Object;
				} else if ( body.NodeType == ExpressionType.ArrayIndex ) {
					BinaryExpression arrayE = (BinaryExpression)body;
					source.Push( GetIndexerText( arrayE.Right ) );
					body = arrayE.Left;
					continue;
				} else if ( body.NodeType == ExpressionType.MemberAccess ) {
					MemberExpression memberE = (MemberExpression)body;
					source.Push( "." + memberE.Member.Name );
					body = memberE.Expression;
				} else if ( body.NodeType == ExpressionType.Convert ) {
					UnaryExpression unaryE = (UnaryExpression)body;
					body = unaryE.Operand;
				} else if ( body.NodeType == ExpressionType.Parameter ) {
					break; // We're at the root of the tree (hopefully)
				} else {
					throw new ArgumentOutOfRangeException( "body.NodeType", body.NodeType, "Unknown ExpressionType, no idea what to do here" );
				}
			}
			if ( source.Count <= 0 ) {
				return string.Empty;
			}
			return string.Join( "", source.ToArray() ).TrimStart( new char[] { '.' } );
		}

		private static bool IsSingleArgumentIndexer( MethodCallExpression methodExpression ) {
			return ( ( ( methodExpression != null )
				&& ( methodExpression.Arguments.Count == 1 ) )
				&& methodExpression.Method.DeclaringType.GetDefaultMembers().OfType<PropertyInfo>().Any<PropertyInfo>(
					delegate( PropertyInfo p ) { return ( p.GetGetMethod() == methodExpression.Method ); }
				)
			);
		}

		private static string GetIndexerText( Expression expression ) {
			return "[" + GetValueText( expression ) + "]";
		}

		private static string GetValueText( Expression expression ) {

			ConstantExpression constE = expression as ConstantExpression;
			if ( constE != null ) {
				return constE.Value.ToString();
			}

			MemberExpression memberE = expression as MemberExpression;
			if ( memberE != null ) {
				ConstantExpression mcE = memberE.Expression as ConstantExpression;
				if ( mcE != null ) {
					object v = mcE.Value;
					var val = v.GetType().GetField( memberE.Member.Name ).GetValue( v );
					return val.ToString();
				}
			}

			BinaryExpression binaryE = expression as BinaryExpression;
			if ( binaryE != null ) {
				// Recurse to get values of each half
				return string.Format( "{0} {1} {2}", GetValueText( binaryE.Left ), binaryE.NodeType, GetValueText( binaryE.Right ) );
			}

			// Recurse because it's better at figuring out the big stuff
			return GetBodyText( expression );
		}

	}

}
