using HelperProjects.AppsettingsEditor.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HelperProjects.AppsettingsEditor.Services;

public class AppSettingsService
{
    private readonly ILogger<AppSettingsService> _logger;

    public AppSettingsService(ILogger<AppSettingsService> logger)
    {
        _logger = logger;
    }

    public AppSettingsModel LoadFromJson(string jsonContent)
    {
        try
        {
            var model = JsonConvert.DeserializeObject<AppSettingsModel>(jsonContent);
            model ??= new AppSettingsModel();
            return model;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading appsettings from JSON");
            throw;
        }
    }

    public string SaveToJson(AppSettingsModel model)
    {
        try
        {
            // Convert calendars definitions back to pipe format for URLs
            var clonedModel = CloneModel(model);
            if (clonedModel.Calendars?.Definitions != null)
            {
                clonedModel.Calendars.Definitions = clonedModel.Calendars.Definitions
                    .ToDictionary(kvp => kvp.Key.Replace(':', '|'), kvp => kvp.Value);
            }

            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            return JsonConvert.SerializeObject(clonedModel, settings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving appsettings to JSON");
            throw;
        }
    }

    private AppSettingsModel CloneModel(AppSettingsModel model)
    {
        var json = JsonConvert.SerializeObject(model);
        return JsonConvert.DeserializeObject<AppSettingsModel>(json) ?? new AppSettingsModel();
    }

    public bool ValidateJson(string jsonContent, out string errorMessage)
    {
        try
        {
            JToken.Parse(jsonContent);
            errorMessage = string.Empty;
            return true;
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
            return false;
        }
    }
}
