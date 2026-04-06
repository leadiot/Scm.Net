using RabbitMQ.Client;

namespace Com.Scm.RabbitMQ.Impl
{
    public class RabbitMQConnection : IRabbitMQConnection
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private bool _isDisposed;

        public RabbitMQConnection(ConnectionFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _connection = factory.CreateConnectionAsync().Result;
        }

        public async Task<IChannel> CreateChannelAsync()
        {
            EnsureNotDisposed();
            return await _connection.CreateChannelAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                // Free any other managed objects here.
            }

            // Free any unmanaged objects here.
            _connection.Dispose();

            _isDisposed = true;
        }

        ~RabbitMQConnection()
        {
            Dispose(false);
        }

        private void EnsureNotDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(RabbitMQConnection));
            }
        }
    }
}
