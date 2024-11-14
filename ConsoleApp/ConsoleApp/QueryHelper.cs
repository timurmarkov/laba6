﻿using ConsoleApp.Model;
using ConsoleApp.Model.Enum;
using ConsoleApp.OutputTypes;

namespace ConsoleApp;

public class QueryHelper : IQueryHelper
{
    /// <summary>
    /// Get Deliveries that has payed
    /// </summary>
    public IEnumerable<Delivery> Paid(IEnumerable<Delivery> deliveries) => deliveries.Where(d => d.PaymentId != null); //TODO: Завдання 1

    /// <summary>
    /// Get Deliveries that now processing by system (not Canceled or Done)
    /// </summary>
    public IEnumerable<Delivery> NotFinished(IEnumerable<Delivery> deliveries) => deliveries.Where(d => d.Status != DeliveryStatus.Cancelled && d.Status != DeliveryStatus.Done); //TODO: Завдання 2
    
    /// <summary>
    /// Get DeliveriesShortInfo from deliveries of specified client
    /// </summary>
    public IEnumerable<DeliveryShortInfo> DeliveryInfosByClient(IEnumerable<Delivery> deliveries, string clientId) => deliveries
        .Where(d => d.ClientId == clientId)
        .Select(d => new DeliveryShortInfo
        {
            Id = d.Id,
            StartCity = d.Direction.Origin.City,
            EndCity = d.Direction.Destination.City,
            ClientId = d.ClientId,
            Type = d.Type,
            LoadingPeriod = d.LoadingPeriod,
            ArrivalPeriod = d.ArrivalPeriod,
            Status = d.Status,
            CargoType = d.CargoType
        }); //TODO: Завдання 3

    /// <summary>
    /// Get first ten Deliveries that starts at specified city and have specified type
    /// </summary>
    public IEnumerable<Delivery> DeliveriesByCityAndType(IEnumerable<Delivery> deliveries, string cityName, DeliveryType type) => deliveries
        .Where(d => d.Direction.Origin.City == cityName && d.Type == type)
        ; //.Take(10) щоб взяти 10  //TODO: Завдання 4

    /// <summary>
    /// Order deliveries by status, then by start of loading period
    /// </summary>
    public IEnumerable<Delivery> OrderByStatusThenByStartLoading(IEnumerable<Delivery> deliveries) => deliveries
            .OrderBy(d => d.Status)
            .ThenBy(d => d.LoadingPeriod.Start);//TODO: Завдання 5

    /// <summary>
    /// Count unique cargo types
    /// </summary>
    public int CountUniqCargoTypes(IEnumerable<Delivery> deliveries) => deliveries
            .Select(d => d.CargoType)
            .Distinct()
            .Count(); //TODO: Завдання 6

    /// <summary>
    /// Group deliveries by status and count deliveries in each group
    /// </summary>
    public Dictionary<DeliveryStatus, int> CountsByDeliveryStatus(IEnumerable<Delivery> deliveries) => deliveries
            .GroupBy(d => d.Status)
            .ToDictionary(g => g.Key, g => g.Count());//TODO: Завдання 7

    /// <summary>
    /// Group deliveries by start-end city pairs and calculate average gap between end of loading period and start of arrival period (calculate in minutes)
    /// </summary>
    public IEnumerable<AverageGapsInfo> AverageTravelTimePerDirection(IEnumerable<Delivery> deliveries) => deliveries
            .GroupBy(delivery => new { StartCity = delivery.Direction.Origin.City, EndCity = delivery.Direction.Destination.City })
            .Select(group => new AverageGapsInfo
            {
                StartCity = group.Key.StartCity,
                EndCity = group.Key.EndCity,
                AverageGap = group.Average(delivery =>
                    (delivery.ArrivalPeriod.Start.Value - delivery.LoadingPeriod.End.Value).Minutes) //TotalMinutes
            });//TODO: Завдання 8

    /// <summary>
    /// Paging helper
    /// </summary>
    public IEnumerable<TElement> Paging<TElement, TOrderingKey>(IEnumerable<TElement> elements,
        Func<TElement, TOrderingKey> ordering,
        Func<TElement, bool>? filter = null,
        int countOnPage = 100,
        int pageNumber = 1) => 
        elements
        .Where(filter ?? (_ => true))
        .OrderBy(ordering)
        .Skip((pageNumber - 1) * countOnPage)
        .Take(countOnPage); //TODO: Завдання 9 
}