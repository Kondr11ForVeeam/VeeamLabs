using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _1._2.Lab
{
    class Program
    {
        static void Main(string[] args)
        {
            var pub = new Publisher<string>();
            var eventBus = CustomEventBus<string>.Instance;
            var sub1 = new Subscriber("sub1");
            var sub2 = new Subscriber("sub2");

            eventBus.Suscribe(pub.GetType(), sub1.HandleCustomEvent);
            eventBus.Suscribe(pub.GetType(), sub2.HandleCustomEvent);

            pub.DoSomething(eventBus);
            Console.ReadKey();

        }
    }

    public class CustomEventArgs : EventArgs
    {
        public object EventHost { get; set; }
        public MethodInfo ActiveMethod { get; set; }
    }

    public class CustomEventBus<T>
    {
        private static CustomEventBus<T> _instance = null;
        private static readonly object _lock = new object();
        private Dictionary<Type, List<CustomEventArgs>> dicEvent = new Dictionary<Type, List<CustomEventArgs>>();

        protected CustomEventBus()
        {
        }

        public static CustomEventBus<T> Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new CustomEventBus<T>();
                    }
                    return _instance;
                }
            }
        }

        public void PostMessage(Type type, params object[] args)
        {
            List<CustomEventArgs> arr = dicEvent[type];
            foreach (CustomEventArgs eventArgs in arr)
            {
                eventArgs.ActiveMethod.Invoke(eventArgs.EventHost, args);
            }
        }

        public void Suscribe(Type tEvent, Action<T> action)
        {
            if (!dicEvent.Keys.Contains(tEvent))
            {
                dicEvent[tEvent] = new List<CustomEventArgs>();
            }
            dicEvent[tEvent].Add(new CustomEventArgs()
            {
                EventHost = action.Target,
                ActiveMethod = action.Method
            });
        }

        public void UnSuscribe(Type tEvent)
        {
            dicEvent[tEvent].Clear();
        }
    }

    class Publisher<T>
    {
        public void DoSomething(CustomEventBus<T> eventBus)
        {
            eventBus.PostMessage(this.GetType(), $"Event triggered at {DateTime.Now}");
        }
    }

    class Subscriber
    {
        private readonly string _id;

        public Subscriber(string id)
        {
            _id = id;
        }

        public void HandleCustomEvent(string message)
        {
            Console.WriteLine($"{_id} received this message: {message}");
        }
    }
}