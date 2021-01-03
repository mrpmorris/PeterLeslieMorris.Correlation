using System;
using System.Linq.Expressions;

namespace PeterLeslieMorris.Correlation.Helpers
{
	public static class GetObjectPropertyValueDelegateFactory
	{
		public static GetObjectPropertyValueDelegate<TValue> Create<TValue>(Type instanceType, string propertyName)
		{
			if (instanceType == null)
				throw new ArgumentNullException(nameof(instanceType));
			if (propertyName == null)
				throw new ArgumentNullException(nameof(propertyName));

			// Entry of the delegate
			var instanceParam = Expression.Parameter(typeof(object), "instance");

			// Cast the instance from "object" to the correct type
			var instanceExpr = Expression.TypeAs(instanceParam, instanceType);

			// Get the property's value
			var property = instanceType.GetProperty(propertyName);
			var propertyExpr = Expression.Property(instanceExpr, property);

			// Create delegate
			var lambda = Expression.Lambda<GetObjectPropertyValueDelegate<TValue>>(propertyExpr, instanceParam);
			return lambda.Compile();
		}
	}
}
