﻿namespace notifier.Domain.Models;





[Table("ServiceNotifications", Schema = "dbo")]

public class ServiceNotfications : BaseEntity
{
    public int RetryCount { get; set; } = 0;

    public int ServiceTestId {  get; set; }

    public ServiceTest ServiceTest { get; set; } = null!;

    public NotificationType NotificationType { get; set; }

    public ICollection<ServiceTestLog> Log { get; } = new List<ServiceTestLog>();

    public required string MessageFormat {  get; set; }

    public required string MessageSuccess {  get; set; }

    public int ErrorRetryCount {  get; set; } = 0;

    public int MaxRetryCount { get; set; } = 50;

}
