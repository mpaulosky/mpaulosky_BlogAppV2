using BlogApp.Fixtures;

using C3D.Extensions.Playwright.AspNetCore.Xunit;

using FluentAssertions;

namespace BlogApp.Tests;

// This fixture creates a new web application, new browser, and a single page for the lifetime of the fixture
// The fixture is in context for the duration of all the tests in a single class.
public class ModalFixture : PlaywrightPageFixture<AssemblyClassLocator>
{
	public ModalFixture(IMessageSink output) : base(output)
	{
	}

	public bool SkipTest { get; set; }

	private readonly Guid _Uniqueid = Guid.NewGuid();

	public override LogLevel MinimumLogLevel => LogLevel.Warning;

	public override string? Environment => "Test";

	protected override IHost CreateHost(IHostBuilder builder)
	{
		// ServicesExtensions.SocialMediaProviders = new List<IConfigureProvider> { new StartStubSocialMediaProvider() };
		builder.AddTestConfiguration(jsonConfiguration: CONFIGURATION);
		builder.UseOnlyInMemoryService();
		builder.UseUniqueDb(_Uniqueid);
		return base.CreateHost(builder);
	}

	private const string CONFIGURATION = """
		{
			"ModerationEnabled": "false"
		}
	""";


	// Temp hack to see if it is a timing issue in github actions
	public async override Task InitializeAsync()
	{
		await base.InitializeAsync();
		await Services.ApplyStartUpDelay();
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize",
		Justification = "Base class calls SuppressFinalize")]
	public async override ValueTask DisposeAsync()
	{
		await base.DisposeAsync();

		var logger = MessageSink.CreateLogger<PlaywrightFixture>();
		await _Uniqueid.CleanUpDbFilesAsync(logger);
	}
}

// We don't pull in the default TestsBase class as we have a different fixture in use for these tests.
[TestCaseOrderer(ordererTypeName: "BlogAppTest.PriorityOrderer", ordererAssemblyName: "BlogAppTest")]
public class ModalWebTests : IClassFixture<ModalFixture>
{
	private IPage Page => _Webapp.Page; //Shared page for all tests
	private readonly ModalFixture _Webapp;
	private readonly ITestOutputHelper _OutputHelper;

	public ModalWebTests(ModalFixture webapp, ITestOutputHelper outputHelper)
	{
		_Webapp = webapp;
		_OutputHelper = outputHelper;
	}

	// Creating PriorityOrder to help run tests in an order
	// Refs: https://learn.microsoft.com/en-us/dotnet/core/testing/order-unit-tests?pivots=xunit
	// There are test dependencies here - so state of one can affect the other.
	// We use the Xunit.SkippableFact package (https://github.com/AArnott/Xunit.SkippableFact) to allow skipping a test
	// At the start of each test, we check the fixture flag and skip if set, then set it.
	// At the end of the test (if we complete) we reset the flag.

	[Fact(), TestPriority(1)]
	public async Task CanLaunchModal()
	{

		Skip.If(_Webapp.SkipTest, "Previous test failed");
		_Webapp.SkipTest = true;

		// await using var trace = await page.TraceAsync("Can Launch Modal", true, true, true);

		var page = await Page.GotoHomePage();

		var result = await page.TitleAsync();

		// Assert
		result.Should().Be("Blog Home");

		await page.CloseAsync();

	}

}
