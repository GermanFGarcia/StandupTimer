using CommunityToolkit.Maui.Markup;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace StandupTimer;

public class AppPage : ContentPage
{
    #region UI

    public AppPage(AppVM appVM) 
    {
        this.appVM = appVM; 

        BindingContext = appVM;

        Content = new VerticalStackLayout
        {
            // top panel
            new HorizontalStackLayout
            {
                // official time
                new Label().Bind(Label.TextProperty, static (AppVM vm) => vm.OfficialTime).TextColor(Colors.White),
            }.End(),

            // central panel
            new Grid
            {
                ColumnDefinitions = Columns.Define(Star, Auto),
                RowDefinitions = Rows.Define(Auto, Auto),

                Children =
                {
                    // turn time
                    // negative margins to compensate for letters ascender/descender space
                    new Label().Row(0).RowSpan(2).Column(0).Margins(0,-18,0,0).Bind(Label.TextProperty, static (AppVM vm) => vm.TurnSpan).FontSize(60).TextColor(Colors.White),

                    // standup time
                    new Label().Row(0).Column(1).End().Bottom().Margins(0,10,2,0).Bind(Label.TextProperty, static (AppVM vm) => vm.StandupSpan).FontSize(20).TextColor(Colors.White),

                    // turn count
                    new Label().Row(1).Column(1).End().Top().Margins(0,0,3,0).Bind(Label.TextProperty, static (AppVM vm) => vm.TurnCount).FontSize(14).TextColor(Colors.White),
                }
            }.FillVertical(),

            // bottom panel
            new Grid
            {
                ColumnDefinitions = Columns.Define(Star, Star),

                Children =
                {
                    new HorizontalStackLayout
                    {
                        new ImageButton().Assign(out ImageButton resetButton).Style(ResetIconButtonStyle).Bind(IsVisibleProperty, static (AppVM vm) => vm.ResetButtonVisible),
                        new ImageButton().Assign(out ImageButton playButton).Style(PlayIconButtonStyle).Bind(IsVisibleProperty, static (AppVM vm) => vm.PlayButtonVisible),
                        new ImageButton().Assign(out ImageButton pauseButton).Style(PauseIconButtonStyle).Bind(IsVisibleProperty, static (AppVM vm) => vm.PauseButtonVisible),
                        new ImageButton().Assign(out ImageButton nextButton).Style(NextIconButtonStyle).Bind(IsVisibleProperty, static (AppVM vm) => vm.NextButtonVisible),
                    }.Column(0),

                    new HorizontalStackLayout
                    {
                        new ImageButton().Assign(out ImageButton startButton).Style(StartIconButtonStyle).Bind(IsVisibleProperty, static (AppVM vm) => vm.StartButtonVisible),
                        new ImageButton().Assign(out ImageButton stopButton).Style(StopIconButtonStyle).Bind(IsVisibleProperty, static (AppVM vm) => vm.StopButtonVisible)
                    }.Column(1).End()
                }
            }.Margins(-5,5,0,0),
        }
        .Paddings(30, 5, 20, 5)
        .Bind(BackgroundColorProperty, static (AppVM vm) => vm.BackgroundColor);

        // subscribe control events to custom events to mimick command behaviors
        startButton.Clicked += StartButtonClickedEventHandler;
        stopButton.Clicked += StopButtonClickedEventHandler;

        resetButton.Clicked += ResetButtonClickedEventHandler;
        playButton.Clicked += PlayButtonClickedEventHandler;
        pauseButton.Clicked += PauseButtonClickedEventHandler;
        nextButton.Clicked += NextButtonClickedEventHandler;

    }

    #endregion

    #region Fields

    AppVM appVM;

    #endregion

    #region Styles

    private static Thickness iconButtonPadding = new Thickness(5);

    public static Style ResetIconButtonStyle { get; } = new Style<ImageButton>(
        (ImageButton.AspectProperty, Aspect.Fill),
        (ImageButton.BackgroundColorProperty, Colors.Transparent),
        (ImageButton.BorderColorProperty, Colors.Transparent),
        (ImageButton.PaddingProperty, iconButtonPadding),
        (ImageButton.SourceProperty, "reset.png")
    );
    public static Style PlayIconButtonStyle { get; } = new Style<ImageButton>(
        (ImageButton.AspectProperty, Aspect.Fill),
        (ImageButton.BackgroundColorProperty, Colors.Transparent),
        (ImageButton.BorderColorProperty, Colors.Transparent),
        (ImageButton.PaddingProperty, iconButtonPadding),
        (ImageButton.SourceProperty, "play.png")
    );
    public static Style PauseIconButtonStyle { get; } = new Style<ImageButton>(
        (ImageButton.AspectProperty, Aspect.Fill),
        (ImageButton.BackgroundColorProperty, Colors.Transparent),
        (ImageButton.BorderColorProperty, Colors.Transparent),
        (ImageButton.PaddingProperty, iconButtonPadding),
        (ImageButton.SourceProperty, "pause.png")
    );
    public static Style NextIconButtonStyle { get; } = new Style<ImageButton>(
        (ImageButton.AspectProperty, Aspect.Fill),
        (ImageButton.BackgroundColorProperty, Colors.Transparent),
        (ImageButton.BorderColorProperty, Colors.Transparent),
        (ImageButton.PaddingProperty, iconButtonPadding),
        (ImageButton.SourceProperty, "next.png")
    );
    public static Style StartIconButtonStyle { get; } = new Style<ImageButton>(
        (ImageButton.AspectProperty, Aspect.Fill),
        (ImageButton.BackgroundColorProperty, Colors.Transparent),
        (ImageButton.BorderColorProperty, Colors.Transparent),
        (ImageButton.PaddingProperty, iconButtonPadding),
        (ImageButton.SourceProperty, "start.png")
    );
    public static Style StopIconButtonStyle { get; } = new Style<ImageButton>(
        (ImageButton.AspectProperty, Aspect.Fill),
        (ImageButton.BackgroundColorProperty, Colors.Transparent),
        (ImageButton.BorderColorProperty, Colors.Transparent),
        (ImageButton.PaddingProperty, iconButtonPadding),
        (ImageButton.SourceProperty, "stop.png")
    );

    #endregion

    #region Event Handlers

    private void StartButtonClickedEventHandler(object sender, EventArgs args)
    {
        appVM.StartCommand.Execute(null);
    }

    private void StopButtonClickedEventHandler(object sender, EventArgs args)
    {
        appVM.StopCommand.Execute(null);
    }
    
    private void ResetButtonClickedEventHandler(object sender, EventArgs args)
    {
        appVM.ResetCommand.Execute(null);
    }

    private void PlayButtonClickedEventHandler(object sender, EventArgs args)
    {
        appVM.PlayCommand.Execute(null);
    }

    private void PauseButtonClickedEventHandler(object sender, EventArgs args)
    {
        appVM.PauseCommand.Execute(null);
    }

    private void NextButtonClickedEventHandler(object sender, EventArgs args)
    {
        appVM.NextCommand.Execute(null);
    }

    #endregion
}
