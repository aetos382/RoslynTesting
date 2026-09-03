using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aetos.RoslynTesting.Tests;

[TestClass]
public sealed class Test1
{
    [TestMethod]
    public async Task TestMethod1()
    {
        var driver = new RoslynDriver();

        driver.AddSource(
            "hello.cs",
            """
            Console.WriteLine("hello, world.");
            """);

        await driver.RunAsync(this.CancellationToken).ConfigureAwait(false);
    }

    public Test1(TestContext testContext)
    {
        ArgumentNullException.ThrowIfNull(testContext);

        this._testContext = testContext;
    }

    public CancellationToken CancellationToken => this._testContext.CancellationToken;

    private readonly TestContext _testContext;
}
