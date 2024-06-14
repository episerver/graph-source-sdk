using System.Linq.Expressions;
using System.Reflection;

namespace Optimizely.Graph.Source.Sdk.ExpressionHelper
{
    public static class ExpressionExtensions
    {
        public static Type GetMemberReturnType(this MemberExpression memberExpression)
        {
            return memberExpression.Member.GetMemberReturnType();
        }

        public static string GetFieldPath(this Expression fieldSelector)
        {
            var visitor = new FieldPathVisitor(fieldSelector);
            return visitor.GetPath();
        }

        public static Type GetReturnType(this Expression expression)
        {
            if (expression is LambdaExpression)
            {
                expression = ((LambdaExpression)expression).Body;
            }
            Type fieldType = null;

            if (expression is MemberExpression)
            {
                var memberExpression = (MemberExpression)expression;
                fieldType = memberExpression.Type;
            }
            else if (expression is MethodCallExpression)
            {
                var methodExpression = (MethodCallExpression)expression;
                fieldType = methodExpression.Method.ReturnType;
            }
            else if (expression is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)expression;
                fieldType = unaryExpression.Operand.Type;
            }
            else if (expression is ParameterExpression)
            {
                var parameterExpression = (ParameterExpression)expression;
                fieldType = parameterExpression.Type;
            }

            if (fieldType == null)
            {
                throw new ApplicationException(
                    string.Format(
                        "Unable to retrieve the field type (such as return value) from expression of type {0}.",
                        expression.GetType().Name));
            }
            return fieldType;
        }

        public static Type GetDeclaringType(this Expression fieldSelector)
        {
            Type member = null;

            var selector = fieldSelector as LambdaExpression;
            if (selector != null)
            {
                var memberExpression = selector.Body as MemberExpression;
                if (memberExpression != null)
                    member = memberExpression.Member.DeclaringType;
                var methodCallExpression = selector.Body as MethodCallExpression;
                if (methodCallExpression != null)
                    member = methodCallExpression.Method.DeclaringType;
            }
            if (fieldSelector is MemberExpression)
            {
                member = ((MemberExpression)fieldSelector).Member.DeclaringType;
            }

            if (fieldSelector is MethodCallExpression)
            {
                member = ((MethodCallExpression)fieldSelector).Method.DeclaringType;
            }
            return member;
        }

        /// <summary>
        /// Returns a list of <typeparamref name="TExpression"/> instances
        /// that matches the <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="TExpression">The type of <see cref="Expression"/>
        /// to search for.</typeparam>
        /// <param name="expression">The <see cref="Expression"/> that represents the sub tree for which to start searching.</param>
        /// <param name="predicate">The <see cref="Func{T,TResult}"/> used to filter the result</param>
        /// <returns>A list of <see cref="Expression"/> instances that matches the given predicate.</returns>
        public static IEnumerable<TExpression> Find<TExpression>(this Expression expression, Func<TExpression, bool> predicate) where TExpression : Expression
        {
            var finder = new ExpressionFinder<TExpression>();
            return finder.Find(expression, predicate);
        }

        /// <summary>
        /// Searches for expressions using the given <paramref name="predicate"/> and replaces matches with
        /// the result from the <paramref name="replaceWith"/> delegate.
        /// </summary>
        /// <typeparam name="TExpression">The type of <see cref="Expression"/> to search for.</typeparam>
        /// <param name="expression">The <see cref="Expression"/> that represents the sub tree
        /// for which to start searching.</param>
        /// <param name="predicate">The <see cref="Func{T,TResult}"/> used to filter the result</param>
        /// <param name="replaceWith">The <see cref="Func{T,TResult}"/> used to specify the replacement expression.</param>
        /// <returns>The modified <see cref="Expression"/> tree.</returns>
        public static Expression Replace<TExpression>(this Expression expression, Func<TExpression, bool> predicate, Func<TExpression, Expression> replaceWith) where TExpression : Expression
        {

            var replacer = new ExpressionReplacer<TExpression>();

            return replacer.Replace(expression, predicate, replaceWith);

        }

        public static bool IsGetItemInvokationOnGenericDictionary(this MethodCallExpression methodCall)
        {
            if (methodCall == null || methodCall.Object == null)
            {
                return false;
            }

            var invokationTargetType = methodCall.Object.Type;
            if (!invokationTargetType.IsGenericType)
            {
                return false;
            }

            var typeArguments = invokationTargetType.GetGenericArguments();
            if (typeArguments.Length != 2)
            {
                return false;
            }

            var dictionaryType = typeof(IDictionary<,>).MakeGenericType(typeArguments);
            if (!dictionaryType.IsAssignableFrom(invokationTargetType))
            {
                return false;
            }
            if (methodCall.Method.Name != "get_Item")
            {
                return false;
            }

            return true;
        }

        public static Type GetMemberReturnType(this MemberInfo member)
        {
            if (member is PropertyInfo)
            {
                return ((PropertyInfo)member).PropertyType;
            }
            if (member is MethodInfo)
            {
                return ((MethodInfo)member).ReturnType;
            }

            return ((FieldInfo)member).FieldType;
        }
    }
}
