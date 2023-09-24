using CommunityToolkit.Mvvm.Messaging;

namespace StandupTimer;

public class AppModel
{
    #region Constructors

    public AppModel()
    {
        IsStandupStarted = false;

        Now = DateTime.Now;
        OfficalTime = Now;
        TurnCount = 0;

        var timer = Application.Current.Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromMilliseconds(30);
        timer.Tick += (sender, eventArgs) =>
        {
            Now = DateTime.Now;
            OfficalTime = Now;

            WeakReferenceMessenger.Default.Send(new TickMessage());
        };

        timer.Start();
    }

    #endregion

    #region Properties

    private DateTime Now { get; set; }

    public DateTime OfficalTime { get; set; }

    public bool IsStandupStarted { get; set; }
    public DateTime StandupStartTime { get; set; }
    public TimeSpan StandupTime => 
        IsStandupStarted ?
            Now - StandupStartTime :
            TimeSpan.Zero;

    public bool IsTurnPaused { get; set; }
    public DateTime TurnStartTime { get; set; }
    public DateTime TurnPauseTime { get; set; }
    public TimeSpan TurnTime => 
        IsStandupStarted ? 
            IsTurnPaused ?
                TurnPauseTime - TurnStartTime :
                Now - TurnStartTime : 
            TimeSpan.Zero;

    public int TurnCount { get; set; }

    private TimeSpan HalfTime = new TimeSpan(0, 0, 30);
    private TimeSpan WarningTime = new TimeSpan(0, 0, 50);
    private TimeSpan OutTime = new TimeSpan(0, 1, 0);

    public TurnStatus TurnStatus  
    {
        get
        {
            if (TimeSpan.Compare(TurnTime, HalfTime) == -1) { return TurnStatus.Ok; }
            if (TimeSpan.Compare(TurnTime, WarningTime) == -1) { return TurnStatus.Half; }
            if (TimeSpan.Compare(TurnTime, OutTime) == -1) { return TurnStatus.Warning; }
            return TurnStatus.Out;
        }
    }

    #endregion

    #region Methods

    public void StartStandup()
    {
        IsStandupStarted = true;   
        StandupStartTime = Now;
        IsTurnPaused = true;    
        TurnStartTime = Now;
        TurnPauseTime = Now;

        WeakReferenceMessenger.Default.Send(new StatusChangedMessage());
    }

    public void StopStandup()
    {
        IsStandupStarted = false;
        IsTurnPaused = true;
        TurnPauseTime = Now;

        WeakReferenceMessenger.Default.Send(new StatusChangedMessage());
    }

    public void NextTurn()
    {
        IsTurnPaused = true;
        TurnStartTime = Now;
        TurnPauseTime = Now;
        TurnCount++;

        WeakReferenceMessenger.Default.Send(new StatusChangedMessage());
    }

    public void ResumeTurn()
    {
        IsTurnPaused = false;
        TurnStartTime += Now - TurnPauseTime;

        WeakReferenceMessenger.Default.Send(new StatusChangedMessage());
    }

    public void PauseTurn()
    {
        IsTurnPaused = true;
        TurnPauseTime = Now;

        WeakReferenceMessenger.Default.Send(new StatusChangedMessage());
    }

    public void ResetTurn()
    {
        TurnStartTime = Now;
        TurnPauseTime = Now;
    }

    #endregion
}

public class TickMessage { public TickMessage() { } }
public class StatusChangedMessage { public StatusChangedMessage() { } } 

public enum TurnStatus { Ok, Half, Warning, Out }