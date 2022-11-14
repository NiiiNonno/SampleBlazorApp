using Android.App;
using Android.Runtime;
using Nonno.Assets.Collections;

namespace SampleBlazorApp;

[Application]
public class MainApplication : MauiApplication
{
	public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
        Configuration.Default = new Configuration(FileSystem.AppDataDirectory);
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
