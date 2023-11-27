using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows.Input;

namespace StandupTimer;

public class AppVM : ObservableObject
{
    #region Constructors

    public AppVM(AppModel appModel)
    {
        this.appModel = appModel;

        WeakReferenceMessenger.Default.Register<TickMessage>(this, TickMessageHandler);
        WeakReferenceMessenger.Default.Register<StatusChangedMessage>(this, StatusChangedMessageHandler);

        StartButtonVisible = !appModel.IsStandupStarted;
        StopButtonVisible = appModel.IsStandupStarted;
    }

    #endregion

    #region Fields
    
    AppModel appModel;

    #endregion

    #region Time Properties

    private string officialTime;
    public string OfficialTime
    {
        get => officialTime;
        set => SetProperty(ref officialTime, value);
    }

    private string standupTime;
    public string StandupSpan
    {
        get => standupTime;
        set => SetProperty(ref standupTime, value);
    }

    private string turnTime;
    public string TurnSpan
    {
        get => turnTime;
        set => SetProperty(ref turnTime, value);
    }

    private int turnCount;
    public int TurnCount 
    {
        get => turnCount;
        set => SetProperty(ref turnCount, value); 
    }

    private Color backgroundColor;
    public Color BackgroundColor
    {
        get => backgroundColor;
        set => SetProperty(ref backgroundColor, value);
    }

    #endregion

    #region Visibility Properties

    private bool startButtonVisible;
    public bool StartButtonVisible
    {
        get => startButtonVisible;
        set => SetProperty(ref startButtonVisible, value);
    }

    private bool stopButtonVisible;
    public bool StopButtonVisible
    {
        get => stopButtonVisible;
        set => SetProperty(ref stopButtonVisible, value);
    }

    private bool resetButtonVisible;
    public bool ResetButtonVisible
    {
        get => resetButtonVisible;
        set => SetProperty(ref resetButtonVisible, value);
    }

    private bool playButtonVisible;
    public bool PlayButtonVisible
    {
        get => playButtonVisible;
        set => SetProperty(ref playButtonVisible, value);
    }

    private bool pauseButtonVisible;
    public bool PauseButtonVisible
    {
        get => pauseButtonVisible;
        set => SetProperty(ref pauseButtonVisible, value);
    }

    private bool nextButtonVisible;
    public bool NextButtonVisible
    {
        get => nextButtonVisible;
        set => SetProperty(ref nextButtonVisible, value);
    }

    #endregion

    #region Commands

    private ICommand startCommand;
    public ICommand StartCommand => startCommand ??= new RelayCommand(StartStandup);
    private void StartStandup()
    {
        appModel.StartStandup();
    }

    private ICommand stopCommand;
    public ICommand StopCommand => stopCommand ??= new RelayCommand(StopStartup);
    private void StopStartup()
    {
        appModel.StopStandup();
    }

    private ICommand resetCommand;
    public ICommand ResetCommand => resetCommand ??= new RelayCommand(ResetTurn);
    private void ResetTurn()
    {
        appModel.ResetTurn();   
    }

    private ICommand playCommand;
    public ICommand PlayCommand => playCommand ??= new RelayCommand(ResumeTurn);
    private void ResumeTurn()
    {
        appModel.ResumeTurn();
    }

    private ICommand pauseCommand;
    public ICommand PauseCommand => pauseCommand ??= new RelayCommand(PauseTurn);
    private void PauseTurn()
    {
        appModel.PauseTurn();
    }

    private ICommand nextCommand;
    public ICommand NextCommand => nextCommand ??= new RelayCommand(NextTurn);
    private void NextTurn()
    {
        appModel.NextTurn();
    }

    #endregion

    #region Message Handlers

    private void TickMessageHandler(object receiver, TickMessage message)
    {
        OfficialTime = appModel.OfficalTime.ToString(@"H:mm:ss");
        StandupSpan = appModel.StandupSpan.ToString(@"mm\:ss");
        TurnSpan = appModel.TurnSpan.ToString(@"m\:ss");

        switch (appModel.TurnStatus)
        {
            case TurnStatus.Ok: BackgroundColor = Colors.ForestGreen.AddLuminosity(0.1f); break;
            case TurnStatus.Half: BackgroundColor = Colors.OliveDrab; break;
            case TurnStatus.Warning: BackgroundColor = Colors.Orange; break;
            case TurnStatus.Out: BackgroundColor = Colors.Red; break;
        }
    }

    private void StatusChangedMessageHandler(object receiver, StatusChangedMessage message)
    {
        StartButtonVisible = ! appModel.IsStandupStarted;
        StopButtonVisible = appModel.IsStandupStarted;

        TurnCount = appModel.TurnCount; 

        ResetButtonVisible = appModel.IsStandupStarted;
        PlayButtonVisible = appModel.IsStandupStarted && appModel.IsTurnPaused;
        PauseButtonVisible = appModel.IsStandupStarted && ! appModel.IsTurnPaused;
        NextButtonVisible = appModel.IsStandupStarted;
    }

    #endregion
}
