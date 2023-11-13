namespace ContactManagement.RabbitMqService;

public class RabbitMqConfiguration
{
    // RabbitMQ sunucu adresi
    public string HostName { get; set; }
    
    public int Port { get; set; }

    // RabbitMQ sunucusuna bağlanmak için kullanılacak kullanıcı adı
    public string UserName { get; set; }

    // RabbitMQ sunucusuna bağlanmak için kullanılacak parola
    public string Password { get; set; }
}