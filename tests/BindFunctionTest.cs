using System.Linq;

#if NETFX_CORE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using SetUp = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestInitializeAttribute;
using TestFixture = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using Test = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
#else
using NUnit.Framework;
#endif

namespace SQLite.Tests
{
	[TestFixture]
	public class BindFunctionTest
	{
		class Result
		{
			public int Value { get; set; }
		}

		[Test]
		public void BindReturnInt()
		{
			using (var db = new TestDb())
			{
				db.BindFunction("get_int_value", 0, GetIntValue);

				var res = db.Query<Result>("select get_int_value() as Value").First();
				Assert.AreEqual(42, res.Value);
			}
		}

		[Test]
		public void BindReturnIntWithArgs()
		{
			using (var db = new TestDb())
			{
				db.BindFunction("add2", 2, Add);

				var res = db.Query<Result>("select add2(42, 17) as Value").First();
				Assert.AreEqual(42 + 17, res.Value);
			}
		}

		#region Bound functions

		private object GetIntValue(object[] args)
		{
			return 42;
		}

		private object Add(object[] args)
		{
			var res = 0l;
			foreach (var arg in args)
			{
				res += (long)arg;
			}
			return res;
		}

		#endregion
	}
}
