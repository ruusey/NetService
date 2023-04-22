using NetService.Repo;

namespace NetService.Async
{
    public abstract class ServiceThread

    {
        private Thread _thread { get; }
        private EventLogRepo _repo { get; set; }
        protected ServiceThread(EventLogRepo context) {
            this._repo = context;
            _thread = new Thread(new ThreadStart(this.RunThread));
        }
        public EventLogRepo Repository { get { return _repo; } set { } }

        public void Start() => _thread.Start();
        public void Join() => _thread.Join();
        public bool IsAlive => _thread.IsAlive;

        // Override in base class
        public abstract void RunThread();
    }
}
