namespace MSTest.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Assert拡張クラス
    /// </summary>
    public static class AssertEx
    {

        /// <summary>
        /// 指定したメソッドの実行時に指定した例外が発生することを確認します。<br />
        /// 例外が発生しない場合、または指定した例外と異なる例外が発生した場合、アサーションは失敗します。
        /// </summary>
        /// <typeparam name="T">検証対象の例外の型</typeparam>
        /// <param name="action">検証対象のメソッド</param>
        /// <returns>検証対象の例外</returns>
        public static T Throws<T>(Action action) where T : System.Exception
        {
            try
            {
                action();
                throw new AssertFailedException("AssertEx.Throws に失敗しました。例外が発生しません。");
            }
            catch (AssertFailedException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                if (ex is T)
                    return ex as T;
                else
                    throw new AssertFailedException(string.Format("AssertEx.Throws に失敗しました。<{0}>が必要ですが、<{1}>が指定されました。", typeof(T).FullName, ex.GetType().FullName));
            }
        }

        /// <summary>
        /// 等値演算子を使用して、指定した 2 つのオブジェクトのプロパティ値が等しいことを確認します。<br />
        /// これらが同一でない場合、アサーションは失敗します。
        /// </summary>
        /// <typeparam name="T">検証対象のオブジェクトの型</typeparam>
        /// <param name="expected">予測値</param>
        /// <param name="actual">実測値</param>
        /// <param name="propertyName">検証対象のプロパティ名</param>
        public static void AreEqual<T>(T expected, T actual, string propertyName)
        {
            var propInfo      = typeof(T).GetProperty(propertyName);
            var expectedValue = propInfo.GetValue(expected);
            var actualValue   = propInfo.GetValue(actual);

            if (!object.Equals(expectedValue, actualValue))
                throw new AssertFailedException(string.Format("AssertEx.Throws に失敗しました。<{0}>が必要ですが、<{1}>が指定されました。", expectedValue, actualValue));
        }

        /// <summary>
        /// 等値演算子を使用して、指定した 2 つのオブジェクトのプロパティ値が等しいことを確認します。<br />
        /// これらが同一でない場合、アサーションは失敗します。
        /// </summary>
        /// <typeparam name="T">検証対象のオブジェクトの型</typeparam>
        /// <param name="expected">予測値</param>
        /// <param name="actual">実測値</param>
        /// <param name="propertyName">検証対象のプロパティ名</param>
        public static void AreEqual<T>(T expected, T actual, Expression<Func<T, object>> property)
        {
            var member = FindProperty(property);
            AreEqual(expected, actual, member.Name);
        }

        /// <summary>
        /// プロパティ検索処理
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        private static MemberInfo FindProperty(Expression exp)
        {
            switch (exp.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return ((MemberExpression)exp).Member;
                case ExpressionType.Lambda:
                    return FindProperty(((LambdaExpression)exp).Body);
                case ExpressionType.Convert:
                    return FindProperty(((UnaryExpression)exp).Operand);
                default:
                    throw new ArgumentException();
            }
        }
    }
}
