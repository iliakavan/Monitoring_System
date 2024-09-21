namespace notifier.Infrastructure.UnitsOfWork;




public class UnitOfWork
    (
        AppDbcontext context,
        IProjectRepository projectRepository,
        IProjectOffcialRepository projectOffcialRepository,
        IServiceNotificationRepository serviceNotificationRepository,
        IServiceRepository serviceRepository,
        IServiceTestRepository serviceTestRepository,
        IServiceTestLogRepository serviceTestLogRepository,
        ITelegramMassageLogRepository telegramMassageLogRepository,
        IUserRepository userRepository,
        IErrorLogRepository errorLogRepository
        
    ) : IUnitsOfWorks
{
    private readonly IProjectRepository projectRepo = projectRepository;
    private readonly IProjectOffcialRepository ProjectOfficialsRepo = projectOffcialRepository;
    private readonly IServiceNotificationRepository serviceNotificationRepo = serviceNotificationRepository;
    private readonly IServiceRepository serviceRepo = serviceRepository;
    private readonly IServiceTestRepository serviceTestRepo = serviceTestRepository;
    private readonly IServiceTestLogRepository serviceTestLogRepo = serviceTestLogRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IErrorLogRepository _errorLogRepository = errorLogRepository;
    private readonly ITelegramMassageLogRepository _telegramMassageLogRepository = telegramMassageLogRepository;
    private readonly AppDbcontext _context = context;


    public IProjectOffcialRepository ProjectOffcialRepo => ProjectOfficialsRepo;

    public IProjectRepository ProjectRepo => projectRepo;

    public IServiceNotificationRepository NotificationRepo => serviceNotificationRepo;

    public IServiceRepository ServiceRepo => serviceRepo;

    public IServiceTestRepository ServiceTestRepo => serviceTestRepo;

    public IServiceTestLogRepository ServiceTestLogRepo => serviceTestLogRepo;

    public ITelegramMassageLogRepository TelegramMassageLogRepo => _telegramMassageLogRepository;

    public IUserRepository UserRepo => _userRepository;

    public IErrorLogRepository ErrorLogRepo => _errorLogRepository;

    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task SaveChanges() 
    {
        await _context.SaveChangesAsync();
    }
}
