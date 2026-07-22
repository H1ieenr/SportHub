namespace demo_project.api.host
{
    public class WorkerConfiguration
    {
        public RedisConfiguration Redis { get; set; }

        public RabbitMQConfiguration RabbitMQ { get; set; }
    }
}
