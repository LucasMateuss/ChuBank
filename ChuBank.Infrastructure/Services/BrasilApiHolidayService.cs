using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using ChuBank.Domain.Interfaces;
using ChuBank.Infrastructure.DTOs;

namespace ChuBank.Infrastructure.Services;

public class BrasilApiHolidayService : IHolidayService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private const string BaseUrl = "https://brasilapi.com.br/api/feriados/v1";

    public BrasilApiHolidayService(HttpClient httpClient, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _cache = cache;
    }

    public async Task<bool> IsBusinessDayAsync(DateTime date)
    {
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
        {
            return false;
        }

        var holidays = await GetHolidaysAsync(date.Year);

        bool isHoliday = holidays.Any(h => h.Date.Date == date.Date);

        return !isHoliday; 
    }

    private async Task<List<HolidayDto>> GetHolidaysAsync(int year)
    {
        string cacheKey = $"holidays_{year}";

        if (_cache.TryGetValue(cacheKey, out List<HolidayDto>? cachedHolidays))
        {
            return cachedHolidays ?? new List<HolidayDto>();
        }

        try
        {
            var response = await _httpClient.GetFromJsonAsync<List<HolidayDto>>($"{BaseUrl}/{year}");

            if (response != null)
            {
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(24));

                _cache.Set(cacheKey, response, cacheOptions);
                return response;
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Falha ao consultar serviço de feriados.", ex);
        }

        return new List<HolidayDto>();
    }
}
