namespace demo_project.app.contracts
{
    public class ClusterConfiguration
    {
        public RedisConfiguration Redis { get; set; }

        public RabbitMQConfiguration RabbitMQ { get; set; }
    }
}
