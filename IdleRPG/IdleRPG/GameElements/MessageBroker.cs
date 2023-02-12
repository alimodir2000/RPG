using System.Reactive.Subjects;

namespace IdleRPG.GameElements
{
    public class MessageBroker
    {
        private static Subject<>
        private static MessageBroker instance;

        private Dictionary<Type, List<Action<object>>> subscribers = new Dictionary<Type, List<Action<object>>>();

        private MessageBroker() { }

        public static MessageBroker Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MessageBroker();
                }
                return instance;
            }
        }

        public void Subscribe<T>(Action<object> action)
        {
            Type type = typeof(T);
            if (!subscribers.ContainsKey(type))
            {
                subscribers[type] = new List<Action<object>>();
            }
            subscribers[type].Add(action);
        }

        public void Publish<T>(T message)
        {
            Type type = typeof(T);
            if (subscribers.ContainsKey(type))
            {
                foreach (Action<object> subscriber in subscribers[type])
                {
                    subscriber(message);
                }
            }
        }
    }

}