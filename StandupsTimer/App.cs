using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;
using Microsoft.UI;
#if WINDOWS
using Microsoft.UI.Windowing;
#endif

namespace StandupTimer;

public static class CommonApp
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        builder
            // create the app
            .UseMauiApp<LauncherApp>()
            // initialize the .NET MAUI Community Toolkit 
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMarkup();

        return builder.Build();
	}
}

public class LauncherApp : Application
{
	public LauncherApp()
	{
		var appModel = new AppModel();
		var appVM = new AppVM(appModel);
		MainPage = new AppPage(appVM);
    }

    // In Windows, override this method to set the required window size.
    // In Android, not necessary, the app takes the whole screen
    #if WINDOWS
    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);

        window.Width = 330;
        window.Height = 175;
		window.Title = "Stand-up Timer";

        // retrieve the window handle (HWND) of the current WinUI 3 window
        // to set native window properties
        Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, view) => 
        {
            var nativeWindow = handler.PlatformView;
            var wHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
            var wId = Win32Interop.GetWindowIdFromWindow(wHandle);
            var wPresenter = AppWindow.GetFromWindowId(wId).Presenter as OverlappedPresenter;

            wPresenter.IsAlwaysOnTop = true;
            wPresenter.IsResizable = false;
            wPresenter.IsMaximizable = false;
        });
                
        return window;
    }
    #endif
}
