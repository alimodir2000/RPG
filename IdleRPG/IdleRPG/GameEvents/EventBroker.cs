using System.Reactive.Subjects;
using System.Xml.Serialization;

namespace IdleRPG.GameEvents
{
    /// <summary>
    /// This class facsilate the communication amonge game classes
    /// It is a singleton
    /// </summary>
    public class EventBroker : IObservable<GameEvent>
    {
        private static Subject<GameEvent> _subscribers;
        private static EventBroker _instance;

        private EventBroker()
        {
            _subscribers = new Subject<GameEvent>();
        }

        public static EventBroker Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EventBroker();
                }
                return _instance;
            }
        }


        public IDisposable Subscribe(IObserver<GameEvent> observer)
        {
            return _subscribers.Subscribe(observer);
        }

        public void Publish(GameEvent gameEvent)
        {
            _subscribers.OnNext(gameEvent);
        }
    }

}