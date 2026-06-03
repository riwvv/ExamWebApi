using ExamWebApi.Services;

namespace ExamWebApi.Extensions;

public static class AppExtensions {
    public static IServiceCollection AddService(this IServiceCollection services) {
        services.AddScoped<CreateBuildingService>();
        services.AddScoped<CreateElevatorService>();
        services.AddScoped<SearchElevatorService>();
        services.AddScoped<ActionElevatorService>();

        return services;
    }
}
