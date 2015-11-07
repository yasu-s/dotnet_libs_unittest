namespace MSTest.Extensions.Test
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MSTest.Extensions;

    /// <summary>
    /// AssertEx テストクラス
    /// </summary>
    [TestClass]
    public class AssertExTest
    {
        /// <summary>
        /// AssertEx.Throws 
        /// 正常系
        /// </summary>
        [TestMethod]
        public void ThrowsTest1()
        {
            Action action = () => { throw new ApplicationException("Test"); };
            var ex = AssertEx.Throws<ApplicationException>(action);
            
            Assert.AreEqual("Test", ex.Message);
        }

        /// <summary>
        /// AssertEx.Throws 
        /// 例外が発生しないケース
        /// </summary>
        [TestMethod]
        public void ThrowsTest2()
        {
            try
            {
                Action action = () => { };
                AssertEx.Throws<ApplicationException>(action);
            }
            catch (System.Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(AssertFailedException));
                Assert.AreEqual("AssertEx.Throws に失敗しました。例外が発生しません。", ex.Message);
            }
        }

        /// <summary>
        /// AssertEx.Throws 
        /// 検証対象の例外の型が異なる
        /// </summary>
        [TestMethod]
        public void ThrowsTest3()
        {
            try
            {
                Action action = () => 
                {
                    int num1 = 1;
                    int num2 = 0;
                    int num3 = num1 / num2;
                };

                AssertEx.Throws<ApplicationException>(action);
            }
            catch (System.Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(AssertFailedException));
                Assert.AreEqual(string.Format("AssertEx.Throws に失敗しました。<{0}>が必要ですが、<{1}>が指定されました。", typeof(ApplicationException).FullName, typeof(DivideByZeroException).FullName), ex.Message);
            }
        }
        
    }
}
