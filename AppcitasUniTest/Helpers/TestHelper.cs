using System;
using System.Net;
using Apcitas;
using Apcitas.WebService;

namespace AppcitasUniTest.Helpers;

public sealed class TestHelper
{
    private static readonly Lazy<TestHelper> _lazyInstance = new Lazy<TestHelper>(() => new TestHelper());

    public static TestHelper Instance
    {
        get { return _lazyInstance.Value; }
    }

    public HttpClient Client { get; set; }

    private TestHelper()
    {
        Client = new APIWebApplicationFactory<Startup>().CreateDefaultClient();
    } 
}
