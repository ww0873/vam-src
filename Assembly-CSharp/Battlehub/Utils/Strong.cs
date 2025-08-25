using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Battlehub.Utils
{
	// Token: 0x020002AC RID: 684
	public class Strong
	{
		// Token: 0x06001018 RID: 4120 RVA: 0x0005B5E4 File Offset: 0x000599E4
		public Strong()
		{
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x0005B5EC File Offset: 0x000599EC
		public static PropertyInfo PropertyInfo<T, U>(Expression<Func<T, U>> expression)
		{
			return (PropertyInfo)Strong.MemberInfo<T, U>(expression);
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x0005B5FC File Offset: 0x000599FC
		public static MemberInfo MemberInfo<T, U>(Expression<Func<T, U>> expression)
		{
			MemberExpression memberExpression = expression.Body as MemberExpression;
			if (memberExpression != null)
			{
				return memberExpression.Member;
			}
			throw new ArgumentException("Expression is not a member access", "expression");
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x0005B634 File Offset: 0x00059A34
		public static MethodInfo MethodInfo<T>(Expression<Func<T, Delegate>> expression)
		{
			UnaryExpression unaryExpression = (UnaryExpression)expression.Body;
			MethodCallExpression methodCallExpression = (MethodCallExpression)unaryExpression.Operand;
			ConstantExpression constantExpression = (ConstantExpression)methodCallExpression.Arguments.Last<Expression>();
			return (MethodInfo)constantExpression.Value;
		}
	}
}
