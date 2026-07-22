namespace demo_project.api.host
{
    public class RedisConfiguration
    {
        //
        // Summary:
        //     The configuration used to connect to Redis.
        public string Configuration { get; set; }

        //
        // Summary:
        //     The Redis instance name.
        public string InstanceName { get; set; }
    }

    public class RabbitMQConfiguration
    {
        /// <summary>
        /// The RabbitMQ host to connect to (should be a valid hostname)
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// The RabbitMQ port to connect
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// The Username for connecting to the host
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password for connection to the host
        /// MAYBE this should be a SecureString instead of a regular string
        /// </summary>
        public string Password { get; set; }
    }
}
