using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = S616.Analayzer.Test.CSharpCodeFixVerifier<
    S616.Analayzer.S616AnalayzerAnalyzer,
    S616.Analayzer.S616AnalayzerCodeFixProvider>;

namespace S616.Analayzer.Test
{
    [TestClass]
    public class S616AnalayzerUnitTest
    {
        //No diagnostics expected to show up
        [TestMethod]
        public async Task TestMethod1()
        {
            var test = @"";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public async Task TestMethod2()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class {|#0:TypeName|}
        {   
        }
    }";

            var fixtest = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TYPENAME
        {   
        }
    }";

            var expected = VerifyCS.Diagnostic("S616Analayzer").WithLocation(0).WithArguments("TypeName");
            await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public async Task TestMethod4()
        {
            var test = @"
using System;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
          if (true)
              Console.WriteLine(""This is a test"");
				}
	}
}
";

            var fixtest = @"
using System;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
          if (true) 
					{
              Console.WriteLine(""This is a test"");
					}
				}
	}
}
";

            await VerifyCS.VerifyCodeFixAsync(test, fixtest);
        }
    }
}