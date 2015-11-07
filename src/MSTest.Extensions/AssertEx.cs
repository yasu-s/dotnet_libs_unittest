namespace MSTest.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
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

    }
}
