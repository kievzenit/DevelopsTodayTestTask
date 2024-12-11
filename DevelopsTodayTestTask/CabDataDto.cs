namespace DevelopsTodayTestTask;

public record CabDataDto(
    DateTime TpepPickupDatetime,
    DateTime TpepDropoffDatetime,
    int PassengerCount,
    double TripDistance,
    string StoreAndFwdFlag,
    int PULocationId,
    int DOLocationId,
    decimal FareAmount,
    decimal TipAmount);
